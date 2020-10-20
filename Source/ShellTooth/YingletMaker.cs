using System;
using RimWorld;
using Verse;
using AlienRace;

namespace ShellTooth
{
 
    public partial class YingletMaker
    {
        public static void MakeYinglet(Pawn pawn)
        {
            try
            {
                switch (pawn.def.defName)
                {
                    case "Alien_Yinglet":
                        Log.Error("Tried to transform a yinglet into a yinglet! That probably shouldn't happen!");
                        break;
                    case "Human":
                        ApplyColor(pawn);
                        pawn.story.hairDef = DefDatabase<HairDef>.GetNamed("Shaved");
                        Messages.Message($"Something terrible has happened to {pawn}!", pawn, MessageTypeDefOf.NeutralEvent, true);
                        goto case "Alien_Younglet";
                    case "Alien_Younglet":
                        pawn.def = ThingDef.Named("Alien_Yinglet");
                        pawn.GetComp<AlienPartGenerator.AlienComp>().crownType = "Yinglet";
                        pawn.GetComp<AlienPartGenerator.AlienComp>().addonVariants = AddonAdder(pawn);
                        BackstoryGen(pawn);
                        break;
                    default:
                        PawnGenerationRequest request = new PawnGenerationRequest(
                            kind: PawnKindDef.Named("Yinglet"),
                            faction: pawn.Faction,
                            tile: pawn.Tile,
                            forceGenerateNewPawn: true,
                            canGeneratePawnRelations: false,
                            allowFood: false,
                            allowAddictions: false,
                            inhabitant: true,
                            fixedGender: pawn.gender,
                            fixedBiologicalAge: pawn.ageTracker.AgeBiologicalYearsFloat,
                            fixedChronologicalAge: pawn.ageTracker.AgeChronologicalYearsFloat
                        );
                        Pawn newbie = PawnGenerator.GeneratePawn(request);
                        IntVec3 ploc = pawn.Position;
                        Map pmap = pawn.Map;
                        newbie.Name = pawn.Name;
                        pawn.DeSpawn();
                        pawn.Discard(silentlyRemoveReferences: true);
                        GenSpawn.Spawn(newbie, ploc, pmap, WipeMode.Vanish);
                        newbie.Rotation = pawn.Rotation;
                        break;
                }
            }
            catch (NullReferenceException err)
            {
                Log.Error($"ShellTooth error: Tried to transform a {pawn.def.defName} into a yinglet, but failed! Here's why:");
                Log.Error(err.ToString());
            }
        }
    }   
}   