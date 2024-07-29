using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using UnityEngine;
using System.Linq.Expressions;
using System.Net.NetworkInformation;

namespace ShellTooth
{
    internal class DebugGizmoWindow
    {
        public static Command_Action GetGizmo(YingComp comp)
        {
            Texture2D icon = ContentFinder<Texture2D>.Get("World/Expanding/Yinglet/Yinglet_Player_Icon", true);
            return new Command_Action
            {
                defaultLabel = "DEBUG Dev Window",
                defaultDesc = ($"Let's take a look"),
                icon = icon,
                action = () => Find.WindowStack.Add(new Dialog_DebugWindow(comp.parent as Pawn))
            };
        }
    }
}
