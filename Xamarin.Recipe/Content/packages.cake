 BuildParameters.Tasks.DotNetCoreRestoreTask = Task("DotNet-Restore")
    .WithCriteria(() => BuildParameters.ShouldUseDotNet)
    .Does(() => 
    { 
        Verbose("Restoring {0}...", BuildParameters.SolutionFilePath); 

        DotNetCoreRestore(BuildParameters.SolutionFilePath.FullPath, ToolSettings.DotNetRestoreSettings());
    });

BuildParameters.Tasks.NuGetRestoreTask = Task("NuGet-Restore")
    .WithCriteria(() => !BuildParameters.ShouldUseDotNet)
    .Does(() => 
    {
        Verbose("Restoring {0}...", BuildParameters.SolutionFilePath);

        NuGetRestore(BuildParameters.SolutionFilePath, ToolSettings.NuGetRestoreSettings());
    });

BuildParameters.Tasks.AddNuGetPackageSourceTask =
    Task("Add-NuGet-Source")
        .IsDependentOn("Clean")
        .WithCriteria(() => BuildParameters.NuGetPackageSources.Any())
        .DoesForEach(BuildParameters.NuGetPackageSources, source =>
        {
            Verbose("Adding Package Source: {0} - {1}", source.Name, source.Source);
            NuGetAddSource(source.Name, source.Source, source.Settings);
        });

public class NuGetPackageSource
{
    public string Name { get; set; }
    
    public string Source { get; set; }

    public NuGetSourcesSettings Settings { get; set; }
}