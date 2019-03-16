using System;
using System.Collections.Generic;
using System.Text;
using Configuration;
using Nuke.Common;

partial class XamarinBuild
{
    [Parameter] string Title { get; }

    [Parameter]
    Platform Target { get; }

    [Parameter("Configuration to build - Default is 'Dev'")]
    MobileConfiguration Configuration { get; } = MobileConfiguration.Dev;

    [Parameter] string AndroidProject { get; }

    [Parameter] string iOSProject { get; }

    [Parameter] bool CopyImages { get; }

    [Parameter] string BundleIdenfitier { get; }

    [Parameter] string RepositoryIdenfitier { get; }
}
