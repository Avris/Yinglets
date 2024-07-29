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
    internal class DebugGizmoGiveEgg
    {
        public static Command_Action GetGizmo(YingComp comp)
        {
            Texture2D icon = ContentFinder<Texture2D>.Get("Things/Item/Yegg/YEGG", true);
            return new Command_Action
            {
                defaultLabel = "DEBUG: Give egg",
                defaultDesc = ($"Make {comp.parent} lay an egg."),
                icon = icon,
                action = () => EggMaker.KnockUp(comp.parent as Pawn, comp.parent as Pawn, 1f)
            };
        }
    }
}