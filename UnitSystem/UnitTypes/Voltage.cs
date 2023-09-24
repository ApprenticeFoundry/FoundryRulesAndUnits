using System;
using System.Collections.Generic;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	public class Voltage : MeasuredValue
	{

		public static Func<UnitCategory> Category = () =>
		{
			return new UnitCategory("Voltage");
		};

		public Voltage() :
			base(UnitFamilyName.Volume)
		{
		}

		public Voltage(double value, string? units = null) :
			base(UnitFamilyName.Temperature)
		{
			Init(Category(), value, units);
		}

		public override double As(string units)
		{
			return ConvertAs(Category(), units);
		}

		public static Voltage operator +(Voltage left, Voltage right) => new(left.Value() + right.Value(), left.Internal());
		public static Voltage operator -(Voltage left, Voltage right) => new(left.Value() - right.Value(), left.Internal());
	}
}
