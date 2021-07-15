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
            Log.Message("Flapping flapper flapped flapping flapper cutely");
            Log.Message($"An egg has been laid! Version {ShellTooth.Version}.");
            SetConfigs();
            reflectUponScendef();
        }
        static void SetConfigs()
        {
            List<string> thingList = new List<string>() { "Plant_TreeAnima", "Wall" };

            ThingDef treeAnima = DefDatabase<ThingDef>.AllDefsListForReading.Find(f => f.defName == "Plant_TreeAnima");
            treeAnima.GetCompProperties<CompProperties_MeditationFocus>().focusTypes.Add(YingDefOf.Whiskery);


            /*SetSpecificPrivateNodeEtc();*/
        }

        /// <summary> Harmony is neat, but fuck that whole convoluted mess.</summary>
        static bool reflectUponScendef() 
        {
            List<ScenarioDef> scenDefs = DefDatabase<ScenarioDef>.AllDefsListForReading;
            foreach (ScenarioDef sd in scenDefs)
            {
                List<ScenPartDef> types = new List<ScenPartDef>() { YingDefOf.EnableForaging };
                var scenfield = sd.scenario.GetType().GetField("parts", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                List<ScenPart> scenparts = (List<ScenPart>)scenfield.GetValue(sd.scenario);
                if (!scenparts.Exists(s => s.def == YingDefOf.EnableForaging))
                {
                    scenparts.Add(ScenarioMaker.MakeScenPart(YingDefOf.EnableForaging));
                }
            }
            return true;
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
