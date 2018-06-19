#addin nuget:?package=Cake.Plist&version=0.4.0

BuildParameters.Tasks.iOSArchiveTask = Task("iOSArchive")
        .WithCriteria(BuildParameters.IsRunningOnUnix)
        .IsDependentOn("iPhone-Build")
    .Does(() =>
    {

    });

Task("iPhone-Build")
    .IsDependentOn("iPhone-Bundle")
    .WithCriteria(BuildParameters.IsRunningOnUnix)
    .Does(() =>
    {
        // MSBuild(BuildParameters.SolutionFilePath, configurator =>
        //             configurator
        //                 .SetConfiguration(BuildParameters.Configuration)
        //                 .SetPlatform(BuildParameters.Platform)
        //                 .SetVerbosity(ToolSettings.MSBuildVerbosity)
        //                 .UseToolVersion(MSBuildToolVersion.VS2017));
    
        XBuild(BuildParameters.SolutionFilePath, configurator =>
                    configurator
                        .SetConfiguration(BuildParameters.Configuration)
                        .SetVerbosity(Verbosity.Minimal)
                        .UseToolVersion(XBuildToolVersion.NET40));
    });

Task("iPhone-Bundle")
    .Does(() =>
    {
        dynamic plist = DeserializePlist(BuildParameters.PlistFilePath);

        plist["CFBundleShortVersionString"] = BuildParameters.Version.SemVersion;
        plist["CFBundleVersion"] = BuildParameters.Version.AssemblySemVer;
        plist["CFBundleIdentifier"] = Environment.BundleIdentifier;

        SerializePlist(BuildParameters.PlistFilePath, plist);
    });
