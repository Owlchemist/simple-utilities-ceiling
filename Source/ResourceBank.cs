using Verse;
using UnityEngine;

namespace CeilingUtilities
{
	[StaticConstructorOnStartup]
	internal static class ResourceBank
	{
		public static readonly Texture2D icon = ContentFinder<Texture2D>.Get("UI/ShowCeilingFixtures", true);
		public static string label = "Owl_ToggleFixtures".Translate();
		public static readonly Graphic FireGraphicMulti = GraphicDatabase.Get<Graphic_FlickerMulti>("Things/OwlSpecial/FireTranparent", ShaderDatabase.TransparentPostLight, Vector2.one, Color.white);
	}
}