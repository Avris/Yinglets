using System;
using Verse;
using RimWorld;
using UnityEngine;
using Verse.AI;
using System.Linq;

namespace ShellTooth
{
	public class ThinkNode_ChancePerHour_Humpin : ThinkNode_ChancePerHour
	{
        protected override float MtbHours(Pawn pawn)
        {
            if (pawn.GetComp<YingComp>() != null && pawn.GetComp<YingComp>().isDesignatedBreeder && pawn.CurrentBed() != null && pawn.CurrentBed().CurOccupants.EnumerableCount() == 2)
            {
                Pawn partnerInMyBed = pawn.CurrentBed().CurOccupants.First(p => p != pawn);
				if (partnerInMyBed.GetComp<YingComp>().isDesignatedBreeder)
				{
					return GetHumpinMtbHours(pawn, partnerInMyBed);
				}
            }
            return -1f;
        }
        // This maths is old and based on vanilla numbers. Need to rewrite it to be more readable.
        public float GetHumpinMtbHours(Pawn pawn, Pawn partner)
		{
			if (pawn.Dead || partner.Dead)
			{
				return -1f;
			}
			if (DebugSettings.alwaysDoLovin)
			{
				return 0.1f;
			}
			if (pawn.needs.food.Starving || partner.needs.food.Starving)
			{
				return -1f;
			}
			if (pawn.health.hediffSet.BleedRateTotal > 0f || partner.health.hediffSet.BleedRateTotal > 0f)
			{
				return -1f;
			}
			float num = HumpinMtbSinglePawnFactor(pawn);
			if (num <= 0f)
			{
				return -1f;
			}
			float num2 = HumpinMtbSinglePawnFactor(partner);
			if (num2 <= 0f)
			{
				return -1f;
			}
			float num3 = 0.02f;
			num3 *= num;
			num3 *= num2;
			num3 /= Mathf.Max(pawn.relations.SecondaryLovinChanceFactor(partner), 0.1f);
			num3 /= Mathf.Max(partner.relations.SecondaryLovinChanceFactor(pawn), 0.1f);
			num3 *= GenMath.LerpDouble(-100f, 100f, 1.3f, 0.7f, pawn.relations.OpinionOf(partner));
			num3 *= GenMath.LerpDouble(-100f, 100f, 1.3f, 0.7f, partner.relations.OpinionOf(pawn));
			if (pawn.health.hediffSet.HasHediff(HediffDefOf.PsychicLove, false))
			{
				num3 /= 5f;
			}
			return num3;
        }
        // This maths is old and based on vanilla numbers. Need to rewrite it to be more readable.
        private float HumpinMtbSinglePawnFactor(Pawn pawn)
		{
			float num = 1f;
			num /= 1f - pawn.health.hediffSet.PainTotal;
			float level = pawn.health.capacities.GetLevel(PawnCapacityDefOf.Consciousness);
			if (level < 0.5f)
			{
				num /= level * 2f;
			}
			float num2 = num / GenMath.FlatHill(0f, 2f, 4f, 12f, 20f, 0.2f, pawn.ageTracker.AgeBiologicalYearsFloat);
			return num2;
		}
	}
}