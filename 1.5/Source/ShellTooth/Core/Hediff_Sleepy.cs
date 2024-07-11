using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace ShellTooth
{
    internal class Hediff_Sleepy : HediffWithComps
    {
        /* Doesn't start ticking until waaaay too late, investigate
        public override void Tick()
        {
            Log.Error("fdff");
            if (!pawn.jobs.curDriver.asleep)
            {
                Log.Error("Removed etc");
                pawn.health.RemoveHediff(this);
            }
        }
        */
    }
}
