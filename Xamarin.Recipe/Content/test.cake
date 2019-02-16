Task("Unit-Test")
    .WithCriteria(() => BuildParameters.ShouldRunUnitTests)
    .Does(() =>
    {
        Verbose("Executing Unit Tests");
        foreach(FilePath path in BuildParameters.Paths.Files.UnitTestFilePaths)
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
                                            XUnit2(BuildParameters.Paths.Files.UnitTestFilePaths, ToolSettings.XUnitSettings());
                                        });

Task("UI-Test")
    .Does(() =>
    {

    });