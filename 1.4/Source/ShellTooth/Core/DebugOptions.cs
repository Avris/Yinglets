using System;
using System.Collections.Generic;
using RimWorld;
using Verse;
using AlienRace;

namespace ShellTooth
{
    class DebugOptions
    {
        [DebugAction("Yinglets", "Work In Progress Button", allowedGameStates = AllowedGameStates.PlayingOnMap)]
        private static void YingletDebugMenu()
        {
            Find.WindowStack.Add(new Dialog_SpawnYingletObject());
        }
        [DebugAction("Yinglets", "Tiplod pregnancy tester", allowedGameStates = AllowedGameStates.PlayingOnMap)]
        private static void TiplodTester()
        {
            Pawn mother = null;
            DebugTool tool2 = new DebugTool("Select the father...", delegate ()
            {
                if (mother == null)
                { 
                    Log.Warning("It didn't set for some reason");
                }
                else
                {
                    IntVec3 from1 = UI.MouseCell();
                    Pawn father = from1.GetFirstPawn(Find.CurrentMap);
                    Hediff_Pregnant hediff_Pregnant = (Hediff_Pregnant)HediffMaker.MakeHediff(HediffDefOf.Pregnant, mother, null);
                    mother.health.AddHediff(hediff_Pregnant, null, null, null);
                    bool work = false;
                    if (mother.health.hediffSet.HasHediff(HediffDefOf.Pregnant, false))
                    { work = true; };
                    Log.Warning($"Tried to make {mother.NameShortColored} pregnant from {father.NameShortColored}. It {(work ? "worked!" : "did not work.")}");
                    DebugTools.curTool = null;
                }
            });
            DebugTool tool1 = new DebugTool("Select the mother...", delegate ()
            {
                IntVec3 from2 = UI.MouseCell();
                mother = from2.GetFirstPawn(Find.CurrentMap);
                if (mother != null)
                {
                    DebugTools.curTool = tool2;
                }
            });
            DebugTools.curTool = tool1;
		}
        private static void WindowTemplate()
        {
        }
    }
}
