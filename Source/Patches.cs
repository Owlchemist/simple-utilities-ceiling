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
			return !CeilingUtilitiesUtility.ceilingFixtureHashes.Contains(A.def.shortHash);
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
					var def = ceilingFixtures[i];
					def.drawerType = drawFixtures ? CeilingUtilitiesUtility.drawerTypeLedger.TryGetValue(def.shortHash, out DrawerType drawerType) ? drawerType : DrawerType.MapMeshOnly : DrawerType.None;
				}

				Find.CurrentMap.mapDrawer.WholeMapChanged(MapMeshFlag.Things | MapMeshFlag.Buildings);
				
                lastVal = drawFixtures;
				LoadedModManager.GetMod<Mod_CeilingUtilities>().WriteSettings();
			}
		}
    }
}