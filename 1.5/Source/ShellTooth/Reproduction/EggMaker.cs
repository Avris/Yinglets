using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace ShellTooth
{
    // Gonna move reproduction-related methods into here over time
    internal class EggMaker
    {
        // Because the vanilla check fails for yinglet lifespans
        public static bool YingSterile(Pawn pawn)
        {
            if (!pawn.ageTracker.CurLifeStage.reproductive)
            {
                return true;
            }
            if (pawn.health.hediffSet.HasHediffPreventsPregnancy())
            {
                return true;
            }
            return false;
        }
    }
}
