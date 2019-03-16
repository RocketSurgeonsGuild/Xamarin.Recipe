using Nuke.Common;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

partial class XamarinBuild
{
    Target DotNetRestore => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
            DotNetTasks.DotNetRestore(restore => restore.SetProjectFile(Solution));
        });

    Target DotNetBuild => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
            DotNetTasks.DotNetBuild(restore => restore.SetProjectFile(Solution));
        });

    Target DotNetPack => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
            DotNetTasks.DotNetRestore(restore => restore.SetProjectFile(Solution));
        });
}
