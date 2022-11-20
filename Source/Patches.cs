using HarmonyLib;
using RimWorld;
using Verse;
using static CeilingUtilities.ResourceBank;
using static CeilingUtilities.ModSettings_CeilingUtilities;
using static CeilingUtilities.CeilingUtilitiesUtility;

namespace CeilingUtilities
{
	//Controls whether or not the little power wire should be drawn, realtime
	[HarmonyPatch(typeof(PowerNetGraphics), nameof(PowerNetGraphics.PrintWirePieceConnecting))]
	public class Patch_PrintWirePieceConnecting
	{
		public static bool Prefix(Thing A, Thing B)
		{
			return !A.def.HasModExtension<CeilingFixture>();
		}
    }

	//Gets the graphics ready on game load
	[HarmonyPatch(typeof(Thing), nameof(Thing.SpawnSetup))]
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

	//Centralized ticket for the fire graphic, vs every comp running its own ticker
	[HarmonyPatch(typeof(GameComponentUtility), nameof(GameComponentUtility.GameComponentTick))]
	public class Patch_GameComponentTick
	{
		static int ticks;
		public static void Postfix()
		{
			if (updateNow = ++ticks == 20) ticks = 0;
		}
    }

	//Visibility toggle
	[HarmonyPatch(typeof(PlaySettings), nameof(PlaySettings.DoPlaySettingsGlobalControls))]
	public class Patch_DoPlaySettingsGlobalControls
	{
        static bool lastVal = drawFixtures;
		public static void Postfix(WidgetRow row, bool worldView)
		{
			if (!useDrawFixturesToggle || worldView) return;
			
			row.ToggleableIcon(ref drawFixtures, icon, label, SoundDefOf.Mouseover_ButtonToggle, null);
			if (drawFixtures != lastVal)
			{
				foreach (var fixture in ceilingFixtures)
				{
					fixture.drawerType = drawFixtures? DrawerType.RealtimeOnly : DrawerType.None;
				}
                lastVal = drawFixtures;
				LoadedModManager.GetMod<Mod_CeilingUtilities>().WriteSettings();
			}
		}
    }
}