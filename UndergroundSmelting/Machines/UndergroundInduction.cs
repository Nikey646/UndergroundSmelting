using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using UnityEngine;

namespace UndergroundSmelting.Machines
{
	public class UndergroundInduction : ForcedInduction
	{
		private UInt16 mnLFUpdates = 0;

		public UndergroundInduction(Segment segment, Int64 x, Int64 y, Int64 z, UInt16 cube, Byte flags, UInt16 lValue)
			: base(segment, x, y, z, cube, flags, lValue)
		{ }

		public static UndergroundInduction CreateInduction(ModCreateSegmentEntityParameters param)
		{
			var induction = new UndergroundInduction(param.Segment, param.X, param.Y, param.Z, param.Cube, param.Flags, param.Value);

#if DEBUG
			Debug.LogError($"Creating Underground Induction");
#endif
			
			return induction;
		}

		public override void LowFrequencyUpdate()
		{
			if (this.mbAttachedToSmelter)
				return;

			this.mnLFUpdates++;

			if (this.mnLFUpdates >= 5)
			{
				if (WorldScript.mbIsServer)
				{
					if (this.mValue == 5 && !DLCOwnership.HasT4() && !DLCOwnership.HasPatreon())
					{
						FloatingCombatTextManager.instance.QueueText(this.mnX, this.mnY, this.mnZ, 1.5f, "Needs Frozen Factory Expansion Pack!", Color.cyan, 1.5f, 64f);
					}
					WorldScript.instance.BuildFromEntity(this.mSegment, this.mnX, this.mnY, this.mnZ, 1, 0);
					DroppedItemData droppedItemData = ItemManager.DropNewCubeStack(515, this.mValue, 1, this.mnX, this.mnY, this.mnZ, Vector3.zero);
					if (droppedItemData != null)
					{
						droppedItemData.mrLifeRemaining *= 10f;
					}
				}
				return;
			}

			var segment = base.AttemptGetSegment(this.mnX, this.mnY - 1L, this.mnZ);

			if (segment == null)
			{
				this.mnLFUpdates = 0;
				return;
			}

			var smelter = segment.SearchEntity(this.mnX, this.mnY - 1L, this.mnZ) as OreSmelterInterface;

			if (smelter == null)
				return;

			if (!smelter.SupportsForcedInduction())
				return;

			if (this.mValue == 0)
				this.SetSmelterDetails(smelter, 25, 2, 8, 192, 1);

			if (this.mValue == 1)
				this.SetSmelterDetails(smelter, 25, 3, 16, 320, 1);

			if (this.mValue == 2)
				this.SetSmelterDetails(smelter, 25, 4, 32, 512, 1);

			if (this.mValue == 3)
				this.SetSmelterDetails(smelter, 25, 4, 32, 512, 2);

			if (this.mValue == 4)
				this.SetSmelterDetails(smelter, 25, 4, 32, 512, 4);

			if (this.mValue == 5)
				this.SetSmelterDetails(smelter, 5, 1, 512, 4096, 1, true);
				
			if (this.mValue > 6)
				Debug.LogError("Error, Forced Induction built with Value of " + this.mValue);

			this.AttachedSmelter = smelter;
			this.mbAttachedToSmelter = true;
			Achievements.UnlockAchievementDelayed(Achievements.eAchievements.eForce); // This will never run because mods yo!
		}

		private void SetSmelterDetails(OreSmelterInterface smelter, Single tempatureGainRate, Single burnRate,
			Single powerRate, Single maxPower, Int32 collectionRate, Boolean tier4 = false)
		{
			if (tier4 && (!DLCOwnership.HasT4() && !DLCOwnership.HasPatreon()))
				return; // Let the Forced Induction be destroyed

			smelter.SetSmelterTemperatureGainRate(tempatureGainRate);
			smelter.SetSmelterBurnRate(burnRate);
			smelter.SetSmelterPowerRate(powerRate);
			smelter.SetSmelterMaxPower(maxPower);
			smelter.SetSmelterCollectionRate(collectionRate);
			smelter.SetSmelterSupportsTier4(tier4);
		}


//		public override String GetPopupText()
//		{
//			return $"Is Too Deep: {this.mbTooDeep}";
//		}
	}
}
