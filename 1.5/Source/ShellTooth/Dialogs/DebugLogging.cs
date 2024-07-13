using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace ShellTooth
{
    internal class DebugLogging
    {
        // Log messages go here to make it easier for the verbose logging toggle to enable. WIP
        public static void BreedingChance(Pawn mother, Pawn father, bool success, float chance)
        {
            Log.Message($"{father} tried to breed with {mother} and {(success == true ? "succeeded" : "did not succeed")} with a chance of {chance}");
        }
        public static void Yinglification(Pawn origin, Pawn destination, string cause)
        {
        }
        public static void BreedCheck(Pawn pawn)
        {
            int tick = Current.Game.tickManager.TicksGame;
            Log.Message($"{pawn} is {(pawn.GetComp<YingComp>().isDesignatedBreeder ? "enabled" : "disabled")} in a bed with {pawn.CurrentBed().CurOccupants.EnumerableCount()} {(pawn.CurrentBed().CurOccupants.EnumerableCount() != 1 ? "occupants" : "occupant")} at tick {tick}");
        }
    }
}
