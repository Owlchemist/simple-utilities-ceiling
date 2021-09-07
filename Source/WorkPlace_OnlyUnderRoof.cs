using Verse;
using RimWorld;
using System.Collections.Generic;

namespace CeilingUtilities
{
	public class PlaceWorker_OnlyUnderRoof : PlaceWorker
	{
		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			List<IntVec3> cellsToCheck = new List<IntVec3>();
			cellsToCheck.Add(loc);

			//Just hardcoded for the long lights right now. This should be improved to be smarter, when time allows. Maybe use GenAdj.CellsOccupiedBy()?
			if ( checkingDef.Size.x == 2)
			{
				IntVec3 loc2 = loc + IntVec3.East.RotatedBy(rot);
				cellsToCheck.Add(loc2);
			}

			foreach (IntVec3 cell in cellsToCheck)
			{
				//Checking for long lights
				Building building = map.thingGrid.ThingAt<Building>(cell);
				if (building != null && (building.def.altitudeLayer == AltitudeLayer.MoteOverhead || building.def.fillPercent == 1f)) return new AcceptanceReport("Both tiles must be clear.");

				//Check if blueprints are in the way
				Blueprint blueprint = map.thingGrid.ThingAt<Blueprint>(cell);
				if (blueprint != null) return new AcceptanceReport("Cannot be placed over this type of blueprint.");

				//Check for roof
				if (map.roofGrid.Roofed(cell) == false) return new AcceptanceReport("Must be placed under a roof.");
			}
			return true;
		}

		public PlaceWorker_OnlyUnderRoof()
		{
		} 
	}
}