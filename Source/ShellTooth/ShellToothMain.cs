using System;
using System.Reflection;
using System.Collections.Generic;
using RimWorld;
using Verse;
namespace ShellTooth
{
    [StaticConstructorOnStartup]
    public static class FLAP
    {
        static FLAP()
        {
            List<FodderDef> allDefsListForReading4 = DefDatabase<FodderDef>.AllDefsListForReading;
            Log.Message($"An egg has been laid! Version {ShellTooth.Version}.");
        }
    }
    public class ShellTooth : Mod
    {
        public static string Version
        {
            get
            {
                Version versionNumber = Assembly.GetExecutingAssembly().GetName().Version;
                DateTime buildDate = new DateTime(2000, 1, 1)
                                        .AddDays(versionNumber.Build).AddSeconds(versionNumber.Revision * 2);
                String displayableVersion = $"{versionNumber} assembled on " + buildDate.ToString("MMM dd yyyy");
                return displayableVersion;
            }
        }
        public ShellTooth(ModContentPack content) : base(content)
        {
        }
        [TweakValue("Hatch Time", 0.1f, 7f)]
        public static float YingletEgg = 7;
        public static ThingCategory Fodder = (ThingCategory)12;
    }
}
