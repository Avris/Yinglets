﻿using System;
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
            if ((YingDefOf.EggYingletFertilized.comps[3] as CompProperties_Hatcher).hatcherDaystoHatch != ShellTooth.yingletEgg)
            {
                (YingDefOf.EggYingletFertilized.comps[3] as CompProperties_Hatcher).hatcherDaystoHatch = ShellTooth.yingletEgg;
            }
        }
    }
}