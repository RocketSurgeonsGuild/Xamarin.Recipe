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

    Information("\n");
    Warning("Building version {0} of " + BuildParameters.Title + " ({1}, {2}) using version {3} of Cake. (IsTagged: {4})",
        BuildParameters.Version.SemVersion,
        BuildParameters.Configuration,
        BuildParameters.Target,
        BuildParameters.Version.CakeVersion,
        BuildParameters.IsTagged);
});

Teardown(context =>
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
        Information("Application Target: {0}", BuildParameters.ApplicationTarget);
        Information("Configuration: {0}", BuildParameters.Configuration);
        Information("IsLocalBuild: {0}", BuildParameters.IsLocalBuild);
        Information("IsRunningOnAzureDevOps: {0}", BuildParameters.IsRunningOnAzureDevOps);
        Information("\n");
        Information("IsDevBranch: {0}", BuildParameters.IsDevBranch);
        Information("IsMainBranch: {0}", BuildParameters.IsMainBranch);
        Information("IsFeatureBranch: {0}", BuildParameters.IsFeatureBranch);
        Information("IsReleaseBranch: {0}", BuildParameters.IsReleaseBranch);
        Information("IsHotFixBranch: {0}", BuildParameters.IsHotFixBranch);
        Information("IsPullRequest: {0}", BuildParameters.IsPullRequest);
        Information("IsTagged: {0}", BuildParameters.IsTagged);
        Information("IsMainRepository: {0}", BuildParameters.IsMainRepository);
        Information("\n");
        Information("Solution FilePath: {0}", MakeAbsolute((FilePath)BuildParameters.SolutionFilePath));
        Information("Solution DirectoryPath: {0}", MakeAbsolute((DirectoryPath)BuildParameters.SolutionDirectoryPath));
        Information("Source DirectoryPath: {0}", MakeAbsolute(BuildParameters.SourceDirectoryPath));
        Information("Build DirectoryPath: {0}", MakeAbsolute(BuildParameters.Paths.Directories.Build));
        Information("Test DirectoryPath: {0}", MakeAbsolute(BuildParameters.TestDirectoryPath));
        Information("Unit Test Glob Pattern: {0}", BuildParameters.UnitTestFilePattern);
        Information("UI Test Glob Pattern: {0}", BuildParameters.UITestFilePattern);
    });

    BuildParameters.Tasks.CleanTask = Task("Clean")
    .IsDependentOn("Show-Info")
    .IsDependentOn("Print-AzureDevOps-Environment-Variables")
    .Does(() =>
    {
        Information("Cleaning...");

        CleanDirectories(BuildParameters.Paths.Directories.ToClean);
    });

    BuildParameters.Tasks.RestoreTask = Task("Restore")
    .IsDependentOn("Clean")
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

BuildParameters.Tasks.BuildTask = Task("Build").IsDependentOn("Restore");

BuildParameters.Tasks.TestTask = Task("Test")
                                    .IsDependentOn("Build")
                                    .IsDependentOn("Unit-Test")
                                    .IsDependentOn("UI-Test");

BuildParameters.Tasks.ArchiveTask = Task("Archive").IsDependentOn("Test").IsDependentOn("Image-Copy");

BuildParameters.Tasks.DefaultTask = Task("Execute").IsDependentOn("Distribute");

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

        SetupTasks(BuildParameters.ApplicationTarget);

        _action(BuildParameters.Target);
    }

    public void RunDotNetCore()
    {
        BuildParameters.IsDotNetCoreBuild = true;
        BuildParameters.IsNuGetBuild = false;

        SetupTasks(BuildParameters.ApplicationTarget);

        _action(BuildParameters.Target);
    }

    public void RuniOS(bool isNetCoreBuild = true, bool isNuGetBuild = false)
    {
        BuildParameters.IsDotNetCoreBuild = isNetCoreBuild;
        BuildParameters.IsNuGetBuild = isNuGetBuild;
        
        SetupiOS();
        
        _action(BuildParameters.Target);
    }

    public void RunAndroid(bool isNetCoreBuild = true, bool isNuGetBuild = false)
    {
        BuildParameters.IsDotNetCoreBuild = isNetCoreBuild;
        BuildParameters.IsNuGetBuild = isNuGetBuild;
        
        SetupAndroid();

        _action(BuildParameters.Target);
    }

    private static void SetupTasks(ApplicationTarget target)
    {
        switch (target)
        {
            case ApplicationTarget.iOS:
                    SetupiOS();
                break;
            case ApplicationTarget.Android:
                    SetupAndroid();
                break;
            case ApplicationTarget.UWP:
                    SetupUWP();
                break;            
            default:
            break;
        }
    }

    private static void SetupiOS()
    {        
        BuildParameters.Tasks.BuildTask.IsDependentOn("iPhone-Build");
        BuildParameters.Tasks.ArchiveTask.IsDependentOn("iOS-Archive");
        BuildParameters.Tasks.AppCenterTask.IsDependentOn("iPhone-AppCenter");
        BuildParameters.Tasks.UploadAzureDevOpsArtifactsTask.IsDependentOn("Upload-AzureDevOps-Ipa");
    }

    private static void SetupAndroid()
    {
        BuildParameters.Tasks.BuildTask.IsDependentOn("Android-Build");
        BuildParameters.Tasks.ArchiveTask.IsDependentOn("Android-Archive");
        BuildParameters.Tasks.AppCenterTask.IsDependentOn("Android-AppCenter");
        BuildParameters.Tasks.UploadAzureDevOpsArtifactsTask.IsDependentOn("Upload-AzureDevOps-Apk");
    }

    private static void SetupUWP()
    {
        
    }
}