public static class Environment
{
	public static string MyGetApiKeyVariable { get; private set; }
	public static string MyGetSourceUrlVariable { get; private set; }
	public static string NuGetApiKeyVariable { get; private set; }
	public static string NuGetSourceUrlVariable { get; private set; }
	public static string MicrosoftTeamsWebHookUrlVariable { get; private set; }
	public static string KeyStoreVariable { get; private set; }
	public static string KeyAliasVariable { get; private set; }
	public static string KeyStorePasswordVariable { get; private set; }
	public static string KeyPasswordVariable { get; private set; }
	public static string XamarinBuildConfigurationVariable { get; private set; }
	public static string AppIconSourceImagePathVariable { get; private set; }
	public static string AppIconDestinationImagePathVariable { get; private set; }
	public static string BundleIdentifierVariable { get; private set; }
	public static string AppCenterTokenVariable { get; private set; }
	public static string AppCenterGroupVariable { get; private set; }
	public static string AppCenterAppNameVariable { get; private set; }
	public static string AppCenterOwnerVariable { get; private set; }

    public static void SetVariableNames(
		string myGetApiKeyVariable = "MYGET_API_KEY",
		string myGetSourceUrlVariable = "MYGET_SOURCE", 
		string nuGetApiKeyVariable = "NUGET_API_KEY",
		string nuGetSourceUrlVariable = "NUGET_SOURCE",
		string microsoftTeamsWebHookUrlVariable = "MICROSOFTTEAMS_WEBHOOKURL",
		string keyStoreVariable = "ANDROID_KEYSTORE",
		string keyStorePasswordVarible = "ANDROID_KEYSTORE_PASSWORD",
		string keyAliasVariable = "ANDROID_KEY_ALIAS",
		string keyPasswordVarible = "ANDROID_KEY_PASSWORD",
		string xamarinBuildConfigurationVariable = "XAMARIN_BUILD_CONFIGURATION",
		string appIconSourceImagePathVariable = "APP_IMG_SRC",
		string appIconDestinationImagePathVariable = "APP_IMG_DEST",
		string bundleIdentifierVariable = "APP_BUNDLE_IDENTIFIER",
		string appCenterTokenVariable = "APPCENTER_API_TOKEN",
		string appCenterGroupVariable = "APPCENTER_GROUP",
		string appCenterAppNameVariable = "APPCENTER_APP_NAME",
		string appCenterOwnerVariable = "APPCENTER_OWNER")
	{
		MyGetApiKeyVariable = myGetApiKeyVariable;
		MyGetSourceUrlVariable = myGetSourceUrlVariable;
		NuGetApiKeyVariable = nuGetApiKeyVariable;
		NuGetSourceUrlVariable = nuGetSourceUrlVariable;
		MicrosoftTeamsWebHookUrlVariable = microsoftTeamsWebHookUrlVariable;
		KeyStoreVariable = keyStoreVariable;
		KeyAliasVariable = keyAliasVariable;
		KeyStorePasswordVariable = keyStorePasswordVarible;
		KeyPasswordVariable = keyPasswordVarible;
		XamarinBuildConfigurationVariable = xamarinBuildConfigurationVariable;
		AppIconSourceImagePathVariable = appIconSourceImagePathVariable;
		AppIconDestinationImagePathVariable = appIconDestinationImagePathVariable;
		BundleIdentifierVariable = bundleIdentifierVariable;
		AppCenterTokenVariable = appCenterTokenVariable;
		AppCenterGroupVariable = appCenterGroupVariable;
		AppCenterAppNameVariable = appCenterAppNameVariable;
		AppCenterOwnerVariable = appCenterOwnerVariable;
	}

    public static void Echo(ICakeContext context)
	{
        context.Information("{0}: {1}", BundleIdentifierVariable, context.EnvironmentVariable(BundleIdentifierVariable));
        context.Information("{0}: {1}", AppCenterOwnerVariable, context.EnvironmentVariable(AppCenterOwnerVariable));
        context.Information("{0}: {1}", AppCenterAppNameVariable, context.EnvironmentVariable(AppCenterAppNameVariable));
        context.Information("{0}: {1}", AppCenterGroupVariable, context.EnvironmentVariable(AppCenterGroupVariable));
        context.Information("{0}: {1}", AppIconSourceImagePathVariable, context.EnvironmentVariable(AppIconSourceImagePathVariable));
        context.Information("{0}: {1}", AppIconDestinationImagePathVariable, context.EnvironmentVariable(AppIconDestinationImagePathVariable));
        context.Information("{0}: {1}", XamarinBuildConfigurationVariable, context.EnvironmentVariable(XamarinBuildConfigurationVariable));
        context.Information("\n");
	}
		
	public static bool ShouldCopyImage
	{
		get
		{
			return !string.IsNullOrEmpty(XamarinBuildConfigurationVariable) && 
				   !string.IsNullOrEmpty(AppIconSourceImagePathVariable) && 
				   !string.IsNullOrEmpty(AppIconDestinationImagePathVariable);
		}
	}
}