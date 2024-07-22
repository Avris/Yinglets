using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using UnityEngine;

namespace ShellTooth
{
    internal class Dialog_DebugWindow : Window
    {
        private Pawn _ying;
        public override Vector2 InitialSize
        {
            get
            {
                return new Vector2(600f, 233f);
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
            Text.Font = GameFont.Medium;
            Text.Anchor = TextAnchor.UpperCenter;
            Widgets.Label(container, "You're now cooking with yinglets!\n\n\n\n");
            Widgets.Label(container, "weh");
            Text.Anchor = TextAnchor.UpperLeft;
        }
    }
}