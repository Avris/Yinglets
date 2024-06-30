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
    }
}
