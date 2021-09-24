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
        static bool lastVal = drawFixtures;
		public static void Postfix(WidgetRow row, bool worldView)
		{
			if (worldView) return;
			
			row.ToggleableIcon(ref drawFixtures, ResourceBank.icon, ResourceBank.label, SoundDefOf.Mouseover_ButtonToggle, null);
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

	[StaticConstructorOnStartup]
	internal static class ResourceBank
	{
		public static readonly Texture2D icon = ContentFinder<Texture2D>.Get("UI/ShowCeilingFixtures", true);
		public static string label = "Owl_ToggleFixtures".Translate();
		public static readonly Graphic FireGraphicMulti = GraphicDatabase.Get<Graphic_FlickerMulti>("Things/OwlSpecial/FireTranparent", ShaderDatabase.TransparentPostLight, Vector2.one, Color.white);
	}
}