using System.Linq;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace CeilingUtilities
{
	[HarmonyPatch(typeof(PowerNetGraphics), "PrintWirePieceConnecting")]
	public class Patch_PrintWirePieceConnecting
	{
		public static bool Prefix(Thing A, Thing B)
		{
			if (A.def.HasModExtension<CeilingFixture>()) return false;
			return true;
		}
    }
	[HarmonyPatch(typeof(Thing), "SpawnSetup")]
	public class Patch_SpawnSetup
	{
		public static void Postfix(ref Thing __instance, Map map)
		{
			if (__instance.def.HasModExtension<CeilingFixture>())
			{
				__instance.def.drawerType = DrawerType.RealtimeOnly;
				map.dynamicDrawManager.RegisterDrawable(__instance);
				__instance.def.drawerType = Patch_DoPlaySettingsGlobalControls.drawFixtures ? DrawerType.RealtimeOnly : DrawerType.None;
			}
		}
    }
}