﻿using System;
using RimWorld;
using Verse;
using Verse.AI;

namespace ShellTooth
{
	class CompTargetEffect_Yingify : CompTargetEffect
	{
		public override void DoEffectOn(Pawn user, Thing target)
		{
			if (!user.IsColonistPlayerControlled || !user.CanReserveAndReach(target, PathEndMode.Touch, Danger.Deadly, 1, 1, null, false))
			{
				return;
			}
			user.jobs.TryTakeOrderedJob(new Job(YingDefOf.Yingify, target, parent) { count = 1 });
		}
	}
}
