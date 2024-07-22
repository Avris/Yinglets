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
				return new Vector2(600f, 160f);
			}
		}
		public Dialog_Introduction()
		{
			this.forcePause = true;
			this.draggable = false;
			this.doCloseX = true;
			this.closeOnClickedOutside = false;
			this.absorbInputAroundWindow = false;
			Text.Font = GameFont.Small;
		}
		public override void DoWindowContents(Rect container)
		{
			Text.Font = GameFont.Medium;
			Text.Anchor = TextAnchor.UpperCenter;
            Widgets.Label(container, "THIS YINGLET MOD IS NOT SUPPORTED FOR 1.3");
            Text.Font = GameFont.Small;
			Widgets.Label(container, "\n\n\n\n\n\nThere are lots of bugs here right now! Run only if absolutely necessary.");
			Text.Font = GameFont.Medium;
			Text.Anchor = TextAnchor.UpperLeft;
		}
	}
}
