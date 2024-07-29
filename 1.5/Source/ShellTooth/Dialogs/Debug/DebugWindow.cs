using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using UnityEngine;
using RimWorld;

namespace ShellTooth
{
    internal class Dialog_DebugWindow : Window
    {
        private Pawn _ying;
        public override Vector2 InitialSize
        {
            get
            {
                return new Vector2(1000f, 1000f);
            }
        }
        public Dialog_DebugWindow(Pawn ying)
        {
            forcePause = true;
            draggable = false;
            doCloseX = true;
            closeOnClickedOutside = false;
            absorbInputAroundWindow = false;
            Text.Font = GameFont.Small;
            _ying = ying;
        }
        public override void DoWindowContents(Rect container)
        {
            Rect test = new Rect(0, 0, 200, 200);
            string nondev = "Disabled in public version. If you're seeing this, please let the dev know!";
            string info = "";
            Widgets.Label(container, nondev);
        }
    }
}