using System;
using System.Collections.Generic;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	public class QuantityFlow : MeasuredValue
	{
		override public UnitFamilyName UnitFamily => UnitFamilyName.QuantityFlow;
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
			return new QuantityFlow(leftValue + rightValue, left.Internal());
		}

		public static QuantityFlow operator -(QuantityFlow left, QuantityFlow right)
		{
			var leftValue = left.As(left.Internal());
			var rightValue = right.As(left.Internal());
			return new QuantityFlow(leftValue - rightValue, left.Internal());
		}

		public static QuantityFlow operator *(QuantityFlow left, double scalar) => new(left.Value() * scalar, left.Internal());
		public static QuantityFlow operator *(double scalar, QuantityFlow right) => new(scalar * right.Value(), right.Internal());
		public static QuantityFlow operator /(QuantityFlow left, double scalar) => new(left.Value() / scalar, left.Internal());

		// Special cross-unit operations
		public static Quantity operator *(QuantityFlow left, Time right) => new(left.Value() * right.Value(), "ea");

		public static bool operator >(QuantityFlow left, QuantityFlow right) => left.As(left.Internal()) > right.As(left.Internal());
		public static bool operator <(QuantityFlow left, QuantityFlow right) => left.As(left.Internal()) < right.As(left.Internal());
		public static bool operator >=(QuantityFlow left, QuantityFlow right) => left.As(left.Internal()) >= right.As(left.Internal());
		public static bool operator <=(QuantityFlow left, QuantityFlow right) => left.As(left.Internal()) <= right.As(left.Internal());

		#endregion


	}


}
