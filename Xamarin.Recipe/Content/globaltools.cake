///////////////////////////////////////////////////////////////////////////////
// TOOLS
///////////////////////////////////////////////////////////////////////////////

private const string CoverletTool = "#tool dotnet:?package=coverlet.console&version=1.5.1";
private const string NerdBankGitVersioning = "#tool dotnet:?package=nbgv&version=2.3.125";
private const string Module = "#module nuget:?package=Cake.DotNetTool.Module&version=0.3.0";

Action<string, Action> RequireGlobalTool = (tool, action) => {
    var script = MakeAbsolute(File(string.Format("./{0}.cake", Guid.NewGuid())));
    try
    {
        var stringBuilder = new StringBuilder();

        stringBuilder.AppendLine(Module);
        stringBuilder.AppendLine(tool);

        System.IO.File.WriteAllText(script.FullPath, stringBuilder.ToString());
        CakeExecuteScript(script, new CakeSettings { ArgumentCustomization = args => args.Append("--bootstrap") });
        CakeExecuteScript(script);
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