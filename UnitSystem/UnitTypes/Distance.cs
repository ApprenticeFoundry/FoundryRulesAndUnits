using System;
using System.Collections.Generic;


namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	public class Distance : MeasuredValue
	{
		public static Func<UnitCategory> Category = () =>
		{
			return new UnitCategory("Distance");
		};

		public Distance() :
			base(UnitFamilyName.Length)
		{
		}

		public Distance(double value, string? units) :
			base(UnitFamilyName.Length)
		{
			Init(Category(), value, units);
		}

		public Distance Assign(double value, string? units)
		{
			if (units == I)
			{
				V = value;
			}
			else
			{
				Init(Category(), value, units);
			}
			return this;
		}

		public Distance Copy()
		{
			return new Distance(Value(), Internal());
		}

		public double Diff(Length other)
		{
			return Value() - other.Value();
		}
		public double Diff(double other)
		{
			return Value() - other;
		}
		public double Sum(Length other)
		{
			return Value() + other.Value();
		}

		public double Sum(double other)
		{
			return Value() + other;
		}



		public override double As(string units)
		{
			return ConvertAs(Category(), units);
		}

		public static Distance operator +(Distance left, double right) => new(left.Value() + right, left.Internal());
		public static Distance operator -(Distance left, double right) => new(left.Value() - right, left.Internal());

		public static Distance operator +(Distance left, Distance right) => new(left.Value() + right.Value(), left.Internal());
		public static Distance operator -(Distance left, Distance right) => new(left.Value() - right.Value(), left.Internal());

	}
}
