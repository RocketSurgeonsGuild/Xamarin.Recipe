Task("MSBuild")
    .Does(() =>
    {
        Information("MSBuild Task");
    });

BuildParameters.Tasks.ArchiveTask = Task("Archive").IsDependentOn("Build");
