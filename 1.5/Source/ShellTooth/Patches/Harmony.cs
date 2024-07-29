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
    // Fixes graphical alignment
    internal class Harmony_HeadFix : PawnRenderSubWorker
    {
        public override void TransformOffset(PawnRenderNode node, PawnDrawParms parms, ref Vector3 offset, ref Vector3 pivot)
        {
            Pawn pawn = parms.pawn;
            if (pawn.def == YingDefOf.Alien_Yinglet)
            {
                if (pawn.CurrentBed() == null)
                // Fixes hats
                {
                    if (parms.facing == Rot4.East || parms.facing == Rot4.West)
                    {
                        offset.z = -0.1f;
                    }
                }
                else
                // Fixes while in bed
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
    }
    internal class Harmony_BodyFix : PawnRenderSubWorker
    {
        public override void TransformOffset(PawnRenderNode node, PawnDrawParms parms, ref Vector3 offset, ref Vector3 pivot)
        {
            Pawn pawn = parms.pawn;
            if (pawn.CurrentBed() != null && pawn.def == YingDefOf.Alien_Yinglet)
            // Fixes while in bed
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
    internal class Harmony_ClothesFix : PawnRenderSubWorker
    {
        public override void TransformOffset(PawnRenderNode node, PawnDrawParms parms, ref Vector3 offset, ref Vector3 pivot)
        {
            Pawn pawn = parms.pawn;
            if (pawn.def == YingDefOf.Alien_Yinglet)
            // Fixes clothes
            {
                if (parms.facing == Rot4.North)
                {
                    offset.x = northx;
                    offset.z = northz;
                }
                if (parms.facing == Rot4.South)
                {
                    offset.x = southx;
                    offset.z = southz;
                }
                if (parms.facing == Rot4.East)
                {
                    offset.x = eastwestx - (eastwestx * 2);
                    offset.z = eastwestz;
                }
                if (parms.facing == Rot4.West)
                {
                    offset.x = eastwestx;
                    offset.z = eastwestz;
                }
            }
        }
        [TweakValue("Shelltooth: clothesfix north x", -0.5f, 0.5f)]
        private static float northx = 0f;
        [TweakValue("Shelltooth: clothesfix north z", -0.5f, 0.5f)]
        private static float northz = 0f;
        [TweakValue("Shelltooth: clothesfix south x", -0.5f, 0.5f)]
        private static float southx = 0f;
        [TweakValue("Shelltooth: clothesfix south z", -0.5f, 0.5f)]
        private static float southz = 0f;
        [TweakValue("Shelltooth: clothesfix east/west x", -0.5f, 0.5f)]
        private static float eastwestx = 0.08f;
        [TweakValue("Shelltooth: clothesfix east/west z", -0.5f, 0.5f)]
        private static float eastwestz = -0.02f;
    }
}
