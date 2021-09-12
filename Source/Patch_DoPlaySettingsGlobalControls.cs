using System.Linq;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace CeilingUtilities
{
	[HarmonyPatch(typeof(PlaySettings), "DoPlaySettingsGlobalControls")]
	public class Patch_DoPlaySettingsGlobalControls
	{
		public static bool drawFixtures = true;
        private static bool lastVal = drawFixtures;
		public static void Postfix(WidgetRow row, bool worldView)
		{
			if (worldView) return;
			
			row.ToggleableIcon(ref drawFixtures, ContentFinder<Texture2D>.Get("UI/ShowCeilingFixtures", true), "Owl_ToggleFixtures".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
			if (drawFixtures != lastVal)
			{
                Mod_CeilingUtilities.ceilingFixtures.ForEach(fixture => fixture.drawerType = drawFixtures? DrawerType.RealtimeOnly : DrawerType.None);
                lastVal = drawFixtures;
			}
		}
    }

	[HarmonyPatch (typeof(DefGenerator), nameof(DefGenerator.GenerateImpliedDefs_PostResolve))]
    static class Patch_DefGenerator
    {
        static void Postfix()
        {
            Mod_CeilingUtilities.ceilingFixtures = DefDatabase<ThingDef>.AllDefs.Where(x => x.HasModExtension<CeilingFixture>()).ToList();
        }
    }
}