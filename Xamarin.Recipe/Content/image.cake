BuildParameters.Tasks.AppIconCopy = Task("App-Icon-Copy")
    .WithCriteria(BuildParameters.ShouldCopyImages)
    .IsDependentOn("Test")
    .Does(() =>
    {
        var xamarinConfiguration = EnvironmentVariable(Environment.XamarinBuildConfigurationVariable);
        var appIconSrcImagePath = EnvironmentVariable(Environment.AppIconSourceImagePathVariable);
        var appIconDestImagePath = EnvironmentVariable(Environment.AppIconDestinationImagePathVariable);

        if(!string.IsNullOrEmpty(xamarinConfiguration) || !string.IsNullOrEmpty(appIconSrcImagePath) || !string.IsNullOrEmpty(appIconDestImagePath))
        {
            Warning("Please set environment variables for application icon copy.");
            return;
        }
        
        var imagePathDirectory = Directory(appIconSrcImagePath);
        var configPath = Directory(xamarinConfiguration);

        var srcDirectory = MakeAbsolute(((DirectoryPath)imagePathDirectory).Combine(configPath));

        Verbose(srcDirectory.FullPath);

        CopyFiles($"{srcDirectory.FullPath}/*.png", appIconDestImagePath);
    });