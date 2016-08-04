using UndergroundSmelting.Machines;

namespace UndergroundSmelting
{
	public class Bootstrap : FortressCraftMod
	{
		
		public override ModRegistrationData Register()
		{
			var mrd = new ModRegistrationData();
			mrd.RegisterEntityHandler(eSegmentEntity.OreSmelter);
			return mrd;
		}

		public override void CreateSegmentEntity(ModCreateSegmentEntityParameters param, ModCreateSegmentEntityResults results)
		{
			if (param.Type == eSegmentEntity.OreSmelter)
				results.Entity = UndergroundSmelter.CreateSmelter(param);
		}

	}
}
