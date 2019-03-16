using System;
using System.Collections.Generic;
using System.Text;
using static Nuke.Common.IO.PathConstruction;

partial class XamarinBuild
{
    AbsolutePath SourceDirectory => RootDirectory / "src";
    AbsolutePath TestsDirectory => RootDirectory / "test";
    AbsolutePath BenchmarkDirectory => RootDirectory / "benchmarks";
    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";
    AbsolutePath PackageDirectory => ArtifactsDirectory / "packages";
    AbsolutePath BenchmarkResultsDirectory => ArtifactsDirectory / "benchmarks";
    AbsolutePath TestResultsDirectory => ArtifactsDirectory / "TestResults";
    AbsolutePath TestResultFilePath => TestResultsDirectory / $"{Parameters.Title}.trx";
}
