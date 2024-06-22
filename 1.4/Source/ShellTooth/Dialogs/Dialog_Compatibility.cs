using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using UnityEngine;

namespace ShellTooth
{
	class Dialog_Compatibility : Window
	{
		public Dialog_Compatibility()
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
		}
	}
}
