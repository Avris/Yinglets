using System;
using System.Collections.Generic;
using Verse;
using RimWorld;
using UnityEngine;
using AlienRace;  
using System.Linq;
using System.Security.Cryptography;
using Verse.AI;
using System.Diagnostics.Eventing.Reader;

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
        public bool isDesignatedBreeder = false;
        public bool checkedScenpart = false;
        public bool wasYounglet = false;
        public bool sterile = false;
        public string wasOtherRace;
        public Pawn knockedUpBy;
        private Pawn ying 
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

            if (ying.jobs.curDriver != null && ying.jobs.curDriver.asleep)
            {
                if (ying.CurrentBed() != null && !ying.CurrentBed().def.building.bed_showSleeperBody && !ying.health.hediffSet.HasHediff(YingDefOf.Sleepying))
                {
                    ying.health.AddHediff(YingDefOf.Sleepying);
                }
                else
                {
                    ying.health.AddHediff(YingDefOf.YingShuteye);
                }
            }
            else if (ying.health.hediffSet.HasHediff(YingDefOf.Sleepying) || ying.health.hediffSet.HasHediff(YingDefOf.YingShuteye))
            {
                ying.health.hediffSet.hediffs.Remove(ying.health.hediffSet.GetFirstHediffOfDef(YingDefOf.Sleepying));
                ying.health.hediffSet.hediffs.Remove(ying.health.hediffSet.GetFirstHediffOfDef(YingDefOf.YingShuteye));
                ying.Drawer.renderer.SetAllGraphicsDirty();
            }

            // This is more expensive than I'd like. Maybe apply it directly from the sterilisation process?
            if (!sterile && ying.health.hediffSet.HasHediff(HediffDefOf.Sterilized))
            {
                sterile = true;
                isDesignatedBreeder = false;
            }

            if (ying.def == YingDefOf.Alien_Yinglet)
            {

                if (Find.TickManager.TicksGame % ShelltoothSettings.yingletGestation == 0)
                {
                    ReapplyYinglet(ying);
                    if ((ying.gender == Gender.Female) && (eggProgress > 0))
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
        }
        public override string CompInspectStringExtra()
        {
            if (ying.gender != Gender.Female || ying.def != YingDefOf.Alien_Yinglet)
            {
                return null;
            }
            string report = "Matriarch's Report: ";
            if (eggProgress == 0)
            {
                if (!ying.ageTracker.CurLifeStage.reproductive)
                {
                    return report + "too young to lay eggs!".Colorize(new Color(1, 1, 0));
                }
                if (sterile)
                {
                    return report + "sterilised - can't lay eggs.".Colorize(new Color(1, 0, 0));
                }
                return report + "no egg fertilised.".Colorize(new Color(0, 1, 0));
            }
            if (Find.TickManager.TicksGame % 60 < 30)
            {
                return report + $"egg progress at { eggProgress.ToStringPercent()}".Colorize(new Color(0, 1, 1));
            }
            else
            {
                return report + $"egg progress at {eggProgress.ToStringPercent()}".Colorize(new Color(0, 0.85f, 1));
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
        public virtual Thing ProduceEgg()
        {
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
            if (ShelltoothSettings.debugMode) 
            {
                yield return DebugGizmoWindow.GetGizmo(this);
                yield return DebugGizmoGiveEgg.GetGizmo(this);
            }
            if (!EggMaker.YingSterile(ying))
            {
                if (eggProgress == 0)
                {
                    yield return GizmoBreeder.GetGizmo(this);
                }
                else
                {
                    yield return GizmoHasEgg.GetGizmo(this);
                }
            }
            else
            {
                if (!ying.ageTracker.CurLifeStage.reproductive)
                {
                    yield return GizmoTooYoung.GetGizmo(this);
                }
                else if (ying.health.hediffSet.HasHediffPreventsPregnancy())
                {
                    yield return GizmoSterile.GetGizmo(this);
                }
            }
        }
    }
}
