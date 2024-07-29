using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using UnityEngine;
using System.Linq.Expressions;

namespace ShellTooth
{
    internal class GizmoSterile
    {
        public static Command_Action GetGizmo(YingComp comp)
        {
            comp.isDesignatedBreeder = false;
            Texture2D icon = ContentFinder<Texture2D>.Get("Things/Mote/NotBreedable", true);
            return new Command_Action
            {
                defaultLabel = "Sterilised!",
                defaultDesc = ($"{comp.parent} is no longer able to breed."),
                icon = icon,
                action = new Action(ShellTooth.DoNothing)
            };
        }
    }
}
