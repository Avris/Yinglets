using System;
using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace ShellTooth
{
    class WorkGiver_Forage : WorkGiver_Scanner
	{
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
				return FodderUtility.GetNearestFodder(pawn);
		}
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.None;
		}

		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!pawn.WorkTagIsDisabled(WorkTags.PlantWork) || !pawn.WorkTagIsDisabled(WorkTags.ManualDumb) || !pawn.WorkTagIsDisabled(WorkTags.Commoner)) 
			{
				return pawn.CanReserve(t, 1, -1, null, forced);
			}
			else 
			{
				return false;
			}
		}

		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(YingDefOf.Forage, t);
		}
	}
}