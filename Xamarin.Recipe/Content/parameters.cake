public static class BuildParameters
{
    public static string Target { get; private set; }
    public static ApplicationTarget ApplicationTarget { get; private set; }
    public static string Configuration { get; private set; }
    public static string Title {get; private set;}
    public static Cake.Core.Configuration.ICakeConfiguration CakeConfiguration { get; private set; }
    public static bool IsLocalBuild { get; private set; }
    public static bool IsRunningOnUnix { get; private set; }
    public static bool IsRunningOnWindows { get; private set; }
    public static bool IsRunningOnAppVeyor { get; private set; }
    public static bool IsRunningOnAzureDevOps {get; private set; }
    public static bool IsPullRequest { get; private set; }
    public static bool IsMainRepository { get; private set; }
    public static bool IsPublicRepository {get; private set; }
    public static bool IsMainBranch { get; private set; }
    public static bool IsDevBranch { get; private set; }
    public static bool IsFeatureBranch { get; private set; }
    public static bool IsReleaseBranch { get; private set; }
    public static bool IsHotFixBranch { get; private set ; }
    public static bool IsTagged { get; private set; }
    public static bool IsPublishBuild { get; private set; }
    public static bool IsReleaseBuild { get; private set; }
    public static bool IsDotNetCoreBuild { get; set; }  
    public static bool IsiOSBuild { get; set; }  
    public static bool IsAndroidBuild { get; set; }
    public static bool IsNuGetBuild { get; set; }
    public static bool TransifexEnabled { get; set; }
    public static bool PrepareLocalRelease { get; set; }

    public static int BuildNumber { get; private set; }
    
    public static bool ShouldUseDotNet { get; private set; }
    
    public static bool ShouldRunGitVersion { get; private set; }
    public static bool ShouldDeployAppCenter { get; private set; }
    public static bool ShouldRunFastlaneDeliver { get; private set; }
    public static bool ShouldRunFastlaneMatch { get; private set; }
    public static bool ShouldRunFastlanePilot { get; private set; }
    public static bool ShouldRunFastlaneSupply { get; private set; }
    public static bool ShouldCopyImages { get; private set; }
    public static bool ShouldRunxUnit { get; private set; }
    public static bool ShouldRunUnitTests { get; private set; }
    public static bool ShouldRunUITests { get; private set; }

    public static BuildVersion Version { get; private set; }
    public static BuildPaths Paths { get; private set; }
    public static BuildTasks Tasks { get; set; }
    public static DirectoryPath RootDirectoryPath { get; private set; }
    public static FilePath SolutionFilePath { get; private set; }
    public static DirectoryPath SourceDirectoryPath { get; private set; }
    public static DirectoryPath SolutionDirectoryPath { get; private set; }
    public static FilePath AndroidProjectPath { get; private set; }
    public static FilePath AndroidManifest { get; private set; }
    public static FilePath IOSProjectPath { get; private set; }
    public static FilePath PlistFilePath { get; private set; }
    public static string Platform { get; private set; }
    public static DirectoryPath TestDirectoryPath { get; private set; }
    public static FilePath IntegrationTestScriptPath { get; private set; }
    public static string ResharperSettingsFileName { get; private set; }
    public static string RepositoryOwner { get; private set; }
    public static string RepositoryName { get; private set; }
    public static FilePath NugetConfig { get; private set; }
    public static ICollection<string> NuGetSources { get; private set; }
    public static ICollection<NuGetPackageSource> NuGetPackageSources { get; private set; }
    public static ICollection<FilePath> UnitTestWhitelist { get; private set; }
    public static ICollection<FilePath> UITestWhitelist { get; private set; }

    static BuildParameters()
    {
        Tasks = new BuildTasks();
    }

    public static bool CanDistribute => !string.IsNullOrEmpty(Environment.AppCenterTokenVariable);

    public static void SetParameters(
        ICakeContext context,
        BuildSystem buildSystem,
        DirectoryPath sourceDirectoryPath,
        string title,
        FilePath solutionFilePath = null,
        DirectoryPath solutionDirectoryPath = null,
        FilePath androidProjectPath = null,
        FilePath iosProjectPath = null,
        FilePath plistFilePath = null,
        string platform = "iPhone",
        DirectoryPath rootDirectoryPath = null,
        DirectoryPath testDirectoryPath = null,
        string integrationTestScriptPath = null,
        string resharperSettingsFileName = null,
        string repositoryOwner = null,
        string repositoryName = null,
        string appVeyorAccountName = null,
        string appVeyorProjectSlug = null,
        bool isPublicRepository = false,
        bool shouldUseDotNet = true,
        bool? shouldRunGitVersion = true,
        bool shouldDeployAppCenter = false,
        bool shouldCopyImages = false,
        bool? shouldRunxUnit = null,
        bool? shouldRunUnitTests = null,
        bool? shouldRunUITests = null,
        bool shouldRunFastlaneMatch = false,
        int buildNumber = 0,
        string mainBranch = "main",
        string devBranch = "dev",
        FilePath androidManifest = null,
        FilePath nugetConfig = null,
        ICollection<string> nuGetSources = null,
        ICollection<NuGetPackageSource> nuGetPackageSources = null,
        ICollection<FilePath> unitTestWhitelist = null,
        ICollection<FilePath> uiTestWhitelist = null)
    {
        if (context == null)
        {
            throw new ArgumentNullException("context");
        }

        SourceDirectoryPath = sourceDirectoryPath;
        Title = title;
        SolutionFilePath = solutionFilePath ?? SourceDirectoryPath.CombineWithFilePath(Title + ".sln");
        SolutionDirectoryPath = solutionDirectoryPath ?? SourceDirectoryPath.Combine(Title);
        RootDirectoryPath = rootDirectoryPath ?? context.MakeAbsolute(context.Environment.WorkingDirectory);
        AndroidProjectPath = androidProjectPath;
        AndroidManifest = androidManifest;
        IOSProjectPath = iosProjectPath;
        PlistFilePath = plistFilePath;
        TestDirectoryPath = testDirectoryPath ?? sourceDirectoryPath;
        IntegrationTestScriptPath = integrationTestScriptPath ?? context.MakeAbsolute((FilePath)"test.cake");
        ResharperSettingsFileName = resharperSettingsFileName ?? string.Format("{0}.sln.DotSettings", Title);
        RepositoryOwner = repositoryOwner ?? string.Empty;
        RepositoryName = repositoryName ?? Title;

        Target = context.Argument("target", "Default");
        ApplicationTarget = context.Argument("application", ApplicationTarget.All);
        Configuration = context.Argument("configuration", "Release");
        PrepareLocalRelease = context.Argument("prepareLocalRelease", false);
        Platform = platform;
        Title = title;
        CakeConfiguration = context.GetConfiguration();
        IsLocalBuild = buildSystem.IsLocalBuild;
        IsRunningOnUnix = context.IsRunningOnUnix();
        IsRunningOnWindows = context.IsRunningOnWindows();
        IsRunningOnAppVeyor = buildSystem.AppVeyor.IsRunningOnAppVeyor;
        IsRunningOnAzureDevOps = buildSystem.TFBuild.IsRunningOnTFS || buildSystem.TFBuild.IsRunningOnVSTS;
        IsPullRequest = !string.IsNullOrEmpty(context.Environment.GetEnvironmentVariable("SYSTEM_PULLREQUEST_PULLREQUESTID"));
        IsMainRepository = StringComparer.OrdinalIgnoreCase.Equals(string.Concat(repositoryOwner, "/", repositoryName), buildSystem.TFBuild.Environment.Repository.RepoName);
        IsPublicRepository = isPublicRepository;
        IsMainBranch = StringComparer.OrdinalIgnoreCase.Equals(mainBranch, buildSystem.TFBuild.Environment.Repository.Branch);
        IsDevBranch = StringComparer.OrdinalIgnoreCase.Equals(devBranch, buildSystem.TFBuild.Environment.Repository.Branch);
        IsFeatureBranch = buildSystem.TFBuild.Environment.Repository.Branch.StartsWith("feature", StringComparison.OrdinalIgnoreCase);
        IsReleaseBranch = buildSystem.TFBuild.Environment.Repository.Branch.StartsWith("release", StringComparison.OrdinalIgnoreCase);
        IsHotFixBranch = buildSystem.TFBuild.Environment.Repository.Branch.StartsWith("hotfix", StringComparison.OrdinalIgnoreCase);
        IsTagged = (!string.IsNullOrWhiteSpace(context.Environment.GetEnvironmentVariable("$git_tag")));

        AndroidManifest = androidManifest;

        NugetConfig = nugetConfig;
        NuGetSources = GetNuGetSources(context, nuGetSources);
        NuGetPackageSources = nuGetPackageSources ?? Array.Empty<NuGetPackageSource>();
        UnitTestWhitelist = unitTestWhitelist ?? Array.Empty<FilePath>();
        UITestWhitelist = uiTestWhitelist ?? Array.Empty<FilePath>();

        IsDotNetCoreBuild = true;

        ShouldUseDotNet = shouldUseDotNet;

        ShouldRunGitVersion = shouldRunGitVersion ?? IsRunningOnUnix;
        ShouldCopyImages = shouldCopyImages;

		SetBuildPaths(BuildPaths.GetPaths(context));

        ShouldDeployAppCenter = (((!IsLocalBuild && !IsPullRequest && (IsMainBranch || IsReleaseBranch || IsDevBranch || IsHotFixBranch || IsTagged)) &&
                                ((context.EnvironmentVariable(Environment.AppCenterTokenVariable) != null) &&
                                    (context.EnvironmentVariable(Environment.AppCenterAppNameVariable) != null) &&
                                    (context.EnvironmentVariable(Environment.AppCenterGroupVariable) != null) &&
                                    (context.EnvironmentVariable(Environment.AppCenterOwnerVariable) != null))) ||
                                shouldDeployAppCenter);

        ShouldRunxUnit = shouldRunxUnit ?? !IsDotNetCoreBuild;

        ShouldRunUnitTests = shouldRunUnitTests ?? unitTestWhitelist.Any();

        ShouldRunUITests = shouldRunUITests ?? uiTestWhitelist.Any();

        ShouldRunFastlaneDeliver = context.DirectoryExists(BuildParameters.Paths.Directories.Metadata) && (IsReleaseBranch || IsHotFixBranch || (IsMainBranch && IsTagged));

        ShouldRunFastlaneMatch = IsiOSBuild && IsRunningOnUnix && shouldRunFastlaneMatch;

        ShouldRunFastlanePilot = IsiOSBuild && (IsReleaseBranch || IsHotFixBranch || (IsMainBranch && IsTagged));

        ShouldRunFastlaneSupply = IsAndroidBuild && (IsReleaseBranch || IsHotFixBranch || (IsMainBranch && IsTagged));

        BuildNumber = buildNumber;
    }

    public static void SetBuildVersion(BuildVersion version)
    {
        Version  = version;
    }

    public static void SetBuildNumber(int buildNumber)
    {
        BuildNumber  = buildNumber;
    }

    public static void SetBuildPaths(BuildPaths paths)
    {
        Paths = paths;
    }

    public static void PrintParameters(ICakeContext context)
    {
        context.Information("============ PARAMETERS ============");
        context.Information("Target: {0}", Target);
        context.Information("ApplicationTarget: {0}", ApplicationTarget);
        context.Information("Platform: {0}", Platform);
        context.Information("\n");

        context.Information("IsLocalBuild: {0}", IsLocalBuild);
        context.Information("IsRunningOnAppVeyor: {0}", IsRunningOnAppVeyor);
        context.Information("IsRunningOnAzureDevOps: {0}", IsRunningOnAzureDevOps);
        context.Information("BuildNumber: {0}", BuildNumber);
        context.Information("\n");

        context.Information("IsDevBranch: {0}", IsDevBranch);
        context.Information("IsMainBranch: {0}", IsMainBranch);
        context.Information("IsFeatureBranch: {0}", IsFeatureBranch);
        context.Information("IsReleaseBranch: {0}", IsReleaseBranch);
        context.Information("IsHotFixBranch: {0}", IsHotFixBranch);
        context.Information("IsPullRequest: {0}", IsPullRequest);
        context.Information("IsTagged: {0}", IsTagged);
        context.Information("IsMainRepository: {0}", IsMainRepository);
        context.Information("IsPublicRepository: {0}", IsPublicRepository);
        context.Information("\n");

        context.Information("IsAndroidBuild: {0}", IsAndroidBuild);
        context.Information("AndroidManifest: {0}", AndroidManifest);
        context.Information("AndroidProjectPath: {0}", AndroidProjectPath);
        context.Information("\n");

        context.Information("IsiOSBuild: {0}", IsiOSBuild);
        context.Information("InfoPlist: {0}", PlistFilePath);
        context.Information("IOSProjectPath: {0}", IOSProjectPath);
        context.Information("\n");

        context.Information("UnitTestWhitelist: Count({0})", UnitTestWhitelist.Count);
        context.Information("UITestWhitelist: Count({0})", UITestWhitelist.Count);
        context.Information("\n");

        context.Information("ShouldCopyImages: {0}", ShouldCopyImages);
        context.Information("\n");

        context.Information("ShouldRunUnitTests: {0}", ShouldRunUnitTests);
        context.Information("ShouldRunUITests: {0}", ShouldRunUITests);
        context.Information("\n");

        context.Information("ShouldDeployAppCenter: {0}", ShouldDeployAppCenter);
        context.Information("\n");

        context.Information("ShouldRunFastlaneDeliver: {0}", ShouldRunFastlaneDeliver);
        context.Information("ShouldRunFastlaneMatch: {0}", ShouldRunFastlaneMatch);
        context.Information("ShouldRunFastlanePilot: {0}", ShouldRunFastlanePilot);
        context.Information("ShouldRunFastlaneSupply: {0}", ShouldRunFastlaneSupply);
        context.Information("\n");

        context.Information("NugetConfig: {0}", NugetConfig);
        context.Information("NuGetSources: {0}", string.Join(", ", NuGetSources));
        context.Information("\n");
    }
}

