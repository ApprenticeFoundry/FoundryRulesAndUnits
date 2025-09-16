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

		public Time(double value, string? units = null) :
			base(UnitFamilyName.Time)
		{
			Init(Category(), value, units);
		}

		public static Time Zero { get { return new Time(0, "s"); } }

		public static Time FromDays(double v)
		{
			return new Time(v, "day");
		}

		public static Time FromSeconds(double v)
		{
			return new Time(v, "s");
		}

		public static Time FromMinutes(double v)
		{
			return new Time(v, "min");
		}

		public static Time FromHours(double v)
		{
			return new Time(v, "hr");
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
