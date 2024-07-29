using System;
using Verse;
using RimWorld;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

namespace ShellTooth
{
    public class ScenPart_YingletWorker : ScenPart
    {
        private bool splashed = false;
        public override void ExposeData()
        {
            base.ExposeData();
        }
        public override void Tick()
        {
            if (!splashed)
            {
                Find.WindowStack.Add(new Dialog_Introduction());
                CompatibilityUpdater.UpdaterCore();
                splashed = true;
            }
            // This applies the debug slider
            CompProperties_Hatcher comp = YingDefOf.EggYingletFertilized.comps.Find(c => c is CompProperties_Hatcher) as CompProperties_Hatcher;
            if (comp.hatcherDaystoHatch != ShelltoothSettings.yingletEgg)
            {
                comp.hatcherDaystoHatch = ShelltoothSettings.yingletEgg;
            }
        }
    }
}