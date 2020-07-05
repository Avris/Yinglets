using System;
using System.Collections.Generic;
using System.Reflection;
using AlienRace;
using HarmonyLib;
using RimWorld;
using Verse;
using UnityEngine;

namespace Yingworld
{
    [HarmonyPatch(typeof(Pawn))]
    internal static class StyleSync
    {
        [HarmonyPatch(typeof(Pawn), nameof(Pawn.SpawnSetup))]
        [HarmonyPostfix]
        static void YingletStyleSync(bool respawningAfterLoad, Pawn __instance)
        {
            if (__instance != null && __instance.def == YingDefOf.Alien_Yinglet)
            {
                // ThingDef_AlienRace pawn = __instance.def as ThingDef_AlienRace;
                AlienPartGenerator.AlienComp comp = __instance.GetComp<AlienPartGenerator.AlienComp>();
                if (comp != null && comp.addonVariants != null)
                {
                    for (int i = 0; i < comp.addonVariants.Count; i++)
                    {
                        comp.addonVariants[i] = 0;
                    }
                    if (Current.ProgramState != ProgramState.Playing) return;
                    __instance.Drawer.renderer.graphics.ResolveAllGraphics();
                    if (__instance.IsColonist) PortraitsCache.SetDirty(__instance);
                }
                else if (comp != null)
                {
                    comp.addonVariants = new List<int>();
                    for (int i = 0; i < 8; i++)
                    {
                        comp.addonVariants.Add(0);
                    }
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
