using System;
using System.Collections.Generic;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	[UnitType(UnitFamilyName.Quantity, Description = "Quantity measurement")]
	public class Quantity : MeasuredValue
	{
		// UnitFamily comes from UnitTypeAttribute - no need for redundant property override
		#region Constructors and Factory Methods

		// UnitGroup injection constructor (preferred for new code)
		public Quantity(UnitGroup unitGroup) : base(unitGroup)
		{
			if (unitGroup.Family != UnitFamilyName.Quantity)
				throw new ArgumentException($"Expected UnitGroup for Quantity, got {unitGroup.Family}");
		}


		#endregion



		// Arithmetic operators with integers
		public static Quantity operator +(Quantity left, int right) => new(left.Value() + right, left.Internal());
		public static Quantity operator -(Quantity left, int right) => new(left.Value() - right, left.Internal());

		// Arithmetic operators with other quantities
		public static Quantity operator +(Quantity left, Quantity right) => new(left.Value() + right.Value(), left.Internal());
		public static Quantity operator -(Quantity left, Quantity right) => new(left.Value() - right.Value(), left.Internal());
		public static Quantity operator *(Quantity quantity, double scalar) => new(quantity.Value() * scalar, quantity.Internal());
		public static Quantity operator *(double scalar, Quantity quantity) => new(scalar * quantity.Value(), quantity.Internal());
		public static Quantity operator /(Quantity quantity, double scalar) => new(quantity.Value() / scalar, quantity.Internal());
		public static double operator /(Quantity left, Quantity right) => left.Value() / right.Value();

		// Special operator for creating flow rates
		public static QuantityFlow operator /(Quantity left, Time right) => new(left.Value() / right.Value(), "ea/s");
	}


}
