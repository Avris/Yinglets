using Verse;
using RimWorld;
#pragma warning disable 0649

namespace ShellTooth
{
    class HediffGiver_AgeRangeDays : HediffGiver
	{
		public override void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
			if ((pawn.ageTracker.AgeBiologicalYears % GenDate.TicksPerDay == 0) && pawn.ageTracker.AgeBiologicalTicks >= GenDate.TicksPerDay * (ageDayMinimum)) {
				float chance = dayRange / (((int)pawn.ageTracker.AgeBiologicalTicks / GenDate.TicksPerDay) - ageDayMinimum);
				if (Rand.Chance((float)chance)) {
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
