using Verse;
using RimWorld;

namespace CeilingUtilities
{
	public class PlaceWorker_OnlyUnderRoof : PlaceWorker
	{
		public override AcceptanceReport AllowsPlacing(BuildableDef def, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			foreach (IntVec3 cell in GenAdj.OccupiedRect(loc, rot, def.Size))
			{
				//Check for roof
				if (!map.roofGrid.Roofed(cell)) return new AcceptanceReport("WorkPlacer_NeedsRoof".Translate());

				//Check what things are in this cell
				foreach (Thing thingHere in map.thingGrid.ThingsListAtFast(cell))
				{
					//Valid building?
					if (thingHere is Building building && (CeilingValidate(building.def))) return new AcceptanceReport("WorkPlacer_OverForbiddenThing".Translate());

					//Or valid blueprint?
					if (thingHere is Blueprint blueprint && CeilingValidate(blueprint.def.entityDefToBuild as ThingDef)) return new AcceptanceReport("WorkPlacer_OverBlueprint".Translate());
				}
			}
			return true;
		}

		static bool CeilingValidate(ThingDef def)
		{
			return (def != null && (def.altitudeLayer == AltitudeLayer.MoteOverhead || def.fillPercent == 1f || def.holdsRoof));
		}
	}
}