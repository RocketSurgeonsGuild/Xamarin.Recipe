Task("Unit-Test")
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

Task("UI-Test")
    .Does(() =>
    {

    });