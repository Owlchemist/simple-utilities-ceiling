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
			foreach (IntVec3 cell in GenAdj.OccupiedRect(loc, rot, def.Size))
			{
				//Check for roof
				if (!map.roofGrid.Roofed(cell)) return new AcceptanceReport("Must be placed under a roof.");

				//Check what things are in this cell
				foreach (Thing thingHere in map.thingGrid.ThingsListAtFast(cell))
				{
					//Valid building?
					Building building = thingHere as Building;
					if (building != null && (CeilingValidate(building.def))) return new AcceptanceReport("Cannot place over these types of things.");

					//Or valid blueprint?
					Blueprint blueprint = thingHere as Blueprint;
					if (blueprint != null && CeilingValidate(blueprint.def.entityDefToBuild as ThingDef)) return new AcceptanceReport("Cannot be placed over this type of blueprint.");
				}
			}
			return true;
		}

		static bool CeilingValidate(ThingDef def)
		{
			if (def != null && (def.altitudeLayer == AltitudeLayer.MoteOverhead || def.fillPercent == 1f || def.holdsRoof)) return true;
			return false;
		}
	}
}