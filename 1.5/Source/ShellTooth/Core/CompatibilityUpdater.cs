using AlienRace;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace ShellTooth
{
    internal class CompatibilityUpdater
    {
        /// <summary>
        /// Update old saves' yinglets to new values
        /// </summary>
        public static void UpdaterCore()
        {
            Dictionary<string, int> defs = new Dictionary<string, int>()
            {
            { "body", 0 },
            { "head", 0 },
            { "kind", 0 },
            { "diff", 0 },
            { "rdif", 0 },
            { "type", 0 },
            { "part", 0 },
            { "rels", 0 }
            };
            List<Pawn> AllYinglets = PawnsFinder.All_AliveOrDead.FindAll((Pawn pawn) => pawn.def == YingDefOf.Alien_Yinglet);
            int total = PawnsFinder.All_AliveOrDead.Count;

            foreach (Pawn pawn in AllYinglets)
            {
                if (pawn.GetComp<YingComp>().updateStamp != ShellTooth.currentVersion)
                    switch (pawn.GetComp<YingComp>().updateStamp)
                    {
                        case "none":
                        case null:
                            FixBodyTypes(pawn, ref defs);
                            FixDefNaming(pawn, ref defs);
                            UpdateAddons(pawn, ref defs);
                            pawn.GetComp<YingComp>().updateStamp = ShellTooth.currentVersion;
                            break;
                        case "1227: applied":
                        case "1227: not applied":
                            FixDefNaming(pawn, ref defs);
                            UpdateAddons(pawn, ref defs);
                            pawn.GetComp<YingComp>().updateStamp = ShellTooth.currentVersion;
                            break;
                        case "Tiplod":
                            UpdateAddons(pawn, ref defs);
                            pawn.GetComp<YingComp>().updateStamp = ShellTooth.currentVersion;
                            break;
                        case "Tiplod Dev":
                            FixRelGhosts(pawn, ref defs);
                            pawn.GetComp<YingComp>().updateStamp = ShellTooth.currentVersion;
                            break;
                        default:
                            FixBodyTypes(pawn, ref defs);
                            FixDefNaming(pawn, ref defs);
                            UpdateAddons(pawn, ref defs);
                            FixRelGhosts(pawn, ref defs);
                            pawn.GetComp<YingComp>().updateStamp = ShellTooth.currentVersion;
                            break;
                    }
            }
            if (defs["body"] > 0 || defs["head"] > 0 || defs["kind"] > 0 || defs["diff"] > 0 || defs["rdif"] > 0)
            {
                string bodynum = ((defs["body"] == 0 || defs["body"] > 1) ? $"{defs["body"]} bodies" : $"{defs["body"]} body");
                string headnum = ((defs["head"] == 0 || defs["head"] > 1) ? $"{defs["head"]} heads" : $"{defs["head"]} head");
                string kindnum = ((defs["kind"] == 0 || defs["kind"] > 1) ? $"{defs["kind"]} kinds" : $"{defs["kind"]} kind");
                string diffnum = ((defs["diff"] == 0 || defs["diff"] > 1) ? $"{defs["diff"]} hediffs" : $"{defs["diff"]} hediff");
                string rdifnum = ((defs["rdif"] == 0 || defs["rdif"] > 1) ? $"{defs["rdif"]} missing hediffs" : $"{defs["rdif"]} missing hediff");
                string typenum = ((defs["type"] == 0 || defs["type"] > 1) ? $"{defs["type"]} bodytypes" : $"{defs["type"]} bodytype");
                string partnum = ((defs["part"] == 0 || defs["part"] > 1) ? $"{defs["part"]} bodyaddons" : $"{defs["part"]} bodyaddon");
                Log.Warning($"Checked {AllYinglets.Count} yinglets out of {total} pawns. Fixed {bodynum}, {headnum}, {kindnum}, {diffnum}, {rdifnum}, {typenum}, and {partnum}");
            }
        }
        /// <summary>
        /// Reapplies missing scenpart from comp
        /// </summary>
        private static void ReapplyScenpart()
        {
            var scenario = Current.Game.Scenario.GetType().GetField("parts", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            List<ScenPart> scenparts = (List<ScenPart>)scenario.GetValue(Current.Game.Scenario);
            scenparts.Add(ScenarioMaker.MakeScenPart(YingDefOf.YingletWorker));
            Log.Warning("ShellTooth: restored missing scenpart, please don't remove that!");
        }
        /// <summary>
        /// Updates to the new bodyaddon structure
        /// </summary>
        /// <param name="pawn"></param>
        private static void UpdateAddons(Pawn pawn, ref Dictionary<string, int> defs)
        {
            List<int> parts = pawn.GetComp<AlienPartGenerator.AlienComp>().addonVariants;
            if ((pawn.def.defName == "Alien_Yinglet") && (parts != null) && (parts.Count == 15))
            {
                if (pawn.gender == Gender.Female)
                {
                    pawn.GetComp<AlienPartGenerator.AlienComp>().addonVariants = new List<int> { parts[0], parts[1], parts[3], parts[5], parts[7], parts[8], parts[10], parts[11], parts[12] };
                }
                else
                {
                    pawn.GetComp<AlienPartGenerator.AlienComp>().addonVariants = new List<int> { parts[0], parts[1], parts[4], parts[6], parts[7], parts[9], parts[13], parts[14], parts[12] };
                }
                defs["part"]++;
            }
        }
        /// <summary>
        /// Fixes saves from when bodytypes were applied incorrectly
        /// </summary>
        /// <param name="pawn"></param>
        private static void FixBodyTypes(Pawn pawn, ref Dictionary<string, int> defs)
        {
            BodyTypeDef bt = pawn.story.bodyType;
            if (pawn.def.defName == "Alien_Yinglet")
            {
                if ((pawn.gender == Gender.Female) && !(bt == YingDefOf.YingFem || bt == YingDefOf.Ying))
                {
                    pawn.story.bodyType = YingletMaker.BodyTyper(pawn);
                    defs["type"]++;
                }
                else if ((pawn.gender == Gender.Male) && (bt != YingDefOf.Ying))
                {
                    pawn.story.bodyType = YingDefOf.Ying;
                    defs["type"]++;
                }
            }
        }
        /// <summary>
        /// Updates saves to use new def naming conventions
        /// </summary>
        public static void FixDefNaming(Pawn pawn, ref Dictionary<string, int> defs)
        {
            if (pawn.RaceProps.body != YingDefOf.YingletBody)
            {
                pawn.RaceProps.body = YingDefOf.YingletBody;
                defs["body"]++;
            }
            if (pawn.story.headType != YingDefOf.YingletHead)
            {
                pawn.story.headType = YingDefOf.YingletHead;
                defs["head"]++;
            }
            if (pawn.kindDef != YingDefOf.YingletKind)
            {
                pawn.kindDef = YingDefOf.YingletKind;
                defs["kind"]++;
            }
            if (pawn.health.hediffSet.HasHediff(HediffDef.Named("Yinglet")))
            {
                pawn.health.AddHediff(YingDefOf.Yingletness);
                pawn.health.RemoveHediff(pawn.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named("Yinglet")));
                defs["diff"]++;
            }
            else if (!pawn.health.hediffSet.HasHediff(YingDefOf.Yingletness))
            {
                pawn.health.AddHediff(YingDefOf.Yingletness);
                defs["rdif"]++;
            }
        }

        /// <summary>
        /// Updates saves to use new def naming conventions
        /// </summary>
        public static void FixRelGhosts(Pawn pawn, ref Dictionary<string, int> defs)
        {
            foreach (Pawn rel in pawn.relations.RelatedPawns)
            {
                if (rel.Destroyed)
                {
                    rel.relations.hidePawnRelations = true;
                    defs["rels"]++;
                }
            }
        }
    }
}
