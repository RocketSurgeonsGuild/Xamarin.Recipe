public static class Environment
{
	public static string AppCenterAppNameVariable { get; private set; }
	public static string AppCenterGroupVariable { get; private set; }
	public static string AppCenterOwnerVariable { get; private set; }
	public static string AppCenterTokenVariable { get; private set; }
	public static string XamarinBuildConfigurationVariable { get; private set; }
	public static string BundleIdentifierVariable { get; private set; }
	public static string AppIconSourceImagePathVariable { get; private set; }
	public static string AppIconDestinationImagePathVariable { get; private set; }
	public static string KeyStoreVariable { get; private set; }
	public static string KeyAliasVariable { get; private set; }
	public static string KeyStorePasswordVariable { get; private set; }
	public static string KeyPasswordVariable { get; private set; }

    public static void SetVariableNames(
		string appCenterAppNameVariable = "APPCENTER_APP_NAME",
		string appCenterGroupVariable = "APPCENTER_GROUP",
		string appCenterOwnerVariable = "APPCENTER_OWNER",
		string appCenterTokenVariable = "APPCENTER_API_TOKEN",
		string appIconSourceImagePathVariable = "APP_IMG_SRC",
		string appIconDestinationImagePathVariable = "APP_IMG_DEST",
		string xamarinBuildConfigurationVariable = "XAMARIN_BUILD_CONFIGURATION",
		string bundleIdentifierVariable = "APP_BUNDLE_IDENTIFIER",
		string keyStoreVariable = "ANDROID_KEYSTORE",
		string keyStorePasswordVarible = "ANDROID_KEYSTORE_PASSWORD",
		string keyAliasVariable = "ANDROID_KEY_ALIAS",
		string keyPasswordVarible = "ANDROID_KEY_PASSWORD")
	{
		AppCenterAppNameVariable = appCenterAppNameVariable;
		AppCenterGroupVariable = appCenterGroupVariable;
		AppCenterOwnerVariable = appCenterOwnerVariable;
		AppCenterTokenVariable = appCenterTokenVariable;
		XamarinBuildConfigurationVariable = xamarinBuildConfigurationVariable;
		BundleIdentifierVariable = bundleIdentifierVariable;
		AppIconSourceImagePathVariable = appIconSourceImagePathVariable;
		AppIconDestinationImagePathVariable = appIconDestinationImagePathVariable;
		KeyStoreVariable = keyStoreVariable;
		KeyStorePasswordVariable = keyStorePasswordVarible;
		KeyAliasVariable = keyAliasVariable;
		KeyPasswordVariable = keyPasswordVarible;
	}

    public static void Echo(ICakeContext context)
	{
        context.Information("==================== ECHO ====================");
        context.Information("{0}: {1}", BundleIdentifierVariable, context.EnvironmentVariable(BundleIdentifierVariable));
        context.Information("{0}: {1}", AppCenterOwnerVariable, context.EnvironmentVariable(AppCenterOwnerVariable));
        context.Information("{0}: {1}", AppCenterAppNameVariable, context.EnvironmentVariable(AppCenterAppNameVariable));
        context.Information("{0}: {1}", AppCenterGroupVariable, context.EnvironmentVariable(AppCenterGroupVariable));
        context.Information("{0}: {1}", AppIconSourceImagePathVariable, context.EnvironmentVariable(AppIconSourceImagePathVariable));
        context.Information("{0}: {1}", AppIconDestinationImagePathVariable, context.EnvironmentVariable(AppIconDestinationImagePathVariable));
        context.Information("{0}: {1}", XamarinBuildConfigurationVariable, context.EnvironmentVariable(XamarinBuildConfigurationVariable));
        context.Information("\n");
        context.Information("{0}: {1}", KeyStoreVariable, context.EnvironmentVariable(KeyStoreVariable));
        context.Information("{0}: {1}", KeyAliasVariable, context.EnvironmentVariable(KeyAliasVariable));
        context.Information("{0}: {1}", KeyStorePasswordVariable, context.EnvironmentVariable(KeyStorePasswordVariable));
        context.Information("{0}: {1}", KeyPasswordVariable, context.EnvironmentVariable(KeyPasswordVariable));
        context.Information("\n");
	}

	public static void Print(ICakeContext context)
	{
        context.Information("{0}: {1}", nameof(AppCenterAppNameVariable), AppCenterAppNameVariable);
        context.Information("{0}: {1}", nameof(AppCenterGroupVariable), AppCenterGroupVariable);
        context.Information("{0}: {1}", nameof(AppCenterOwnerVariable), AppCenterOwnerVariable);
        context.Information("{0}: {1}", nameof(AppCenterTokenVariable), AppCenterTokenVariable);
        context.Information("{0}: {1}", nameof(XamarinBuildConfigurationVariable), XamarinBuildConfigurationVariable);
        context.Information("{0}: {1}", nameof(BundleIdentifierVariable), BundleIdentifierVariable);
        context.Information("{0}: {1}", nameof(AppIconSourceImagePathVariable), AppIconSourceImagePathVariable);
        context.Information("{0}: {1}", nameof(AppIconDestinationImagePathVariable), AppIconDestinationImagePathVariable);
        context.Information("{0}: {1}", nameof(KeyStoreVariable), KeyStoreVariable);
        context.Information("{0}: {1}", nameof(KeyStorePasswordVariable), KeyStorePasswordVariable);
        context.Information("{0}: {1}", nameof(KeyAliasVariable), KeyAliasVariable);
        context.Information("{0}: {1}", nameof(KeyPasswordVariable), KeyPasswordVariable);
        context.Information("\n");
	}
		
	public static bool ShouldCopyImage =>
		!string.IsNullOrEmpty(XamarinBuildConfigurationVariable) &&
		!string.IsNullOrEmpty(AppIconSourceImagePathVariable) &&
		!string.IsNullOrEmpty(AppIconDestinationImagePathVariable);
}