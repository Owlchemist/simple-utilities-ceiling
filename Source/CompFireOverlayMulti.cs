using System.Collections.Generic;
using UnityEngine;
using Verse;
using RimWorld;
using static CeilingUtilities.ResourceBank;
using static CeilingUtilities.CeilingUtilitiesUtility;

namespace CeilingUtilities
{
	public class CeilingFixture : DefModExtension {}
	public class CompProperties_FireOverlayMulti : CompProperties_FireOverlay
	{
		public CompProperties_FireOverlayMulti()
		{
			this.compClass = typeof(CompFireOverlayMulti);
		}

		public override void DrawGhost(IntVec3 center, Rot4 rot, ThingDef thingDef, Color ghostCol, AltitudeLayer drawAltitude, Thing thing = null)
		{
			return;
		}

		public List<Vector3> offsets;
	}
	

	[StaticConstructorOnStartup]
	public class CompFireOverlayMulti : CompFireOverlay
	{
		public CompProperties_FireOverlayMulti propsCache;
		bool hasRefuelableComp;
		Vector3 drawPos;
		public int frame = 0;
		public Matrix4x4[] matrices;
		Rot4 rot = Rot4.North;
		
		public new CompProperties_FireOverlayMulti Props
		{
			get
			{
				return (CompProperties_FireOverlayMulti)this.props;
			}
		}

		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			propsCache = Props;
			fireCache.AddDistinct(this.parent.thingIDNumber, this);
			matrices = new Matrix4x4[propsCache.offsets.Count];
			hasRefuelableComp = refuelableComp != null;
			drawPos = parent.DrawPos;
			SetupMatrices();
		}

		public override void PostDeSpawn(Map map)
		{
			fireCache.Remove(this.parent.thingIDNumber);
			base.PostDeSpawn(map);
		}

		public override void PostDraw()
		{
			if (hasRefuelableComp && refuelableComp.HasFuel) FireGraphicMulti.Draw(drawPos, rot, parent, 0f);
		}

		void SetupMatrices()
		{
			for (int i = matrices.Length; i-- > 0;)
			{
				matrices[i] = default(Matrix4x4);

				//Only reason I'm not passing the actual position (first argument) is so that Perspective: Buidings is able to change the position on-the-fly. The y is never changes so it's hard coded.
				matrices[i].SetTRS(new Vector3(0,6.081081f,0), Quaternion.identity, new Vector3(propsCache.fireSize, 1f, propsCache.fireSize));
			}
		}
	}
}