using System;
using RimWorld;
using Verse;
using System.Collections.Generic;
using AlienRace;
using UnityEngine;

namespace ShellTooth
{
    public class Hediff_Yingify : HediffWithComps
    {
        public override void PostMake()
        {
            if (pawn.def.defName == "Alien_Younglet")
            {
                YingletMaker yinglify = new YingletMaker();
                yinglify.MakeYinglet(pawn);
                pawn.health.RemoveHediff(this);
                Letter letter = LetterMaker.MakeLetter($"Adulthood: {pawn}", $"{pawn} has grown into an adolescent yinglet!", YingDefOf.YoungletGrown);
                Find.LetterStack.ReceiveLetter(letter, null);
                Messages.Message($"{pawn} has grown into an adolescent yinglet!", pawn, MessageTypeDefOf.PositiveEvent, true);
            }
        }
    }
}