using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using RimWorld;
using Verse;
using AlienRace;

namespace Yingworld
{
    public static class ChangeToYingUtil
    {
        /// <summary>
        /// safely change the pawns race 
        /// </summary>
        /// <param name="pawn"></param>
        /// <param name="race"></param>
        /// <param name="reRollTraits">if race related traits should be reRolled</param>
        public static void ChangePawnRace([NotNull] Pawn pawn)
        {
            if (pawn == null) throw new ArgumentNullException(nameof(pawn));

            Faction faction = pawn.Faction; // The faction of the pawn (raider, colonist, tribe, etc.)
            Map map = pawn.Map;             // The current map of the pawn (usually 0 for the home map, but can be null or another value).
            bool removed = false;           // Sentinel value to check if the pawn needs to be re-added to their map's list of things.

            // If the pawn is on a map (i.e. isn't on the world, but actually inside of your home base, an enemy outpost, a treasure trove, etc.)
            if (map != null)
            {
                RegionListersUpdater.DeregisterInRegions(pawn, map); // Move the human pawn into storage.
                if (map.listerThings.Contains(pawn))
                {
                    map.listerThings.Remove(pawn); // Similar to above, but prevents the corpse of the pawn from causing issues.
                    removed = true;
                }
            }

            // Make the pawn derpy.
            pawn.def = YingDefOf.Alien_Yinglet;

            if (removed && !map.listerThings.Contains(pawn))
            {
                map.listerThings.Add(pawn);
            }

            // If the pawn was on a map, return it from storage.
            if (map != null)
            {
                RegionListersUpdater.RegisterInRegions(pawn, map);
            }

            // Refresh the map's storage.
            map?.mapPawns.UpdateRegistryForPawn(pawn);
            
            // Refresh the pawn's appearence.
            RefreshGraphics(pawn);
            
            // Update the pawn's information (skin color, addons and the like).
            if (Current.ProgramState == ProgramState.Playing) pawn.ExposeData();

            // Make sure the yinglet stays the same faction.
            if (pawn.Faction != faction) pawn.SetFaction(faction);
        }

        /// <summary> Refresh the graphics of a pawn (including its portrait if it's a colonist). </summary>
        /// <param name="pawn"> The pawn to update the grphics of. </param>
        public static void RefreshGraphics([NotNull] Pawn pawn)
        {
            if (Current.ProgramState != ProgramState.Playing) return; //make sure we don't refresh the graphics while the game is loading
            pawn.Drawer.renderer.graphics.ResolveAllGraphics();
            if (pawn.IsColonist)
            {
                PortraitsCache.SetDirty(pawn);
            }
        }
    }
}
