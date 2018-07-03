public static class Environment
{
	public static string MyGetApiKeyVariable { get; private set; }
	public static string MyGetSourceUrlVariable { get; private set; }
	public static string NuGetApiKeyVariable { get; private set; }
	public static string NuGetSourceUrlVariable { get; private set; }
	public static string MicrosoftTeamsWebHookUrlVariable { get; private set; }
	public static string KeyStoreVariable { get; private set; }
	public static string KeyStoreAliasVariable { get; private set; }
	public static string KeyStorePasswordVariable { get; private set; }
	public static string XamarinBuildConfigurationVariable { get; private set; }
	public static string AppIconSourceImagePathVariable { get; private set; }
	public static string AppIconDestinationImagePathVariable { get; private set; }
	public static string BundleIdentifier { get; private set; }

    public static void SetVariableNames(
		string myGetApiKeyVariable = "MYGET_API_KEY",
		string myGetSourceUrlVariable = "MYGET_SOURCE", 
		string nuGetApiKeyVariable = "NUGET_API_KEY",
		string nuGetSourceUrlVariable = "NUGET_SOURCE",
		string microsoftTeamsWebHookUrlVariable = "MICROSOFTTEAMS_WEBHOOKURL",
		string keyStoreVariable = "ANDROID_KEYSTORE",
		string keyStoreAliasVariable = "ANDROID_KEYSTORE_ALIAS",
		string keyStorePasswordVarible = "ANDROID_KEYSTORE_PASSWORD",
		string xamarinBuildConfigurationVariable = "XAMARIN_BUILD_CONFIGURATION",
		string appIconSourceImagePathVariable = "APP_IMG_SRC",
		string appIconDestinationImagePathVariable = "APP_IMG_DEST",
		string bundleIdentifier = "APP_BUNDLE_IDENTIFIER")
		{
			MyGetApiKeyVariable = myGetApiKeyVariable;
			MyGetSourceUrlVariable = myGetSourceUrlVariable;
			NuGetApiKeyVariable = nuGetApiKeyVariable;
			NuGetSourceUrlVariable = nuGetSourceUrlVariable;
			MicrosoftTeamsWebHookUrlVariable = microsoftTeamsWebHookUrlVariable;
			KeyStoreVariable = keyStoreVariable;
			KeyStoreAliasVariable = keyStoreAliasVariable;
			KeyStorePasswordVariable = keyStorePasswordVarible;
			XamarinBuildConfigurationVariable = xamarinBuildConfigurationVariable;
			AppIconSourceImagePathVariable = appIconSourceImagePathVariable;
			AppIconDestinationImagePathVariable = appIconDestinationImagePathVariable;
			BundleIdentifier = bundleIdentifier;
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