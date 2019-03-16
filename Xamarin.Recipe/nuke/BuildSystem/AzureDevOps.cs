using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using GlobExpressions;
using Nuke.Common;

partial class XamarinBuild
{
    Expression<Func<bool>> IsRunningOnAzureDevOps => () => Parameters.IsRunningOnAzureDevOps;

    Expression<Func<bool>> ShouldPublishTestResults => () => Parameters.ShouldPublishTestResults;

    Target PrintAzureDevOpsEnvironment => _ => _
        .Before(Clean)
        .OnlyWhenStatic(IsRunningOnAzureDevOps)
        .Executes(() =>
        {
            Information("AGENT_ID: {0}", EnvironmentVariable("AGENT_ID"));
            Information("AGENT_NAME: {0}", EnvironmentVariable("AGENT_NAME"));
            Information("AGENT_VERSION: {0}", EnvironmentVariable("AGENT_VERSION"));
            Information("AGENT_JOBNAME: {0}", EnvironmentVariable("AGENT_JOBNAME"));
            Information("AGENT_JOBSTATUS: {0}", EnvironmentVariable("AGENT_JOBSTATUS"));
            Information("AGENT_MACHINE_NAME: {0}", EnvironmentVariable("AGENT_MACHINE_NAME"));
            Information("\n");

            Information("BUILD_BUILDID: {0}", EnvironmentVariable("BUILD_BUILDID"));
            Information("BUILD_BUILDNUMBER: {0}", EnvironmentVariable("BUILD_BUILDNUMBER"));
            Information("BUILD_DEFINITIONNAME: {0}", EnvironmentVariable("BUILD_DEFINITIONNAME"));
            Information("BUILD_DEFINITIONVERSION: {0}", EnvironmentVariable("BUILD_DEFINITIONVERSION"));
            Information("BUILD_QUEUEDBY: {0}", EnvironmentVariable("BUILD_QUEUEDBY"));
            Information("\n");

            Information("BUILD_SOURCEBRANCHNAME: {0}", EnvironmentVariable("BUILD_SOURCEBRANCHNAME"));
            Information("BUILD_SOURCEVERSION: {0}", EnvironmentVariable("BUILD_SOURCEVERSION"));
            Information("BUILD_REPOSITORY_NAME: {0}", EnvironmentVariable("BUILD_REPOSITORY_NAME"));
            Information("BUILD_REPOSITORY_PROVIDER: {0}", EnvironmentVariable("BUILD_REPOSITORY_PROVIDER"));
        });

    Target UploadAzureDevOpsArtifacts => _ => _
        .OnlyWhenStatic(IsRunningOnAzureDevOps)
        .Executes(() =>
        {
            Information($"##vso[artifact.upload containerfolder=Packages;artifactname=Packages;]{PackageDirectory}");
        });

    Target PublishAzureDevOpsTestResults => _ => _
        .Before(PublishAzureDevOpsCodeCoverage)
        .OnlyWhenStatic(ShouldPublishTestResults)
        .OnlyWhenStatic(IsRunningOnAzureDevOps)
        .Executes(() =>
        {
            IEnumerable<FileInfo> files = new DirectoryInfo(TestResultsDirectory).GlobFiles("**/*.trx");
            AzureDevOpsTestResultsCommand(files, $"{Parameters.Title} - {GitVersion.BranchName}", Configuration);
        });

    Target PublishAzureDevOpsCodeCoverage => _ => _
        .OnlyWhenStatic(ShouldPublishTestResults)
        .OnlyWhenStatic(IsRunningOnAzureDevOps);

    Target AzureDevOps => _ => _
        .DependsOn(PrintAzureDevOpsEnvironment)
        .DependsOn(UploadAzureDevOpsArtifacts)
        .DependsOn(PublishAzureDevOpsTestResults)
        .DependsOn(PublishAzureDevOpsCodeCoverage)
        .OnlyWhenStatic(IsRunningOnAzureDevOps);

    /// <summary>
    /// Logs the Azure DevOps test results command.
    /// </summary>
    /// <param name="files">The files.</param>
    /// <param name="title">The title.</param>
    /// <param name="platform">The platform.</param>
    /// <param name="configuration">The configuration.</param>
    /// <param name="testType">Type of the test.</param>
    static void AzureDevOpsTestResultsCommand(IEnumerable<FileInfo> files, string title, string platform = "x64", string configuration = "Release", string testType = "VSTest")
    {
        var resultFiles = string.Join(',', files.Select(x => x.FullName.Replace('\\', Path.DirectorySeparatorChar)));
        Information($"##vso[results.publish type={testType};mergeResults=false;platform={platform}4;config={configuration};runTitle='{title}';publishRunAttachments=true;resultFiles={resultFiles};]");
    }

    static void AzureDevOpsCommand(string command, Dictionary<string, string> properties, string value)
    {
        var props = string.Join(string.Empty, properties.Select(pair => string.Format(CultureInfo.InvariantCulture, "{0}={1};", pair.Key, pair.Value)));
        Information($"##vso[{command} {props}]{value}");
    }
}
