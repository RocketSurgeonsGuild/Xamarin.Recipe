BuildParameters.Tasks.ImageCopyTask = Task("Image-Copy")
    .IsDependentOn("Test")
    .Does(() =>
    {
        var xamarinConfiguration = EnvironmentVariable(Environment.XamarinBuildConfigurationVariable);
        var appIconSrcImagePath = EnvironmentVariable(Environment.AppIconSourceImagePathVariable);
        var appIconDestImagePath = EnvironmentVariable(Environment.AppIconDestinationImagePathVariable);
        
        var imagePathDirectory = Directory(appIconSrcImagePath);
        var configPath = Directory(xamarinConfiguration);

        var srcDirectory = MakeAbsolute(((DirectoryPath)imagePathDirectory).Combine(configPath));

        Verbose(srcDirectory.FullPath);

        CopyFiles($"{srcDirectory.FullPath}/*.png", appIconDestImagePath);
    });