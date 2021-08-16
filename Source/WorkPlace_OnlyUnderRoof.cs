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

			if ( checkingDef.Size.x == 2)
			{
				IntVec3 loc2 = loc;
				if (rot.AsInt == 0) loc2.x++;
				else if (rot.AsInt == 1) loc2.z--;
				else if (rot.AsInt == 2) loc2.x--;
				else loc2.z++;
				cellsToCheck.Add(loc2);
			}

			foreach (IntVec3 cell in cellsToCheck)
			{
				Building building = map.thingGrid.ThingAt<Building>(cell);
				if (building != null && (building.def.altitudeLayer == AltitudeLayer.MoteOverhead || building.def.fillPercent == 1f))
				{
					return false;
				}
				Blueprint blueprint = map.thingGrid.ThingAt<Blueprint>(cell);
				if (blueprint != null)
				{
					return false;
				}
				if (map.roofGrid.Roofed(cell) == false) return false;
			}
			return true;
		}

		public PlaceWorker_OnlyUnderRoof()
		{
		} 
	}
}
