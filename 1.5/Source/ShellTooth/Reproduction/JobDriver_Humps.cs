using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace ShellTooth
{
    public class JobDriver_Humps : JobDriver
	{
		private Pawn Partner
		{
			get
			{
				return (Pawn)((Thing)job.GetTarget(PartnerInd));
			}
		}
		private Building_Bed Bed
		{
			get
			{
				return (Building_Bed)((Thing)job.GetTarget(BedInd));
			}
		}
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look(ref ticksLeft, "ticksLeft", 0, false);
		}
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return pawn.Reserve(Partner, job, 1, -1, null, errorOnFailed) && pawn.Reserve(Bed, job, Bed.SleepingSlotsCount, 0, null, errorOnFailed);
		}
		public override bool CanBeginNowWhileLyingDown()
		{
			return JobInBedUtility.InBedOrRestSpotNow(pawn, job.GetTarget(BedInd));
		}
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(BedInd);
			this.FailOnDespawnedOrNull(PartnerInd);
			this.FailOn(() => !Partner.health.capacities.CanBeAwake);
			this.KeepLyingDown(BedInd);
			yield return Toils_Bed.ClaimBedIfNonMedical(BedInd, TargetIndex.None);
			yield return Toils_Bed.GotoBed(BedInd);
			yield return new Toil
			{
				initAction = delegate ()
				{
					if (Partner.CurJob == null || Partner.CurJob.def != YingDefOf.Humps)
					{
						Job newJob = JobMaker.MakeJob(YingDefOf.Humps, pawn, Bed);
						Partner.jobs.StartJob(newJob, JobCondition.InterruptForced, null, false, true, null, null, false, false);
						ticksLeft = (int)(2500f * Mathf.Clamp(Rand.Range(0.1f, 1.1f), 0.1f, 2f));
						return;
					}
					ticksLeft = 9999999;
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
			Toil toil = Toils_LayDown.LayDown(BedInd, true, false, false, false);
			toil.FailOn(() => Partner.CurJob == null || Partner.CurJob.def != YingDefOf.Humps);
			toil.AddPreTickAction(delegate
			{
				ticksLeft--;
				if (ticksLeft <= 0)
				{
					base.ReadyForNextToil();
					return;
				}
				if (pawn.IsHashIntervalTick(100))
				{
					FleckMaker.ThrowMetaIcon(pawn.Position, pawn.Map, FleckDefOf.Heart, 0.42f);
					FleckMaker.ThrowMetaIcon(pawn.Position, pawn.Map, FleckDefOf.Heart, 0.42f);
				}
			});
			toil.AddFinishAction(delegate
			{
				// Adapted from vanilla decomp (clean up the gotos)
				Thought_Memory thought_Memory = (Thought_Memory)ThoughtMaker.MakeThought(ThoughtDefOf.GotSomeLovin);
				if (pawn.health != null && pawn.health.hediffSet != null)
				{
					if (pawn.health.hediffSet.hediffs.Any((Hediff h) => h.def == HediffDefOf.LoveEnhancer))
					{
						goto IL_C4;
					}
				}
				if (Partner.health == null || Partner.health.hediffSet == null)
				{
					goto IL_CF;
				}
				if (!Partner.health.hediffSet.hediffs.Any((Hediff h) => h.def == HediffDefOf.LoveEnhancer))
				{
					goto IL_CF;
				}
			IL_C4:
				thought_Memory.moodPowerFactor = 1.5f;
			IL_CF:
				if (pawn.needs.mood != null)
				{
					pawn.needs.mood.thoughts.memories.TryGainMemory(thought_Memory, Partner);
				}
				if (pawn.gender == Gender.Female && Partner.gender == Gender.Male)
                {
                    Humped(pawn, Partner);
                }
				pawn.mindState.canLovinTick = Find.TickManager.TicksGame + GenerateRandomMinTicksToNextLovin(pawn);
            });
			toil.socialMode = RandomSocialMode.Off;
			yield return toil; 
			yield break;
        }
        public static void Humped(Pawn mother, Pawn father)
        {
			if (!EggMaker.YingSterile(mother) && !EggMaker.YingSterile(father))
			{

                float chance = Rand.Range(0f,0.9f);
				YingComp comp = mother.GetComp<YingComp>();
				if ((comp.eggProgress == 0))
                {
                    comp.knockedUpBy = father;
					comp.eggProgress = 0.01f;
				}
			}
        }
        private int GenerateRandomMinTicksToNextLovin(Pawn pawn)
		{
			if (DebugSettings.alwaysDoLovin)
			{
				return 100;
			}
			float num = LovinIntervalHoursFromAgeCurve.Evaluate(pawn.ageTracker.AgeBiologicalYearsFloat);
			num = Rand.Gaussian(num, 0.3f);
			if (num < 0.5f)
			{
				num = 0.5f;
			}
			return (int)(num * 2500f);
        }
        private static readonly SimpleCurve LovinIntervalHoursFromAgeCurve = new SimpleCurve
        {
            {
                new CurvePoint(0.5f, 0.015f),
                true
            },
            {
                new CurvePoint(5f, 0.15f),
                true
            },
            {
                new CurvePoint(10f, 4f),
                true
            },
            {
                new CurvePoint(15f, 12f),
                true
            },
            {
                new CurvePoint(20f, 36f),
                true
            }
        };
        private int ticksLeft;
		private TargetIndex PartnerInd = TargetIndex.A;
		private TargetIndex BedInd = TargetIndex.B;
	}
}
