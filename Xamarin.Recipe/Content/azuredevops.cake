BuildParameters.Tasks.AzureDevOpsTask = Task("AzureDevOps")
    .WithCriteria(() => BuildParameters.IsRunningOnVSTS)
    .IsDependentOn("Upload-AzureDevOps-Artifacts")
    .IsDependentOn("Clear-AzureDevOps-Cache");


    BuildParameters.Tasks.PrintAzureDevOpsEnvironmentVariablesTask = Task("Print-AzureDevOps-Environment-Variables")
        .WithCriteria(() => BuildParameters.IsRunningOnVSTS)
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
        });

    BuildParameters.Tasks.UploadAzureDevOpsArtifactsTask = Task("Upload-AzureDevOps-Artifacts")
        .Does(() =>
        {

        });
        
    BuildParameters.Tasks.ClearAzureDevOpsCacheTask = Task("Clear-AzureDevOps-Cache")
        .Does(() =>
        {

        });