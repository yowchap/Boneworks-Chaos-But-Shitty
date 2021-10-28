using MelonLoader;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle(YOWC.ShittyBWChaos.BuildInfo.Name)]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany(YOWC.ShittyBWChaos.BuildInfo.Company)]
[assembly: AssemblyProduct(YOWC.ShittyBWChaos.BuildInfo.Name)]
[assembly: AssemblyCopyright("Created by " + YOWC.ShittyBWChaos.BuildInfo.Author)]
[assembly: AssemblyTrademark(YOWC.ShittyBWChaos.BuildInfo.Company)]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
//[assembly: Guid("")]
[assembly: AssemblyVersion(YOWC.ShittyBWChaos.BuildInfo.Version)]
[assembly: AssemblyFileVersion(YOWC.ShittyBWChaos.BuildInfo.Version)]
[assembly: NeutralResourcesLanguage("en")]
[assembly: MelonInfo(typeof(YOWC.ShittyBWChaos.Main), YOWC.ShittyBWChaos.BuildInfo.Name, YOWC.ShittyBWChaos.BuildInfo.Version, YOWC.ShittyBWChaos.BuildInfo.Author, YOWC.ShittyBWChaos.BuildInfo.DownloadLink)]


// Create and Setup a MelonModGame to mark a Mod as Universal or Compatible with specific Games.
// If no MelonModGameAttribute is found or any of the Values for any MelonModGame on the Mod is null or empty it will be assumed the Mod is Universal.
// Values for MelonModGame can be found in the Game's app.info file or printed at the top of every log directly beneath the Unity version.
[assembly: MelonGame("Stress Level Zero", "BONEWORKS")]
[assembly: MelonPriorityAttribute(int.MaxValue)]