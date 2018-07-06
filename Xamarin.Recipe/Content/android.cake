BuildParameters.Tasks.AndroidArchiveTask = Task("AndroidArchive")
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
    .Does(() => 
    {
        MSBuild(BuildParameters.AndroidProjectPath, configurator =>
                    configurator
                        .SetConfiguration(BuildParameters.Configuration)
                        .SetVerbosity(ToolSettings.MSBuildVerbosity)
                        .UseToolVersion(MSBuildToolVersion.VS2017));
    });