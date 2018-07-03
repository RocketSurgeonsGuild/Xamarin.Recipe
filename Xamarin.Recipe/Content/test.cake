Task("Unit-Test")
    .Does(() =>
    {
        Verbose("Executing Unit Tests");
        foreach(FilePath path in BuildParameters.Paths.Files.UnitTestFilePaths)
        {
            Verbose("Executing: {0}", path);

            DotNetCoreTest(path.FullPath, new DotNetCoreTestSettings
                { 
                    Configuration = BuildParameters.Configuration,
                    NoBuild = true
                });
        }
    });

Task("UI-Test")
    .Does(() =>
    {

    });