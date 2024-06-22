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
			Widgets.Label(container, "Welcome to the Yinglet mod public beta!");
			Text.Font = GameFont.Small;
			Widgets.Label(container, "\n\nThis is a work in progress - expect unfinished stuff and possible bugs." +
				"\n\n\n" +
				"Please make sure to check the workshop page's buglist before reporting issues.\n" +
				"Otherwise, feel free to poke us in #yingworld channel on the official OoPs server!");
			Text.Font = GameFont.Medium;
			Text.Anchor = TextAnchor.UpperLeft;
		}
	}
}
