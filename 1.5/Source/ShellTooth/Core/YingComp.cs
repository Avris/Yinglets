using System;
using System.Collections.Generic;
using Verse;
using RimWorld;
using UnityEngine;
using AlienRace;  
using System.Linq;

namespace ShellTooth
{
    public class CompProperties_Yinglet : CompProperties
    {
        public CompProperties_Yinglet()
        {
            compClass = typeof(YingComp);
        }
    }
    public class YingComp : ThingComp
    {
        public float eggProgress = 0;
        public string updateStamp = "none";
        public bool checkedScenpart = false;
        public bool isDesignatedBreeder = false;
        public bool wasYounglet = false;
        public string wasOtherRace;
        public Pawn knockedUpBy;

        private Pawn thisPawn 
        {
            get
            {
                return parent as Pawn;
            }
        }
        public override void PostExposeData()
        {
            base.PostExposeData();

            Scribe_Values.Look(ref eggProgress, label: "eggProgress", defaultValue: 0f);
            Scribe_Values.Look(ref isDesignatedBreeder, label: "isDesignatedBreeder");
            Scribe_Values.Look(ref updateStamp, label: "updateStamp");
            Scribe_Values.Look(ref wasYounglet, label: "wasYounglet");
            Scribe_Values.Look(ref wasOtherRace, label: "wasOtherRace");
        }
        public override void CompTick()
        {
            if (!checkedScenpart)
            {
                if (!Current.Game.Scenario.AllParts.Any((ScenPart part) => part is ScenPart_YingletWorker))
                {
                    var scenario = Current.Game.Scenario.GetType().GetField("parts", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    List<ScenPart> scenparts = (List<ScenPart>)scenario.GetValue(Current.Game.Scenario);
                    scenparts.Add(ScenarioMaker.MakeScenPart(YingDefOf.YingletWorker));
                    Log.Warning("ShellTooth: restored missing scenpart, please don't remove that!");
                }
            }
            if (Find.TickManager.TicksGame % ShellTooth.yingletGestation == 0)
            {
                ReapplyYinglet(thisPawn);
                if ((thisPawn.gender == Gender.Female) && (eggProgress > 0))
                {
                    if (eggProgress != 1f)
                    {
                        if (eggProgress < 0.99f)
                        {
                            eggProgress += 0.01f;
                        }
                        else
                        {
                            eggProgress = 1;
                        }
                    }
                }
            }
        }
        public override string CompInspectStringExtra()
        {
            if (eggProgress == 0)
            {
                return null;
            }
            return "EggProgress".Translate() + ": " + eggProgress.ToStringPercent();
        }
        public static void ReapplyYinglet(Pawn pawn)
        {
            if (!(pawn.health.hediffSet.HasHediff(YingDefOf.Yingletness)))
            {
                pawn.health.AddHediff(YingDefOf.Yingletness);
                Log.Warning($"Reapplied missing yingletness to {pawn}");
            }
        }
        public virtual Thing ProduceEgg()
        {
            eggProgress = 0f;
            Thing thing = ThingMaker.MakeThing(DefDatabase<ThingDef>.GetNamed("EggYingletFertilized"), null);
            CompHatcher compHatcher = thing.TryGetComp<CompHatcher>();
            if (compHatcher != null)
            {
                compHatcher.hatcheeFaction = parent.Faction;
                Pawn pawn = parent as Pawn;
                if (pawn != null)
                {
                    compHatcher.hatcheeParent = pawn;
                }
                if (knockedUpBy != null)
                {
                    compHatcher.otherParent = knockedUpBy;
                }
            }
            return thing;
        }
        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            Texture2D heart = ContentFinder<Texture2D>.Get("Things/Mote/Heart", true);
            Texture2D noheart = ContentFinder<Texture2D>.Get("Things/Mote/SpeechSymbols/Breakup", true);
            Command_Toggle commandToggle = new Command_Toggle
            {
                defaultLabel = isDesignatedBreeder ? "Breeding ON" : "Breeding OFF",
                defaultDesc = isDesignatedBreeder ?
                $"{parent} is assigned to breeding. Putting two available yinglets in the same bed might result in an egg!" :
                $"{parent} is not assigned to breeding. Breeding attempts won't happen!",
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
