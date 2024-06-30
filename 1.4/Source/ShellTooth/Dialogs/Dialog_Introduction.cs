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
			Widgets.Label(container, "You're now cooking with yinglets!" 
			+ "\n\n\n\nRimworld 1.4 is not fully supported. Please update to 1.5!");
			Text.Anchor = TextAnchor.UpperLeft;
		}
	}
}
