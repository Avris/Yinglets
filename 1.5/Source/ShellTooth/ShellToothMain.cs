using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;
using UnityEngine;
using LudeonTK;

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
        public static bool debugMode;
        public ShellTooth(ModContentPack content) : base(content)
        {
        }
        [TweakValue("Yinglets: Chance to fertilise egg", 0f, 1f)]
        public static float yingletEggChance = 0.5f;
        [TweakValue("Yinglets: Younglet hatch time", 0.01f, 7f)]
        public static float yingletEgg = 4f;
        [TweakValue("Yinglets: Gestation time", 1f, 120f)]
        public static float yingletGestation = 120f;
        [TweakValue("Yinglets: Younglet adulthood minimum in days", 0f, 14f)]
        public static float yingletAdultDaysMinimum = 14f;
    }
}
