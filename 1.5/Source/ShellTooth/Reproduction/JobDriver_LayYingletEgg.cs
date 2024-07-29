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
        // [artially vanilla-adapted code: clean up
        protected override IEnumerable<Toil> MakeNewToils()
        {
            int layType = 3;
            yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
            yield return Toils_General.Wait(500);
            yield return Toils_General.Do(delegate
            {
                Letter letter;
                Messages.Message($"todo", pawn, MessageTypeDefOf.PositiveEvent, true);
                Thing thing = pawn.GetComp<YingComp>().ProduceEgg();
                if (job.GetTarget(TargetIndex.A).HasThing && EggBoxComp.Accepts(thing.def))
                {
                    layType = 1;
                    EggBoxComp.innerContainer.TryAdd(thing, true);
                }
                else
                {
                    GenPlace.TryPlaceThing(thing, pawn.Position, pawn.Map, ThingPlaceMode.Near, delegate (Thing t, int i)
                    {
                        if (pawn.Faction != Faction.OfPlayer)
                        {
                            layType = 2;
                            t.SetForbidden(true, true);
                        }
                    }, null, default);
                }
                switch (layType)
                {
                    case 1:
                        Messages.Message($"{pawn} has laid an egg!", pawn, MessageTypeDefOf.PositiveEvent, true);
                        Find.LetterStack.ReceiveLetter(LetterMaker.MakeLetter($"Egg laid: {pawn}", $"{pawn} has laid an egg!", YingDefOf.YingletEgg), null);
                        break;
                    case 2:
                        Messages.Message($"A yinglet from another faction has laid an egg!", pawn, MessageTypeDefOf.NeutralEvent, true);
                        Find.LetterStack.ReceiveLetter(LetterMaker.MakeLetter($"Egg laid: {pawn}", $"{pawn} from {pawn.Faction.Name} has laid an egg in your territory.", YingDefOf.YingletEggNoBox), null);
                        break;
                    default:
                        Messages.Message($"{pawn} has laid an egg without a nest box!", pawn, MessageTypeDefOf.NegativeEvent, true);
                        Find.LetterStack.ReceiveLetter(LetterMaker.MakeLetter($"Egg laid: {pawn}", $"{pawn} has laid an egg without a nest box. You should build some!", YingDefOf.YingletEggNoBox), null);
                        break;
                }
                return;
            });
            yield break;
        }
    }
}

