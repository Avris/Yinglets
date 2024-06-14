using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using RimWorld;
using UnityEngine;
using ShellTooth.Core;

namespace ShellTooth
{
	public class JobDriver_Humps : JobDriver
	{
		private Pawn Partner
		{
			get
			{
				return (Pawn)((Thing)this.job.GetTarget(this.PartnerInd));
			}
		}
		private Building_Bed Bed
		{
			get
			{
				return (Building_Bed)((Thing)this.job.GetTarget(this.BedInd));
			}
		}
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksLeft, "ticksLeft", 0, false);
		}
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Partner, this.job, 1, -1, null, errorOnFailed) && this.pawn.Reserve(this.Bed, this.job, this.Bed.SleepingSlotsCount, 0, null, errorOnFailed);
		}
		public override bool CanBeginNowWhileLyingDown()
		{
			return JobInBedUtility.InBedOrRestSpotNow(this.pawn, this.job.GetTarget(this.BedInd));
		}
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(this.BedInd);
			this.FailOnDespawnedOrNull(this.PartnerInd);
			this.FailOn(() => !this.Partner.health.capacities.CanBeAwake);
			this.KeepLyingDown(this.BedInd);
			yield return Toils_Bed.ClaimBedIfNonMedical(this.BedInd, TargetIndex.None);
			yield return Toils_Bed.GotoBed(this.BedInd);
			yield return new Toil
			{
				initAction = delegate ()
				{
					if (this.Partner.CurJob == null || this.Partner.CurJob.def != YingDefOf.Humps)
					{
						Job newJob = JobMaker.MakeJob(YingDefOf.Humps, this.pawn, this.Bed);
						this.Partner.jobs.StartJob(newJob, JobCondition.InterruptForced, null, false, true, null, null, false, false);
						this.ticksLeft = (int)(2500f * Mathf.Clamp(Rand.Range(0.1f, 1.1f), 0.1f, 2f));
						return;
					}
					this.ticksLeft = 9999999;
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
			Toil toil = Toils_LayDown.LayDown(this.BedInd, true, false, false, false);
			toil.FailOn(() => this.Partner.CurJob == null || this.Partner.CurJob.def != YingDefOf.Humps);
			toil.AddPreTickAction(delegate
			{
				this.ticksLeft--;
				if (this.ticksLeft <= 0)
				{
					base.ReadyForNextToil();
					return;
				}
				if (this.pawn.IsHashIntervalTick(100))
				{
					FleckMaker.ThrowMetaIcon(this.pawn.Position, this.pawn.Map, FleckDefOf.Heart, 0.42f);
				}
			});
			toil.AddFinishAction(delegate
			{
				/// Adapted from vanilla decomp (clean up the gotos)
				Thought_Memory thought_Memory = (Thought_Memory)ThoughtMaker.MakeThought(ThoughtDefOf.GotSomeLovin);
				if (this.pawn.health != null && this.pawn.health.hediffSet != null)
				{
					if (this.pawn.health.hediffSet.hediffs.Any((Hediff h) => h.def == HediffDefOf.LoveEnhancer))
					{
						goto IL_C4;
					}
				}
				if (this.Partner.health == null || this.Partner.health.hediffSet == null)
				{
					goto IL_CF;
				}
				if (!this.Partner.health.hediffSet.hediffs.Any((Hediff h) => h.def == HediffDefOf.LoveEnhancer))
				{
					goto IL_CF;
				}
			IL_C4:
				thought_Memory.moodPowerFactor = 1.5f;
			IL_CF:
				if (this.pawn.needs.mood != null)
				{
					this.pawn.needs.mood.thoughts.memories.TryGainMemory(thought_Memory, this.Partner);
				}
				if (this.pawn.gender == Gender.Female && this.Partner.gender == Gender.Male)
                {
                    Humped(this.pawn, this.Partner);
                }
				this.pawn.mindState.canLovinTick = Find.TickManager.TicksGame + this.GenerateRandomMinTicksToNextLovin(this.pawn);
            });
			toil.socialMode = RandomSocialMode.Off;
			yield return toil; 
			yield break;
        }
        public static void Humped(Pawn mother, Pawn father)
        {
			if (!YingSterile(mother) && !YingSterile(father))
			{
				bool success = false;
				float chance = Rand.Value;
				CompEggLayer compEggLayer = mother.TryGetComp<CompEggLayer>();
				if (compEggLayer != null)
				{
					compEggLayer.Fertilize(father);
				}
				if (chance < 0.5f)
				{
					Hediff_Pregnant hediff_Pregnant = (Hediff_Pregnant)HediffMaker.MakeHediff(HediffDefOf.Pregnant, mother);
					hediff_Pregnant.SetParents(null, father, null);
					mother.health.AddHediff(hediff_Pregnant);
					success = true;
				}
				DebugLogging.BreedingChance(mother, father, success, chance);
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
		// Because the vanilla check fails for yinglet lifespans
        public static bool YingSterile(Pawn pawn)
        {
            if (!pawn.ageTracker.CurLifeStage.reproductive)
            {
                return true;
            }
            if (pawn.RaceProps.Humanlike)
            {
                if (!ModsConfig.BiotechActive)
                {
                    return true;
                }
            }
            if (pawn.health.hediffSet.HasHediffPreventsPregnancy())
            {
                return true;
            }
            return false;
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
