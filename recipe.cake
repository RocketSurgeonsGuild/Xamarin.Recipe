#load nuget:https://www.myget.org/F/cake-contrib/api/v2?package=Cake.Recipe&prerelease

Environment.SetVariableNames();

BuildParameters.SetParameters(context: Context,
                            buildSystem: BuildSystem,
                            sourceDirectoryPath: "./src",
                            title: "Rocket.Surgery.Xamarin.Recipe",
                            repositoryOwner: "RocketSurgeonsGuild",
                            repositoryName: "Xamarin.Recipe",
                            appVeyorAccountName: "RocketSurgeonsGuild",
                            masterBranchName: "main",
                            developBranchName: "dev",
                            nuspecFilePath: "./Xamarin.Recipe/Rocket.Surgery.Xamarin.Recipe.nuspec");

BuildParameters.PrintParameters(Context);

ToolSettings.SetToolSettings(context: Context);

BuildParameters.Tasks.CleanTask
    .IsDependentOn("Generate-Version-File");

Task("Generate-Version-File")
    .Does(() => 
    {
        var buildMetaDataCodeGen = TransformText(@"
        public class BuildMetaData
        {
            public static string Date { get; } = ""<%date%>"";
            public static string Version { get; } = ""<%version%>"";
        }",
        "<%",
        "%>")
        .WithToken("date", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"))
        .WithToken("version", BuildParameters.Version.SemVersion)
        .ToString();

        System.IO.File.WriteAllText(
            "./Xamarin.Recipe/Content/version.cake",
            buildMetaDataCodeGen
            );
    });

Task("UploadAzureDevOpsArtifacts")
    .WithCriteria(() => TFBuild.IsRunningOnVSTS)
    .Does(() =>
    {
        BuildSystem
            .TFBuild
            .Commands
            .UploadArtifactDirectory(BuildParameters.Paths.Directories.NuGetPackages.MakeAbsolute(Context.Environment).FullPath, "Packages");
    });

Task("AzureDevOps")
    .IsDependentOn("Publish-MyGet-Packages")
    .IsDependentOn("UploadAzureDevOpsArtifacts");

Build.RunNuGet();