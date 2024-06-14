using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace ShellTooth.Core
{
    internal class DebugLogging
    {
        public static void BreedingChance(Pawn mother, Pawn father, bool success, float chance)
        {
            Log.Message($"{father} tried to breed with {mother} and {(success == true ? "succeeded" : "did not succeed")} with a chance of {chance}");
        }
        public static void Yinglification(Pawn origin, Pawn destination, string cause)
        {
        }
    }
}
