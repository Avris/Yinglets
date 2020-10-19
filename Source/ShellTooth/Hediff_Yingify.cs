using System;
using RimWorld;
using Verse;

namespace ShellTooth
{
    public class Hediff_Yingify : HediffWithComps
    {
        public override void PostMake()
        {
            if (pawn.def.defName != "Yinglet") {
                pawn.Strip();
                YingletMaker.MakeYinglet(pawn);
                pawn.health.RemoveHediff(this);
                Messages.Message($"{pawn} has grown into an adolescent yinglet!", pawn, MessageTypeDefOf.PositiveEvent, true);
            }
        }
    }
}