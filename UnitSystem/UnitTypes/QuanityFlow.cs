using System;
using System.Collections.Generic;


namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	public class QuantityFlow : MeasuredValue
	{
		public static Func<UnitCategory> Category = () =>
		{
			return new UnitCategory("QuanityFlow");
		};

		public QuantityFlow() :
			base(UnitFamilyName.QuantityFlow)
		{
		}

		public QuantityFlow(double value, string? units = null) :
			base(UnitFamilyName.QuantityFlow)
		{
			Init(Category(), value, units);
		}



		public override double As(string units)
		{
			return ConvertAs(Category(), units);
		}


		public static QuantityFlow operator +(QuantityFlow left, QuantityFlow right) => new(left.Value() + right.Value(), left.Internal());
		public static QuantityFlow operator -(QuantityFlow left, QuantityFlow right) => new(left.Value() - right.Value(), left.Internal());

		public static Quantity operator *(QuantityFlow left, Time right) => new(left.Value() * right.Value(), "ea");

	}
}
