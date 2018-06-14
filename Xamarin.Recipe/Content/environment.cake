public static class Environment
{
	public static string MyGetApiKeyVariable { get; private set; }
	public static string MyGetSourceUrlVariable { get; private set; }
	public static string NuGetApiKeyVariable { get; private set; }
	public static string NuGetSourceUrlVariable { get; private set; }
	public static string MicrosoftTeamsWebHookUrlVariable { get; private set; }

    public static void SetVariableNames(
		string myGetApiKeyVariable = null,
        string myGetSourceUrlVariable = null, 
        string nuGetApiKeyVariable = null,
        string nuGetSourceUrlVariable = null,
        string microsoftTeamsWebHookUrlVariable = null)
    {
		MyGetApiKeyVariable = myGetApiKeyVariable ?? "MYGET_API_KEY";
		MyGetSourceUrlVariable = myGetSourceUrlVariable ?? "MYGET_SOURCE";
		NuGetApiKeyVariable = nuGetApiKeyVariable ?? "NUGET_API_KEY";
		NuGetSourceUrlVariable = nuGetSourceUrlVariable ?? "NUGET_SOURCE";
		MicrosoftTeamsWebHookUrlVariable = microsoftTeamsWebHookUrlVariable ?? "MICROSOFTTEAMS_WEBHOOKURL";
    }
}