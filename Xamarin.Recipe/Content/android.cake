BuildParameters.Tasks.AndroidArchiveTask = Task("AndroidArchive")
    .IsDependentOn("Android-Build")
    .Does(() =>
    {

    });

    Task("Android-Build")
        .Does(() => 
        {
            // https://docs.microsoft.com/en-us/xamarin/android/deploy-test/building-apps/build-process
            // TODO: Android Package Signing
            // https://github.com/ghuntley/appstore-automation-with-fastlane/blob/master/build.cake#L142-L162
            MSBuild(BuildParameters.AndroidProjectPath, configurator =>
                        configurator
                            .SetConfiguration(BuildParameters.Configuration)
                            .SetVerbosity(ToolSettings.MSBuildVerbosity)
                            .UseToolVersion(MSBuildToolVersion.VS2017));
        });