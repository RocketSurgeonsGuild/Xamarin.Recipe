using System;
using System.Linq;
using Configuration;
using Nuke.Common;
using Nuke.Common.BuildServers;
using Nuke.Common.Git;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.GitVersion;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;

partial class XamarinBuild : NukeBuild
{
    public static int Main () => Execute<XamarinBuild>(x => x.Compile);

    static BuildParameters Parameters { get; set; }
    [Solution] readonly Solution Solution;
    [GitRepository] readonly GitRepository GitRepository;
    [GitVersion] readonly GitVersion GitVersion;

    /// <inheritdoc />
    protected override void OnBuildInitialized()
    {
        Parameters = new BuildParameters(this);
    }

    Target ShowInfo => _ => _
        .Executes(() =>
        {
            Information("Configuration: {0}", Configuration);

            Information("RepositoryName: {0}", Parameters.RepositoryName);
            Information("IsMainRepo: {0}", Parameters.IsMainRepo);
            Information("IsMasterBranch: {0}", Parameters.IsMasterBranch);
            Information("IsDevelopmentBranch: {0}", Parameters.IsDevelopmentBranch);
            Information("IsReleaseBranch: {0}", Parameters.IsReleaseBranch);
            Information("IsPullRequest: {0}", Parameters.IsPullRequest);

            Information("IsLocalBuild: {0}", Parameters.IsLocalBuild);
            Information("IsRunningOnUnix: {0}", Parameters.IsRunningOnUnix);
            Information("IsRunningOnWindows: {0}", Parameters.IsRunningOnWindows);
            Information("IsRunningOnAzureDevOps: {0}", Parameters.IsRunningOnAzureDevOps);

            Information("IsReleasable: {0}", Parameters.IsReleasable);
            Information("IsTestFlightReleasable: {0}", Parameters.IsTestFlightRelease);
        });

    Target Clean => _ => _
        .Executes(() =>
        {
        });

    Target Restore => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
        });

    Target Execute => _ => _
        .DependsOn(DotNetBuild);
}
