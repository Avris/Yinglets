using System;
using RimWorld;
using Verse;

namespace ShellTooth
{
    [DefOf]
    public static class DefOfYinglet
    {
        static DefOfYinglet()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(ThingCategoryDefOf));
        }

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

    }
}
