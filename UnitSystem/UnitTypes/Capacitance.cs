using System;
using System.Collections.Generic;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	public class Capacitance : MeasuredValue
	{

		public static Func<UnitCategory> Category = () =>
		{
			return new UnitCategory("Capacitance");
		};

		public Capacitance() :
			base(UnitFamilyName.Capacitance)
		{
		}

		public Capacitance(double value, string? units = null) :
			base(UnitFamilyName.Capacitance)
		{
			Init(Category(), value, units);
		}

		public override double As(string units)
		{
			return ConvertAs(Category(), units);
		}

		public static Capacitance operator +(Capacitance left, Capacitance right) => new(left.Value() + right.Value(), left.Internal());
		public static Capacitance operator -(Capacitance left, Capacitance right) => new(left.Value() - right.Value(), left.Internal());
	}
}
