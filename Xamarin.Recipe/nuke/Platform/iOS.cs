using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Nuke.Common;

partial class XamarinBuild
{
    Expression<Func<bool>> IsiOSPlatform = () => Parameters.Target == Platform.iOS;

    Target iOSBuild => _ => _
        .OnlyWhenStatic(IsiOSPlatform)
        .Executes();

    Target iOSArchive => _ => _
        .OnlyWhenStatic(IsiOSPlatform)
        .Executes();

    Target PublishIpa => _ => _
        .OnlyWhenStatic(IsiOSPlatform);
}
