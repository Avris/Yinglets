using System;
using Verse;
using RimWorld;
using UnityEngine;

namespace ShellTooth
{
	public class ScenPart_EnableForaging : ScenPart
	{
		private IntVec3 cell = new IntVec3(60, 0, 180);
		private int checkedTick;
		private int checkedInterval;
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look(ref checkedTick, "checkedTick", 0);
			Scribe_Values.Look(ref checkedInterval, "checkedInterval", 600);
		}
		public override void Tick()
		{
			if ((YingDefOf.EggYingletFertilized.comps[3] as CompProperties_Hatcher).hatcherDaystoHatch != ShellTooth.YingletEgg) 
			{
				(YingDefOf.EggYingletFertilized.comps[3] as CompProperties_Hatcher).hatcherDaystoHatch = ShellTooth.YingletEgg;
			}
			/*if (cell.GetThingList(Current.Game.CurrentMap).Count == 0)
			{
				GenSpawn.Spawn(ThingDef.Named("FodderA"), cell, Current.Game.CurrentMap, WipeMode.Vanish);
			}*/
			if (Current.Game.tickManager.TicksGame - checkedTick >= checkedInterval)
			{
				FodderUtility.GeneratePlants(Current.Game.CurrentMap);
				checkedTick = Current.Game.tickManager.TicksGame;
			}
		}
	}
}