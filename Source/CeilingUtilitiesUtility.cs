using System.Linq;
using Verse;
using System.Collections.Generic;

namespace CeilingUtilities
{
	//Keeps a cached list of which defs are ceiling mounted, for use of the visibiltiy toggle
    [StaticConstructorOnStartup]
	public static class CeilingUtilitiesUtility
    {
		public static ThingDef[] ceilingFixtures;
		public static Dictionary<int, CompFireOverlayMulti> fireCache = new Dictionary<int, CompFireOverlayMulti>();
		public static bool updateNow;
        static CeilingUtilitiesUtility()
        {
            ceilingFixtures = DefDatabase<ThingDef>.AllDefsListForReading.Where(x => x.HasModExtension<CeilingFixture>()).ToArray();
        }
    }
}