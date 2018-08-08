using System;

public class BuildPaths
{
    public BuildFiles Files { get; private set; }
    public BuildDirectories Directories { get; private set; }
    public static BuildPaths GetPaths(ICakeContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException("context");
        }
 
        // Directories
        var buildDirectoryPath             = "./BuildArtifacts";
        var tempBuildDirectoryPath         = buildDirectoryPath + "/temp";
        var iOSArtifactDirectoryPath       = buildDirectoryPath + "/ios";
        var publishedxUnitTestsDirectory   = tempBuildDirectoryPath + "/_PublishedxUnitTests";
        var publishedWebsitesDirectory     = tempBuildDirectoryPath + "/_PublishedWebsites";
        var publishedApplicationsDirectory = tempBuildDirectoryPath + "/_PublishedApplications";
        var publishedLibrariesDirectory    = tempBuildDirectoryPath + "/_PublishedLibraries";
        var publishedDocumentationDirectory= buildDirectoryPath + "/Documentation";

        var nugetNuspecDirectory = "./nuspec/nuget";

        var testResultsDirectory = buildDirectoryPath + "/TestResults";
        var inspectCodeResultsDirectory = testResultsDirectory + "/InspectCode";
        var dupFinderResultsDirectory = testResultsDirectory + "/DupFinder";
        var xUnitTestResultsDirectory = testResultsDirectory + "/xUnit";

        var testCoverageDirectory = buildDirectoryPath + "/TestCoverage";

        var packagesDirectory = buildDirectoryPath + "/Packages";
        var nuGetPackagesOutputDirectory = packagesDirectory + "/NuGet";
        var chocolateyPackagesOutputDirectory = packagesDirectory + "/Chocolatey";

        var unitTestPattern = BuildParameters.TestDirectoryPath + BuildParameters.UnitTestFilePattern;
        var uiTestPattern = BuildParameters.TestDirectoryPath + BuildParameters.UITestFilePattern;

        // Files
        var testCoverageOutputFilePath = ((DirectoryPath)testCoverageDirectory).CombineWithFilePath("OpenCover.xml");
        var solutionInfoFilePath = ((DirectoryPath)BuildParameters.SourceDirectoryPath).CombineWithFilePath("SolutionInfo.cs");
        var buildLogFilePath = ((DirectoryPath)buildDirectoryPath).CombineWithFilePath("MsBuild.log");
        var unitTestFilePaths = context.GetFiles(unitTestPattern).ToArray();
        var uiTestFilePaths = context.GetFiles(uiTestPattern).ToArray();

        var repoFilesPaths = new FilePath[] {
            "LICENSE",
            "README.md"
        };

        var buildDirectories = new BuildDirectories(
            buildDirectoryPath,
            tempBuildDirectoryPath,
            iOSArtifactDirectoryPath,
            publishedxUnitTestsDirectory,
            publishedWebsitesDirectory,
            publishedApplicationsDirectory,
            publishedLibrariesDirectory,
            publishedDocumentationDirectory,
            nugetNuspecDirectory,
            testResultsDirectory,
            inspectCodeResultsDirectory,
            dupFinderResultsDirectory,
            xUnitTestResultsDirectory,
            testCoverageDirectory,
            nuGetPackagesOutputDirectory,
            packagesDirectory);

        var buildFiles = new BuildFiles(
            context,
            repoFilesPaths,
            unitTestFilePaths,
            uiTestFilePaths,
            testCoverageOutputFilePath,
            solutionInfoFilePath,
            buildLogFilePath);

        return new BuildPaths
        {
            Files = buildFiles,
            Directories = buildDirectories
        };
    }
}

public class BuildFiles
{
    public ICollection<FilePath> RepoFilesPaths { get; private set; }

    public FilePath TestCoverageOutputFilePath { get; private set; }

    public FilePath SolutionInfoFilePath { get; private set; }

    public FilePath BuildLogFilePath { get; private set; }
    
    public ICollection<FilePath> UnitTestFilePaths { get; private set; }
    
    public ICollection<FilePath> UITestFilePaths { get; private set; }

