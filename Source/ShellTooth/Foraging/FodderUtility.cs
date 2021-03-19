using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using RimWorld;

namespace ShellTooth
{
	class FodderUtility
	{
		public static void GeneratePlants(Map map)
		{
			TallyTerrain(map);
			tileTypes.ForEach(delegate (String terrain)
			{
				if (CanSpawnFodder(map.Index, terrain))
				{
					IntVec3 cell = GetValidCell(map, terrain);
					if (cell != new IntVec3(-1, -1, -1))
					{
						ThingDef thing = defList[terrain][new Random().Next(defList[terrain].Count)];
						GenSpawn.Spawn(thing, cell, map, WipeMode.Vanish);
					}
					else
					{
						int count = fodList[terrain].ContainsKey(map.Index) ? fodList[terrain][map.Index].Count : 0;
						Log.Message($"ShellTooth on build {ShellTooth.Version}: failed to get valid cell. Count was {count} for {terList[terrain][map.Index]} tiles.");
					}
				}
			});
		}
		public static IEnumerable<Fodder> GetNearestFodder(Pawn pawn) 
		{
			List<Fodder> fodderWorkable = new List<Fodder>();
			foreach (Dictionary<int, List<Fodder>> fodderlist in fodList.Values)
			{
				if (fodderlist.ContainsKey(pawn.Map.Index) && fodderlist[pawn.Map.Index].Count != 0)
				{
					fodderlist[pawn.Map.Index].ForEach(delegate (Fodder fodder) {
						if (!fodder.IsForbidden(pawn)
						&& !fodder.Position.IsForbidden(pawn)
						&& pawn.CanReach(fodder, PathEndMode.OnCell, Danger.None, false, TraverseMode.ByPawn) 
						&& fodder.Position.DistanceToSquared(pawn.Position) < 3000f)
						{
							fodderWorkable.Add(fodder);
						}
					});
				}
			}
			return fodderWorkable;
		}
		public static IntVec3 GetValidCell(Map map, string terrain)
		{
			IntVec3 result = new IntVec3(-1, -1, -1);
			CellRect cellRect = CellRect.WholeMap(map);
			int i = 0;
			if (terList.ContainsKey(terrain))
				while (i < 500 || result == new IntVec3(-1, -1, -1))
				{
					IntVec3 cell = cellRect.RandomCell;
					if (cell.GetTerrain(map).defName == terrain && cell.GetThingList(map).Count == 0) {
						result = cell;
						break;
					}
					i++;
				}
			return result;
		}
		public static bool CanSpawnFodder(int m, String t) {
			if (!defList.ContainsKey(t))						{ return false; };
			if (defList[t].Count == 0)							{ return false; };
			if (terList[t][m] == 0)								{ return false; };
			if (fodList[t].ContainsKey(m))
			{
				if (fodList[t][m].Count >= 15)					{ return false; };
				if (fodList[t][m].Count >= terList[t][m] / 100) { return false; };
			}
			return true;
			}
		public static void TallyTerrain(Map map)
		{
			CellRect cellRect = CellRect.WholeMap(map);
			foreach (Dictionary<int, int> dict in terList.Values) {
				dict[map.Index] = 0;
			}
			foreach (IntVec3 cell in cellRect) {
				if (terList.ContainsKey(cell.GetTerrain(map).defName) && cell.GetThingList(map).Count == 0) {
					terList[cell.GetTerrain(map).defName][map.Index]++;
				}
			}
			if (terList["WaterOceanShallow"][map.Index] == 0) {
				terList["Sand"][map.Index] = 0;
			}
		}
		public static List<String> tileTypes = new List<String>()
		{
			"Sand", 
			"Marsh", 
			"WaterShallow", 
			"WaterMovingShallow",
			"WaterOceanShallow"
		};	
		public static Dictionary<string, List<FodderDef>> defList = new Dictionary<string, List<FodderDef>>()
		{
			{ tileTypes[0], new List<FodderDef>() },
			{ tileTypes[1], new List<FodderDef>() },
			{ tileTypes[2], new List<FodderDef>() },
			{ tileTypes[3], new List<FodderDef>() },
			{ tileTypes[4], new List<FodderDef>() }
		};
		public static Dictionary<string, Dictionary<int, int>> terList = new Dictionary<string, Dictionary<int, int>>()
		{
			{ tileTypes[0], new Dictionary<int, int>()},
			{ tileTypes[1], new Dictionary<int, int>()},
			{ tileTypes[2], new Dictionary<int, int>()},
			{ tileTypes[3], new Dictionary<int, int>()},
			{ tileTypes[4], new Dictionary<int, int>()}
		};
		public static Dictionary<string, Dictionary<int, List<Fodder>>> fodList = new Dictionary<string, Dictionary<int, List<Fodder>>>()
		{
			{ tileTypes[0], new Dictionary<int, List<Fodder>>()},
			{ tileTypes[1], new Dictionary<int, List<Fodder>>()},
			{ tileTypes[2], new Dictionary<int, List<Fodder>>()},
			{ tileTypes[3], new Dictionary<int, List<Fodder>>()},
			{ tileTypes[4], new Dictionary<int, List<Fodder>>()},
		};
	}
}