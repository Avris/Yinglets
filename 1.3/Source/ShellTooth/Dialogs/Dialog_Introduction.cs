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
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(container, "Not a public version of the yinglet mod!\nExpect unfinished stuff and possible bugs.");
			Text.Anchor = TextAnchor.UpperLeft;
		}
	}
}
