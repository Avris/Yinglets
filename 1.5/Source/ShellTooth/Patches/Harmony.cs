using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using LudeonTK;
using RimWorld;
using System.Net.NetworkInformation;

namespace ShellTooth
{
    // Fixes graphical alignment while in bed
    internal class Harmony_HeadFix : PawnRenderSubWorker
    {
        public override void TransformOffset(PawnRenderNode node, PawnDrawParms parms, ref Vector3 offset, ref Vector3 pivot)
        {
            Pawn pawn = parms.pawn;
            if (pawn.CurrentBed() != null && pawn.def == YingDefOf.Alien_Yinglet)
            {
                if (pawn.CurrentBed().Rotation == Rot4.South)
                {
                    if (parms.facing == Rot4.South)
                    {
                        offset.z = -0.01f;
                    }
                    if (parms.facing == Rot4.East || parms.facing == Rot4.West)
                    {
                        offset.z = -0.07f;
                    }
                }
                if (pawn.CurrentBed().Rotation == Rot4.East || pawn.CurrentBed().Rotation == Rot4.West)
                {
                    if (parms.facing == Rot4.East || parms.facing == Rot4.West)
                    {
                        offset.z = -0.13f;
                    }
                }
                if (pawn.CurrentBed().Rotation == Rot4.North)
                {
                    if (parms.facing == Rot4.South)
                    {
                        offset.z = -0.23f;
                    }
                    if (parms.facing == Rot4.East || parms.facing == Rot4.West)
                    {
                        offset.z = -0.3f;
                    }
                }
            }
        }
    }
    internal class Harmony_BodyFix : PawnRenderSubWorker
    {
        public override void TransformOffset(PawnRenderNode node, PawnDrawParms parms, ref Vector3 offset, ref Vector3 pivot)
        {
            Pawn pawn = parms.pawn;
            if (pawn.CurrentBed() != null && pawn.def == YingDefOf.Alien_Yinglet)
            {
                if (pawn.CurrentBed().Rotation == Rot4.South)
                {
                    if (parms.facing == Rot4.South)
                    {
                        offset.z = -0.01f;
                    }
                    if (parms.facing == Rot4.East || parms.facing == Rot4.West)
                    {
                        offset.z = -0.03f;
                    }
                }
                if (pawn.CurrentBed().Rotation == Rot4.East || pawn.CurrentBed().Rotation == Rot4.West)
                {
                    if (parms.facing == Rot4.East || parms.facing == Rot4.West)
                    {
                        offset.y = -0.13f;
                    }
                }
                if (pawn.CurrentBed().Rotation == Rot4.North)
                {
                    if (parms.facing == Rot4.South)
                    {
                        offset.z = 0.4f;
                    }
                    if (parms.facing == Rot4.East || parms.facing == Rot4.West)
                    {
                        offset.z = 0.4f;
                    }
                }
            }
        }
    }
}
