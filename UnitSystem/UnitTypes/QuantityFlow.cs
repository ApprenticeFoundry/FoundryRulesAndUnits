using System;
using System.Collections.Generic;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	[UnitType(UnitFamilyName.QuantityFlow, Description = "Quantity flow rate measurement")]
	public class QuantityFlow : MeasuredValue
	{
		// UnitFamily comes from UnitTypeAttribute - no need for redundant property override
		#region Constructors and Factory Methods

		// UnitGroup injection constructor (preferred for new code)
		public QuantityFlow(UnitGroup unitGroup) : base(unitGroup)
		{
			if (unitGroup.Family != UnitFamilyName.QuantityFlow)
				throw new ArgumentException($"Expected UnitGroup for QuantityFlow, got {unitGroup.Family}");
		}

		#endregion

		#region Unit Conversion
		// As() method inherited from MeasuredValue with UnitGroup conversion

		#endregion

		#region Operators

		public static QuantityFlow operator +(QuantityFlow left, QuantityFlow right)
		{
			var leftValue = left.As(left.Internal());
			var rightValue = right.As(left.Internal());
			var result = new QuantityFlow(left.UnitGroup);
			result.Init(leftValue + rightValue, left.Internal());
			return result;
		}

		public static QuantityFlow operator -(QuantityFlow left, QuantityFlow right)
		{
			var leftValue = left.As(left.Internal());
			var rightValue = right.As(left.Internal());
			var result = new QuantityFlow(left.UnitGroup);
			result.Init(leftValue - rightValue, left.Internal());
			return result;
		}

		public static QuantityFlow operator *(QuantityFlow left, double scalar)
		{
			var result = new QuantityFlow(left.UnitGroup);
			result.Init(left.Value() * scalar, left.Internal());
			return result;
		}

		public static QuantityFlow operator *(double scalar, QuantityFlow right)
		{
			var result = new QuantityFlow(right.UnitGroup);
			result.Init(scalar * right.Value(), right.Internal());
			return result;
		}

		public static QuantityFlow operator /(QuantityFlow left, double scalar)
		{
			var result = new QuantityFlow(left.UnitGroup);
			result.Init(left.Value() / scalar, left.Internal());
			return result;
		}

		// Special cross-unit operations - need proper Quantity constructor
		public static Quantity operator *(QuantityFlow left, Time right)
		{
			// Create using appropriate UnitGroup for Quantity family
			var quantityGroup = left.UnitGroup; // Will need to adjust this
			var result = new Quantity(quantityGroup);
			result.Init(left.Value() * right.Value(), "ea");
			return result;
		}

		public static bool operator >(QuantityFlow left, QuantityFlow right) => left.As(left.Internal()) > right.As(left.Internal());
		public static bool operator <(QuantityFlow left, QuantityFlow right) => left.As(left.Internal()) < right.As(left.Internal());
		public static bool operator >=(QuantityFlow left, QuantityFlow right) => left.As(left.Internal()) >= right.As(left.Internal());
		public static bool operator <=(QuantityFlow left, QuantityFlow right) => left.As(left.Internal()) <= right.As(left.Internal());

		#endregion


	}


}
