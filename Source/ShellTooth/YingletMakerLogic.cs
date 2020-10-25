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
        public BodyTypeDef BodyTyper(Pawn pawn)
        {
        System.Random r = new System.Random();
            switch (pawn.gender) {
                case Gender.Male:
                    Log.Error($"Applied Ying bodytype to {pawn}");
                    return DefOfYinglet.Ying;
                case Gender.Female:
                    BodyTypeDef bodyType = (r.Next(0, 4) < 4) ? DefOfYinglet.YingFem : DefOfYinglet.Ying;
                    Log.Error($"Applied {bodyType} bodytype to {pawn}");
                    return bodyType;
                default:
                    Log.Error($"Assigning bodytype to {pawn} with unexpected gender {pawn.gender} - this may break!");
                    return DefOfYinglet.Ying; ;
            }
        }
        public AlienPartGenerator.ExposableValueTuple<Color, Color> MakeColor(Color first, Color second)
        {
            return new AlienPartGenerator.ExposableValueTuple<Color, Color>(first, second);
        }
        public void ApplyColor(Pawn pawn)
        {
            pawn.GetComp<AlienPartGenerator.AlienComp>().crownType = "Yinglet";
            System.Random r = new System.Random();
            pawn.GetComp<AlienPartGenerator.AlienComp>().ColorChannels["skin"] = MakeColor(skinFirst[r.Next(skinFirst.Count)], skinSecond[r.Next(skinSecond.Count)]);
            pawn.GetComp<AlienPartGenerator.AlienComp>().ColorChannels["eye"] = MakeColor(eyeFirst[r.Next(eyeFirst.Count)], new Color(1f, 1f, 1f, 1f));
            pawn.GetComp<AlienPartGenerator.AlienComp>().ColorChannels["hair"].first = hairFirst[r.Next(hairFirst.Count)];
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

        private List<Color> skinFirst = new List<Color> {
        new Color(0.8f,0.71f,0.64f,1.0f),
        new Color(0.71f,0.6f,0.47f,1.0f),
        new Color(0.51f,0.39f,0.28f,1.0f),
        new Color(0.69f,0.58f,0.39f,1.0f),
        new Color(0.84f,0.8f,0.76f,1.0f),
        new Color(0.69f,0.69f,0.69f,1.0f),
        new Color(0.51f,0.4f,0.28f,1.0f),
        new Color(0.75f,0.53f,0.24f,1.0f),
        new Color(0.42f,0.31f,0.22f,1.0f),
        new Color(0.6f,0.48f,0.31f,1.0f),
        new Color(0.8f,0.71f,0.59f,1.0f),
        new Color(0.33f,0.33f,0.35f,1.0f),
        new Color(0.89f,0.89f,0.88f,1.0f),
        new Color(0.6f,0.51f,0.4f,1.0f),
        new Color(0.87f,0.89f,0.74f,1.0f),
        new Color(0.71f,0.67f,0.64f,1.0f),
        new Color(0.44f,0.39f,0.35f,1.0f),
        new Color(0.72f,0.6f,0.47f,1.0f),
        new Color(0.65f,0.5f,0.35f,1.0f),
        new Color(0.8275f,0.8235f,0.886f,1.0f)
        };
        private List<Color> skinSecond = new List<Color> {
        new Color(0.71f,0.55f,0.46f,1.0f),
        new Color(0.88f,0.75f,0.66f,1.0f),
        new Color(0.39f,0.45f,0.61f,1.0f),
        new Color(0.81f,0.52f,0.46f,1.0f)
        };
        private List<Color> eyeFirst = new List<Color> {
        new Color(1.0f,0.78f,0.36f,1.0f),
        new Color(1.0f,0.82f,0.28f,1.0f),
        new Color(0.57f,0.61f,0.78f,1.0f),
        new Color(0.18f,0.62f,0.74f,1.0f),
        new Color(1.0f,0.84f,0.82f,1.0f),
        new Color(1.0f,1.0f,1.0f,1.0f)
        };
        private List<Color> hairFirst = new List<Color> {
        new Color(0.87f,0.79f,0.57f,1.0f),
        new Color(0.23f,0.19f,0.15f,1.0f),
        new Color(0.35f,0.3f,0.24f,1.0f),
        new Color(0.59f,0.3f,0.15f,1.0f),
        new Color(0.77f,0.78f,0.81f,1.0f),
        new Color(0.61f,0.5f,0.35f,1.0f),
        new Color(0.43f,0.3f,0.16f,1.0f),
        new Color(1f,1.0f,1.0f,1.0f)
        };
    }
}
