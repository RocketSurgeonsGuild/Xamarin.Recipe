#addin nuget:?package=Cake.AppCenter&version=1.1.0

BuildParameters.Tasks.AppCenterTask = Task("AppCenter")
                                            .WithCriteria(() => BuildParameters.ShouldDeployAppCenter);

BuildParameters.Tasks.DistributeTask = Task("Distribute")
                                            .Does(() =>
                                            {
                                                Information("Uploading to App Center");
                                            });