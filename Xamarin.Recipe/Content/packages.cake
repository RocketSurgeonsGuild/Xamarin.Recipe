 BuildParameters.Tasks.DotNetRestoreTask = Task("DotNet-Restore")
    .WithCriteria(() => BuildParameters.UseDotNet)
    .Does(() => 
    { 
        Information("Restoring {0}...", BuildParameters.SolutionFilePath); 

        DotNetCoreRestore(BuildParameters.SolutionFilePath, ToolSettings.DotNetRestoreSettings());
    });

BuildParameters.Tasks.NuGetRestoreTask = Task("NuGet-Restore")
    .WithCriteria(() => !BuildParameters.UseDotNet)
    .Does(() => 
    { 
        Information("Restoring {0}...", BuildParameters.SolutionFilePath);

        NuGetRestore(BuildParameters.SolutionFilePath, ToolSettings.NuGetRestoreSettings());
    }); 