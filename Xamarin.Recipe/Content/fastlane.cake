#addin nuget:?package=Cake.Fastlane&version=0.4.1

BuildParameters.Tasks.FastlaneTask = Task("Fastlane");

BuildParameters.Tasks.FastlaneDeliverTask =
    Task("Fastlane-Deliver")
        .WithCriteria(() => BuildParameters.ShouldRunFastlaneDeliver)
        .IsDependentOn("Archive")
        .IsDependentOn("AppCenter")
        .Does(() =>
        {
            Fastlane.Deliver(ToolSettings.FastlaneDeliverConfigurator);
        });