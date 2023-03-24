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
        static CeilingUtilitiesUtility()
        {
            var workingList = new List<ThingDef>();
            var list = DefDatabase<ThingDef>.AllDefsListForReading;
            for (int i = list.Count; i-- > 0;)
            {
                var def = list[i];
                if (def.HasModExtension<CeilingFixture>()) workingList.Add(def);
            }
            ceilingFixtures = workingList.ToArray();
        }
    }
}