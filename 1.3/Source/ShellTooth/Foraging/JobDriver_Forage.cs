using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using RimWorld;

namespace ShellTooth
{
	public class JobDriver_Forage : JobDriver
	{
		private Fodder fodder
		{
			get
			{
				return (Fodder)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed);
		}
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.Touch);
			Toil foragerToil = new Toil();
			foragerToil.initAction = delegate ()
			{
			};
			int statMultiplier = (int)(this.pawn.GetStatValue(StatDefOf.GeneralLaborSpeed, true) * this.pawn.skills.GetSkill(SkillDefOf.Plants).Level);
			Toil toil = Toils_General.Wait((10000 / statMultiplier), TargetIndex.None);
			toil.WithProgressBarToilDelay(TargetIndex.A, false);
			toil.FailOnDespawnedOrNull(TargetIndex.A);
			toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			yield return foragerToil;
			yield return Toils_General.Do(new Action(delegate () {
				fodder.foragedBy = this.pawn;
				fodder.Destroy(DestroyMode.KillFinalize);
			}));
		}
	}
}
