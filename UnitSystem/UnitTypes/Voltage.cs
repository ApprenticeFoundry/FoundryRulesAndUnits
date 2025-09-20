using System;
using System.Collections.Generic;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	public class Voltage : MeasuredValue
	{
		#region Constructors and Factory Methods

		// UnitGroup injection constructor (preferred for new code)
		public Voltage(UnitGroup unitGroup) : base(unitGroup)
		{
			if (unitGroup.Family != UnitFamilyName.Voltage)
				throw new ArgumentException($"Expected UnitGroup for Voltage, got {unitGroup.Family}");
		}

		public Voltage(UnitGroup unitGroup, double value, string? units = null) : base(unitGroup)
		{
			if (unitGroup.Family != UnitFamilyName.Voltage)
				throw new ArgumentException($"Expected UnitGroup for Voltage, got {unitGroup.Family}");
			Init(value, units);
		}

		// Backward compatibility constructor for JSON deserialization
		public Voltage(double value, string units) : base()
		{
			V = value;
			I = units;
			U = units;
		}

		#endregion

		#region Unit Conversion - inherited from MeasuredValue

		#endregion

		#region Operators

		public static Voltage operator +(Voltage left, Voltage right)
		{
			var result = new Voltage(left.UnitGroup);
			var leftValue = left.As(left.Internal());
			var rightValue = right.As(left.Internal());
			result.Init(leftValue + rightValue, left.Internal());
			return result;
		}

		public static Voltage operator -(Voltage left, Voltage right)
		{
			var result = new Voltage(left.UnitGroup);
			var leftValue = left.As(left.Internal());
			var rightValue = right.As(left.Internal());
			result.Init(leftValue - rightValue, left.Internal());
			return result;
		}

		public static Voltage operator *(Voltage left, double scalar) 
		{
			var result = new Voltage(left.UnitGroup);
			result.Init(left.Value() * scalar, left.Internal());
			return result;
		}
		
		public static Voltage operator *(double scalar, Voltage right) 
		{
			var result = new Voltage(right.UnitGroup);
			result.Init(scalar * right.Value(), right.Internal());
			return result;
		}
		
		public static Voltage operator /(Voltage left, double scalar) 
		{
			var result = new Voltage(left.UnitGroup);
			result.Init(left.Value() / scalar, left.Internal());
			return result;
		}

		public static bool operator >(Voltage left, Voltage right) => left.As(left.Internal()) > right.As(left.Internal());
		public static bool operator <(Voltage left, Voltage right) => left.As(left.Internal()) < right.As(left.Internal());
		public static bool operator >=(Voltage left, Voltage right) => left.As(left.Internal()) >= right.As(left.Internal());
		public static bool operator <=(Voltage left, Voltage right) => left.As(left.Internal()) <= right.As(left.Internal());

		#endregion


	}


}
