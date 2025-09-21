using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]

	public class Dimensionless : MeasuredValue
	{
		override public UnitFamilyName UnitFamily => UnitFamilyName.None;
		#region Constructors and Factory Methods

		// UnitGroup injection constructor (preferred for new code)
		public Dimensionless(UnitGroup unitGroup) : base(unitGroup)
		{
			if (unitGroup.Family != UnitFamilyName.None)
				throw new ArgumentException($"Expected UnitGroup for None (Dimensionless), got {unitGroup.Family}");
		}


		#endregion

		#region Unit Conversion

		// As() method inherited from MeasuredValue with UnitGroup conversion

		#endregion

		#region Operators

		public static Dimensionless operator +(Dimensionless left, Dimensionless right)
		{
			var leftValue = left.As(left.Internal());
			var rightValue = right.As(left.Internal());
			return new Dimensionless(leftValue + rightValue, left.Internal());
		}

		public static Dimensionless operator -(Dimensionless left, Dimensionless right)
		{
			var leftValue = left.As(left.Internal());
			var rightValue = right.As(left.Internal());
			return new Dimensionless(leftValue - rightValue, left.Internal());
		}

		public static Dimensionless operator *(Dimensionless left, Dimensionless right)
		{
			return new Dimensionless(left.Value() * right.Value(), left.Internal());
		}

		public static Dimensionless operator /(Dimensionless left, Dimensionless right)
		{
			return new Dimensionless(left.Value() / right.Value(), left.Internal());
		}

		public static Dimensionless operator *(Dimensionless left, double scalar) => new(left.Value() * scalar, left.Internal());
		public static Dimensionless operator *(double scalar, Dimensionless right) => new(scalar * right.Value(), right.Internal());
		public static Dimensionless operator /(Dimensionless left, double scalar) => new(left.Value() / scalar, left.Internal());

		public static bool operator >(Dimensionless left, Dimensionless right) => left.As(left.Internal()) > right.As(left.Internal());
		public static bool operator <(Dimensionless left, Dimensionless right) => left.As(left.Internal()) < right.As(left.Internal());
		public static bool operator >=(Dimensionless left, Dimensionless right) => left.As(left.Internal()) >= right.As(left.Internal());
		public static bool operator <=(Dimensionless left, Dimensionless right) => left.As(left.Internal()) <= right.As(left.Internal());

		#endregion


	}


}