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
		public static bool Prefix(Thing A)
		{
			return !A.def.HasModExtension<CeilingFixture>();
		}
    }

	//Gets the graphics ready on game load
	[HarmonyPatch(typeof(Building), nameof(Building.SpawnSetup))]
	class Patch_SpawnSetup
	{
		static void Postfix(Thing __instance, Map map)
		{
			if (__instance.def.HasModExtension<CeilingFixture>())
			{
				__instance.def.drawerType = DrawerType.RealtimeOnly;
				map.dynamicDrawManager.RegisterDrawable(__instance);
				__instance.def.drawerType = drawFixtures ? DrawerType.RealtimeOnly : DrawerType.None;
			}
		}
    }

	//Visibility toggle
	[HarmonyPatch(typeof(PlaySettings), nameof(PlaySettings.DoPlaySettingsGlobalControls))]
	class Patch_DoPlaySettingsGlobalControls
	{
        static bool lastVal = drawFixtures;
		static void Postfix(WidgetRow row, bool worldView)
		{
			if (!useDrawFixturesToggle || worldView) return;
			
			row.ToggleableIcon(ref drawFixtures, icon, label, SoundDefOf.Mouseover_ButtonToggle, null);
			if (drawFixtures != lastVal)
			{
				for (int i = ceilingFixtures.Length; i-- > 0;)
				{
					ceilingFixtures[i].drawerType = drawFixtures? DrawerType.RealtimeOnly : DrawerType.None;
				}
                lastVal = drawFixtures;
				LoadedModManager.GetMod<Mod_CeilingUtilities>().WriteSettings();
			}
		}
    }
}