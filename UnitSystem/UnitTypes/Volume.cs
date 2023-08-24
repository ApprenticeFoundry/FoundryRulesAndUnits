using System;
using System.Collections.Generic;

namespace FoundryRulesAndUnits.Units
{

	[System.Serializable]
	public class Volume : MeasuredValue
	{
		public static Func<UnitCategory> Category = () =>
		{
			return new UnitCategory("Volume");
		};

		public Volume() :
			base(UnitFamilyName.Volume)
		{
		}

		public Volume(double value, string? units) :
			base(UnitFamilyName.Volume)
		{
			Init(Category(), value, units);
		}

		public override double As(string units)
		{
			return ConvertAs(Category(), units);
		}


		public static Volume operator +(Volume left, Volume right) => new(left.Value() + right.Value(), left.Internal());
		public static Volume operator -(Volume left, Volume right) => new(left.Value() - right.Value(), left.Internal());
	}
}
