using Verse;
using RimWorld;
using System.Collections.Generic;
using System.Linq;

namespace CeilingUtilities
{
	public class PlaceWorker_OnlyUnderRoof : PlaceWorker
	{
		public override AcceptanceReport AllowsPlacing(BuildableDef def, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			var cellsToCheck = GenAdj.OccupiedRect(loc, rot, def.Size).ToList();

			foreach (IntVec3 cell in cellsToCheck)
			{
				Building building = map.thingGrid.ThingAt<Building>(cell);
				if (building != null && Filter(building.def)) return new AcceptanceReport("Cannot place over these types of things.");

				//Check if blueprints are in the way
				Blueprint blueprint = map.thingGrid.ThingAt<Blueprint>(cell);
				if (blueprint != null && Filter(blueprint.def.entityDefToBuild as ThingDef)) return new AcceptanceReport("Cannot be placed over this type of blueprint.");

				//Check for roof
				if (!map.roofGrid.Roofed(cell)) return new AcceptanceReport("Must be placed under a roof.");
			}
			return true;
		}

		private static bool Filter(ThingDef def)
		{
			if (def.altitudeLayer == AltitudeLayer.MoteOverhead || def.fillPercent == 1f || def.holdsRoof) return true;
			return false;
		}

		public PlaceWorker_OnlyUnderRoof()
		{
		} 
	}
}