using System;
using System.Collections.Generic;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	public class DataFlow : MeasuredValue
	{

		public static Func<UnitCategory> Category = () =>
		{
			return new UnitCategory("DataFlow");
		};

		public DataFlow() :
			base(UnitFamilyName.DataFlow)
		{
		}

		public DataFlow(double value, string? units = null) :
			base(UnitFamilyName.DataFlow)
		{
			Init(Category(), value, units);
		}

		public override double As(string units)
		{
			return ConvertAs(Category(), units);
		}

		public static DataFlow operator +(DataFlow left, DataFlow right) => new(left.Value() + right.Value(), left.Internal());
		public static DataFlow operator -(DataFlow left, DataFlow right) => new(left.Value() - right.Value(), left.Internal());
	}
}
