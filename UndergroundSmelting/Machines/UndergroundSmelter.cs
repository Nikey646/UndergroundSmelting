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
		private static UndergroundSmelter _smelter;

		private UndergroundSmelter(Segment segment, Int64 x, Int64 y, Int64 z, UInt16 cube, Byte flags, UInt16 lValue)
			: base(segment, x, y, z, cube, flags, lValue)
		{ }

		internal static UndergroundSmelter CreateSmelter(ModCreateSegmentEntityParameters param)
		{
			_smelter = new UndergroundSmelter(param.Segment, param.X, param.Y, param.Z, param.Cube, param.Flags, param.Value)
			{
				mbTooDeep = false,
				mrSmeltTime = 99.9f
			};

			if (DifficultySettings.mbCasualResource)
				_smelter.mrSmeltTime = 10f;
			if (DifficultySettings.mbRushMode)
				_smelter.mrSmeltTime = 3f;
			if (_smelter.mValue == 1)
			{
				_smelter.mrSmeltTime /= 2f;
				_smelter.mrPowerRate *= 2f;
				_smelter.mnOrePerBar *= 4;
			}

			return _smelter;
		}

//		public override String GetPopupText()
//		{
//			return $"Is Too Deep: {_smelter.mbTooDeep}" +
//				$"Smelt Time: {_smelter.mrSmeltTime}";
//		}
	}
}
