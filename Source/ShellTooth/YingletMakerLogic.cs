using System;
using System.Collections.Generic;
using Verse;
using AlienRace;
using UnityEngine;
using RimWorld;
using UnityEngine.Experimental.PlayerLoop;

namespace ShellTooth
{
    public partial class YingletMaker
    {
        public Pawn ReplaceWithYing(Pawn pawn) {
            PawnGenerationRequest request = new PawnGenerationRequest(
                kind: PawnKindDef.Named("Yinglet"),
                faction: pawn.Faction,
                tile: pawn.Tile,
                forceGenerateNewPawn: true,
                canGeneratePawnRelations: false,
                allowFood: false,
                allowAddictions: false,
                inhabitant: true,
                fixedChronologicalAge: pawn.ageTracker.AgeBiologicalYearsFloat,
                fixedBiologicalAge: pawn.ageTracker.AgeBiologicalYearsFloat
            );
            Pawn newbie = PawnGenerator.GeneratePawn(request);
            newbie.Name = pawn.Name;
            newbie.Rotation = pawn.Rotation;
            newbie.relations = pawn.relations;
            newbie.interactions = pawn.interactions;
            newbie.story.bodyType = DefOfYinglet.Ying;
            newbie.apparel = new Pawn_ApparelTracker(newbie);
            if (pawn.def.defName == "Alien_Younglet")
            {
                newbie.GetComp<AlienPartGenerator.AlienComp>().ColorChannels["eye"] = pawn.GetComp<AlienPartGenerator.AlienComp>().ColorChannels["eye"];
                newbie.GetComp<AlienPartGenerator.AlienComp>().ColorChannels["skin"] = pawn.GetComp<AlienPartGenerator.AlienComp>().ColorChannels["skin"];
                newbie.GetComp<AlienPartGenerator.AlienComp>().ColorChannels["hair"] = pawn.GetComp<AlienPartGenerator.AlienComp>().ColorChannels["hair"];
                newbie.gender = pawn.gender;
                newbie.story.bodyType = BodyTyper(newbie);
                newbie.GetComp<YingComp>().wasYounglet = true;
            }
            else 
            {
                newbie.GetComp<YingComp>().wasOtherRace = pawn.def.defName;
            }
            return newbie;
        }
        public static BodyTypeDef BodyTyper(Pawn pawn)
        {
        System.Random r = new System.Random();
            switch (pawn.gender) {
                case Gender.Male:
                    return DefOfYinglet.Ying;
                case Gender.Female:
                    BodyTypeDef bodyType = (r.Next(0, 4) < 4) ? DefOfYinglet.YingFem : DefOfYinglet.Ying;
                    return bodyType;
                default:
                    Log.Error($"Assigning bodytype to {pawn} with unexpected gender {pawn.gender} - this may break!");
                    return DefOfYinglet.Ying;
            }
        }
        public void BackstoryGen(Pawn pawn) {
            PawnGenerationRequest request = new PawnGenerationRequest(
                kind: PawnKindDef.Named("Yinglet"),
                faction: pawn.Faction,
                forceGenerateNewPawn: true,
                fixedGender: pawn.gender,
                fixedBiologicalAge: 3f
                );
            Pawn genPawn = PawnGenerator.GeneratePawn(request); 
            pawn.verbTracker = genPawn.verbTracker;
            if (pawn.story.childhood.ToString() == "(Younglet)") {
                pawn.story.childhood = genPawn.story.childhood;
                pawn.story.adulthood = genPawn.story.adulthood;
            }
            if (pawn.story.adulthood == null) {
                pawn.story.adulthood = genPawn.story.adulthood;
            }
        }
        public List<int> AddonAdder(Pawn pawn)
        {
            System.Random r = new System.Random();
            int fur = r.Next(0, 1);
            int tth = r.Next(0, 1);
            int ear = r.Next(0, 2);
            int eye = r.Next(0, 3);
            int hrf = r.Next(0, 4);     //Hair Female
            int hrm = r.Next(0, 5);     //Hair Male
            List<int> bodyAddons = new List<int> { fur, fur, fur, fur, fur, tth, tth, ear, eye, eye, hrf, hrf, hrf, hrm, hrm };
            return bodyAddons;
        }
    }
}
