Task("Unit-Test")
    .WithCriteria(() => BuildParameters.ShouldRunUnitTests)
    .Does(() =>
    {
        Verbose("Executing Unit Tests");
        foreach(FilePath path in BuildParameters.UnitTestWhitelist)
        {
            var fullPath = MakeAbsolute(path).FullPath;

            Verbose("Executing: {0}", fullPath);
            DotNetCoreTest(fullPath, ToolSettings.DotNetTestSettings());
        }
    });

BuildParameters.Tasks.TestxUnitTask = Task("xUnit-Tests")
                                        .WithCriteria(() => BuildParameters.ShouldRunxUnit)
                                        .Does(() =>
                                        {
                                            XUnit2(BuildParameters.UnitTestWhitelist, ToolSettings.XUnitSettings());
                                        });

BuildParameters.Tasks.TestxUnitCoverletGenerateTask = Task("Test-xUnit-Coverlet")
    .IsDependentOn("Install-Coverlet")
    .WithCriteria(() => DirectoryExists(BuildParameters.TestDirectoryPath))
    .Does(() => {
        EnsureDirectoryExists(BuildParameters.TestDirectoryPath);

        // Clean the directories since we'll need to re-generate the debug type.
        CleanDirectories($"./{BuildParameters.SolutionDirectoryPath}/**/obj/{BuildParameters.Configuration}");
        CleanDirectories($"./{BuildParameters.SolutionDirectoryPath}/**/bin/{BuildParameters.Configuration}");

        Information("Testing and performing coverage testing");
        foreach (var projectPath in BuildParameters.UnitTestWhitelist)
        {
            var packageName = projectPath.GetFilenameWithoutExtension();
            BuildProject(projectPath, true);

            var parsedProject = ParseAnalyzeProject(projectPath);

            if(parsedProject.OutputPaths == null || parsedProject.RootNameSpace == null || parsedProject.OutputType == null)
            {
                Information("OutputPaths: {0}", string.Join(", ", parsedProject.OutputPaths.Select(x => x.ToString())));
                Information("RootNameSpace: {0}", parsedProject.RootNameSpace);
                Information("OutputType: {0}", parsedProject.OutputType);
                throw new Exception(string.Format("Unable to parse project file correctly: {0}", projectPath));
            }

            Information($"Performing unit and coverage tests on {parsedProject.AssemblyName}");

            foreach (var outputPath in parsedProject.OutputPaths)
            {
                var testFile = outputPath.Combine($"{parsedProject.AssemblyName}.dll");

                var arguments = ToolSettings.SetCoverageProcessArguments(new ProcessArgumentBuilder().AppendQuoted(testFile.ToString()));
                arguments = arguments
                    .AppendSwitchQuoted("--output", outputPath.CombineWithFilePath(File("coverage-results.xml")).ToString())
                    .AppendSwitch("--target", "dotnet")
                    .AppendSwitchQuoted("--targetargs",  $"test {projectPath}  --no-build -c {BuildParameters.Configuration} --logger:trx;LogFileName=testresults.trx -r {outputPath}");

                var processSettings = new ProcessSettings {
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    Arguments = arguments,
                };

                StartProcess(Context.Tools.Resolve("coverlet*").ToString(), processSettings);
            }

            Information($"Finished unit and coverage testing {packageName}");
        }
    });

Task("UI-Test")
    .WithCriteria(() => BuildParameters.ShouldRunUITests)
    .IsDependentOn("Unit-Test");