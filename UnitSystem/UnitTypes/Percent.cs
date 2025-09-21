using System;
using System.Collections.Generic;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	public class Percent : MeasuredValue
	{
		override public UnitFamilyName UnitFamily => UnitFamilyName.Percent;
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
			return new Percent(leftValue + rightValue, left.Internal());
		}

		public static Percent operator -(Percent left, Percent right)
		{
			var leftValue = left.As(left.Internal());
			var rightValue = right.As(left.Internal());
			return new Percent(leftValue - rightValue, left.Internal());
		}

		public static Percent operator *(Percent left, double scalar) => new(left.Value() * scalar, left.Internal());
		public static Percent operator *(double scalar, Percent right) => new(scalar * right.Value(), right.Internal());
		public static Percent operator /(Percent left, double scalar) => new(left.Value() / scalar, left.Internal());

		public static bool operator >(Percent left, Percent right) => left.As(left.Internal()) > right.As(left.Internal());
		public static bool operator <(Percent left, Percent right) => left.As(left.Internal()) < right.As(left.Internal());
		public static bool operator >=(Percent left, Percent right) => left.As(left.Internal()) >= right.As(left.Internal());
		public static bool operator <=(Percent left, Percent right) => left.As(left.Internal()) <= right.As(left.Internal());

		#endregion


	}


}
