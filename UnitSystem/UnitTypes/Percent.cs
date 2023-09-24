using System;
using System.Collections.Generic;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	public class Percent : MeasuredValue
	{

		public static Func<UnitCategory> Category = () =>
		{
			return new UnitCategory("Percent");
		};

		public Percent() :
			base(UnitFamilyName.Percent)
		{
		}

		public Percent(double value, string? units = null) :
			base(UnitFamilyName.Percent)
		{
			Init(Category(), value, units);
		}

		public override double As(string units)
		{
			return ConvertAs(Category(), units);
		}

		public static Percent operator +(Percent left, Percent right) => new(left.Value() + right.Value(), left.Internal());
		public static Percent operator -(Percent left, Percent right) => new(left.Value() - right.Value(), left.Internal());
	}
}
