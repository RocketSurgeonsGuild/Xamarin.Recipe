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

    public static void SetVariableNames(
		string myGetApiKeyVariable = null,
		string myGetSourceUrlVariable = null, 
		string nuGetApiKeyVariable = null,
		string nuGetSourceUrlVariable = null,
		string microsoftTeamsWebHookUrlVariable = null,
		string keyStoreVariable = null,
		string keyStoreAliasVariable = null,
		string keyStorePasswordVarible = null)
    {
		MyGetApiKeyVariable = myGetApiKeyVariable ?? "MYGET_API_KEY";
		MyGetSourceUrlVariable = myGetSourceUrlVariable ?? "MYGET_SOURCE";
		NuGetApiKeyVariable = nuGetApiKeyVariable ?? "NUGET_API_KEY";
		NuGetSourceUrlVariable = nuGetSourceUrlVariable ?? "NUGET_SOURCE";
		MicrosoftTeamsWebHookUrlVariable = microsoftTeamsWebHookUrlVariable ?? "MICROSOFTTEAMS_WEBHOOKURL";
		KeyStoreVariable = keyStoreVariable ?? "ANDROID_KEYSTORE";
		KeyStoreAliasVariable = keyStoreAliasVariable ?? "ANDROID_KEYSTORE_ALIAS";
		KeyStorePasswordVariable = keyStorePasswordVarible ?? "ANDROID_KEYSTORE_PASSWORD";
    }
}