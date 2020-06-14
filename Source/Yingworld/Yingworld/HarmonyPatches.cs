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
        [HarmonyPatch(typeof(PawnGraphicSet), nameof(PawnGraphicSet.ResolveAllGraphics))]
        [HarmonyPostfix]
        static void ColorChannelPostfix(PawnGraphicSet __instance)
        {
            if (__instance.pawn != null && __instance.pawn.def == YingDefOf.Alien_Yinglet)
            {
                AlienPartGenerator.AlienComp comp = __instance.pawn.GetComp<AlienPartGenerator.AlienComp>();
                if (comp != null)
                {
                    comp.ColorChannels.TryGetValue("tail").first = comp.ColorChannels.TryGetValue("skin").first;
                    comp.ColorChannels.TryGetValue("tail").second = comp.ColorChannels.TryGetValue("hair").first;
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
