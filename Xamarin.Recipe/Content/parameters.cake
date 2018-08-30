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
    public static bool IsRunningOnVSTS {get; private set; }
    public static bool IsPullRequest { get; private set; }
    public static bool IsMainRepository { get; private set; }
    public static bool IsPublicRepository {get; private set; }
    public static bool IsMasterBranch { get; private set; }
    public static bool IsDevelopBranch { get; private set; }
    public static bool IsFeatureBranch { get; private set; }
    public static bool IsReleaseBranch { get; private set; }
    public static bool IsHotFixBranch { get; private set ; }
    public static bool IsTagged { get; private set; }
    public static bool IsPublishBuild { get; private set; }
    public static bool IsReleaseBuild { get; private set; }
    public static bool ShouldRunGitVersion { get; private set; }
    public static string AppCenterApiKey { get; private set; }  
    public static bool IsDotNetCoreBuild { get; set; }
    public static bool IsNuGetBuild { get; set; }
    public static bool TransifexEnabled { get; set; }
    public static bool PrepareLocalRelease { get; set; }
    public static bool ShouldDeployAppCenter { get; set; }
    public static bool ShouldCopyImages { get; private set; }
    public static BuildVersion Version { get; private set; }
    public static BuildPaths Paths { get; private set; }
    public static BuildTasks Tasks { get; set; }
    public static DirectoryPath RootDirectoryPath { get; private set; }
    public static FilePath SolutionFilePath { get; private set; }
    public static DirectoryPath SourceDirectoryPath { get; private set; }
    public static DirectoryPath SolutionDirectoryPath { get; private set; }
    public static FilePath AndroidProjectPath { get; private set; }
    public static FilePath IOSProjectPath { get; private set; }
    public static FilePath PlistFilePath { get; private set; }
    public static string Platform { get; private set; }
    public static DirectoryPath TestDirectoryPath { get; private set; }
    public static FilePath IntegrationTestScriptPath { get; private set; }
    public static string UnitTestFilePattern { get; private set; }
    public static string UITestFilePattern { get; private set; }
    public static string ResharperSettingsFileName { get; private set; }
    public static string RepositoryOwner { get; private set; }
    public static string RepositoryName { get; private set; }
    public static FilePath AndroidManifest { get; private set; }
    public static FilePath NugetConfig { get; private set; }
    public static ICollection<string> NuGetSources { get; private set; }

    static BuildParameters()
    {
        Tasks = new BuildTasks();
    }

    public static bool CanDistribute
    {
        get
        {
            return !string.IsNullOrEmpty(BuildParameters.AppCenterApiKey);
        }
    }

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
        string unitTestFilePattern = null,
        string uiTestFilePattern = null,
        string integrationTestScriptPath = null,
        string resharperSettingsFileName = null,
        string repositoryOwner = null,
        string repositoryName = null,
        string appVeyorAccountName = null,
        string appVeyorProjectSlug = null,
        bool isPublicRepository = false,
        bool? shouldRunGitVersion = true,
        bool shouldDeployAppCenter = false,
        bool shouldCopyImages = false,
        string mainBranch = "main",
        string devBranch = "dev",
        FilePath androidManifest = null,
        FilePath nugetConfig = null,
        ICollection<string> nuGetSources = null)
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
        IOSProjectPath = iosProjectPath;
        PlistFilePath = plistFilePath;
        TestDirectoryPath = testDirectoryPath ?? sourceDirectoryPath;
        UnitTestFilePattern = unitTestFilePattern ?? "/**/*.Tests.csproj";
        UITestFilePattern = uiTestFilePattern ?? "/**/*.Tests.csproj";
        IntegrationTestScriptPath = integrationTestScriptPath ?? context.MakeAbsolute((FilePath)"test.cake");
        ResharperSettingsFileName = resharperSettingsFileName ?? string.Format("{0}.sln.DotSettings", Title);
        RepositoryOwner = repositoryOwner ?? string.Empty;
        RepositoryName = repositoryName ?? Title;
        // AppVeyorAccountName = appVeyorAccountName ?? RepositoryOwner.Replace("-", "").ToLower();
        // AppVeyorProjectSlug = appVeyorProjectSlug ?? Title.Replace(".", "-").ToLower();

        Target = context.Argument("target", "Default");
        ApplicationTarget = context.Argument("application", ApplicationTarget.Android);
        Configuration = context.Argument("configuration", "Release");
        PrepareLocalRelease = context.Argument("prepareLocalRelease", false);
        Platform = platform;
        Title = title;
        CakeConfiguration = context.GetConfiguration();
        IsLocalBuild = buildSystem.IsLocalBuild;
        IsRunningOnUnix = context.IsRunningOnUnix();
        IsRunningOnWindows = context.IsRunningOnWindows();
        IsRunningOnAppVeyor = buildSystem.AppVeyor.IsRunningOnAppVeyor;
        IsRunningOnVSTS = buildSystem.TFBuild.IsRunningOnTFS || buildSystem.TFBuild.IsRunningOnVSTS;
        IsPullRequest = !string.IsNullOrEmpty(context.Environment.GetEnvironmentVariable("SYSTEM_PULLREQUEST_PULLREQUESTNUMBER"));
        IsMainRepository = StringComparer.OrdinalIgnoreCase.Equals(string.Concat(repositoryOwner, "/", repositoryName), buildSystem.TFBuild.Environment.Repository.RepoName);
        IsPublicRepository = isPublicRepository;
        IsMasterBranch = StringComparer.OrdinalIgnoreCase.Equals(mainBranch, buildSystem.TFBuild.Environment.Repository.Branch);
        IsDevelopBranch = StringComparer.OrdinalIgnoreCase.Equals(devBranch, buildSystem.TFBuild.Environment.Repository.Branch);
        IsFeatureBranch = buildSystem.TFBuild.Environment.Repository.Branch.StartsWith("feature", StringComparison.OrdinalIgnoreCase);
        IsReleaseBranch = buildSystem.TFBuild.Environment.Repository.Branch.StartsWith("release", StringComparison.OrdinalIgnoreCase);
        IsHotFixBranch = buildSystem.TFBuild.Environment.Repository.Branch.StartsWith("hotfix", StringComparison.OrdinalIgnoreCase);
        IsTagged = (!string.IsNullOrWhiteSpace(context.Environment.GetEnvironmentVariable("$git_tag")));

        AndroidManifest = androidManifest;

        NugetConfig = context.MakeAbsolute(nugetConfig ?? (FilePath)"./NuGet.Config");
        NuGetSources = GetNuGetSources(context, nuGetSources);

        IsDotNetCoreBuild = true;

        ShouldRunGitVersion = shouldRunGitVersion ?? IsRunningOnUnix;
        ShouldCopyImages = shouldCopyImages;

		SetBuildPaths(BuildPaths.GetPaths(context));

        ShouldDeployAppCenter = ((!IsLocalBuild && !IsPullRequest && (IsMasterBranch || IsReleaseBranch || IsDevelopBranch || IsHotFixBranch || IsTagged)) || shouldDeployAppCenter);
    }

    public static void SetBuildVersion(BuildVersion version)
    {
        Version  = version;
    }

    public static void SetBuildPaths(BuildPaths paths)
    {
        Paths = paths;
    }

    public static void PrintParameters(ICakeContext context)
    {
        context.Information("IsLocalBuild: {0}", IsLocalBuild);
        context.Information("\n");

        context.Information("========== REPOSITORY ==========");
        context.Information("IsDevelopBranch: {0}", IsDevelopBranch);
        context.Information("IsMasterBranch: {0}", IsMasterBranch);
        context.Information("IsReleaseBranch: {0}", IsReleaseBranch);
        context.Information("IsHotFixBranch: {0}", IsHotFixBranch);
        context.Information("IsTagged: {0}", IsTagged);
        context.Information("IsPullRequest: {0}", IsPullRequest);
        context.Information("IsMainRepository: {0}", IsMainRepository);
        context.Information("IsPublicRepository: {0}", IsPublicRepository);
        context.Information("==============================");
        context.Information("\n");

        context.Information("========== ANDROID ==========");
        context.Information("AndroidManifest: {0}", AndroidManifest);
        context.Information("AndroidProjectPath: {0}", AndroidProjectPath);
        context.Information("==============================");
        context.Information("\n");

        context.Information("========== iOS ==========");
        context.Information("InfoPlist: {0}", PlistFilePath);
        context.Information("IOSProjectPath: {0}", IOSProjectPath);
        context.Information("==============================");
        context.Information("\n");

        context.Information("========== NUGET ==========");
        context.Information("NugetConfig: {0} ({1})", NugetConfig, context.FileExists(NugetConfig));
        context.Information("NuGetSources: {0}", string.Join(", ", NuGetSources));
        context.Information("==============================");
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
        if (context.FileExists(BuildParameters.NugetConfig))
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