using FoundryRulesAndUnits.Extensions;
using System;
using System.Collections.Generic;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	[UnitType(UnitFamilyName.Bearing, Description = "Bearing/bearing measurement")]
	public class Bearing : MeasuredValue
	{
		// UnitFamily comes from UnitTypeAttribute - no need for redundant property override
		#region Constructors and Factory Methods

		// UnitGroup injection constructor (preferred for new code)
		public Bearing(UnitGroup unitGroup) : base(unitGroup)
		{
			if (unitGroup.Family != UnitFamilyName.Bearing)
				throw new ArgumentException($"Expected UnitGroup for Bearing, got {unitGroup.Family}");
		}


		#endregion

		#region Unit Conversion
		// As() method inherited from MeasuredValue with UnitGroup conversion

		#endregion

		#region Legacy Methods (Maintained for Compatibility)

		public Bearing Assign(double value, string? units)
		{
			if (units == I)
			{
				V = value;
			}
			else
			{
				Init(value, units);
			}
			return this;
		}

		public Bearing Assign(Bearing source)
		{
			if (source.I == I)
			{
				V = source.Value();
			}
			else
			{
				Init(source.Value(), source.U);
			}
			return this;
		}

		public Bearing Copy()
		{
			var result = new Bearing(UnitGroup);
			result.Init(Value(), Internal());
			return result;
		}

		public Bearing Degrees(double value)
		{
			V = _unitGroup.Convert(value, "deg", Internal());
			return this;
		}

		#endregion

		#region Operators

		public static Bearing operator +(Bearing left, Bearing right)
		{
			var leftValue = left.As(left.Internal());
			var rightValue = right.As(left.Internal());
			var result = new Bearing(left.UnitGroup);
			result.Init(leftValue + rightValue, left.Internal());
			return result;
		}

		public static Bearing operator -(Bearing left, Bearing right)
		{
			var leftValue = left.As(left.Internal());
			var rightValue = right.As(left.Internal());
			var result = new Bearing(left.UnitGroup);
			result.Init(leftValue - rightValue, left.Internal());
			return result;
		}

		public static Bearing operator *(Bearing left, double scalar)
		{
			var result = new Bearing(left.UnitGroup);
			result.Init(left.Value() * scalar, left.Internal());
			return result;
		}

		public static Bearing operator *(double scalar, Bearing right)
		{
			var result = new Bearing(right.UnitGroup);
			result.Init(scalar * right.Value(), right.Internal());
			return result;
		}

		public static Bearing operator /(Bearing left, double scalar)
		{
			var result = new Bearing(left.UnitGroup);
			result.Init(left.Value() / scalar, left.Internal());
			return result;
		}

		public static bool operator >(Bearing left, Bearing right) => left.As(left.Internal()) > right.As(left.Internal());
		public static bool operator <(Bearing left, Bearing right) => left.As(left.Internal()) < right.As(left.Internal());
		public static bool operator >=(Bearing left, Bearing right) => left.As(left.Internal()) >= right.As(left.Internal());
		public static bool operator <=(Bearing left, Bearing right) => left.As(left.Internal()) <= right.As(left.Internal());

		#endregion


	}


}
