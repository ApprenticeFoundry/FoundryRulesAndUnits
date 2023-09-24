using System;
using System.Collections.Generic;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	public class Resistance : MeasuredValue
	{

		public static Func<UnitCategory> Category = () =>
		{
			return new UnitCategory("Resistance");
		};

		public Resistance() :
			base(UnitFamilyName.Resistance)
		{
		}

		public Resistance(double value, string? units = null) :
			base(UnitFamilyName.Resistance)
		{
			Init(Category(), value, units);
		}

		public override double As(string units)
		{
			return ConvertAs(Category(), units);
		}

		public static Resistance operator +(Resistance left, Resistance right) => new(left.Value() + right.Value(), left.Internal());
		public static Resistance operator -(Resistance left, Resistance right) => new(left.Value() - right.Value(), left.Internal());
	}
}
