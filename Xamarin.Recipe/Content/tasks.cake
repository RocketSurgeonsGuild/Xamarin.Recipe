public class BuildTasks
{
    public CakeTaskBuilder<ActionTask> DupFinderTask { get; set; }
    public CakeTaskBuilder<ActionTask> InspectCodeTask { get; set; }
    public CakeTaskBuilder<ActionTask> AnalyzeTask { get; set; }
    public CakeTaskBuilder<ActionTask> ShowInfoTask { get; set; }
    public CakeTaskBuilder<ActionTask> CleanTask { get; set; }
    public CakeTaskBuilder<ActionTask> DotNetCoreCleanTask { get; set; }
    public CakeTaskBuilder<ActionTask> RestoreTask { get; set; }
    public CakeTaskBuilder<ActionTask> DotNetCoreRestoreTask { get; set; }
    public CakeTaskBuilder<ActionTask> BuildTask { get; set; }
    public CakeTaskBuilder<ActionTask> ImageCopyTask {get; set; }
    public CakeTaskBuilder<ActionTask> iOSArchiveTask { get; set; }
    public CakeTaskBuilder<ActionTask> AndroidArchiveTask { get; set; }
    public CakeTaskBuilder<ActionTask> ArchiveTask { get; set; }
    public CakeTaskBuilder<ActionTask> DistributeTask { get; set; }
    public CakeTaskBuilder<ActionTask> DotNetCoreBuildTask { get; set; }
    public CakeTaskBuilder<ActionTask> PackageTask { get; set; }
    public CakeTaskBuilder<ActionTask> DefaultTask { get; set; }
    public CakeTaskBuilder<ActionTask> AppVeyorTask { get; set; }
    public CakeTaskBuilder<ActionTask> ReleaseNotesTask { get; set; }
    public CakeTaskBuilder<ActionTask> ClearCacheTask { get; set; }
    public CakeTaskBuilder<ActionTask> PreviewTask { get; set; }
    public CakeTaskBuilder<ActionTask> PublishDocsTask { get; set; }
    public CakeTaskBuilder<ActionTask> UploadCodecovReportTask { get; set; }
    public CakeTaskBuilder<ActionTask> UploadCoverallsReportTask { get; set; }
    public CakeTaskBuilder<ActionTask> UploadCoverageReportTask { get; set; }
    public CakeTaskBuilder<ActionTask> CreateReleaseNotesTask { get; set; }
    public CakeTaskBuilder<ActionTask> ExportReleaseNotesTask { get; set; }
    public CakeTaskBuilder<ActionTask> PublishGitHubReleaseTask { get; set; }
    public CakeTaskBuilder<ActionTask> InstallReportGeneratorTask { get; set; }
    public CakeTaskBuilder<ActionTask> InstallReportUnitTask { get; set; }
    public CakeTaskBuilder<ActionTask> InstallOpenCoverTask { get; set; }
    public CakeTaskBuilder<ActionTask> TestTask { get; set; }
    public CakeTaskBuilder<ActionTask> TestxUnitTask { get; set; }
    public CakeTaskBuilder<ActionTask> TestUITask { get; set; }
    public CakeTaskBuilder<ActionTask> IntegrationTestTask { get;set; }
    public CakeTaskBuilder<ActionTask> AppCenterTask { get; set; }
    public CakeTaskBuilder<ActionTask> FastlaneTask { get; set; }
    public CakeTaskBuilder<ActionTask> CleanDocumentationTask { get; set; }
    public CakeTaskBuilder<ActionTask> DeployGraphDocumentation {get; set;}
    public CakeTaskBuilder<ActionTask> PublishDocumentationTask { get; set; }
    public CakeTaskBuilder<ActionTask> PreviewDocumentationTask { get; set; }
    public CakeTaskBuilder<ActionTask> ForcePublishDocumentationTask { get; set; }
}