using UnityEngine;
using Verse;
using static CeilingUtilities.ModSettings_CeilingUtilities;

namespace CeilingUtilities
{
	public class Graphic_FlickerMulti : Graphic_Flicker
	{
		public Graphic_FlickerMulti()
		{
		}

		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			if (!drawFixtures || thingDef == null || this.subGraphics == null) return;

			CompFireOverlayMulti comp = thing.TryGetComp<CompFireOverlayMulti>();
			if (comp == null) return;

			if (Find.TickManager.TicksGame % 20 == 0 && ++comp.frame == base.subGraphics.Length) comp.frame = 0;

			for (int i = 0; i < comp.numOfOffsets; i++)
			{
				comp.matrix[i].m03 = loc.x + comp.Props.offsets[i].x;
				comp.matrix[i].m23 = loc.z + comp.Props.offsets[i].z;
				Graphics.DrawMesh(MeshPool.plane10, comp.matrix[i], this.subGraphics[(comp.frame + i) % 3].MatSingle, 0);
			}
		}
	}
}
