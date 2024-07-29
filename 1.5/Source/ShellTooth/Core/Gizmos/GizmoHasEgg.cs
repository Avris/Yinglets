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
    internal class GizmoHasEgg
    {
        public static Command_Action GetGizmo(YingComp comp)
        {
            Texture2D icon = ContentFinder<Texture2D>.Get("Things/Item/Yegg/YEGG", true);
            return new Command_Action
            {
                defaultLabel = "Has egg!",
                defaultDesc = ($"{comp.parent} can't be assigned to breeding while she's growing an egg."),
                icon = icon,
                action = new Action(ShellTooth.DoNothing)
            };
        }
    }
}
