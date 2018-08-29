BuildParameters.Tasks.AppCenterTask = Task("AppCenter")
                                            .WithCriteria(BuildParameters.ShouldDeployAppCenter);

BuildParameters.Tasks.DistributeTask = Task("Distribute")
                                            .Does(() =>
                                            {
                                                Information("Uploading to App Center");
                                            });