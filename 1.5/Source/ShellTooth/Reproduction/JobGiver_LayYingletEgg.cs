using System;
using Verse;
using Verse.AI;
using RimWorld;

namespace ShellTooth
{
    public class JobGiver_LayYingletEgg : ThinkNode_JobGiver
    {
        // Vanilla-adapted code: clean up
        protected override Job TryGiveJob(Pawn pawn)
        {
            Job result = null; 
            if ((pawn.def == YingDefOf.Alien_Yinglet) && (pawn.GetComp<YingComp>().eggProgress == 1))
            {
                PathEndMode peMode = PathEndMode.OnCell;
                TraverseParms traverseParms = TraverseParms.For(pawn, Danger.Some, TraverseMode.ByPawn, false, false, false);
                if (pawn.Faction == Faction.OfPlayer)
                {
                    Thing bestEggBox = GetBestEggBox(pawn, peMode, traverseParms);
                    if (bestEggBox != null)
                    {
                        result = JobMaker.MakeJob(YingDefOf.LayYingletEgg, bestEggBox);
                    }

                }
                Thing thing = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(DefDatabase<ThingDef>.GetNamed("EggYingletFertilized")), peMode, traverseParms, 30f, (Thing x) => pawn.GetRoom(RegionType.Set_All) == null || x.GetRoom(RegionType.Set_All) == pawn.GetRoom(RegionType.Set_All), null, 0, -1, false, RegionType.Set_Passable, false);
                result = JobMaker.MakeJob(YingDefOf.LayYingletEgg, (thing != null) ? thing.Position : RimWorld.RCellFinder.RandomWanderDestFor(pawn, pawn.Position, 5f, null, Danger.Some));
            }
            return result;
        }
        private Thing GetBestEggBox(Pawn pawn, PathEndMode peMode, TraverseParms tp)
        {
            ThingDef eggDef = DefDatabase<ThingDef>.GetNamed("EggYingletFertilized");
            return GenClosest.ClosestThing_Regionwise_ReachablePrioritized(pawn.Position, pawn.Map, ThingRequest.ForDef(ThingDefOf.EggBox), peMode, tp, 30f, new Predicate<Thing>(IsUsableBox), new Func<Thing, float>(GetScore), 10);

            bool IsUsableBox(Thing thing)
            {
                if (!thing.Spawned || thing.IsForbidden(pawn) || !pawn.CanReserve((LocalTargetInfo)thing) || !pawn.Position.InHorDistOf(thing.Position, 30f))
                    return false;
                CompEggContainer comp = thing.TryGetComp<CompEggContainer>();
                return comp != null && comp.Accepts(eggDef);
            }

            float GetScore(Thing thing)
            {
                CompEggContainer comp = thing.TryGetComp<CompEggContainer>();
                if (comp == null || comp.Full)
                    return 0.0f;
                int? stackCount = comp.ContainedThing?.stackCount;
                return (stackCount.HasValue ? new float?(stackCount.GetValueOrDefault() * 5f) : new float?()) ?? 0.5f;
            }
        }
    }
}
