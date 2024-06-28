using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using UnityEngine;

namespace ShellTooth
{
	public class Dialog_Introduction : Window
	{
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(600f, 200f);
			}
		}
		public Dialog_Introduction()
		{
			forcePause = true;
			draggable = false;
			doCloseX = true;
			closeOnClickedOutside = false;
			absorbInputAroundWindow = false;
			Text.Font = GameFont.Small;
		}
		public override void DoWindowContents(Rect container)
		{
			Text.Font = GameFont.Medium;
			Text.Anchor = TextAnchor.UpperCenter;
            Widgets.Label(container, "You're now cooking with yinglets!");
            Text.Font = GameFont.Small;
            Text.Anchor = TextAnchor.UpperLeft;
            Widgets.Label(container, "\n\n\n\nHeads up! 1.5 support is new, and old saves may load wrong. Try not to overwrite them!\n\n\nPlease report gamebreaking bugs in the Steam thread or on the #YingWorld channel on the OoPs discord, and have fun!");
        }
	}
}
