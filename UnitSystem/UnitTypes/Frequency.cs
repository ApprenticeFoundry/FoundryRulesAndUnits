using System;
using System.Collections.Generic;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	public class Frequency : MeasuredValue
	{

		public static Func<UnitCategory> Category = () =>
		{
			return new UnitCategory("Frequency");
		};

		public Frequency() :
			base(UnitFamilyName.Frequency)
		{
		}

		public Frequency(double value, string? units = null) :
			base(UnitFamilyName.Frequency)
		{
			Init(Category(), value, units);
		}

		public override double As(string units)
		{
			return ConvertAs(Category(), units);
		}

		public static Frequency operator +(Frequency left, Frequency right) => new(left.Value() + right.Value(), left.Internal());
		public static Frequency operator -(Frequency left, Frequency right) => new(left.Value() - right.Value(), left.Internal());
	}
}
