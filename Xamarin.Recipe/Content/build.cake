///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(context =>
{    
    Information(Figlet(BuildParameters.Title));
    
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
        Information("NuGet Config FilePath: {0}", BuildParameters.NugetConfig);
        Information("Unit Test Whitelist", BuildParameters.UnitTestWhitelist);
        Information("UI Test Whitelist", BuildParameters.UITestWhitelist);
        Information("NuGet Config FilePath: {0}", BuildParameters.NugetConfig);
        Information("\n");
        Information("ShouldUseDotNet: {0}", BuildParameters.ShouldUseDotNet);
        Information("ShouldCopyImages: {0}", BuildParameters.ShouldCopyImages);
        Information("ShouldDeployAppCenter", BuildParameters.ShouldDeployAppCenter);
        Information("ShouldRunxUnit", BuildParameters.ShouldRunxUnit);
        Information("ShouldRunUnitTests: {0}", BuildParameters.ShouldRunUnitTests);
        Information("ShouldRunUITests: {0}", BuildParameters.ShouldRunUITests);
        Information("ShouldRunFastlaneDeliver", BuildParameters.ShouldRunFastlaneDeliver);
        Information("ShouldRunFastlaneMatch", BuildParameters.ShouldRunFastlaneMatch);
        Information("ShouldRunFastlanePilot: {0}", BuildParameters.ShouldRunFastlanePilot);
        Information("ShouldRunFastlaneSupply: {0}", BuildParameters.ShouldRunFastlaneSupply);
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
                                        .IsDependentOn("Add-NuGet-Source");

BuildParameters.Tasks.BuildTask = Task("Build").IsDependentOn("Restore");

BuildParameters.Tasks.TestTask = Task("Test")
                                    .IsDependentOn("Build")
                                    .IsDependentOn("Unit-Test")
                                    .IsDependentOn("UI-Test");

BuildParameters.Tasks.ArchiveTask = Task("Archive")
                                        .IsDependentOn("Test")
                                        .IsDependentOn("App-Icon-Copy");

BuildParameters.Tasks.DefaultTask = Task("Execute").IsDependentOn("Distribute");

public Builder Build => new Builder(target => RunTarget(target));

public class Builder
{
    private Action<string> _action;

    public Builder(Action<string> action)
    {
        _action = action;
    }

    public void RuniOS(bool isNetCoreBuild = true)
    {
        SetupiOS(isNetCoreBuild);
        
        _action(BuildParameters.Target);
    }

    public void RunAndroid(bool isNetCoreBuild = true)
    {
        SetupAndroid(isNetCoreBuild);

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
                Setup();
            break;
        }
    }

    private static void SetupiOS(bool isNetCoreBuild = true)
    {
        Setup();
        
        BuildParameters.IsiOSBuild = true;
        BuildParameters.IsDotNetCoreBuild = isNetCoreBuild;
        
        BuildParameters.Tasks.BuildTask.IsDependentOn("iPhone-Build");
        BuildParameters.Tasks.ArchiveTask.IsDependentOn("iOS-Archive");
        BuildParameters.Tasks.AppCenterTask.IsDependentOn("iPhone-AppCenter");
        BuildParameters.Tasks.UploadAzureDevOpsArtifactsTask.IsDependentOn("Upload-AzureDevOps-Ipa");
        BuildParameters.Tasks.FastlaneTask.IsDependentOn("Fastlane-Deliver");
    }

    private static void SetupAndroid(bool isNetCoreBuild = true)
    {
        Setup();
        BuildParameters.IsAndroidBuild = true;
        BuildParameters.IsDotNetCoreBuild = isNetCoreBuild;
        
        BuildParameters.Tasks.BuildTask.IsDependentOn("Android-Build");
        BuildParameters.Tasks.ArchiveTask.IsDependentOn("Android-Archive");
        BuildParameters.Tasks.AppCenterTask.IsDependentOn("Android-AppCenter");
        BuildParameters.Tasks.UploadAzureDevOpsArtifactsTask.IsDependentOn("Upload-AzureDevOps-Apk");
    }

    private static void SetupUWP()
    {
        
    }

    private static void Setup()
    {
        if(BuildParameters.ShouldRunxUnit)
        {
            BuildParameters.Tasks.TestTask.IsDependentOn("xUnit-Tests");
        }
        
        if (BuildParameters.ShouldUseDotNet)
        {
            BuildParameters.Tasks.RestoreTask.IsDependentOn("DotNet-Restore");
        }
        else
        {
            BuildParameters.Tasks.RestoreTask.IsDependentOn("NuGet-Restore");
        }
    }
}