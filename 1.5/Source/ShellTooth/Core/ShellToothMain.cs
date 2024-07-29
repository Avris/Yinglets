using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;
using UnityEngine;
using LudeonTK;
using System.Runtime;

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
        public static ShelltoothSettings settings;
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
            settings = GetSettings<ShelltoothSettings>();
        }

        public override string SettingsCategory() => "Yinglets!"; 
        
        public override void DoSettingsWindowContents(Rect inRect)
        {
            base.DoSettingsWindowContents(inRect);
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);
            listingStandard.CheckboxLabeled(label: "Enable Debug", ref ShelltoothSettings.debugMode, tooltip: "Enables the Debug mode.");
            listingStandard.End();
        }
        public override void WriteSettings()
        {
            base.WriteSettings();
            settings.Update();
        }
        public static string currentVersion = "Tiplod Update 2 Dev";
        public static void DoNothing()
        {
            // Blame Tynan for this needing to exist.
        }
    }
    public class ShelltoothSettings : ModSettings 
    {

        public static bool debugMode;
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref debugMode, "DebugMode", false);
        }
        public void Update()
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
