using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace ShellTooth
{
    internal class Harmony_HatFix : PawnRenderSubWorker
    {
        public override void TransformOffset(PawnRenderNode node, PawnDrawParms parms, ref Vector3 offset, ref Vector3 pivot)
        {
            Pawn pawn = parms.pawn;
            if (pawn.def == YingDefOf.Alien_Yinglet)
            {
                if (parms.facing == Rot4.South)
                {
                    offset.z = 0f;
                }
                if (parms.facing == Rot4.East || parms.facing == Rot4.West)
                {
                    offset.z = -0.1f;
                }
                if (parms.facing == Rot4.North)
                {
                    offset.z = 0f;
                }
            }
        }
    }
}
