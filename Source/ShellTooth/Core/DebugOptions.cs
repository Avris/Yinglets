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
        private static void WindowTemplate()
        {
        }
    }
}
