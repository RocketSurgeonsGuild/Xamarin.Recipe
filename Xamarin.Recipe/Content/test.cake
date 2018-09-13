Task("Unit-Test")
    .Does(() =>
    {
        Verbose("Executing Unit Tests");
        foreach(FilePath path in BuildParameters.Paths.Files.UnitTestFilePaths)
        {
            var fullPath = MakeAbsolute(path).FullPath;

            Verbose("Executing: {0}", fullPath);

            DotNetCoreTest(fullPath, new DotNetCoreTestSettings
                { 
                    Configuration = BuildParameters.Configuration,
                    Framework = ToolSettings.TestFramework,
                    NoBuild = ToolSettings.TestNoBuild
                });
        }
    });

Task("UI-Test")
    .Does(() =>
    {

    });