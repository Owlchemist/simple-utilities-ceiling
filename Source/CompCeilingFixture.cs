using Verse;

namespace CeilingUtilities
{
	public class CompCeilingFixture : ThingComp
	{
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.parent?.Map.GetComponent<MapCompont_CeilingUtilities>()?.CheckVisibility(this.parent, this.parent.Map);
		}
	}
}