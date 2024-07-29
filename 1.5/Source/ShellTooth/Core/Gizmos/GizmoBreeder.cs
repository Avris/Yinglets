using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using UnityEngine;

namespace ShellTooth
{
    internal class GizmoBreeder
    {
        public static Command_Toggle GetGizmo(YingComp comp)
        {
            Texture2D heart = ContentFinder<Texture2D>.Get("Things/Mote/Heart", true);
            Texture2D noheart = ContentFinder<Texture2D>.Get("Things/Mote/SpeechSymbols/Breakup", true);
            return new Command_Toggle
            {
                defaultLabel = comp.isDesignatedBreeder ? "Breeding ON" : "Breeding OFF",
                defaultDesc = comp.isDesignatedBreeder ?
                $"{comp.parent} is assigned to breeding. Putting two available yinglets in the same bed might result in an egg!" :
                $"{comp.parent} is not assigned to breeding. Breeding attempts won't happen!",
                icon = comp.isDesignatedBreeder ? heart : noheart,
                tutorTag = "MakeBreedable",
                isActive = () => comp.isDesignatedBreeder,
                toggleAction = delegate ()
                {
                    comp.isDesignatedBreeder = !comp.isDesignatedBreeder;
                }
            };
        }
    }
}
