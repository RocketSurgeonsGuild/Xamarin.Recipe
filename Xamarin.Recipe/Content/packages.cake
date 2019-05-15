 BuildParameters.Tasks.DotNetRestoreTask = Task("DotNet-Restore")
    .WithCriteria(() => BuildParameters.UseDotNet)
    .Does(() => 
    { 
        Information("Restoring {0}...", BuildParameters.SolutionFilePath); 

        DotNetCoreRestore(BuildParameters.SolutionFilePath, ToolSettings.DotNetRestoreSettings());
    }); 