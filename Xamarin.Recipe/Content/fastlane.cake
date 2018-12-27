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

BuildParameters.Tasks.FastlaneMatchTask =
    Task("Fastlane-Match")
        .WithCriteria(() => BuildParameters.ShouldRunFastlaneMatch)
        .IsDependentOn("iPhone-Info-Plist")
        .Does(() =>
        {
            Fastlane.Match(ToolSettings.FastlaneMatchConfigurator);
        });

BuildParameters.Tasks.FastlanePilotTask =
    Task("Fastlane-Pilot")
        .WithCriteria(() => BuildParameters.ShouldRunFastlanePilot)
        .IsDependentOn("Archive")
        .IsDependentOn("AppCenter")
        .Does(() =>
        {
            Fastlane.Pilot(ToolSettings.FastlaneDeliverConfigurator);
        });



BuildParameters.Tasks.FastlaneSupplyTask =
    Task("Fastlane-Supply")
        .WithCriteria(() => BuildParameters.ShouldRunFastlaneSupply)
        .IsDependentOn("Android-Archive")
        .IsDependentOn("AppCenter")
        .Does(() =>
        {
            Fastlane.Supply(ToolSettings.FastlaneSupplyConfigurator);
        });