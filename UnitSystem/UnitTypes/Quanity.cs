using System;
using System.Collections.Generic;


namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	public class Quantity : MeasuredValue
	{
		public static Func<UnitCategory> Category = () =>
		{
			return new UnitCategory("Quantity");
		};

		public Quantity() :
			base(UnitFamilyName.Quantity)
		{
		}

		public Quantity(double value, string? units = null) :
			base(UnitFamilyName.Quantity)
		{
			Init(Category(), value, units);
		}



		public override double As(string units)
		{
			return ConvertAs(Category(), units);
		}


		public static Quantity operator +(Quantity left, int right) => new(left.Value() + right, left.Internal());
		public static Quantity operator -(Quantity left, int right) => new(left.Value() - right, left.Internal());

		public static Quantity operator +(Quantity left, Quantity right) => new(left.Value() + right.Value(), left.Internal());
		public static Quantity operator -(Quantity left, Quantity right) => new(left.Value() - right.Value(), left.Internal());

		public static QuantityFlow operator /(Quantity left, Time right) => new(left.Value() / right.Value(), "ea/s");
	}
}
