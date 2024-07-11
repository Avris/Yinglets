using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using RimWorld;
using UnityEngine;
using System.Net.NetworkInformation;

namespace ShellTooth
{
    internal class JobDriver_LayYingletEgg : JobDriver
    {
        public CompEggContainer EggBoxComp
        {
            get
            {
                return job.GetTarget(TargetIndex.A).Thing.TryGetComp<CompEggContainer>();
            }
        }
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return true;
        }
        // Vanilla-adapted code: clean up
        protected override IEnumerable<Toil> MakeNewToils()
        {
            yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
            yield return Toils_General.Wait(500, TargetIndex.None);
            yield return Toils_General.Do(delegate
            {
                Thing thing = pawn.GetComp<YingComp>().ProduceEgg();
                if (job.GetTarget(TargetIndex.A).HasThing && EggBoxComp.Accepts(thing.def))
                {
                    EggBoxComp.innerContainer.TryAdd(thing, true);
                    return;
                }
                GenPlace.TryPlaceThing(thing, pawn.Position, pawn.Map, ThingPlaceMode.Near, delegate (Thing t, int i)
                {
                    if (pawn.Faction != Faction.OfPlayer)
                    {
                        t.SetForbidden(true, true);
                    }
                }, null, default);
            });
            yield break;
        }
    }
}

