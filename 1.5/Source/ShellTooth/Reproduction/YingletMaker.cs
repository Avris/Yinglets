using System;
using RimWorld;
using System.Collections.Generic;
using Verse;
using AlienRace;
using UnityEngine;

namespace ShellTooth
{
    public partial class YingletMaker
    {
        public Pawn MakeYinglet(Pawn pawn)
        {
            Pawn result = new Pawn();
            try
            {
                if (pawn.def.defName == "Alien_Yinglet")
                {
                    Log.Error("Tried to transform a yinglet into a yinglet! That probably shouldn't happen!");
                }
                else
                {
                    Pawn newbie = ReplaceWithYing(pawn);
                    pawn.Strip();
                    IntVec3 ploc1 = pawn.Position;
                    Map pmap1 = pawn.Map;
                    pawn.Destroy();
                    pawn.relations.hidePawnRelations = true;
                    GenSpawn.Spawn(newbie, ploc1, pmap1, WipeMode.Vanish);
                    newbie.TryGetComp<YingComp>().updateStamp = ShellTooth.currentVersion;
                    result = newbie;
                }
            }
            catch (NullReferenceException err)
            {
                Log.Error($"ShellTooth error: Tried to transform a {pawn.def.defName} into a yinglet, but failed! Here's why:");
                Log.Error(err.ToString());
            }
            return result;
        }
    }   
}   