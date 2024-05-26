using Verse;
using RimWorld;
using System;
#pragma warning disable 0649

namespace ShellTooth
{
    class HediffGiver_AgeRangeDays : HediffGiver
	{
		public override void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
			float ageDays = (float)pawn.ageTracker.AgeBiologicalTicks / (float)GenDate.TicksPerDay;
			if (ageDays >= ageDayMinimum + dayRange || ((Math.Round(ageDays % 1, 3) == 0) && (ageDays > ageDayMinimum))) {
				float chance = (ageDays - ageDayMinimum) / dayRange;
				if (Rand.Chance(chance)) {
					if (TryApply(pawn, null)) {
						return;
					}
				}
			}
		}
		public float ageDayMinimum;
		public float dayRange;
	}
}
