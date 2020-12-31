using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace ShellTooth
{
	public class CompTargetable_AliveNonYinglet : CompTargetable
	{
		protected override bool PlayerChoosesTarget
		{
			get
			{
				return true;
			}
		}
		protected override TargetingParameters GetTargetingParameters()
		{
			TargetingParameters validation = new TargetingParameters
			{
				canTargetPawns = true,
				canTargetHumans = true,
				canTargetAnimals = true,
				canTargetBuildings = false,
				canTargetItems = false,
			};
			validation.validator = delegate (TargetInfo targ)
			{
				if (!base.BaseTargetValidator(targ.Thing))
				{
					return false;
				}
				Pawn pawn = targ.Thing as Pawn;
				return pawn != null && (pawn.def.defName != "Alien_Yinglet" && pawn.def.defName != "Alien_Younglet");
			};
			return validation;

		}
public override IEnumerable<Thing> GetTargets(Thing targetChosenByPlayer = null)
		{
			yield return targetChosenByPlayer;
			yield break;
		}
	}
}
