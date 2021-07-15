using System;
using System.Collections.Generic;
using System.Text;
using Verse;
using Verse.AI;
using RimWorld;


namespace ShellTooth
{
	[StaticConstructorOnStartup]
	class Fodder : ThingWithComps
	{

		public FodderDef dref
		{
			get
			{
				return (this.def as FodderDef);
			}
		}
		public FodderDrop forageDrop(Pawn pawn)
		{
				IntVec3 n = dref.rarityThresholds;
			    int multi = (pawn.skills.GetSkill(SkillDefOf.Plants).Level / 20);
				int r = new Random().Next(n.x + n.y + n.z);
				if ((r <= (n.z * multi)) && (dref.rareRewards.Count > 0))
				{
					return dref.rareRewards[new Random().Next(dref.rareRewards.Count)];
				}
				if ((r <= (n.z * multi)) && (dref.uncommonRewards.Count > 0))
				{
					return dref.uncommonRewards[new Random().Next(dref.uncommonRewards.Count)];
				}
				if (dref.commonRewards.Count > 0)
				{
					return dref.commonRewards[new Random().Next(dref.commonRewards.Count)];
				}
				return null;
		}
		public override Graphic Graphic
		{
			get
			{
				return this.def.graphic;
			}
		}
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn pawn)
		{
			if (!this.IsForbidden(pawn) && pawn.RaceProps.Humanlike)
			{
					Action action = delegate ()
					{
						if (true)
						{
							if (pawn.CurJobDef != YingDefOf.Forage && pawn.CurJob.GetTarget(TargetIndex.A).Thing != this)
							{
								Job job = JobMaker.MakeJob(YingDefOf.Forage, this);
								pawn.jobs.TryTakeOrderedJob(job, JobTag.MiscWork);
							}
							pawn.mindState.ResetLastDisturbanceTick();
						}
					};
				yield return FloatMenuUtility.DecoratePrioritizedTask(
					option: new FloatMenuOption("Forage this thing.", action, MenuOptionPriority.Default, null, null, 0f, null, null),
					pawn: pawn,
					target: this,
					reservedText: "Somebody else is doing this!");
			}
			yield break;
		}
		public override void ExposeData()
		{
			base.ExposeData();
		}
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			base.DeSpawn(mode);
		}
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = this.Map;
			base.Destroy(mode);
			if (mode == DestroyMode.KillFinalize && foragedBy != null)
			{
				FodderDrop drop = this.forageDrop(foragedBy);
				int skill = foragedBy.skills.GetSkill(SkillDefOf.Plants).Level;
				int count = drop.count;
				if (drop.winterItem != null && !PlantUtility.GrowthSeasonNow(base.Position, map, false))
				{
					for (int i = 0; i < count; i++)
					{
						if (i == 0 || !drop.scalesWithSkill || skill == 20 || new Random().Next(20) <= skill)
						{
							GenPlace.TryPlaceThing(ThingMaker.MakeThing(drop.winterItem, null), base.Position, map, ThingPlaceMode.Near);
						}
					}
				}
				else
				{
					for (int i = 0; i < count; i++)
					{
						if (i == 0 || !drop.scalesWithSkill || skill == 20 || new Random().Next(20) <= skill)
						{
							GenPlace.TryPlaceThing(ThingMaker.MakeThing(drop.item, null), base.Position, map, ThingPlaceMode.Near);
						}
					}
				}
			}
		}
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (base.Position.GetThingList(base.Map).Count >= 2 || !FodderUtility.tileTypes.Contains(this.Position.GetTerrain(map).defName))
			{
				this.Destroy(DestroyMode.Vanish);
			}
			else if (FodderUtility.fodList[this.Position.GetTerrain(map).defName].ContainsKey(map.Index))
			{
				FodderUtility.fodList[this.Position.GetTerrain(map).defName][map.Index].Add(this);
			}
			else if (!FodderUtility.fodList[this.Position.GetTerrain(map).defName].ContainsKey(map.Index))
			{
				FodderUtility.fodList[this.Position.GetTerrain(map).defName].Add(map.Index, new List<Fodder>() { this });
			}
		}
		public bool blockPlants = true;
		public Pawn foragedBy = null;
	}
}
