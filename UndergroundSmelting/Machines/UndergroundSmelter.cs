using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Object = System.Object;

namespace UndergroundSmelting.Machines
{
	public class UndergroundSmelter : OreSmelter
	{
		private UndergroundSmelter(Segment segment, Int64 x, Int64 y, Int64 z, UInt16 cube, Byte flags, UInt16 lValue)
			: base(segment, x, y, z, cube, flags, lValue)
		{ }

		internal static UndergroundSmelter CreateSmelter(ModCreateSegmentEntityParameters param)
		{
			var smelter = new UndergroundSmelter(param.Segment, param.X, param.Y, param.Z, param.Cube, param.Flags, param.Value)
			{
				mbTooDeep = false,
				mrSmeltTime = 99.9f
			};

			if (DifficultySettings.mbCasualResource)
				smelter.mrSmeltTime = 10f;
			if (DifficultySettings.mbRushMode)
				smelter.mrSmeltTime = 3f;
			if (smelter.mValue == 1)
			{
				smelter.mrSmeltTime /= 2f;
				smelter.mrPowerRate *= 2f;
				smelter.mnOrePerBar *= 4;
			}

			return smelter;
		}
	}
}
