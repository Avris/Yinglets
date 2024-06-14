using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace ShellTooth.Core
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
            };
            List<Pawn> AllYinglets = PawnsFinder.All_AliveOrDead.FindAll((Pawn pawn) => pawn.def == YingDefOf.Alien_Yinglet);
            int total = PawnsFinder.All_AliveOrDead.Count;

            foreach (Pawn pawn in AllYinglets)
            {
                if (pawn.GetComp<YingComp>().updateStamp != currentVersion)
                    switch (pawn.GetComp<YingComp>().updateStamp)
                    {
                        case "none":
                        case null:
                            FixBodyTypes(pawn, ref defs);
                            FixDefNaming(pawn, ref defs);
                            pawn.GetComp<YingComp>().updateStamp = currentVersion;
                            break;
                        case "1227: applied":
                        case "1227: not applied":
                            FixDefNaming(pawn, ref defs);
                            pawn.GetComp<YingComp>().updateStamp = currentVersion;
                            break;
                        default:
                            FixBodyTypes(pawn, ref defs);
                            FixDefNaming(pawn, ref defs);
                            pawn.GetComp<YingComp>().updateStamp = currentVersion;
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
                Log.Warning($"Checked {AllYinglets.Count} yinglets out of {total} pawns. Fixed {bodynum}, {headnum}, {kindnum}, {diffnum}, and {rdifnum}.");
            }
        }
        /// <summary>
        /// Reapplies missing scenpart from comp
        /// </summary>
        private static void ReapplyScenpart()
        {
            var scenario = Current.Game.Scenario.GetType().GetField("parts", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            List<ScenPart> scenparts = (List<ScenPart>)scenario.GetValue(Current.Game.Scenario);
            scenparts.Add(ScenarioMaker.MakeScenPart(YingDefOf.YingletDriver));
            Log.Warning("ShellTooth: restored missing scenpart, please don't remove that!");
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
                }
                else if ((pawn.gender == Gender.Male) && (bt != YingDefOf.Ying))
                {
                    pawn.story.bodyType = YingDefOf.Ying;
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
            else if (!pawn.health.hediffSet.HasHediff(HediffDef.Named("Yingletness")))
            {
                pawn.health.AddHediff(YingDefOf.Yingletness);
                defs["rdif"]++;
            }
        }
        public static string currentVersion = "Tiplod";
    }
}
