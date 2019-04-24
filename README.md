# Xamarin.Recipe

Xamarin.Recipe is a set of convention based scripts for building and deploying Xamarin mobile applications. Most of the code, conventions and concepts for this came from [Cake.Recipe](https://github.com/cake-contrib/Cake.Recipe)

[![License](http://img.shields.io/:license-mit-blue.svg)](https://github.com/RocketSurgeonsGuild/Xamarin.Recipe/blob/dev/LICENSE)

## Information

| | Stable | Pre-release |
|:--:|:--:|:--:|
|GitHub Release|[![GitHub release](https://img.shields.io/github/release/RocketSurgeonsGuild/Xamarin.Recipe.svg)](https://github.com/RocketSurgeonsGuild/Xamarin.Recipe/releases/latest)|
|NuGet|[![NuGet](https://img.shields.io/nuget/v/Rocket.Surgery.Xamarin.Recipe.svg)](https://www.nuget.org/packages/Rocket.Surgery.Xamarin.Recipe)|[![NuGet](https://img.shields.io/nuget/vpre/Rocket.Surgery.Xamarin.Recipe.svg)](https://www.nuget.org/packages/Rocket.Surgery.Xamarin.Recipe)|

## Build Status

|dev|main|
|:--:|:--:|
[![Build status](https://ci.appveyor.com/api/projects/status/u2y7vtbaebmvl617/branch/dev?svg=true)](https://ci.appveyor.com/project/RocketSurgeonsGuild/xamarin-recipe/branch/dev)|[![Build status](https://ci.appveyor.com/api/projects/status/u2y7vtbaebmvl617/branch/dev?svg=true)](https://ci.appveyor.com/project/RocketSurgeonsGuild/xamarin-recipe/branch/main)|
[![Build Status](https://dev.azure.com/rocketsurgeonsguild/Libraries/_apis/build/status/RSG.Xamarin.Recipe?branchName=dev)](https://dev.azure.com/rocketsurgeonsguild/Libraries/_build/latest?definitionId=25&branchName=dev)|[![Build Status](https://dev.azure.com/rocketsurgeonsguild/Libraries/_apis/build/status/RSG.Xamarin.Recipe?branchName=main)](https://dev.azure.com/rocketsurgeonsguild/Libraries/_build/latest?definitionId=25&branchName=main)

# Sample Script

## iOS
```csharp
#load nuget:?package=Rocket.Surgery.Xamarin.Recipe&version=0.5.0

Environment.SetVariableNames();

BuildParameters.SetParameters(
        context: Context,
        buildSystem: BuildSystem,
        sourceDirectoryPath: "./src",
        title: "My.Recipe.Project",
        solutionFilePath: "./Recipe.sln",
        solutionDirectoryPath: "./",
        iosProjectPath: "./src/Recipe.iOS/Recipe.iOS.csproj",
        plistFilePath: "./src/Recipe.iOS/Info.plist",
        platform: "iPhone",
        rootDirectoryPath: "./",
        testDirectoryPath: "./tests",
        unitTestWhitelist: new[] { "./tests/Recipe/Recipe.csproj" },
        integrationTestScriptPath: "./tests/integration/test.cake",
        repositoryOwner: "GitHubUserName",
        repositoryName: "GitHubRepository",
        appVeyorAccountName: "AppVeyorAccount",
        appVeyorProjectSlug: "SLUG",
        isPublicRepository: false,
        shouldRunGitVersion: true,
        shouldDeployAppCenter: false,
        shouldCopyImages: false,
        shouldRunxUnit: true,
        shouldRunUnitTests: true,
        shouldRunFastlaneMatch = false,
        buildNumber: 0,
        mainBranch: "main",
        devBranch: "dev",
        nugetConfig: "./NuGet.config",
        nuGetSources: new[] { "nuget.org", "myget.org" });

BuildParameters.PrintParameters(Context);

ToolSettings.SetToolSettings(context: Context, msBuildToolVersion: MSBuildToolVersion.NET40);

Build.RuniOS();
```

## Android
```csharp
#load nuget:?package=Rocket.Surgery.Xamarin.Recipe&version=0.5.0

Environment.SetVariableNames();

BuildParameters.SetParameters(
        context: Context,
        buildSystem: BuildSystem,
        sourceDirectoryPath: "./src",
        title: "My.Recipe.Project",
        solutionFilePath: "./Recipe.sln",
        solutionDirectoryPath: "./",
        androidProjectPath: "./src/Recipe.Android/Recipe.Android.csproj",
        androidManifest: "./src/Recipe.Android/Properties/AndroidManifest.xml",
        rootDirectoryPath: "./",
        testDirectoryPath: "./tests",
        unitTestWhitelist: new[] { "./tests/Recipe/Recipe.csproj" },
        integrationTestScriptPath: "./tests/integration/test.cake",
        repositoryOwner: "GitHubUserName",
        repositoryName: "GitHubRepository",
        appVeyorAccountName: "AppVeyorAccount",
        appVeyorProjectSlug: "SLUG",
        isPublicRepository: false,
        shouldRunGitVersion: true,
        shouldDeployAppCenter: false,
        shouldCopyImages: false,
        shouldRunxUnit: true,
        shouldRunUnitTests: true,
        shouldRunFastlaneMatch = false,
        buildNumber: 0,
        mainBranch: "main",
        devBranch: "dev",
        nugetConfig: "./NuGet.config",
        nuGetSources: new[] { "nuget.org", "myget.org" });

BuildParameters.PrintParameters(Context);

ToolSettings.SetToolSettings(context: Context);

Build.Android();
```