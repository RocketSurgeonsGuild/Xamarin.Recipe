BuildParameters.Tasks.AndroidArchiveTask = Task("AndroidArchive")
    .IsDependentOn("Android-Build")
    .Does(() =>
    {

    });

    Task("Android-Build")
        .Does(() => 
        {
            MSBuild(BuildParameters.AndroidProjectPath, configurator =>
                        configurator
                            .SetConfiguration(BuildParameters.Configuration)
                            .SetVerbosity(ToolSettings.MSBuildVerbosity)
                            .UseToolVersion(MSBuildToolVersion.VS2017));
        });