using System;
using System.Collections.Generic;


namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	public class Duration : MeasuredValue
	{
		public static Func<UnitCategory> Category = () =>
		{
			return new UnitCategory("Duration");
		};

		public Duration() :
			base(UnitFamilyName.Duration)
		{
		}

		public Duration(double value, string? units = null) :
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



		public static Duration operator +(Duration left, Duration right) => new(left.Value() + right.Value(), left.Internal());
		public static Duration operator -(Duration left, Duration right) => new(left.Value() - right.Value(), left.Internal());

		public static bool operator <=(Duration left, Duration right) => left.Value() <= right.Value();
		public static bool operator >=(Duration left, Duration right) => left.Value() >= right.Value();

	}
}
