using System;
using System.Collections.Generic;
using Verse;
using RimWorld;
using UnityEngine;
using AlienRace;
using ShellTooth.Core;
using System.Linq;

namespace ShellTooth
{
    public class CompProperties_Yinglet : CompProperties
    {
        public CompProperties_Yinglet()
        {
            this.compClass = typeof(YingComp);
        }
    }
    public class YingComp : ThingComp
    {
        public string updateStamp = "none";
        public bool checkedScenpart = false;
        public bool isDesignatedBreeder = false;
        public bool wasYounglet = false;
        public string wasOtherRace;
        public override void PostExposeData()
        {
            base.PostExposeData();

            Scribe_Values.Look(ref this.isDesignatedBreeder, label: "isDesignatedBreeder");
            Scribe_Values.Look(ref this.updateStamp, label: "updateStamp");
            Scribe_Values.Look(ref this.wasYounglet, label: "wasYounglet");
            Scribe_Values.Look(ref this.wasOtherRace, label: "wasOtherRace");
        }
        static bool reflectUponScendef()
        {
            List<ScenarioDef> scenDefs = DefDatabase<ScenarioDef>.AllDefsListForReading;
            foreach (ScenarioDef sd in scenDefs)
            {
                List<ScenPartDef> types = new List<ScenPartDef>() { YingDefOf.YingletDriver };
                var scenfield = sd.scenario.GetType().GetField("parts", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                List<ScenPart> scenparts = (List<ScenPart>)scenfield.GetValue(sd.scenario);
                if (!scenparts.Exists(s => s.def == YingDefOf.YingletDriver))
                {
                    scenparts.Add(ScenarioMaker.MakeScenPart(YingDefOf.YingletDriver));
                }
            }
            return true;
        }
        public override void CompTick()
        {
            if (!checkedScenpart)
            {
                if (!Current.Game.Scenario.AllParts.Any((ScenPart part) => part is ScenPart_YingletDriver))
                {
                    var scenario = Current.Game.Scenario.GetType().GetField("parts", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    List<ScenPart> scenparts = (List<ScenPart>)scenario.GetValue(Current.Game.Scenario);
                    scenparts.Add(ScenarioMaker.MakeScenPart(YingDefOf.YingletDriver));
                    Log.Warning("ShellTooth: restored missing scenpart, please don't remove that!");
                }
            }
            if (Find.TickManager.TicksGame % 100 == 0)
            {
                ReapplyYinglet(this.parent as Pawn);
            }
        }
        public static void ReapplyYinglet(Pawn pawn)
        {
            if (!(pawn.health.hediffSet.HasHediff(YingDefOf.Yingletness)))
            {
                pawn.health.AddHediff(YingDefOf.Yingletness);
                Log.Warning($"Reapplied missing yingletness to {pawn}");
            }
        }
        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            Texture2D heart = ContentFinder<Texture2D>.Get("Things/Mote/Heart", true);
            Texture2D noheart = ContentFinder<Texture2D>.Get("Things/Mote/SpeechSymbols/Breakup", true);
            Command_Toggle commandToggle = new Command_Toggle
            {
                defaultLabel = isDesignatedBreeder ? "Breeding ON" : "Breeding OFF",
                defaultDesc = isDesignatedBreeder ?
                $"{this.parent} is assigned to breeding. Putting two available colonists in the same bed might result in an egg!" :
                $"{this.parent} is not assigned to breeding. Breeding attempts are unlikely in this state!",
                icon = isDesignatedBreeder ? heart : noheart,
                tutorTag = "MakeBreedable",
                isActive = () => isDesignatedBreeder,
                toggleAction = delegate ()
                {
                    isDesignatedBreeder = !isDesignatedBreeder;
                },
            };
            yield return commandToggle;
        }
    }
}
