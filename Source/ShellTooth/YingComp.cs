using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using UnityEngine;

namespace ShellTooth
{
    public class CompProperties_Yinglet : CompProperties
    {
        public CompProperties_Yinglet()
        {
            this.compClass = typeof(YingComp);
        }
    }
    public class YingComp : ThingComp
    {
        public bool isDesignatedBreeder = false;
        public bool wasYounglet = false;
        public string wasOtherRace;
        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref this.isDesignatedBreeder, label: "isDesignatedBreeder");
            Scribe_Values.Look(ref this.wasYounglet, label: "wasYounglet");
            Scribe_Values.Look(ref this.wasOtherRace, label: "wasOtherRace");
        }

    public override IEnumerable<Gizmo> CompGetGizmosExtra()
    {
            Texture2D heart = ContentFinder<Texture2D>.Get("Things/Mote/Heart", true);
            Texture2D noheart = ContentFinder<Texture2D>.Get("Things/Mote/SpeechSymbols/Breakup", true);
            Command_Toggle commandToggle = new Command_Toggle
                {
                defaultLabel = isDesignatedBreeder ? "Breeding ON" : "Breeding OFF",
                defaultDesc = isDesignatedBreeder ? 
                $"{this.parent} is assigned to breeding. Putting two available colonists in the same bed might result in an egg!" :
                $"{this.parent} is not assigned to breeding. Breeding attempts are unlikely in this state!",
                icon = isDesignatedBreeder ? heart : noheart,
                tutorTag = "MakeBreedable",
                isActive = () => isDesignatedBreeder,
                toggleAction = delegate ()
                {
                    isDesignatedBreeder = !isDesignatedBreeder;
                },
            };
            yield return commandToggle;
            }
    }
}
