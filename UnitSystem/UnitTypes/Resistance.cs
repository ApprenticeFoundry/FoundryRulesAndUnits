using System;
using System.Collections.Generic;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	public class Resistance : MeasuredValue
	{
		override public UnitFamilyName UnitFamily => UnitFamilyName.Resistance;
		#region Constructors and Factory Methods

		// UnitGroup injection constructor (preferred for new code)
		public Resistance(UnitGroup unitGroup) : base(unitGroup)
		{
			if (unitGroup.Family != UnitFamilyName.Resistance)
				throw new ArgumentException($"Expected UnitGroup for Resistance, got {unitGroup.Family}");
		}


		#endregion

		#region Unit Conversion
		// As() method inherited from MeasuredValue with UnitGroup conversion

		#endregion

		#region Operators

		public static Resistance operator +(Resistance left, Resistance right)
		{
			var leftValue = left.As(left.Internal());
			var rightValue = right.As(left.Internal());
			return new Resistance(leftValue + rightValue, left.Internal());
		}

		public static Resistance operator -(Resistance left, Resistance right)
		{
			var leftValue = left.As(left.Internal());
			var rightValue = right.As(left.Internal());
			return new Resistance(leftValue - rightValue, left.Internal());
		}

		public static Resistance operator *(Resistance left, double scalar) => new(left.Value() * scalar, left.Internal());
		public static Resistance operator *(double scalar, Resistance right) => new(scalar * right.Value(), right.Internal());
		public static Resistance operator /(Resistance left, double scalar) => new(left.Value() / scalar, left.Internal());

		public static bool operator >(Resistance left, Resistance right) => left.As(left.Internal()) > right.As(left.Internal());
		public static bool operator <(Resistance left, Resistance right) => left.As(left.Internal()) < right.As(left.Internal());
		public static bool operator >=(Resistance left, Resistance right) => left.As(left.Internal()) >= right.As(left.Internal());
		public static bool operator <=(Resistance left, Resistance right) => left.As(left.Internal()) <= right.As(left.Internal());

		#endregion


	}


}
