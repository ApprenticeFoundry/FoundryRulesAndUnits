using FoundryRulesAndUnits.Extensions;
using System;
using System.Collections.Generic;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	[UnitType(UnitFamilyName.Heading, Description = "Heading/bearing measurement")]
	public class Heading : MeasuredValue
	{
		// UnitFamily comes from UnitTypeAttribute - no need for redundant property override
		#region Constructors and Factory Methods

		// UnitGroup injection constructor (preferred for new code)
		public Heading(UnitGroup unitGroup) : base(unitGroup)
		{
			if (unitGroup.Family != UnitFamilyName.Heading)
				throw new ArgumentException($"Expected UnitGroup for Heading, got {unitGroup.Family}");
		}


		#endregion

		#region Unit Conversion
		// As() method inherited from MeasuredValue with UnitGroup conversion

		#endregion

		#region Legacy Methods (Maintained for Compatibility)

		public Heading Assign(double value, string? units)
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

		public Heading Assign(Heading source)
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

		public Heading Copy()
		{
			var result = new Heading(UnitGroup);
			result.Init(Value(), Internal());
			return result;
		}

		public Heading Degrees(double value)
		{
			V = _unitGroup.Convert(value, "deg", Internal());
			return this;
		}

		#endregion

		#region Operators

		public static Heading operator +(Heading left, Heading right)
		{
			var leftValue = left.As(left.Internal());
			var rightValue = right.As(left.Internal());
			var result = new Heading(left.UnitGroup);
			result.Init(leftValue + rightValue, left.Internal());
			return result;
		}

		public static Heading operator -(Heading left, Heading right)
		{
			var leftValue = left.As(left.Internal());
			var rightValue = right.As(left.Internal());
			var result = new Heading(left.UnitGroup);
			result.Init(leftValue - rightValue, left.Internal());
			return result;
		}

		public static Heading operator *(Heading left, double scalar)
		{
			var result = new Heading(left.UnitGroup);
			result.Init(left.Value() * scalar, left.Internal());
			return result;
		}

		public static Heading operator *(double scalar, Heading right)
		{
			var result = new Heading(right.UnitGroup);
			result.Init(scalar * right.Value(), right.Internal());
			return result;
		}

		public static Heading operator /(Heading left, double scalar)
		{
			var result = new Heading(left.UnitGroup);
			result.Init(left.Value() / scalar, left.Internal());
			return result;
		}

		public static bool operator >(Heading left, Heading right) => left.As(left.Internal()) > right.As(left.Internal());
		public static bool operator <(Heading left, Heading right) => left.As(left.Internal()) < right.As(left.Internal());
		public static bool operator >=(Heading left, Heading right) => left.As(left.Internal()) >= right.As(left.Internal());
		public static bool operator <=(Heading left, Heading right) => left.As(left.Internal()) <= right.As(left.Internal());

		#endregion


	}


}
