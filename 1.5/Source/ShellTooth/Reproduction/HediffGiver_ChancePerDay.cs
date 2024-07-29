using Verse;
using RimWorld;
using System;
#pragma warning disable 0649

namespace ShellTooth
{
	class HediffGiver_AgeRangeDays : HediffGiver
	{
		// Replacing next pass this with either more nuanced checks in the worker, or vanilla-style growth
		public override void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
			if (pawn.def == YingDefOf.Alien_Younglet)
			{
				float min = ShelltoothSettings.yingletAdultDaysMinimum;
				float ageDays = pawn.ageTracker.AgeBiologicalTicks / (float)GenDate.TicksPerDay;
				if (ageDays >= min)
				{
					TryApply(pawn, null);
				}
			}
		}
	}
}

