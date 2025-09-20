using System;
using System.Collections.Generic;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	public class Quantity : MeasuredValue
	{
		#region Constructors and Factory Methods

		// UnitGroup injection constructor (preferred for new code)
		public Quantity(UnitGroup unitGroup) : base(unitGroup) 
		{ 
			if (unitGroup.Family != UnitFamilyName.Quantity)
				throw new ArgumentException($"Expected UnitGroup for Quantity, got {unitGroup.Family}");
		}

		public Quantity(UnitGroup unitGroup, double value, string? units = null) : base(unitGroup)
		{
			if (unitGroup.Family != UnitFamilyName.Quantity)
				throw new ArgumentException($"Expected UnitGroup for Quantity, got {unitGroup.Family}");
			Init(value, units);
		}

		/// <summary>


		/// Backward compatibility constructor for JSON deserialization


		/// </summary>


		public Quantity(double value, string units) : base()


		{


			V = value;


			I = units;


			U = units;


		}

		#endregion

		// Factory methods
		public static Quantity FromEach(double value) => new(value, "ea");
		public static Quantity FromItems(double value) => new(value, "items");
		public static Quantity FromPieces(double value) => new(value, "pcs");
		public static Quantity FromUnits(double value) => new(value, "units");


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
