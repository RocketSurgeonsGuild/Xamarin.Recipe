///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(context =>
{
   // Executed BEFORE the first task.
   Information("Running tasks...");
   
    RequireTool(GitVersionTool, () => {
        BuildParameters.SetBuildVersion(
            BuildVersion.CalculatingSemanticVersion(
                context: context
            )
        );
    });

    Information("Building version {0} of " + BuildParameters.Title + " ({1}, {2}) using version {3} of Cake. (IsTagged: {4})",
        BuildParameters.Version.SemVersion,
        BuildParameters.Configuration,
        BuildParameters.Target,
        BuildParameters.Version.CakeVersion,
        BuildParameters.IsTagged);
});

Teardown(ctx =>
{
   // Executed AFTER the last task.
   Information("Finished running tasks.");
});

///////////////////////////////////////////////////////////////////////////////
// TASK DEFINITIONS
///////////////////////////////////////////////////////////////////////////////

BuildParameters.Tasks.ShowInfoTask = Task("Show-Info")
    .Does(() =>
    {
        Information("Target: {0}", BuildParameters.Target);
        Information("Configuration: {0}", BuildParameters.Configuration);
        Information("IsLocalBuild: {0}", BuildParameters.IsLocalBuild);
        Information("IsPullRequest: {0}", BuildParameters.IsPullRequest);
        Information("IsMainRepository: {0}", BuildParameters.IsMainRepository);
        Information("IsMasterBranch: {0}", BuildParameters.IsMasterBranch);
        Information("IsFeatureBranch: {0}", BuildParameters.IsFeatureBranch);
        Information("IsReleaseBranch: {0}", BuildParameters.IsReleaseBranch);
        Information("IsHotFixBranch: {0}", BuildParameters.IsHotFixBranch);
        Information("IsTagged: {0}", BuildParameters.IsTagged);

        Information("Solution FilePath: {0}", MakeAbsolute((FilePath)BuildParameters.SolutionFilePath));
        Information("Solution DirectoryPath: {0}", MakeAbsolute((DirectoryPath)BuildParameters.SolutionDirectoryPath));
        Information("Source DirectoryPath: {0}", MakeAbsolute(BuildParameters.SourceDirectoryPath));
        Information("Build DirectoryPath: {0}", MakeAbsolute(BuildParameters.Paths.Directories.Build));
    });

    BuildParameters.Tasks.CleanTask = Task("Clean")
    .IsDependentOn("Show-Info")
    .Does(() =>
    {
        Information("Cleaning...");

        CleanDirectories(BuildParameters.Paths.Directories.ToClean);
    });

    BuildParameters.Tasks.RestoreTask = Task("Restore")
    .Does(() =>
    {
        Information("Restoring {0}...", BuildParameters.SolutionFilePath);

        NuGetRestore(
            BuildParameters.SolutionFilePath,
            new NuGetRestoreSettings
            {
                Source = BuildParameters.NuGetSources
            });
    });

BuildParameters.Tasks.BuildTask = Task("Build");

BuildParameters.Tasks.TestTask = Task("Test");

BuildParameters.Tasks.DefaultTask = Task("Execute");

public Builder Build
{
    get
    {
        return new Builder(target => RunTarget(target));
    }
}

public class Builder
{
    private Action<string> _action;

    public Builder(Action<string> action)
    {
        _action = action;
    }

    public void Run()
    {
        BuildParameters.IsDotNetCoreBuild = false;
        BuildParameters.IsNuGetBuild = false;

        SetupTasks(BuildParameters.IsDotNetCoreBuild);

        _action(BuildParameters.Target);
    }

    public void RunDotNetCore()
    {
        BuildParameters.IsDotNetCoreBuild = true;
        BuildParameters.IsNuGetBuild = false;

        SetupTasks(BuildParameters.IsDotNetCoreBuild);

        _action(BuildParameters.Target);
    }

    public void RunNuGet()
    {
        BuildParameters.Tasks.PackageTask.IsDependentOn("Create-NuGet-Package");
        BuildParameters.IsDotNetCoreBuild = false;
        BuildParameters.IsNuGetBuild = true;

        _action(BuildParameters.Target);
    }

    private static void SetupTasks(bool isDotNetCoreBuild)
    {
        //TODO: Only set .NET Core dependencies here.
        // Move the remaining IsDependentOn calls to the Task assignment.
                
        BuildParameters.Tasks.RestoreTask.IsDependentOn("Clean");
        BuildParameters.Tasks.BuildTask.IsDependentOn("Restore").IsDependentOn("MSBuild");
        BuildParameters.Tasks.ArchiveTask.IsDependentOn("iOSArchive").IsDependentOn("AndroidArchive");
        BuildParameters.Tasks.AppCenterTask.IsDependentOn("Archive").IsDependentOn("Distribute");
        BuildParameters.Tasks.DefaultTask.IsDependentOn("AppCenter");
    }
}