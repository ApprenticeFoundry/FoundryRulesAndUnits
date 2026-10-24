using System;
using System.Collections.Generic;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	[UnitType(UnitFamilyName.Percent, Description = "Percentage measurement")]
	public class Percent : MeasuredValue
	{
		// UnitFamily comes from UnitTypeAttribute - no need for redundant property override
		#region Constructors and Factory Methods

		// UnitGroup injection constructor (preferred for new code)
		public Percent(UnitGroup unitGroup) : base(unitGroup)
		{
			if (unitGroup.Family != UnitFamilyName.Percent)
				throw new ArgumentException($"Expected UnitGroup for Percent, got {unitGroup.Family}");
		}




		#endregion

		#region Unit Conversion
		// As() method inherited from MeasuredValue with UnitGroup conversion

		#endregion

		#region Operators

		public static Percent operator +(Percent left, Percent right)
		{
			var leftValue = left.As(left.Internal());
			var rightValue = right.As(left.Internal());
			var result = new Percent(left.UnitGroup);
			result.Init(leftValue + rightValue, left.Internal());
			return result;
		}

		public static Percent operator -(Percent left, Percent right)
		{
			var leftValue = left.As(left.Internal());
			var rightValue = right.As(left.Internal());
			var result = new Percent(left.UnitGroup);
			result.Init(leftValue - rightValue, left.Internal());
			return result;
		}

		public static Percent operator *(Percent left, double scalar)
		{
			var result = new Percent(left.UnitGroup);
			result.Init(left.Value() * scalar, left.Internal());
			return result;
		}

		public static Percent operator *(double scalar, Percent right)
		{
			var result = new Percent(right.UnitGroup);
			result.Init(scalar * right.Value(), right.Internal());
			return result;
		}

		public static Percent operator /(Percent left, double scalar)
		{
			var result = new Percent(left.UnitGroup);
			result.Init(left.Value() / scalar, left.Internal());
			return result;
		}

		// A percent beside a plain number is that number's FRACTION: 1 - 2 % is 0.98, and a loss of 2 % is the same loss whether it was
		// declared as 2 % or as 0.02. The result is a plain number (a fraction), not a Percent. Added 2026-10-21 (FoundrySolar: loss chains).
		private double Fraction => As("%") / 100.0;

		public static double operator +(double left, Percent right) => left + right.Fraction;
		public static double operator +(Percent left, double right) => left.Fraction + right;
		public static double operator -(double left, Percent right) => left - right.Fraction;
		public static double operator -(Percent left, double right) => left.Fraction - right;

		public static bool operator >(Percent left, Percent right) => left.As(left.Internal()) > right.As(left.Internal());
		public static bool operator <(Percent left, Percent right) => left.As(left.Internal()) < right.As(left.Internal());
		public static bool operator >=(Percent left, Percent right) => left.As(left.Internal()) >= right.As(left.Internal());
		public static bool operator <=(Percent left, Percent right) => left.As(left.Internal()) <= right.As(left.Internal());

		#endregion


	}


}
