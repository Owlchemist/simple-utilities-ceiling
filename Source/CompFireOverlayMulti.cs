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
			fireCache.Add(this.parent, this);
			matrices = new Matrix4x4[Props.offsets.Count];
			SetupMatrices();
		}

		public override void PostDeSpawn(Map map)
		{
			fireCache.Remove(this.parent);
			base.PostDeSpawn(map);
		}

		public override void PostDraw()
		{
			if (this.refuelableComp != null && !this.refuelableComp.HasFuel) return;
			FireGraphicMulti.Draw(this.parent.DrawPos, rot, this.parent, 0f);
		}

		void SetupMatrices()
		{
			for (int i = 0; i < matrices.Length; ++i)
			{
				matrices[i] = default(Matrix4x4);

				//Only reason I'm not passing the actual position (first argument) is so that Perspective: Buidings is able to change the position on-the-fly. The y is never changes so it's hard coded.
				matrices[i].SetTRS(new Vector3(0,6.081081f,0), Quaternion.identity, new Vector3(Props.fireSize, 1f, Props.fireSize));
			}
		}

		public int frame = 0;

		//Performance cached fields
		public Matrix4x4[] matrices;
		Rot4 rot = Rot4.North;
	}
}