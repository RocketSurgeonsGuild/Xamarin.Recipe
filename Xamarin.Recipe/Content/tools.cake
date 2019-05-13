///////////////////////////////////////////////////////////////////////////////
// TOOLS
///////////////////////////////////////////////////////////////////////////////

private const string GitReleaseManagerTool = "#tool nuget:?package=gitreleasemanager&version=0.7.0";
private const string GitVersionTool = "#tool nuget:?package=GitVersion.CommandLine&version=4.0.0";
private const string WyamTool = "#tool nuget:?package=Wyam&version=1.2.0";
private const string MSBuildExtensionPackTool = "#tool nuget:?package=MSBuild.Extension.Pack&version=1.9.0";
private const string CodecovTool = "#tool nuget:?package=codecov&version=1.5.0";
private const string XUnitTool = "#tool nuget:?package=xunit.runner.console&version=2.4.1";
private const string ReportGeneratorTool = "#tool nuget:?package=ReportGenerator&version=4.1.5";

Action<string, Action> RequireTool = (tool, action) => {
    var script = MakeAbsolute(File(string.Format("./{0}.cake", Guid.NewGuid())));
    try
    {
        System.IO.File.WriteAllText(script.FullPath, tool);
        CakeExecuteScript(script,
            new CakeSettings
            {
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