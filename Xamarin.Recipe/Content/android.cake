BuildParameters.Tasks.AndroidArchiveTask = Task("Android-Archive")
    .IsDependentOn("Android-Build")
    .IsDependentOn("Copy-Apk");

Task("Android-Build")
    .IsDependentOn("Android-Manifest")
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
        
        Verbose("Build Configuration: {0}", BuildParameters.Configuration);
        Verbose("MSBuild Verbosity: {0}", ToolSettings.MSBuildVerbosity);
        Verbose("MSBuild Tool Version: {0}", ToolSettings.MSBuildToolVersion);
        Verbose("AndroidSdkBuildToolsVersion: {0}", ToolSettings.AndroidBuildToolVersion);
        Verbose("Build Platform: {0}", BuildParameters.Platform);

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

Task("Android-Manifest")
    .WithCriteria(() => !string.IsNullOrEmpty(BuildParameters.AndroidManifest.FullPath))
    .Does(() =>
    {
        var manifest = DeserializeAppManifest(BuildParameters.AndroidManifest);

        manifest.VersionName = BuildParameters.Version.Version;
        manifest.VersionCode = BuildParameters.BuildNumber == 0 ? manifest.VersionCode ++ : BuildParameters.BuildNumber;

        Verbose("Version Name: {0}", BuildParameters.Version.Version);
        Verbose("Version Code: {0}", BuildParameters.Version.PreReleaseNumber);

        var bundleIdentifier = EnvironmentVariable(Environment.BundleIdentifierVariable);
        if(!string.IsNullOrEmpty(bundleIdentifier))
        {
            Verbose("Package Name: {0}", bundleIdentifier);
            manifest.PackageName = bundleIdentifier;
        }

        SerializeAppManifest(BuildParameters.AndroidManifest, manifest);
    });

Task("Android-AppCenter")
    .WithCriteria(() => BuildParameters.ShouldDeployAppCenter)
    .IsDependentOn("Android-Archive")
    .Does(() =>
    {
        var androidArtifactsPath = MakeAbsolute(BuildParameters.Paths.Directories.DroidArtifactDirectoryPath);
        Verbose("Android Artifact Directory: {0}", androidArtifactsPath.FullPath);

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

        Verbose("Apk Path: {0}", buildOutputDirectory + "/*.apk");

        var files = GetFiles(buildOutputDirectory + "/*.apk")
                        .Where(x => x.GetFilename().ToString().ToLower().Contains("signed"));

        CopyFiles(files, MakeAbsolute(BuildParameters.Paths.Directories.DroidArtifactDirectoryPath).FullPath);
    });

Task("Upload-AzureDevOps-Apk")
    .Does(() =>
    {
        var artifactPath = BuildParameters.Paths.Directories.DroidArtifactDirectoryPath;
        var fullArtifactPath = MakeAbsolute(artifactPath).FullPath;

        var apk = GetFiles(fullArtifactPath + "/*.apk")
                    .FirstOrDefault(x => x.GetFilename().ToString().ToLower().Contains("signed"));
        
        Verbose("Apk Path: {0}", apk.FullPath);
        
        BuildSystem.TFBuild.Commands.UploadArtifact(artifactPath.ToString(), apk, apk.GetFilename().ToString());
    });
