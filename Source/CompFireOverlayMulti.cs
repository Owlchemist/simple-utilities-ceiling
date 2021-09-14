using System.Collections.Generic;
using UnityEngine;
using Verse;
using RimWorld;

namespace CeilingUtilities
{
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

		public override void PostDraw()
		{
			if (this.refuelableComp != null && !this.refuelableComp.HasFuel) return;

			Vector3 drawPos = this.parent.DrawPos;
			drawPos.y += 0.04054054f;
			FireGraphicMulti.Draw(drawPos, Rot4.North, this.parent, 0f);
		}

		static readonly Graphic FireGraphicMulti = GraphicDatabase.Get<Graphic_FlickerMulti>("Things/OwlSpecial/FireTranparent", ShaderDatabase.TransparentPostLight, Vector2.one, Color.white);
		public int frame = 0;
	}
}