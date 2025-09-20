using System;
using System.Collections.Generic;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	public class Time : MeasuredValue
	{
		#region Constructors and Factory Methods

		// UnitGroup injection constructor (preferred for new code)
		public Time(UnitGroup unitGroup) : base(unitGroup)
		{
			if (unitGroup.Family != UnitFamilyName.Time)
				throw new ArgumentException($"Expected UnitGroup for Time, got {unitGroup.Family}");
		}

		public Time(UnitGroup unitGroup, double value, string? units = null) : base(unitGroup)
		{
			if (unitGroup.Family != UnitFamilyName.Time)
				throw new ArgumentException($"Expected UnitGroup for Time, got {unitGroup.Family}");
			Init(value, units);
		}

		/// <summary>
		/// Backward compatibility constructor for JSON deserialization
		/// </summary>
		public Time(double value, string units) : base()
		{
			V = value;
			I = units;
			U = units;
		}

		#endregion

		// Arithmetic operators
		public static Time operator +(Time left, Time right) 
		{
			var result = new Time(left.UnitGroup);
			result.Init(left.Value() + right.Value(), left.Internal());
			return result;
		}
		
		public static Time operator -(Time left, Time right) 
		{
			var result = new Time(left.UnitGroup);
			result.Init(left.Value() - right.Value(), left.Internal());
			return result;
		}
		
		public static Time operator *(Time time, double scalar) 
		{
			var result = new Time(time.UnitGroup);
			result.Init(time.Value() * scalar, time.Internal());
			return result;
		}
		
		public static Time operator *(double scalar, Time time) 
		{
			var result = new Time(time.UnitGroup);
			result.Init(scalar * time.Value(), time.Internal());
			return result;
		}
		
		public static Time operator /(Time time, double scalar) 
		{
			var result = new Time(time.UnitGroup);
			result.Init(time.Value() / scalar, time.Internal());
			return result;
		}
		
		public static double operator /(Time left, Time right) => left.Value() / right.Value();
	}


}
