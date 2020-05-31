using System;
using System.Reflection;
using AlienRace;
using HarmonyLib;
using RimWorld;
using Verse;
using UnityEngine;

namespace Yingworld
{
    [HarmonyPatch(typeof(GenSpawn))]
    internal static class HarmonyPatches
    {
        [HarmonyPatch(nameof(GenSpawn.Spawn), new Type[] { typeof(Thing), typeof(IntVec3), typeof(Map), typeof(Rot4), typeof(WipeMode), typeof(bool) })]
        [HarmonyPostfix]
        static void SpawnPostfix(ref Thing __result, Thing newThing, IntVec3 loc, Map map, Rot4 rot, WipeMode wipeMode = WipeMode.Vanish, bool respawningAfterLoad = false)
        {
            Pawn pawn = __result as Pawn;
            if (pawn != null && pawn.def == YingDefOf.Alien_Yinglet)
            {
                AlienPartGenerator.AlienComp comp = pawn.GetComp<AlienPartGenerator.AlienComp>();
                if (comp != null)
                {
                    comp.ColorChannels.TryGetValue("tail").first = comp.skinColor;
                    comp.ColorChannels.TryGetValue("tail").second = pawn.story.hairColor;
                }
            }
        }
    }

    [StaticConstructorOnStartup]
    internal static class YingworldPatches
    {
        static YingworldPatches()
        {
            Harmony harmonyInstance = new Harmony("com.yingworld.mod");
            harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
