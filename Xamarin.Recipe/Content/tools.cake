///////////////////////////////////////////////////////////////////////////////
// TOOLS
///////////////////////////////////////////////////////////////////////////////

private const string GitReleaseManagerTool = "#tool nuget:?package=gitreleasemanager&version=0.7.0";
private const string GitVersionTool = "#tool nuget:?package=GitVersion.CommandLine&version=4.0.0";
private const string MSBuildExtensionPackTool = "#tool nuget:?package=MSBuild.Extension.Pack&version=1.9.0";
private const string XUnitTool = "#tool nuget:?package=xunit.runner.console&version=2.4.1";

Action<string, Action> RequireTool = (tool, action) => 
{
    var script = MakeAbsolute(File(string.Format("./{0}.cake", Guid.NewGuid())));
    try
    {
        var arguments = new Dictionary<string, string>();

        if(BuildParameters.CakeConfiguration.GetValue("NuGet_UseInProcessClient") != null) {
            arguments.Add("nuget_useinprocessclient", BuildParameters.CakeConfiguration.GetValue("NuGet_UseInProcessClient"));
        }

        if(BuildParameters.CakeConfiguration.GetValue("Settings_SkipVerification") != null) {
            arguments.Add("settings_skipverification", BuildParameters.CakeConfiguration.GetValue("Settings_SkipVerification"));
        }

        System.IO.File.WriteAllText(script.FullPath, tool);
        CakeExecuteScript(script,
            new CakeSettings
            {
                Arguments = arguments
            });
    }
    finally
    {
        if (FileExists(script))
        {
            DeleteFile(script);
        }
    }

    action();
};