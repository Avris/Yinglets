using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using LudeonTK;

namespace ShellTooth
{
    internal class Harmony_HeadFix : PawnRenderSubWorker
    {
        public override void TransformOffset(PawnRenderNode node, PawnDrawParms parms, ref Vector3 offset, ref Vector3 pivot)
        {
            Pawn pawn = parms.pawn;
            if (pawn.def == YingDefOf.Alien_Yinglet)
            {
                if (parms.facing == Rot4.South)
                {
                    offset.z = -0.01f;
                }
                if (parms.facing == Rot4.East || parms.facing == Rot4.West)
                {
                    offset.z = -0.07f;
                }
                if (parms.facing == Rot4.North)
                {
                    offset.z = 0f;
                }
            }
        }
    }
    internal class Harmony_BodyFix : PawnRenderSubWorker
    {
        public override void TransformOffset(PawnRenderNode node, PawnDrawParms parms, ref Vector3 offset, ref Vector3 pivot)
        {
            Pawn pawn = parms.pawn;
            if (pawn.def == YingDefOf.Alien_Yinglet)
            {
                if (parms.facing == Rot4.South)
                {
                    offset.z = -0.01f;
                }
                if (parms.facing == Rot4.East || parms.facing == Rot4.West)
                {
                    offset.z = 0.03f;
                }
                if (parms.facing == Rot4.North)
                {
                    offset.z = 0f;
                }
            }
        }
    }
}
