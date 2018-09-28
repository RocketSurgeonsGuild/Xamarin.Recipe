BuildParameters.Tasks.AzureDevOpsTask = Task("AzureDevOps")
    .IsDependentOn("Print-AzureDevOps-Environment-Variables")
    .IsDependentOn("Clear-AzureDevOps-Cache")
    .IsDependentOn("Publish-AzureDevOps-Test-Results")
    .IsDependentOn("Upload-AzureDevOps-Artifacts")
    .IsDependentOn("Distribute")
    .IsDependentOn("Fastlane");

BuildParameters.Tasks.PrintAzureDevOpsEnvironmentVariablesTask = Task("Print-AzureDevOps-Environment-Variables")
    .WithCriteria(() => BuildParameters.IsRunningOnAzureDevOps)
    .IsDependentOn("Show-Info")
    .Does(() =>
    {
        Information("AGENT_ID: {0}", EnvironmentVariable("AGENT_ID"));
        Information("AGENT_NAME: {0}", EnvironmentVariable("AGENT_NAME"));
        Information("AGENT_VERSION: {0}", EnvironmentVariable("AGENT_VERSION"));
        Information("AGENT_JOBNAME: {0}", EnvironmentVariable("AGENT_JOBNAME"));
        Information("AGENT_JOBSTATUS: {0}", EnvironmentVariable("AGENT_JOBSTATUS"));
        Information("AGENT_MACHINE_NAME: {0}", EnvironmentVariable("AGENT_MACHINE_NAME"));
        Information("\n");
                    
        Information("BUILD_BUILDID: {0}", EnvironmentVariable("BUILD_BUILDID"));
        Information("BUILD_BUILDNUMBER: {0}", EnvironmentVariable("BUILD_BUILDNUMBER"));
        Information("BUILD_DEFINITIONNAME: {0}", EnvironmentVariable("BUILD_DEFINITIONNAME"));
        Information("BUILD_DEFINITIONVERSION: {0}", EnvironmentVariable("BUILD_DEFINITIONVERSION"));
        Information("BUILD_QUEUEDBY: {0}", EnvironmentVariable("BUILD_QUEUEDBY"));
        Information("\n");

        Information("BUILD_SOURCEBRANCHNAME: {0}", EnvironmentVariable("BUILD_SOURCEBRANCHNAME"));
        Information("BUILD_SOURCEVERSION: {0}", EnvironmentVariable("BUILD_SOURCEVERSION"));
        Information("BUILD_REPOSITORY_NAME: {0}", EnvironmentVariable("BUILD_REPOSITORY_NAME"));
        Information("BUILD_REPOSITORY_PROVIDER: {0}", EnvironmentVariable("BUILD_REPOSITORY_PROVIDER"));
        Information("\n");
    });

BuildParameters.Tasks.UploadAzureDevOpsArtifactsTask = Task("Upload-AzureDevOps-Artifacts")
    .WithCriteria(() => BuildParameters.IsRunningOnAzureDevOps)
    .IsDependentOn("Archive");

BuildParameters.Tasks.PublishAzureDevOpsTestResultsTask = Task("Publish-AzureDevOps-Test-Results")
    .WithCriteria(() => BuildParameters.IsRunningOnAzureDevOps)
    .IsDependentOn("Test")
    .Does(() =>
    {
        var testResultsDirectory = BuildParameters.Paths.Directories.xUnitTestResults;

        Verbose("xUnit Test Result Path: {0}", testResultsDirectory);

        var testResults = testResultsDirectory + "/**/*.trx";

        var results = GetFiles(testResults);

        foreach(var result in results.Select(filepath => filepath.MakeAbsolute(Context.Environment)))
        {
            Verbose("File Path: {0}", result);
            Verbose("File Name: {0}", result.GetFilename());
            // Verbose("Relative Path: {0}", result.MakeAbsolute(Context.Environment).GetRelativePath(testResultsDirectory));
            Verbose("Absolute File Path: {0}", result.MakeAbsolute(Context.Environment));
        }

        Verbose("Test Results: {0}",BuildParameters.Paths.Files.TestResultsFilePath);

        var testResultsData = new TFBuildPublishTestResultsData
        {
            Configuration = BuildParameters.Configuration,
            TestRunTitle = "Unit Tests",
            TestRunner = TFTestRunnerType.XUnit,
            TestResultsFiles = new string[] { BuildParameters.Paths.Files.TestResultsFilePath.ToString() }
        };

        BuildSystem.TFBuild.Commands.PublishTestResults(testResultsData);
    });

BuildParameters.Tasks.ClearAzureDevOpsCacheTask = Task("Clear-AzureDevOps-Cache")
    .WithCriteria(() => BuildParameters.IsRunningOnAzureDevOps)
    .IsDependentOn("Clean")
    .Does(() =>
    {

    });