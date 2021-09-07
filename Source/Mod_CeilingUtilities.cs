using HarmonyLib;
using Verse;

namespace CeilingUtilities
{
	public class Mod_CeilingUtilities : Mod
	{
		public Mod_CeilingUtilities(ModContentPack content) : base(content)
		{
			new Harmony(this.Content.PackageIdPlayerFacing).PatchAll();
		}
    }
}