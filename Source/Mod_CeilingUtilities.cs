using HarmonyLib;
using Verse;
using UnityEngine;
using System.Collections.Generic;
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

		public static List<ThingDef> ceilingFixtures = new List<ThingDef>();
    }

	public class ModSettings_CeilingUtilities : ModSettings
	{
		public override void ExposeData()
		{
			Scribe_Values.Look<bool>(ref drawFixtures, "drawFixtures", true, false);
			Scribe_Values.Look<bool>(ref useDrawFixturesToggle, "useDrawFixturesToggle", true, false);
			base.ExposeData();
		}

		public static bool drawFixtures = true;
		public static bool useDrawFixturesToggle = true;
	}
}