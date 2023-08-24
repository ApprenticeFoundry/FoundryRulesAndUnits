using System;
using System.Collections.Generic;


namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	public class Time : MeasuredValue
	{
		public static Func<UnitCategory> Category = () =>
		{
			return new UnitCategory("Time");
		};

		public Time() :
			base(UnitFamilyName.Time)
		{
		}

		public Time(double value, string? units) :
			base(UnitFamilyName.Duration)
		{
			Init(Category(), value, units);
		}

		public static Duration Zero { get { return new Duration(0, "s"); } }

		public static Duration FromDays(double v)
		{
			return new Duration(v, "d");
		}

		public static Duration FromSeconds(double v)
		{
			return new Duration(v, "s");
		}

		public override double As(string units)
		{
			return ConvertAs(Category(), units);
		}



		public static Time operator +(Time left, Time right) => new(left.Value() + right.Value(), left.Internal());
		public static Time operator -(Time left, Time right) => new(left.Value() - right.Value(), left.Internal());

		//public static bool operator <=(Time left, Duration right) => left.Value() <= right.Value();
		//public static bool operator >=(Time left, Time right) => left.Value() >= right.Value();

	}
}
