using System;
using System.Collections.Generic;
using Verse;
using AlienRace;
using UnityEngine;
using RimWorld;

namespace ShellTooth
{
    public partial class YingletMaker
    {
        public Pawn ReplaceWithYing(Pawn pawn)
        {
            PawnGenerationRequest request = new PawnGenerationRequest(
                kind: PawnKindDef.Named("Yinglet"),
                faction: pawn.Faction,
                tile: pawn.Tile,
                forceGenerateNewPawn: true,
                canGeneratePawnRelations: false,
                allowFood: false,
                allowAddictions: false,
                inhabitant: true,
                fixedChronologicalAge: pawn.ageTracker.AgeChronologicalYearsFloat
            );
            Pawn newbie = PawnGenerator.GeneratePawn(request);
            newbie.Rotation = pawn.Rotation;
            newbie.story.bodyType = YingDefOf.Ying;
            newbie.apparel = new Pawn_ApparelTracker(newbie);
            if (pawn.def.race.Humanlike)
            {
                newbie.Name = pawn.Name;
                newbie.relations = pawn.relations;
                newbie.interactions = pawn.interactions;
                if (pawn.def.defName != "Alien_Younglet")
                {
                    newbie.skills = pawn.skills;
                    newbie.story.Childhood = pawn.story.Childhood;
                    newbie.story.Adulthood = DefDatabase<RimWorld.BackstoryDef>.GetNamed("YingifiedHumanlike");
                    if (pawn.story.traits.HasTrait(TraitDefOf.Beauty))
                    {
                        Trait pawnBeauty = pawn.story.traits.allTraits[pawn.story.traits.allTraits.FindIndex((Trait tr) => tr.def == TraitDefOf.Beauty)];
                        if (newbie.story.traits.HasTrait(TraitDefOf.Beauty))
                        {
                            pawnBeauty = newbie.story.traits.allTraits[newbie.story.traits.allTraits.FindIndex((Trait tr) => tr.def == TraitDefOf.Beauty)];
                        }
                        else
                        {
                            pawn.story.traits.allTraits.Remove(pawnBeauty);
                        }
                    }
                    if (pawn.gender != newbie.gender && !pawn.story.traits.HasTrait(TraitDefOf.Bisexual))
                    {
                        if (pawn.story.traits.HasTrait(TraitDefOf.Gay))
                        {
                            pawn.story.traits.allTraits.Remove(pawn.story.traits.allTraits[pawn.story.traits.allTraits.FindIndex((Trait tr) => tr.def == TraitDefOf.Gay)]);
                        }
                        else
                        {
                            pawn.story.traits.GainTrait(newbie.gender == Gender.Female ? new Trait(TraitDefOf.Bisexual) : new Trait(TraitDefOf.Gay));
                        }
                    }
                    newbie.story.traits = pawn.story.traits;
                }
                else
                {
                    newbie.ageTracker.AgeBiologicalTicks = pawn.ageTracker.AgeBiologicalTicks;
                    newbie.ageTracker.AgeChronologicalTicks = pawn.ageTracker.AgeBiologicalTicks;
                    newbie.GetComp<AlienPartGenerator.AlienComp>().ColorChannels["eye"] = pawn.GetComp<AlienPartGenerator.AlienComp>().ColorChannels["eye"];
                    newbie.GetComp<AlienPartGenerator.AlienComp>().ColorChannels["skin"] = pawn.GetComp<AlienPartGenerator.AlienComp>().ColorChannels["skin"];
                    newbie.GetComp<AlienPartGenerator.AlienComp>().ColorChannels["hair"] = pawn.GetComp<AlienPartGenerator.AlienComp>().ColorChannels["hair"];
                    newbie.gender = pawn.gender;
                    newbie.story.bodyType = BodyTyper(newbie);
                    newbie.GetComp<YingComp>().wasYounglet = true;
                }
            }
            else
            {
                newbie.Name = (pawn.Name == null ? newbie.Name : pawn.Name);
                newbie.story.Adulthood = DefDatabase<RimWorld.BackstoryDef>.GetNamed("YingifiedAnimal");
                newbie.story.Childhood = DefDatabase<RimWorld.BackstoryDef>.GetNamed("YingifiedAnimalChildhood");
            }
            return newbie;
        }
        public static BodyTypeDef BodyTyper(Pawn pawn)
        {
            System.Random r = new System.Random();
            switch (pawn.gender)
            {
                case Gender.Male:
                    return YingDefOf.Ying;
                case Gender.Female:
                    BodyTypeDef bodyType = (r.Next(0, 4) < 3) ? YingDefOf.YingFem : YingDefOf.Ying;
                    Log.Error(bodyType.ToString());
                    return bodyType;
                default:
                    Log.Error($"Assigning bodytype to {pawn} with unexpected gender {pawn.gender} - this may break!");
                    return YingDefOf.Ying;
            }
        }
        public void BackstoryGen(Pawn pawn)
        {
            PawnGenerationRequest request = new PawnGenerationRequest(
                kind: PawnKindDef.Named("Yinglet"),
                faction: pawn.Faction,
                forceGenerateNewPawn: true,
                fixedGender: pawn.gender,
                fixedBiologicalAge: 3f
                );
            Pawn genPawn = PawnGenerator.GeneratePawn(request);
            pawn.verbTracker = genPawn.verbTracker;
            if (pawn.story.Childhood.ToString() == "(Younglet)")
            {
                pawn.story.Childhood = genPawn.story.Childhood;
                pawn.story.Adulthood = genPawn.story.Adulthood;
            }
            if (pawn.story.Adulthood == null)
            {
                pawn.story.Adulthood = genPawn.story.Adulthood;
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
