using System;
using System.Collections.Generic;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	public class Power : MeasuredValue
	{

		public static Func<UnitCategory> Category = () =>
		{
			return new UnitCategory("Power");
		};

		public Power() :
			base(UnitFamilyName.Power)
		{
		}

		public Power(double value, string? units = null) :
			base(UnitFamilyName.Power)
		{
			Init(Category(), value, units);
		}

		public override double As(string units)
		{
			return ConvertAs(Category(), units);
		}

		public static Power operator +(Power left, Power right) => new(left.Value() + right.Value(), left.Internal());
		public static Power operator -(Power left, Power right) => new(left.Value() - right.Value(), left.Internal());
	}
}
