public class BuildTasks
{
    public CakeTaskBuilder DupFinderTask { get; set; }
    public CakeTaskBuilder InspectCodeTask { get; set; }
    public CakeTaskBuilder AnalyzeTask { get; set; }
    public CakeTaskBuilder ShowInfoTask { get; set; }
    public CakeTaskBuilder CleanTask { get; set; }
    public CakeTaskBuilder DotNetCoreCleanTask { get; set; }
    public CakeTaskBuilder RestoreTask { get; set; }
    public CakeTaskBuilder DotNetCoreRestoreTask { get; set; }
    public CakeTaskBuilder BuildTask { get; set; }
    public CakeTaskBuilder ImageCopyTask {get; set; }
    public CakeTaskBuilder iOSArchiveTask { get; set; }
    public CakeTaskBuilder AndroidArchiveTask { get; set; }
    public CakeTaskBuilder ArchiveTask { get; set; }
    public CakeTaskBuilder AppCenterTask { get; set; }
    public CakeTaskBuilder DistributeTask { get; set; }
    public CakeTaskBuilder DotNetCoreBuildTask { get; set; }
    public CakeTaskBuilder PackageTask { get; set; }
    public CakeTaskBuilder DefaultTask { get; set; }
    public CakeTaskBuilder AppVeyorTask { get; set; }
    public CakeTaskBuilder ReleaseNotesTask { get; set; }
    public CakeTaskBuilder ClearCacheTask { get; set; }
    public CakeTaskBuilder PreviewTask { get; set; }
    public CakeTaskBuilder PublishDocsTask { get; set; }
    public CakeTaskBuilder UploadCodecovReportTask { get; set; }
    public CakeTaskBuilder UploadCoverallsReportTask { get; set; }
    public CakeTaskBuilder UploadCoverageReportTask { get; set; }
    public CakeTaskBuilder CreateReleaseNotesTask { get; set; }
    public CakeTaskBuilder ExportReleaseNotesTask { get; set; }
    public CakeTaskBuilder PublishGitHubReleaseTask { get; set; }
    public CakeTaskBuilder InstallReportGeneratorTask { get; set; }
    public CakeTaskBuilder InstallReportUnitTask { get; set; }
    public CakeTaskBuilder InstallOpenCoverTask { get; set; }
    public CakeTaskBuilder TestTask { get; set; }
    public CakeTaskBuilder TestxUnitTask { get; set; }
    public CakeTaskBuilder TestUITask { get; set; }
    public CakeTaskBuilder IntegrationTestTask { get;set; }
    public CakeTaskBuilder FastlaneTask { get; set; }
    public CakeTaskBuilder CleanDocumentationTask { get; set; }
    public CakeTaskBuilder DeployGraphDocumentation {get; set;}
    public CakeTaskBuilder PublishDocumentationTask { get; set; }
    public CakeTaskBuilder PreviewDocumentationTask { get; set; }
    public CakeTaskBuilder ForcePublishDocumentationTask { get; set; }
    public CakeTaskBuilder VSTSTask { get; set; }
    public CakeTaskBuilder PrintVSTSEnvironmentVariablesTask { get; set; }
    public CakeTaskBuilder UploadVSTSArtifactsTask { get; set; }
    public CakeTaskBuilder ClearVSTSCacheTask { get; set; }
}