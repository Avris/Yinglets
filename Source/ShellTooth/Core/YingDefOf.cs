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
        public static ThingDef Alien_Yinglet;
        public static ThingDef Alien_Younglet;
        public static ThingDef YingWorld_Tiplod;

        public static JobDef Forage;
        public static JobDef Yingify;
        public static JobDef Humps;

        public static BodyTypeDef Ying;
        public static BodyTypeDef YingFem;

        public static LetterDef YoungletGrown;

        public static LifeStageDef YoungletHatchling;
        public static LifeStageDef Younglet;
        public static LifeStageDef YoungletOlder;
        public static LifeStageDef YingletTeen;
        public static LifeStageDef Yinglet;

        public static ThingDef EggYingletFertilized;

        public static ThingCategoryDef YingletThing;
    }
}