    public BuildFiles(
        ICakeContext context,
        FilePath[] repoFilesPaths,
        FilePath[] unitTestFilePaths,
        FilePath[] uiTestFilePaths,
        FilePath testCoverageOutputFilePath,
        FilePath solutionInfoFilePath,
        FilePath buildLogFilePath
        )
    {
        RepoFilesPaths = Filter(context, repoFilesPaths);
        TestCoverageOutputFilePath = testCoverageOutputFilePath;
        SolutionInfoFilePath = solutionInfoFilePath;
        BuildLogFilePath = buildLogFilePath;
        UnitTestFilePaths = unitTestFilePaths;
        UITestFilePaths = uiTestFilePaths;
    }

    private static FilePath[] Filter(ICakeContext context, FilePath[] files)
    {
        // Not a perfect solution, but we need to filter PDB files
        // when building on an OS that's not Windows (since they don't exist there).

        if(!context.IsRunningOnWindows())
        {
            return files.Where(f => !f.FullPath.EndsWith("pdb")).ToArray();
        }

        return files;
    }
}

public class BuildDirectories
{
    public DirectoryPath Build { get; private set; }
    public DirectoryPath TempBuild { get; private set; }
    public DirectoryPath IOSArtifactDirectoryPath { get; private set; }
    public DirectoryPath PublishedNUnitTests { get; private set; }
    public DirectoryPath PublishedxUnitTests { get; private set; }
    public DirectoryPath PublishedMSTestTests { get; private set; }
    public DirectoryPath PublishedVSTestTests { get; private set; }
    public DirectoryPath PublishedFixieTests { get; private set; }
    public DirectoryPath PublishedWebsites { get; private set; }
    public DirectoryPath PublishedApplications { get; private set; }
    public DirectoryPath PublishedLibraries { get; private set; }
    public DirectoryPath PublishedDocumentation { get; private set; }
    public DirectoryPath NugetNuspecDirectory { get; private set; }
    public DirectoryPath ChocolateyNuspecDirectory { get; private set; }
    public DirectoryPath TestResults { get; private set; }
    public DirectoryPath InspectCodeTestResults { get; private set; }
    public DirectoryPath DupFinderTestResults { get; private set; }
    public DirectoryPath NUnitTestResults { get; private set; }
    public DirectoryPath xUnitTestResults { get; private set; }
    public DirectoryPath MSTestTestResults { get; private set; }
    public DirectoryPath VSTestTestResults { get; private set; }
    public DirectoryPath FixieTestResults { get; private set; }
    public DirectoryPath TestCoverage { get; private set; }
    public DirectoryPath NuGetPackages { get; private set; }
    public DirectoryPath ChocolateyPackages { get; private set; }
    public DirectoryPath Packages { get; private set; }
    public ICollection<DirectoryPath> ToClean { get; private set; }

    public BuildDirectories(
        DirectoryPath build,
        DirectoryPath tempBuild,
        DirectoryPath iOSArtifactDirectoryPath,
        DirectoryPath publishedxUnitTests,
        DirectoryPath publishedWebsites,
        DirectoryPath publishedApplications,
        DirectoryPath publishedLibraries,
        DirectoryPath publishedDocumentation,
        DirectoryPath nugetNuspecDirectory,
        DirectoryPath testResults,
        DirectoryPath inspectCodeTestResults,
        DirectoryPath dupFinderTestResults,
        DirectoryPath xunitTestResults,
        DirectoryPath testCoverage,
        DirectoryPath nuGetPackages,
        DirectoryPath packages)
    {
        Build = build;
        TempBuild = tempBuild;
        IOSArtifactDirectoryPath = iOSArtifactDirectoryPath;
        PublishedxUnitTests = publishedxUnitTests;
        PublishedWebsites = publishedWebsites;
        PublishedApplications = publishedApplications;
        PublishedLibraries = publishedLibraries;
        PublishedDocumentation = publishedDocumentation;
        NugetNuspecDirectory = nugetNuspecDirectory;
        TestResults = testResults;
        InspectCodeTestResults = inspectCodeTestResults;
        DupFinderTestResults = dupFinderTestResults;
        xUnitTestResults = xunitTestResults;
        TestCoverage = testCoverage;
        NuGetPackages = nuGetPackages;
        Packages = packages;
        
        ToClean = new[] {
            Build,
            TempBuild,
            IOSArtifactDirectoryPath,
            TestResults,
            TestCoverage
        };
    }
}