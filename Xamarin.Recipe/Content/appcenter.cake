BuildParameters.Tasks.AppCenterTask = Task("AppCenter");

Task("Distribute")
    .IsDependentOn("Archive")
    .Does(() =>
    {
        Information("Uploading to App Center");
    });