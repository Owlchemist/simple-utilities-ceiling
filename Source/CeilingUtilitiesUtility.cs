using Verse;
using System.Collections.Generic;
using Settings = CeilingUtilities.ModSettings_CeilingUtilities;

namespace CeilingUtilities
{
	//Keeps a cached list of which defs are ceiling mounted, for use of the visibility toggle
    [StaticConstructorOnStartup]
	public static class CeilingUtilitiesUtility
    {
		public static HashSet<ushort> ceilingFixtureHashes = new HashSet<ushort>();
        public static ThingDef[] ceilingFixtures;
		public static Dictionary<int, CompFireOverlayMulti> fireCache = new Dictionary<int, CompFireOverlayMulti>();
        public static Dictionary<ushort, DrawerType> drawerTypeLedger = new Dictionary<ushort, DrawerType>();
        static CeilingUtilitiesUtility()
        {
            var workingList = new List<ThingDef>();
            var list = DefDatabase<ThingDef>.AllDefsListForReading;
            for (int i = list.Count; i-- > 0;)
            {
                var def = list[i];
                if (def.HasModExtension<CeilingFixture>()) 
                {
                    workingList.Add(def);
                    ceilingFixtureHashes.Add(def.shortHash);
                    drawerTypeLedger.Add(def.shortHash, def.drawerType);

                    if (!Settings.drawFixtures) def.drawerType = DrawerType.None;
                }
            }
            ceilingFixtures = workingList.ToArray();
        }
    }
}