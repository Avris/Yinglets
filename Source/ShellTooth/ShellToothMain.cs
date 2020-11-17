using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace ShellTooth
{
    [StaticConstructorOnStartup]
    public static class FLAP
    {
        static FLAP()
        {
            Log.Message("An egg has been laid!");
        }
    }
    public class ShellTooth : Mod
    {
        public ShellTooth(ModContentPack content) : base(content)
        {
        }
    }
}
