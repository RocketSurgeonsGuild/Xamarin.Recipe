// #addin "nuget:https://api.nuget.org/v3/index.json?package=Cake.Xamarin&version=1.3.0.15"
// using System.Xml.Linq;
// using System.Security.Cryptography.X509Certificates;

// var appxManifest = File("./src/App.UWP/Package.appxmanifest");
// var appPackageDirectory = Directory("./src/App.UWP/AppPackages");

// Task("UWP")
//     .IsDependentOn("UWP-Version")
// 	.Does(()=>
// 	{
//         var uwpProject = File("./src/App.UWP/App.UWP.csproj");

// 		Information("Building UWP Target: {0}", MakeAbsolute(uwpProject));

// 		MSBuild(uwpProject, settings =>
// 				settings
// 					.WithTarget("Publish")
//                     .SetVerbosity(Verbosity.Minimal)
//                     .UseToolVersion(MSBuildToolVersion.VS2015)
// 					.SetConfiguration(configuration)
//                     .WithProperty("ProjectName", "App")
// 					.WithProperty("TreatWarningsAsErrors","false")
// 					.WithProperty("AppxBundle","Always")
// 					.WithProperty("AppxBundlePlatforms","x86"));
// 	});

// Task("UWP-Version")
//     .IsDependentOn("Version")
//     .Does(() =>
//     {
//         Information("Build Version: {0}", BuildVersion.Version);
//         Information("NuGet Version: {0}", BuildVersion.NuGetVersion);
//         Information("Semantic Version: {0}", BuildVersion.SemVersion);
//         Information("Assembly Semantic Version {0}", BuildVersion.AssemblySemVer);
//         Information("Full Semantic Version {0}", BuildVersion.FullSemVer);

//         XDocument packageManifest = XDocument.Load(appxManifest);

//         const string xmlNameSpace = "http://schemas.microsoft.com/appx/manifest/foundation/windows10";

//         var manifestProperties = (from manifest in packageManifest.Elements()
//                                     let identity = manifest.Descendants("{" + xmlNameSpace + "}" + "Identity")
//                                     select new
//                                     {
//                                         Version = identity.Attributes("Version")
//                                                         .Select(semVer => semVer.Value)
//                                                         .FirstOrDefault()
//                                     }).FirstOrDefault();

//         var package = packageManifest.Elements().ToList();
//         if (manifestProperties != null)
//         {
//             if (package.Elements("{" + xmlNameSpace + "}" + "Identity").Attributes("Version").Any())
//             {
//                 package.Elements("{" + xmlNameSpace + "}" + "Identity").Attributes("Version").Single().Value = BuildVersion.AssemblySemVer;
//             }

//             packageManifest.Save(appxManifest, SaveOptions.None);
//         }
//     });

// Task("UWP-Bundle-Identifier")
//     .Does(() =>
//     {

//     });

// Task("Install-Certificate")
//     .ContinueOnError()
//     .Does(() =>
//     {            
//         var personalStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
//         try
//         {
//             X509Certificate2Collection collection = new X509Certificate2Collection();
//             var cert = File("./src/App.UWP/App.UWP_TemporaryKey.pfx");
//             var password = EnvironmentVariable("CERT_PWD");

//             if(string.IsNullOrEmpty(password))
//             {
//                 Warning("Password is not specified.");
//             }

//             collection.Import(cert, password, X509KeyStorageFlags.MachineKeySet);

//             personalStore.Open(OpenFlags.ReadWrite);
//             personalStore.AddRange(collection);
//         }
//         finally
//         {
//             personalStore.Close();
//         }
//     });

// Task("Zip-Bundle")
//     .IsDependentOn("UWP")
//     .Does(() =>
//     {
//         EnsureDirectoryExists("./artifacts/uwp");
//         var appPackageDirectories = GetDirectories("./src/**/AppPackages/App_*");

//         foreach(var directory in appPackageDirectories)
//         {
//             Information("Directory Path: {0}", directory.FullPath);
//         }

//         if(!appPackageDirectories.Skip(1).Any())
//         {
//             Information("Packaging Directory: {0}", appPackageDirectories.FirstOrDefault());
//             Zip(appPackageDirectories.FirstOrDefault(), "./artifacts/uwp/App.UWP_" + BuildVersion.AssemblySemVer + ".zip");
//         }
//     });