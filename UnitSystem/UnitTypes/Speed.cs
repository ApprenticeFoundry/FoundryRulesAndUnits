using System;
using System.Collections.Generic;


namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	public class Speed : MeasuredValue
	{
		public static Func<UnitCategory> Category = () =>
		{
			return new UnitCategory("Speed");
		};

		public Speed() :
			base(UnitFamilyName.Speed)
		{
		}

		public Speed(double value, string? units) :
			base(UnitFamilyName.Speed)
		{
			Init(Category(), value, units);
		}

		public static Speed FromMetersPerSecond(double v)
		{
			return new Speed(v, "m/s");
		}

		public override double As(string units)
		{
			return ConvertAs(Category(), units);
		}

		public void MilesPerHour(double value)
		{
			throw new NotImplementedException();
		}

		public static Speed operator +(Speed left, Speed right) => new(left.Value() + right.Value(), left.Internal());
		public static Speed operator -(Speed left, Speed right) => new(left.Value() - right.Value(), left.Internal());

	}
}
