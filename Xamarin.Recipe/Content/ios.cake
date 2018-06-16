BuildParameters.Tasks.iOSArchiveTask = Task("iOSArchive")
        .WithCriteria(BuildParameters.IsRunningOnUnix)
        .IsDependentOn("iPhone-Build")
    .Does(() =>
    {

    });

    Task("iPhone-Build")
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
                            .SetConfiguration("Debug")
                            .SetVerbosity(Verbosity.Minimal)
                            .UseToolVersion(XBuildToolVersion.NET40));
        });