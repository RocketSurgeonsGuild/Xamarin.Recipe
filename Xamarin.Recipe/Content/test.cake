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

Task("UI-Test")
    .WithCriteria(() => BuildParameters.ShouldRunUITests)
    .IsDependentOn("Unit-Test");