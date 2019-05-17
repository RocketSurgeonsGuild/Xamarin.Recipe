 BuildParameters.Tasks.DotNetRestoreTask = Task("DotNet-Restore")
    .WithCriteria(() => BuildParameters.ShouldUseDotNet)
    .Does(() => 
    { 
        Verbose("Restoring {0}...", BuildParameters.SolutionFilePath); 

        DotNetCoreRestore(BuildParameters.SolutionFilePath, ToolSettings.DotNetRestoreSettings());
    });

BuildParameters.Tasks.NuGetRestoreTask = Task("NuGet-Restore")
    .WithCriteria(() => !BuildParameters.ShouldUseDotNet)
    .Does(() => 
    {
        Verbose("Restoring {0}...", BuildParameters.SolutionFilePath);

        NuGetRestore(BuildParameters.SolutionFilePath, ToolSettings.NuGetRestoreSettings());
    }); 