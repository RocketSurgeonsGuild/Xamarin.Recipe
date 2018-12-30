#addin nuget:?package=Cake.Plist&version=0.4.0

BuildParameters.Tasks.iOSArchiveTask = Task("iOS-Archive")
    .WithCriteria(() => BuildParameters.IsRunningOnUnix)
    .IsDependentOn("iPhone-Build")
    .IsDependentOn("Copy-Ipa");

Task("iPhone-Build")
    .IsDependentOn("iPhone-Info-Plist")
    .IsDependentOn("Fastlane-Match")
    .WithCriteria(() => BuildParameters.IsRunningOnUnix)
    .Does(() =>
    {
        Verbose("Build Configuration: {0}", BuildParameters.Configuration);
        Verbose("MSBuild Verbosity: {0}", ToolSettings.MSBuildVerbosity);
        Verbose("MSBuild Tool Version: {0}", ToolSettings.MSBuildToolVersion);
        Verbose("Build Platform: {0}", BuildParameters.Platform);

        MSBuild(BuildParameters.SolutionFilePath, configurator =>
                    configurator
                        .SetConfiguration(BuildParameters.Configuration)
                        .SetVerbosity(ToolSettings.MSBuildVerbosity)
                        .UseToolVersion(ToolSettings.MSBuildToolVersion)
                        .WithProperty("Platform", BuildParameters.Platform)
                        .WithProperty("BuildIpa", "true"));
    });

Task("iPhone-XBuild")
    .Does(() =>
    {
        XBuild(BuildParameters.SolutionFilePath, configurator =>
                    configurator
                        .SetConfiguration(BuildParameters.Configuration)
                        .SetVerbosity(Verbosity.Minimal)
                        .UseToolVersion(XBuildToolVersion.NET40)
                        .WithProperty("Platform", BuildParameters.Platform));
    });

Task("iPhone-Info-Plist")
    .WithCriteria(() => !string.IsNullOrEmpty(BuildParameters.PlistFilePath.FullPath))
    .Does(() =>
    {
        dynamic plist = DeserializePlist(BuildParameters.PlistFilePath);
        
        Verbose("CFBundleShortVersionString: {0}", BuildParameters.Version.Version);
        Verbose("CFBundleVersion: {0}", BuildParameters.Version.PreReleaseNumber);
        plist["CFBundleShortVersionString"] = BuildParameters.Version.Version;
        plist["CFBundleVersion"] = BuildParameters.Version.BuildMetaData;

        var bundleIdentifier = EnvironmentVariable(Environment.BundleIdentifierVariable);
        if(!string.IsNullOrEmpty(bundleIdentifier))
        {
            Verbose("CFBundleIdentifier: {0}", bundleIdentifier);
            plist["CFBundleIdentifier"] = bundleIdentifier;
        }

        SerializePlist(BuildParameters.PlistFilePath, plist);
    });

Task("iPhone-AppCenter")
    .WithCriteria(() => BuildParameters.ShouldDeployAppCenter)
    .IsDependentOn("iOS-Archive")
    .Does(() =>
    {
        var iosArtifactsPath = MakeAbsolute(BuildParameters.Paths.Directories.IOSArtifactDirectoryPath);
        Verbose("iOS Artifact Directory: {0}", iosArtifactsPath.FullPath);

        var ipaPath = GetFiles(iosArtifactsPath.FullPath + "/*.ipa");

        var ipa = ipaPath.FirstOrDefault()?.FullPath;

        Verbose("IPA: {0}", ipa);

        var settings = new AppCenterDistributeReleaseSettings
        {
            File = ipaPath.FirstOrDefault()?.FullPath,
            Token = EnvironmentVariable(Environment.AppCenterTokenVariable),
            App = $"{EnvironmentVariable(Environment.AppCenterOwnerVariable)}/{EnvironmentVariable(Environment.AppCenterAppNameVariable)}",
            Group = EnvironmentVariable(Environment.AppCenterGroupVariable)
        };

        AppCenterDistributeRelease(settings);
    });

Task("Copy-Ipa")
    .IsDependentOn("iPhone-Build")
    .Does(() =>
    {
        var buildOutputDirectory = MakeAbsolute(BuildParameters.IOSProjectPath.GetDirectory().Combine("bin").Combine(BuildParameters.Platform).Combine(BuildParameters.Configuration));

        Verbose("Build Output Directory: {0}", buildOutputDirectory);

        var artifactDirectoryPath = MakeAbsolute(BuildParameters.Paths.Directories.IOSArtifactDirectoryPath);

        Verbose("IOS Artifact Directory: {0}", artifactDirectoryPath);

        var files = GetFiles(buildOutputDirectory + "/*.ipa");

        CopyFiles(files,  artifactDirectoryPath);
    });

Task("Upload-AzureDevOps-Ipa")
    .Does(() =>
    {
        var artifactPath = BuildParameters.Paths.Directories.IOSArtifactDirectoryPath;

        var ipa = GetFiles(MakeAbsolute(artifactPath).FullPath + "/*.ipa").FirstOrDefault();

        Verbose("Ipa Path: {0}", ipa.FullPath);

        BuildSystem.TFBuild.Commands.UploadArtifact(artifactPath.ToString(), ipa, ipa.GetFilename().ToString());
    });