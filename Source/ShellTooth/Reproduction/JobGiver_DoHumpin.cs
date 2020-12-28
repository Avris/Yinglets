using System;
using Verse;
using Verse.AI;
using RimWorld;

namespace ShellTooth
{
	public class JobGiver_DoHumpin : ThinkNode_JobGiver
	{
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (Find.TickManager.TicksGame < pawn.mindState.canLovinTick)
			{
				return null;
			}
			if (pawn.CurrentBed() == null || pawn.CurrentBed().Medical || !pawn.health.capacities.CanBeAwake || pawn.CurrentBed().CurOccupants.EnumerableCount() != 2)
			{
				return null;
			}
			Pawn partnerInMyBed = new Pawn();
			foreach (Pawn pawns in pawn.CurrentBed().CurOccupants)
			{
				if (pawns != pawn && pawns.GetComp<YingComp>().isDesignatedBreeder)
				{
					partnerInMyBed = pawns;
				}
			}
			if (partnerInMyBed == null || !partnerInMyBed.health.capacities.CanBeAwake || Find.TickManager.TicksGame < partnerInMyBed.mindState.canLovinTick)
			{
				return null;
			}
			if (!pawn.CanReserve(partnerInMyBed, 1, -1, null, false) || !partnerInMyBed.CanReserve(pawn, 1, -1, null, false))
			{
				return null;
			}
			return JobMaker.MakeJob(DefOfYinglet.Humps, partnerInMyBed, pawn.CurrentBed());
		}
	}
}
