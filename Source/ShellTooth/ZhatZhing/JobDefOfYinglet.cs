using System;
using RimWorld;
using Verse;

namespace ShellTooth
{
    [DefOf]
    public static class JobDefOfYinglet
    {
        static JobDefOfYinglet()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(ThingCategoryDefOf));
        }

        public static JobDef Yingify;
        public static JobDef Humps;
    }
}
