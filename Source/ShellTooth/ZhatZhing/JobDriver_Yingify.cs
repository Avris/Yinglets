using System;
using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace ShellTooth
{
	public class JobDriver_Yingify : JobDriver
	{
		private Pawn Victim
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}
		private Thing Item
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			if (!this.pawn.Reserve(this.Victim, this.job, 1, -1, null, false))
			{
				return false;
			}
			if (!this.pawn.Reserve(this.Item, this.job, 1, -1, null, false))
			{
				return false;
			}
			return true;
		}
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch).FailOnDespawnedOrNull(TargetIndex.B).FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Haul.StartCarryThing(TargetIndex.B, false, false, false);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnDespawnedOrNull(TargetIndex.A);
			Toil toil = Toils_General.Wait(60, TargetIndex.None);
			toil.WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			toil.FailOnDespawnedOrNull(TargetIndex.A);
			toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			yield return toil;
			yield return Toils_General.Do(new Action(this.Yingify));
			yield break;
		}
		private void Yingify()
		{
			Pawn victim = this.Victim;
			victim.Strip();
			YingletMaker yinglify = new YingletMaker();
			yinglify.MakeYinglet(victim);
			Messages.Message($"Something awful has happened to {victim}!", victim, MessageTypeDefOf.PositiveEvent, true);
			this.Item.Destroy(DestroyMode.Vanish);
		}
	}
}
