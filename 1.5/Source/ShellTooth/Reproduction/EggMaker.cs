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
        public static void KnockUp(Pawn mother, Pawn father, float progress = 0.01f)
        {
            YingComp comp = mother.GetComp<YingComp>();
            comp.isDesignatedBreeder = false;
            comp.knockedUpBy = father;
            comp.eggProgress = progress;
        }
    }
}
