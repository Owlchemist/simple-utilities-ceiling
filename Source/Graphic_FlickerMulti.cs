using System;
using RimWorld;
using UnityEngine;
using Verse;
using System.Linq;
using System.Reflection;
using HarmonyLib;

namespace CeilingUtilities
{
	public class Graphic_FlickerMulti : Graphic_Flicker
	{
		public Graphic_FlickerMulti()
		{
		}
		
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			if (!Patch_DoPlaySettingsGlobalControls.drawFixtures) return;

			if (thingDef == null)
			{
				Log.ErrorOnce("Fire DrawWorker with null thingDef: " + loc, 3427324);
				return;
			}
			if (this.subGraphics == null)
			{
				Log.ErrorOnce("Graphic_FlickerMulti has no subgraphics " + thingDef, 358773632);
				return;
			}
			var comp = thing.TryGetComp<CompFireOverlayMulti>(); //todo: this is laggy, cache it or something...

			if (Find.TickManager.TicksGame % 20 == 0) comp.frame++;
			if (comp.frame >= this.subGraphics.Length) comp.frame = 0;
			
			
			float num4 = comp.Props.fireSize;
			var s = new Vector3(num4, 1f, num4);

			for (int i = 0; i < comp.Props.offsets.Count; i++)
			{
				Fire fire = thing as Fire;

				Graphic graphic = this.subGraphics[(comp.frame + i) % 3];

				Vector3 a = GenRadial.RadialPattern[(Find.TickManager.TicksGame % 20) % GenRadial.RadialPattern.Length].ToVector3() / GenRadial.MaxRadialPatternRadius;
				a *= 0.05f;
				Vector3 vector = (loc + a * num4) + comp.Props.offsets[i];

				Matrix4x4 matrix = default(Matrix4x4);
				matrix.SetTRS(vector, Quaternion.identity, s);
				Graphics.DrawMesh(MeshPool.plane10, matrix, graphic.MatSingle, 0);
			}
		}
	}
}
