using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;
namespace ShellTooth
{
    [StaticConstructorOnStartup]
    public static class FLAP
    {
        static FLAP()
        {
            Log.Message("Flapping flapper flapped flapping flapper cutely!!!");
            Log.Message($"An egg has been laid! Version {ShellTooth.Version}.");
            SetConfigs();
        }
        static void SetConfigs()
        {
            List<string> thingList = new List<string>() { "Plant_TreeAnima", "Wall" };

            ThingDef treeAnima = DefDatabase<ThingDef>.AllDefsListForReading.Find(f => f.defName == "Plant_TreeAnima");
            treeAnima.GetCompProperties<CompProperties_MeditationFocus>().focusTypes.Add(YingDefOf.Whiskery);


            /*SetSpecificPrivateNodeEtc();*/
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
        [TweakValue("Younglet Hatch Time", 0.1f, 7f)]
        public static float YingletEgg = 7;
        [TweakValue("Younglet Adulthood Time", 0.1f, 7f)]
        public static float A = 7;
        public static ThingCategory Fodder = (ThingCategory)12;
    }
}
