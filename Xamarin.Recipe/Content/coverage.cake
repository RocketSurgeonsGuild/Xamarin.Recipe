BuildParameters.Tasks.InstallCoverletTask = Task("Install-Coverlet")
    .Does(() => RequireGlobalTool(CoverletTool, () => { }));
                                            
BuildParameters.Tasks.InstallReportGeneratorTask = Task("Install-ReportGenerator")
    .Does(() => RequireTool(ReportGeneratorTool, () => {
    }));
    
BuildParameters.Tasks.GenerateCoverageReportTask = Task("Generate-Coverage-Report")
    .IsDependentOn("Test")
    .IsDependentOn("Install-ReportGenerator")
    .Does(() =>
    {
        var reportFiles = GetFiles(BuildParameters.TestDirectoryPath.FullPath + "/**/coverage-results.xml");

        if (reportFiles != null && reportFiles.Count > 0)
        {
            ReportGenerator(
                reportFiles,
                BuildParameters.TestDirectoryPath.Combine("./Report"),
                new ReportGeneratorSettings 
                {
                    ReportTypes = new[] { ReportGeneratorReportType.Cobertura, ReportGeneratorReportType.Html },
                });
        }
    });
