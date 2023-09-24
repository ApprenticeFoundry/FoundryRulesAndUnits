using System;
using System.Collections.Generic;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	public class DataStorage : MeasuredValue
	{

		public static Func<UnitCategory> Category = () =>
		{
			return new UnitCategory("DataStorage");
		};

		public DataStorage() :
			base(UnitFamilyName.DataStorage)
		{
		}

		public DataStorage(double value, string? units = null) :
			base(UnitFamilyName.DataStorage)
		{
			Init(Category(), value, units);
		}

		public override double As(string units)
		{
			return ConvertAs(Category(), units);
		}

		public static DataStorage operator +(DataStorage left, DataStorage right) => new(left.Value() + right.Value(), left.Internal());
		public static DataStorage operator -(DataStorage left, DataStorage right) => new(left.Value() - right.Value(), left.Internal());
	}
}