public static Cake.Core.Configuration.ICakeConfiguration GetConfiguration(this ICakeContext context)
{
    var configProvider = new Cake.Core.Configuration.CakeConfigurationProvider(context.FileSystem, context.Environment);
    var arguments = (IDictionary<string, string>)context.Arguments.GetType().GetProperty("Arguments").GetValue(context.Arguments);
    return configProvider.CreateConfiguration(context.Environment.WorkingDirectory,arguments);
}

public static ICollection<string> GetNuGetSources(ICakeContext context, ICollection<string> nuGetSources)
{
    if (nuGetSources == null)
    {
        if (BuildParameters.NugetConfig != null && context.FileExists(BuildParameters.NugetConfig))
        {
            nuGetSources = (
                from configuration in System.Xml.Linq.XDocument.Load(BuildParameters.NugetConfig.FullPath).Elements("configuration")
                from packageSources in configuration.Elements("packageSources")
                from add in packageSources.Elements("add")
                from value in add.Attributes("value")
                select value.Value
                ).ToArray();
        }
        else
        {
            // TODO: Use parameter for Cake Contrib feed from environment variable, similar to BuildParameters.MyGet.SourceUrl
            nuGetSources = new []
            {
                "https://api.nuget.org/v3/index.json",
                "https://www.myget.org/F/cake-contrib/api/v3/index.json"
            };
        }
    }
    
    return nuGetSources;
}