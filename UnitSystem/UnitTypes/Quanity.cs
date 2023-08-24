using System;
using System.Collections.Generic;


namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	public class Quanity : MeasuredValue
	{
		public static Func<UnitCategory> Category = () =>
		{
			return new UnitCategory("Quanity");
		};

		public Quanity() :
			base(UnitFamilyName.Quanity)
		{
		}

		public Quanity(double value, string? units = null) :
			base(UnitFamilyName.Quanity)
		{
			Init(Category(), value, units);
		}



		public override double As(string units)
		{
			return ConvertAs(Category(), units);
		}


		public static Quanity operator +(Quanity left, int right) => new(left.Value() + right, left.Internal());
		public static Quanity operator -(Quanity left, int right) => new(left.Value() - right, left.Internal());

		public static Quanity operator +(Quanity left, Quanity right) => new(left.Value() + right.Value(), left.Internal());
		public static Quanity operator -(Quanity left, Quanity right) => new(left.Value() - right.Value(), left.Internal());

	}
}
