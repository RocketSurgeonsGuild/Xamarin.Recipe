BuildParameters.Tasks.AppCenterTask = Task("AppCenter")
                                            .WithCriteria(() => BuildParameters.ShouldDeployAppCenter);

BuildParameters.Tasks.DistributeTask = Task("Distribute")
                                            .WithCriteria(() => BuildParameters.CanDistribute)
                                            .IsDependentOn("Archive")
                                            .IsDependentOn("AppCenter")
                                            .IsDependentOn("Fastlane");