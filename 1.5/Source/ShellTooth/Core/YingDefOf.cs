using System;
using RimWorld;
using Verse;

namespace ShellTooth
{
    [DefOf]
    public static class YingDefOf
    {
        static YingDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(ThingCategoryDefOf));
        }
        public static BackstoryDef YingifiedHumanlike;

        public static BodyDef YingletBody;
        public static BodyTypeDef Ying;
        public static BodyTypeDef YingFem;

        public static HeadTypeDef YingletHead;

        public static HediffDef Sleepying;
        public static HediffDef Yingletness;
        public static HediffDef YingShuteye;

        public static JobDef Forage;
        public static JobDef Humps;
        public static JobDef LayYingletEgg;
        public static JobDef Yingify;

        public static LetterDef YoungletGrown;

        public static MeditationFocusDef Whiskery;

        public static LifeStageDef YoungletHatchling;
        public static LifeStageDef Younglet;
        public static LifeStageDef YoungletOlder;
        public static LifeStageDef YingletTeen;
        public static LifeStageDef YingletAdult;

        public static PawnKindDef YingletKind;

        public static ScenPartDef EnableForaging;
        public static ScenPartDef YingletWorker;

        public static ThingCategoryDef YingletThing;

        public static ThingDef Alien_Yinglet;
        public static ThingDef Alien_Younglet;
        public static ThingDef YingWorld_Tiplod;
        public static ThingDef EggYingletFertilized;
    }
}
