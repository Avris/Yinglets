using System;
using Verse;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace ShellTooth
{
	public class ThinkNode_ChancePerHour_Humpin : ThinkNode_ChancePerHour
	{
		protected override float MtbHours(Pawn pawn)
		{
			if (pawn.def.defName == "Alien_Younglet") 
			{
				return -1f;
			}
			int tick = Current.Game.tickManager.TicksGame;
			Pawn partnerInMyBed = new Pawn();
			try
			{
				if (pawn.CurrentBed() == null || !pawn.GetComp<YingComp>().isDesignatedBreeder || pawn.CurrentBed().CurOccupants.EnumerableCount() != 2)
				{
					if (pawn.CurrentBed() != null)
					{
						Log.Message($"{pawn} is {(pawn.GetComp<YingComp>().isDesignatedBreeder ? "enabled" : "disabled")} in a bed with {pawn.CurrentBed().CurOccupants.EnumerableCount()} {(pawn.CurrentBed().CurOccupants.EnumerableCount() != 1 ? "occupants" : "occupant")} at tick {tick}");
					}
					return -1f;
				}
				foreach (Pawn pawns in pawn.CurrentBed().CurOccupants)
				{
					if (pawns != pawn && pawns.GetComp<YingComp>().isDesignatedBreeder)
					{
						partnerInMyBed = pawns;
					}
				}
			}
			catch (NullReferenceException nre) 
			{ 
				Log.Message("ShellTooth: attempted mtb check had NRE " + nre.ToString());
			}
			float MTB = GetHumpinMtbHours(pawn, partnerInMyBed);
			Log.Message($"{pawn} and {partnerInMyBed} have an MTB of {MTB} at tick {tick}");
			return MTB;
		}
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