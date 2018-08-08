#addin nuget:?package=Cake.Plist&version=0.4.0

BuildParameters.Tasks.iOSArchiveTask = Task("iOSArchive")
        .WithCriteria(() => BuildParameters.IsRunningOnUnix)
        .IsDependentOn("iPhone-Build")
    .Does(() =>
    {

    });

Task("iPhone-Build")
    .IsDependentOn("iPhone-Bundle")
    .WithCriteria(() => IsRunningOnUnix())
    .Does(() =>
    {
        MSBuild(BuildParameters.SolutionFilePath, configurator =>
                    configurator
                        .SetConfiguration(BuildParameters.Configuration)
                        .SetVerbosity(ToolSettings.MSBuildVerbosity)
                        .UseToolVersion(MSBuildToolVersion.VS2017)
                        .WithProperty("Platform", BuildParameters.Platform)
                        .WithProperty("BuildIpa", "true"));
    
        // XBuild(BuildParameters.SolutionFilePath, configurator =>
        //             configurator
        //                 .SetConfiguration(BuildParameters.Configuration)
        //                 .SetVerbosity(Verbosity.Minimal)
        //                 .UseToolVersion(XBuildToolVersion.NET40));
    });

Task("iPhone-Bundle")
    .WithCriteria(() => !string.IsNullOrEmpty(BuildParameters.PlistFilePath.FullPath))
    .Does(() =>
    {
        var bundleIdentifier = EnvironmentVariable(Environment.BundleIdentifierVariable);

        dynamic plist = DeserializePlist(BuildParameters.PlistFilePath);
        
        plist["CFBundleShortVersionString"] = BuildParameters.Version.Version;
        plist["CFBundleVersion"] = BuildParameters.Version.PreReleaseNumber;

        if(!string.IsNullOrEmpty(bundleIdentifier))
        {
            plist["CFBundleIdentifier"] = bundleIdentifier;
        }

        SerializePlist(BuildParameters.PlistFilePath, plist);
    });
