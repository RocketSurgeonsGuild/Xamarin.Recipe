public static class ToolSettings
{
    public static string[] DupFinderExcludePattern { get; private set; }
    public static string[] DupFinderExcludeFilesByStartingCommentSubstring { get; private set; }
    public static int? DupFinderDiscardCost { get; private set; }
    public static bool? DupFinderThrowExceptionOnFindingDuplicates { get; private set; }
    public static string TestCoverageFilter { get; private set; }
    public static string TestCoverageExcludeByAttribute { get; private set; }
    public static string TestCoverageExcludeByFile { get; private set; }
    public static string TestFramework { get; private set; }
    public static bool TestNoBuild { get; private set; }
    public static bool TestNoRestore { get; private set; }
    public static PlatformTarget BuildPlatformTarget { get; private set; }
    public static MSBuildToolVersion MSBuildToolVersion { get; private set; }
    public static Verbosity MSBuildVerbosity { get; private set; }
    public static int MaxCpuCount { get; private set; }
    public static DirectoryPath OutputDirectory { get; private set; }
    public static string AndroidBuildToolVersion { get; private set; }
    public static Func<DotNetCoreTestSettings> DotNetTestSettings { get; private set; }

    public static void SetToolSettings(
        ICakeContext context,
        string[] dupFinderExcludePattern = null,
        string testCoverageFilter = null,
        string testCoverageExcludeByAttribute = null,
        string testCoverageExcludeByFile = null,
        string testFramework = "netcoreapp2.0",
        bool testNoBuild = true,
        bool testNoRestore = true,
        PlatformTarget? buildPlatformTarget = null,
        MSBuildToolVersion msBuildToolVersion = MSBuildToolVersion.Default,
        Verbosity msBuildVerbosity = Verbosity.Quiet,
        int? maxCpuCount = null,
        DirectoryPath outputDirectory = null,
        string[] dupFinderExcludeFilesByStartingCommentSubstring = null,
        int? dupFinderDiscardCost = null,
        bool? dupFinderThrowExceptionOnFindingDuplicates = null,
        string androidBuildToolVersion = "27.0.2",
        Func<DotNetCoreTestSettings> dotNetTestSettings = null
    )
    {
        context.Information("Setting up tools...");

        var absoluteTestDirectory = context.MakeAbsolute(BuildParameters.TestDirectoryPath);
        var absoluteSourceDirectory = context.MakeAbsolute(BuildParameters.SolutionDirectoryPath);
        DupFinderExcludePattern = dupFinderExcludePattern ??
            new string[]
            {
                string.Format("{0}/{1}.Tests/**/*.cs", absoluteTestDirectory, BuildParameters.Title),
                string.Format("{0}/**/*.AssemblyInfo.cs", absoluteSourceDirectory)
            };
        DupFinderExcludeFilesByStartingCommentSubstring = dupFinderExcludeFilesByStartingCommentSubstring;
        DupFinderDiscardCost = dupFinderDiscardCost;
        DupFinderThrowExceptionOnFindingDuplicates = dupFinderThrowExceptionOnFindingDuplicates;
        TestCoverageFilter = testCoverageFilter ?? string.Format("+[{0}*]* -[*.Tests]*", BuildParameters.Title);
        TestCoverageExcludeByAttribute = testCoverageExcludeByAttribute ?? "*.ExcludeFromCodeCoverage*";
        TestCoverageExcludeByFile = testCoverageExcludeByFile ?? "*/*Designer.cs;*/*.g.cs;*/*.g.i.cs";
        TestFramework = testFramework;
        TestNoBuild = testNoBuild;
        TestNoRestore = testNoRestore;
        BuildPlatformTarget = buildPlatformTarget ?? PlatformTarget.MSIL;
        MSBuildToolVersion = msBuildToolVersion;
        MaxCpuCount = maxCpuCount ?? 0;
        OutputDirectory = outputDirectory;
        MSBuildVerbosity = msBuildVerbosity;
        AndroidBuildToolVersion = androidBuildToolVersion;
        DotNetTestSettings = dotNetTestSettings ?? _defaultDotNetTestSettings;
    }

    private static Func<DotNetCoreTestSettings> _defaultDotNetTestSettings = () => new DotNetCoreTestSettings
                {
                    Configuration = BuildParameters.Configuration,
                    Framework = ToolSettings.TestFramework,
                    NoBuild = ToolSettings.TestNoBuild,
                    NoRestore = ToolSettings.TestNoRestore,
                    ResultsDirectory = BuildParameters.Paths.Directories.xUnitTestResults,
                    Logger = $"trx;LogFileName={BuildParameters.Title}.trx"
                };
}