using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Nuke.Common;
using Nuke.Common.BuildServers;
using Nuke.Common.Git;
using Nuke.Common.Tools.GitVersion;

partial class XamarinBuild
{
    public class BuildParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BuildParameters"/> class.
        /// </summary>
        /// <param name="build">The build.</param>
        public BuildParameters(XamarinBuild build)
        {
            Title = build.Title;
            Target = build.Target;
            RepositoryName = build.GitRepository.Identifier;

            Branch = build.GitRepository?.Branch ?? string.Empty;
            IsMasterBranch = Branch.Equals(MasterBranch);
            IsDevelopmentBranch = Branch.Equals(DevelopBranch);
            IsReleaseBranch = Branch.Contains(ReleaseBranchPrefix);
            IsPullRequest = build.GitRepository.Head?.Contains("pull") ?? false;

            IsMainRepo = build.GitRepository.Identifier == build.RepositoryIdenfitier;

            IsReleasable = !IsLocalBuild && IsMainRepo && (IsDevelopmentBranch || IsMasterBranch || IsReleaseBranch);

            ShouldPublishTestResults = IsMainRepo && IsRunningOnAzureDevOps;
            ShouldPublishCoverageReports = IsMainRepo && IsRunningOnAzureDevOps;

            IsNuGetRelease = IsReleasable && ShouldPublishNuGet && (IsMasterBranch || IsReleaseBranch);
            IsTestFlightRelease = !IsLocalBuild && IsMainRepo && (IsDevelopmentBranch || IsMasterBranch || IsReleaseBranch);

            ShouldPublishMyGet = !IsLocalBuild && HasEnvironmentVariable(MyGetApiKey) && HasEnvironmentVariable(MyGetSource);
            ShouldPublishNuGet = !IsLocalBuild && HasEnvironmentVariable(NuGetApiKey) && HasEnvironmentVariable(NuGetSource);
        }

        /// <summary>
        /// Gets the title.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Gets the target platform.
        /// </summary>
        public Platform Target { get; }

        /// <summary>
        /// Gets the master branch.
        /// </summary>
        [Parameter]
        public string MasterBranch { get; } = "master";

        /// <summary>
        /// Gets the develop branch.
        /// </summary>
        [Parameter]
        public string DevelopBranch { get; } = "dev";

        /// <summary>
        /// Gets the name of the repository.
        /// </summary>
        public string RepositoryName { get; }

        /// <summary>
        /// Gets the repository branch.
        /// </summary>
        public string Branch { get; }

        /// <summary>
        /// Gets the release branch prefix.
        /// </summary>
        public string ReleaseBranchPrefix { get; } = "release";

        /// <summary>
        /// Gets a value indicating whether this instance is local a build.
        /// </summary>
        public bool IsLocalBuild => Host == HostType.Console;

        /// <summary>
        /// Gets a value indicating whether this instance is running on unix.
        /// </summary>
        public bool IsRunningOnUnix => RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ||
                                       RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

        /// <summary>
        /// Gets a value indicating whether this instance is running on windows.
        /// </summary>
        public bool IsRunningOnWindows => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        /// <summary>
        /// Gets a value indicating whether this instance is running on azure dev ops.
        /// </summary>
        public bool IsRunningOnAzureDevOps =>
            Host == HostType.TeamServices || Environment.GetEnvironmentVariable("LOGNAME") == "vsts";

        /// <summary>
        /// Gets a value indicating whether this instance is main repo.
        /// </summary>
        public bool IsMainRepo { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is master branch.
        /// </summary>
        public bool IsMasterBranch { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is development branch.
        /// </summary>
        public bool IsDevelopmentBranch { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is release branch.
        /// </summary>
        public bool IsReleaseBranch { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is pull request.
        /// </summary>
        public bool IsPullRequest { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is releasable.
        /// </summary>
        public bool IsReleasable { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is nu get release.
        /// </summary>
        public bool IsNuGetRelease { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is test flight release.
        /// </summary>
        public bool IsTestFlightRelease { get; }

        /// <summary>
        /// Gets MyGet source.
        /// </summary>
        public string MyGetSource => "MYGET_SOURCE";

        /// <summary>
        /// Gets MyGet API key.
        /// </summary>
        public string MyGetApiKey => "MYGET_API_KEY";

        /// <summary>
        /// Gets the NuGet source.
        /// </summary>
        public string NuGetSource => "NUGET_SOURCE";

        /// <summary>
        /// Gets the NuGet API key.
        /// </summary>
        public string NuGetApiKey => "NUGET_API_KEY";

        /// <summary>
        /// Gets a value indicating whether [should publish test results].
        /// </summary>
        public bool ShouldPublishTestResults { get; }

        /// <summary>
        /// Gets a value indicating whether [should publish coverage reports].
        /// </summary>
        public bool ShouldPublishCoverageReports { get; }

        /// <summary>
        /// Gets a value indicating whether [should publish my get].
        /// </summary>
        public bool ShouldPublishMyGet { get; }

        /// <summary>
        /// Gets a value indicating whether [should publish nu get].
        /// </summary>
        public bool ShouldPublishNuGet { get; }
    }
}
