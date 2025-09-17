using System;
using System.Collections.Generic;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	public class Dimensionless : MeasuredValue
	{

		public static Func<UnitCategory> Category = () =>
		{
			return new UnitCategory("dimensionless")
				.Units("count", "Count")
				.Units("ratio", "Ratio")
				.Units("factor", "Factor")
				.Units("scalar", "Scalar")
				.Units("1", "Unity")
				.Units("each", "Each")
				.Units("pcs", "Pieces")
				.Units("units", "Units")
				// All these are equivalent - no conversions needed
				.Conversion(1.0, "dimensionless", 1.0, "count")
				.Conversion(1.0, "dimensionless", 1.0, "ratio")
				.Conversion(1.0, "dimensionless", 1.0, "factor")
				.Conversion(1.0, "dimensionless", 1.0, "scalar")
				.Conversion(1.0, "dimensionless", 1.0, "1")
				.Conversion(1.0, "dimensionless", 1.0, "each")
				.Conversion(1.0, "dimensionless", 1.0, "pcs")
				.Conversion(1.0, "dimensionless", 1.0, "units");
		};

		public Dimensionless() :
			base(UnitFamilyName.None)
		{
		}

		public Dimensionless(double value, string? units = null) :
			base(UnitFamilyName.None)
		{
			Init(Category(), value, units ?? "dimensionless");
		}

		public override double As(string units)
		{
			return ConvertAs(Category(), units);
		}

		public static Dimensionless operator +(Dimensionless left, Dimensionless right) => new(left.Value() + right.Value(), left.Internal());
		public static Dimensionless operator -(Dimensionless left, Dimensionless right) => new(left.Value() - right.Value(), left.Internal());
		public static Dimensionless operator *(Dimensionless left, Dimensionless right) => new(left.Value() * right.Value(), left.Internal());
		public static Dimensionless operator /(Dimensionless left, Dimensionless right) => new(left.Value() / right.Value(), left.Internal());
	}
}