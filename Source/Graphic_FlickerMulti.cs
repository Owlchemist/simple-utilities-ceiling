using UnityEngine;
using UnityEngine.Rendering;
using Verse;
using static CeilingUtilities.ModSettings_CeilingUtilities;
using static CeilingUtilities.CeilingUtilitiesUtility;

namespace CeilingUtilities
{
	public class Graphic_FlickerMulti : Graphic_Flicker
	{
		public Graphic_FlickerMulti()
		{
		}

		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			if (!drawFixtures || thingDef == null || this.subGraphics == null || !fireCache.TryGetValue(thing.thingIDNumber, out CompFireOverlayMulti comp)) return;

			if (updateNow && ++comp.frame == base.subGraphics.Length) comp.frame = 0;

			for (int i = 0; i < comp.matrices.Length; ++i)
			{
				Matrix4x4 matrix = comp.matrices[i];
				Vector3 vector = ((CompProperties_FireOverlayMulti)comp.props).offsets[i];
				matrix.m03 = loc.x + vector.x;
				matrix.m23 = loc.z + vector.z;

				Graphics.Internal_DrawMesh_Injected
				(
					MeshPool.plane10, //Mesh
					0, //SubMeshIndex
					ref matrix, //Matrix
					((Graphic_Single)this.subGraphics[(comp.frame + i) % 3]).mat, //Material
					0, //Layer
					null, //Camera
					null, //MaterialPropertiesBlock
					ShadowCastingMode.Off, //castShadows
					false, //recieveShadows
					null, //probeAnchor
					LightProbeUsage.Off, //LightProbeUseage
					null //LightProbeProxyVolume
				);
			}
		}
	}
}
