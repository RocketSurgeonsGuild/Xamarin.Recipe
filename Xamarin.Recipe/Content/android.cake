#addin nuget:?package=Cake.AndroidAppManifest&version=1.1.0

BuildParameters.Tasks.AndroidArchiveTask = Task("Android-Archive")
    .IsDependentOn("Android-Build")
    .Does(() =>
    {
        var keyStore = EnvironmentVariable(Environment.KeyStoreVariable);
        if (string.IsNullOrEmpty(keyStore))
        {
            throw new Exception("The Android key store environment variable is not defined.");
        }

        var keyStoreAlias = EnvironmentVariable(Environment.KeyStoreAliasVariable);
        if (string.IsNullOrEmpty(keyStoreAlias))
        {
            throw new Exception("The Android key store alias environment variable is not defined.");
        }

        var keyStorePassword = EnvironmentVariable(Environment.KeyStorePasswordVariable);
        if (string.IsNullOrEmpty(keyStorePassword))
        {
            throw new Exception("The Android key store password environment variable is not defined.");
        }
        
        MSBuild(BuildParameters.AndroidProjectPath, configurator =>
                    configurator
                        .SetConfiguration(BuildParameters.Configuration)
                        .SetVerbosity(ToolSettings.MSBuildVerbosity)
                        .UseToolVersion(MSBuildToolVersion.VS2017)
                        .WithTarget("SignAndroidPackage")
                        .WithProperty("AndroidSigningStorePass", keyStorePassword)
                        .WithProperty("AndroidSigningKeyStore", keyStore)
                        .WithProperty("AndroidSigningKeyAlias", keyStoreAlias)
                        .WithProperty("AndroidSigningKeyPass", keyStorePassword));
    });

Task("Android-Build")
    .IsDependentOn("Android-Manifest")
    .Does(() => 
    {
        MSBuild(BuildParameters.AndroidProjectPath, configurator =>
                    configurator
                        .SetConfiguration(BuildParameters.Configuration)
                        .SetVerbosity(ToolSettings.MSBuildVerbosity)
                        .UseToolVersion(MSBuildToolVersion.VS2017));
    });

Task("Android-Manifest")
    .Does(() =>
    {
        var manifest = DeserializeAppManifest(BuildParameters.AndroidManifest);

        manifest.VersionName = BuildParameters.Version.Version;
        manifest.VersionCode = BuildParameters.Version.PreReleaseNumber;
        // manifest.PackageName = Environment.BundleIdentifier;

        SerializeAppManifest(BuildParameters.AndroidManifest, manifest);
    });