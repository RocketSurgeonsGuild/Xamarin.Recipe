#addin nuget:?package=Cake.AndroidAppManifest&version=1.1.0

BuildParameters.Tasks.AndroidArchiveTask = Task("Android-Archive")
    .IsDependentOn("Android-Build")
    .IsDependentOn("Test")
    .Does(() =>
    {
        var keyStore = MakeAbsolute(File(EnvironmentVariable(Environment.KeyStoreVariable)));
        if (string.IsNullOrEmpty(keyStore.FullPath))
        {
            Warning("The Android key store environment variable is not defined.");
        }

        var keyStoreAlias = EnvironmentVariable(Environment.KeyAliasVariable);
        if (string.IsNullOrEmpty(keyStoreAlias))
        {
           Warning("The Android key store alias environment variable is not defined.");
        }

        var keyStorePassword = EnvironmentVariable(Environment.KeyStorePasswordVariable);
        if (string.IsNullOrEmpty(keyStorePassword))
        {
            Warning("The Android key store password environment variable is not defined.");
        }

        var keyPassword = EnvironmentVariable(Environment.KeyPasswordVariable);
        if(string.IsNullOrEmpty(keyPassword))
        {
            Warning("The Android key password environment variable is not defined.");
        }
        
        MSBuild(BuildParameters.AndroidProjectPath, configurator =>
                    configurator
                        .SetConfiguration(BuildParameters.Configuration)
                        .SetVerbosity(ToolSettings.MSBuildVerbosity)
                        .UseToolVersion(ToolSettings.MSBuildToolVersion)
                        .WithTarget("SignAndroidPackage")
                        .WithProperty("AndroidKeyStore", "true")
                        .WithProperty("AndroidSigningKeyStore", keyStore.FullPath)
                        .WithProperty("AndroidSigningStorePass", keyStorePassword)
                        .WithProperty("AndroidSigningKeyAlias", keyStoreAlias)
                        .WithProperty("AndroidSigningKeyPass", keyStorePassword)
                        .WithProperty("AndroidSdkBuildToolsVersion", ToolSettings.AndroidBuildToolVersion));
    });

Task("Android-Build")
    .IsDependentOn("Android-Manifest")
    .Does(() => 
    {
        MSBuild(BuildParameters.AndroidProjectPath, configurator =>
            configurator
                .SetConfiguration(BuildParameters.Configuration)
                .SetVerbosity(ToolSettings.MSBuildVerbosity)
                .UseToolVersion(ToolSettings.MSBuildToolVersion)
                .WithProperty("AndroidSdkBuildToolsVersion", ToolSettings.AndroidBuildToolVersion));
    });

Task("Android-Manifest")
    .WithCriteria(() => !string.IsNullOrEmpty(BuildParameters.AndroidManifest.FullPath))
    .Does(() =>
    {
        var bundleIdentifier = EnvironmentVariable(Environment.BundleIdentifierVariable);
        var manifest = DeserializeAppManifest(BuildParameters.AndroidManifest);

        manifest.VersionName = BuildParameters.Version.Version;
        manifest.VersionCode = BuildParameters.Version.PreReleaseNumber;

        if(!string.IsNullOrEmpty(bundleIdentifier))
        {
            manifest.PackageName = bundleIdentifier;
        }

        SerializeAppManifest(BuildParameters.AndroidManifest, manifest);
    });

Task("Android-AppCenter")
    .WithCriteria(() => BuildParameters.ShouldDeployAppCenter)
    .IsDependentOn("Android-Archive")
    .IsDependentOn("Copy-Apk")
    .Does(() =>
    {
        var androidArtifactsPath = MakeAbsolute(BuildParameters.Paths.Directories.DroidArtifactDirectoryPath);
        Verbose("iOS Artifact Diretory: {0}", androidArtifactsPath.FullPath);

        var apkPath = GetFiles(androidArtifactsPath.FullPath + "/*.apk");

        var apk = apkPath.FirstOrDefault()?.FullPath;

        Verbose("apk: {0}", apk);

        var settings = new AppCenterDistributeReleaseSettings
        {
            File = apk,
            Token = EnvironmentVariable(Environment.AppCenterTokenVariable),
            App = $"{EnvironmentVariable(Environment.AppCenterOwnerVariable)}/{EnvironmentVariable(Environment.AppCenterAppNameVariable)}",
            Group = EnvironmentVariable(Environment.AppCenterGroupVariable)
        };

        AppCenterDistributeRelease(settings);
    });

Task("Copy-Apk")
    .IsDependentOn("Android-Build")
    .Does(() =>
    {
        var buildOutputDirectory = MakeAbsolute(BuildParameters.AndroidProjectPath.GetDirectory().Combine("bin").Combine(BuildParameters.Configuration));

        Verbose("Build Output Directory: {0}", buildOutputDirectory);

        // CopyDirectory(buildOutputDirectory, MakeAbsolute(BuildParameters.Paths.Directories.DroidArtifactDirectoryPath));

        Verbose("Apk Path: {0}", buildOutputDirectory + "/*.apk");

        var files = GetFiles(buildOutputDirectory + "/*.apk")
                        .FirstOrDefault(x => x.GetFilename().ToString().ToLower().Contains("signed"));

        CopyFiles(files, MakeAbsolute(BuildParameters.Paths.Directories.DroidArtifactDirectoryPath));
    });