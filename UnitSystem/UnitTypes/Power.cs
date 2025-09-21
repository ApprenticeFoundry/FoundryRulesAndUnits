using System;
using System.Collections.Generic;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	[UnitType(UnitFamilyName.Power, Description = "Power measurement")]
	public class Power : MeasuredValue
	{
		// UnitFamily comes from UnitTypeAttribute - no need for redundant property override
		#region Constructors and Factory Methods

		/// <summary>
		/// Constructor with UnitGroup injection - preferred for new code
		/// </summary>
		public Power(UnitGroup unitGroup) : base(unitGroup)
		{
			if (unitGroup.Family != UnitFamilyName.Power)
				throw new ArgumentException($"Expected UnitGroup for Power, got {unitGroup.Family}");
		}

		#endregion

		#region Operators

		public static Power operator +(Power left, Power right)
		{
			var result = new Power(left.UnitGroup);
			result.Init(left.Value() + right.Value(), left.Internal());
			return result;
		}

		public static Power operator -(Power left, Power right)
		{
			var result = new Power(left.UnitGroup);
			result.Init(left.Value() - right.Value(), left.Internal());
			return result;
		}

		public static Power operator *(Power left, double scalar)
		{
			var result = new Power(left.UnitGroup);
			result.Init(left.Value() * scalar, left.Internal());
			return result;
		}

		public static Power operator *(double scalar, Power right)
		{
			var result = new Power(right.UnitGroup);
			result.Init(scalar * right.Value(), right.Internal());
			return result;
		}

		public static Power operator /(Power left, double scalar)
		{
			var result = new Power(left.UnitGroup);
			result.Init(left.Value() / scalar, left.Internal());
			return result;
		}

		public static double operator /(Power left, Power right) => left.Value() / right.Value();

		#endregion
	}


}
