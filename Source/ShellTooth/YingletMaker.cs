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
        public void MakeYinglet(Pawn pawn)
        {
            try
            {
                Pawn newbie = ReplaceWithYing(pawn);
                switch (pawn.def.defName)
                {
                    case "Alien_Yinglet":
                        Log.Error("Tried to transform a yinglet into a yinglet! That probably shouldn't happen!");
                        break;
                    case "Alien_Younglet":
                        IntVec3 ploc1 = pawn.Position;
                        Map pmap1 = pawn.Map;
                        newbie.GetComp<AlienPartGenerator.AlienComp>().ColorChannels["eye"] = pawn.GetComp<AlienPartGenerator.AlienComp>().ColorChannels["eye"];
                        newbie.GetComp<AlienPartGenerator.AlienComp>().ColorChannels["skin"] = pawn.GetComp<AlienPartGenerator.AlienComp>().ColorChannels["skin"];
                        newbie.GetComp<AlienPartGenerator.AlienComp>().ColorChannels["hair"] = pawn.GetComp<AlienPartGenerator.AlienComp>().ColorChannels["hair"];
                        newbie.gender = pawn.gender;
                        newbie.ageTracker.AgeBiologicalTicks = pawn.ageTracker.AgeBiologicalTicks;
                        newbie.GetComp<YingComp>().wasYounglet = true;
                        pawn.DeSpawn();
                        GenSpawn.Spawn(newbie, ploc1, pmap1, WipeMode.Vanish);
                        break;
                    default:
                        pawn.Strip();
                        IntVec3 ploc2 = pawn.Position;
                        Map pmap2 = pawn.Map;
                        newbie.GetComp<YingComp>().wasOtherRace = pawn.def.defName;
                        GenSpawn.Spawn(newbie, ploc2, pmap2, WipeMode.Vanish);
                        pawn.DeSpawn();
                        break;
                }
            }
            catch (NullReferenceException err)
            {
                Log.Error($"ShellTooth error: Tried to transform a {pawn.def.defName} into a yinglet, but failed! Here's why:");
                Log.Error(err.ToString());
            }
        }
    }   
}   