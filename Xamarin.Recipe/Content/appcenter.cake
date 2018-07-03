BuildParameters.Tasks.AppCenterTask = Task("AppCenter");

BuildParameters.Tasks.DistributeTask = Task("Distribute")
                                            .IsDependentOn("Archive")
                                            .Does(() =>
                                            {
                                                Information("Uploading to App Center");
                                            });