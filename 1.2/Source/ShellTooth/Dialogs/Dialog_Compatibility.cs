using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using UnityEngine;

namespace ShellTooth.Dialogs
{
	class Dialog_Compatibility : Window
	{
		public Dialog_Compatibility()
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
		}
	}
}
