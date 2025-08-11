using System;
using System.Collections.Generic;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	public class Current : MeasuredValue
	{

		public static Func<UnitCategory> Category = () =>
		{
			return new UnitCategory("Current");
		};

		public Current() :
			base(UnitFamilyName.Current)
		{
		}

		public Current(double value, string? units = null) :
			base(UnitFamilyName.Current)
		{
			Init(Category(), value, units);
		}

		public override double As(string units)
		{
			return ConvertAs(Category(), units);
		}

		public static Current operator +(Current left, Current right) => new(left.Value() + right.Value(), left.Internal());
		public static Current operator -(Current left, Current right) => new(left.Value() - right.Value(), left.Internal());
	}
}
