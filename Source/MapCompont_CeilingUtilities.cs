using System.Linq;
using Verse;
using System.Collections.Generic;
using System.Collections;

namespace CeilingUtilities
{
	public class MapCompont_CeilingUtilities : MapComponent
	{

         public MapCompont_CeilingUtilities(Map map) : base(map)
        {
            return;
        }
		public override void FinalizeInit()
        {
            //This doesn't need to run if visibile was left to true
            if (Patch_DoPlaySettingsGlobalControls.drawFixtures == true) return;

            //Find all ceiling fixtures
            this.map.listerThings.AllThings.Where(x => x.def.HasComp(typeof(CompCeilingFixture)) == true)?.ToList().ForEach(fixture => CheckVisibility(fixture, fixture.Map));
        }

        public void CheckVisibility(Thing fixture, Map map)
		{
			//The mesh printer and draw registry will ignore this thing if it's invisible, so we temporarily change it
			fixture.def.drawerType = DrawerType.RealtimeOnly;
            //Now print it
			map.mapDrawer.MapMeshDirty(fixture.Position, MapMeshFlag.Things);
			map.mapDrawer.MapMeshDirty(fixture.Position, MapMeshFlag.Buildings);
			//Also register it for real time drawing
			map.dynamicDrawManager.RegisterDrawable(fixture);
			//Now change it back to what it actually is
			fixture.def.drawerType = Patch_DoPlaySettingsGlobalControls.drawFixtures ? DrawerType.RealtimeOnly : DrawerType.None;
		}
    }
}