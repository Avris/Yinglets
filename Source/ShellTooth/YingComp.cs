using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;
using UnityEngine;
using AlienRace;

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
        public string updateStamp = "none";
        public bool isDesignatedBreeder = false;
        public bool wasYounglet = false;
        public string wasOtherRace;
        public override void PostExposeData()
        {
            base.PostExposeData();

            Scribe_Values.Look(ref this.isDesignatedBreeder, label: "isDesignatedBreeder");
            Scribe_Values.Look(ref this.updateStamp, label: "updateStamp");
            Scribe_Values.Look(ref this.wasYounglet, label: "wasYounglet");
            Scribe_Values.Look(ref this.wasOtherRace, label: "wasOtherRace");
        }
        public override void CompTick()
        {
            if (updateStamp == "none" || updateStamp == null)
            {
                updateStamp = CheckBodyType(this.parent as Pawn) ? "1227: applied" : "1227: not applied";
            }
        }
        private bool CheckBodyType(Pawn pawn)
        {
            BodyTypeDef bt = pawn.story.bodyType;
            if ((pawn.def.defName == "Alien_Yinglet") && (pawn.gender == Gender.Female))
            {
                if (!(bt == YingDefOf.YingFem || bt == YingDefOf.Ying))
                {
                    (this.parent as Pawn).story.bodyType = YingletMaker.BodyTyper(pawn);
                    Log.Error($"Fixed bodytype for pawn {pawn}");
                    return true;
                }
            }
            if ((pawn.def.defName == "Alien_Yinglet") && (pawn.gender == Gender.Male))
            {
                if (bt != YingDefOf.Ying)
                {
                    (this.parent as Pawn).story.bodyType = YingDefOf.Ying;
                    Log.Error($"Fixed bodytype for pawn {pawn}");
                    return true;
                }
            }
            return false;
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
