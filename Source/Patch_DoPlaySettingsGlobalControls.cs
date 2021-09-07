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
                DefDatabase<ThingDef>.AllDefs.Where(x => x.HasComp(typeof(CompCeilingFixture)) == true)?.ToList().ForEach(fixture => fixture.drawerType = drawFixtures? DrawerType.RealtimeOnly : DrawerType.None);
                lastVal = drawFixtures;
			}
		}
    }
}