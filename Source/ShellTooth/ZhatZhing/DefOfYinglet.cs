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
        public static LetterDef Younglet;
    }
}
