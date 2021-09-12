using HarmonyLib;
using Verse;
using System.Collections.Generic;

namespace CeilingUtilities
{
	public class Mod_CeilingUtilities : Mod
	{
		public Mod_CeilingUtilities(ModContentPack content) : base(content)
		{
			new Harmony(this.Content.PackageIdPlayerFacing).PatchAll();
		}

		public static List<ThingDef> ceilingFixtures = new List<ThingDef>();
    }
}