using System;
using System.Collections.Generic;

namespace FoundryRulesAndUnits.Units
{

	[System.Serializable]
	public class Area : MeasuredValue
	{


		public static Func<UnitCategory> Category = () =>
		{
			return new UnitCategory("Area");
		};

		public Area() :
			base(UnitFamilyName.Area)
		{
		}

		public Area(double value, string? units) :
			base(UnitFamilyName.Area)
		{
			Init(Category(), value, units);
		}

		public override double As(string units)
		{
			return ConvertAs(Category(), units);
		}

		public static Area operator +(Area left, Area right) => new(left.Value() + right.Value(), left.Internal());
		public static Area operator -(Area left, Area right) => new(left.Value() - right.Value(), left.Internal());
	}
}