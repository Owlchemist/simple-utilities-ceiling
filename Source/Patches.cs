using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;
using static CeilingUtilities.ResourceBank;
using static CeilingUtilities.Mod_CeilingUtilities;
using static CeilingUtilities.ModSettings_CeilingUtilities;

namespace CeilingUtilities
{
	//Controls whether or not the little power wire should be drawn, realtime
	[HarmonyPatch(typeof(PowerNetGraphics), "PrintWirePieceConnecting")]
	public class Patch_PrintWirePieceConnecting
	{
		public static bool Prefix(Thing A, Thing B)
		{
			if (A.def.HasModExtension<CeilingFixture>()) return false;
			return true;
		}
    }

	//Gets the graphics ready on game load
	[HarmonyPatch(typeof(Thing), "SpawnSetup")]
	public class Patch_SpawnSetup
	{
		public static void Postfix(ref Thing __instance, Map map)
		{
			if (__instance.def.HasModExtension<CeilingFixture>())
			{
				__instance.def.drawerType = DrawerType.RealtimeOnly;
				map.dynamicDrawManager.RegisterDrawable(__instance);
				__instance.def.drawerType = drawFixtures ? DrawerType.RealtimeOnly : DrawerType.None;
			}
		}
    }

	//Keeps a cached list of which defs are ceiling mounted, for use of the visibiltiy toggle
	[HarmonyPatch (typeof(DefGenerator), nameof(DefGenerator.GenerateImpliedDefs_PostResolve))]
    static class Patch_DefGenerator
    {
        static void Postfix()
        {
            ceilingFixtures = DefDatabase<ThingDef>.AllDefs.Where(x => x.HasModExtension<CeilingFixture>()).ToList();
        }
    }

	//Visibility toggle
	[HarmonyPatch(typeof(PlaySettings), "DoPlaySettingsGlobalControls")]
	public class Patch_DoPlaySettingsGlobalControls
	{
        static bool lastVal = drawFixtures;
		public static void Postfix(WidgetRow row, bool worldView)
		{
			if (!useDrawFixturesToggle) return;
			if (worldView) return;
			
			row.ToggleableIcon(ref drawFixtures, icon, label, SoundDefOf.Mouseover_ButtonToggle, null);
			if (drawFixtures != lastVal)
			{
                ceilingFixtures.ForEach(fixture => fixture.drawerType = drawFixtures? DrawerType.RealtimeOnly : DrawerType.None);
                lastVal = drawFixtures;
				LoadedModManager.GetMod<Mod_CeilingUtilities>().WriteSettings();
			}
		}
    }
}