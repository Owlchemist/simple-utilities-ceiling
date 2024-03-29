using HarmonyLib;
using Verse;
using UnityEngine;
using static CeilingUtilities.ModSettings_CeilingUtilities;

namespace CeilingUtilities
{
	public class Mod_CeilingUtilities : Mod
	{
		public Mod_CeilingUtilities(ModContentPack content) : base(content)
		{
			new Harmony(this.Content.PackageIdPlayerFacing).PatchAll();
			base.GetSettings<ModSettings_CeilingUtilities>();
		}

		public override void DoSettingsWindowContents(Rect inRect)
		{
			Listing_Standard options = new Listing_Standard();
			options.Begin(inRect);
			options.CheckboxLabeled("Utilities_useDrawFixturesToggle".Translate(), ref useDrawFixturesToggle, "Utilities_useDrawFixturesToggle".Translate());
			options.End();
			base.DoSettingsWindowContents(inRect);
		}

		public override string SettingsCategory()
		{
			return "Simple Utilities: Ceiling";
		}

		public override void WriteSettings()
		{
			base.WriteSettings();
		}
    }

	public class ModSettings_CeilingUtilities : ModSettings
	{
		public override void ExposeData()
		{
			Scribe_Values.Look(ref drawFixtures, "drawFixtures", true);
			Scribe_Values.Look(ref useDrawFixturesToggle, "useDrawFixturesToggle", true);
			base.ExposeData();
		}

		public static bool drawFixtures = true, useDrawFixturesToggle = true;
	}
}