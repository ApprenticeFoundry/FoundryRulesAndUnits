using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]

	public class Duration : MeasuredValue
	{
		override public UnitFamilyName UnitFamily => UnitFamilyName.Duration;
		#region Constructors and Factory Methods

		// UnitGroup injection constructor (preferred for new code)
		public Duration(UnitGroup unitGroup) : base(unitGroup)
		{
			if (unitGroup.Family != UnitFamilyName.Duration)
				throw new ArgumentException($"Expected UnitGroup for Duration, got {unitGroup.Family}");
		}


		// Static properties
		public static Duration Zero => new(0, "s");

		// Factory methods
		public static Duration FromSeconds(double value) => new(value, "s");
		public static Duration FromMinutes(double value) => new(value, "min");
		public static Duration FromHours(double value) => new(value, "hr");
		public static Duration FromDays(double value) => new(value, "d");
		public static Duration FromWeeks(double value) => new(value, "week");

		// Arithmetic operators
		public static Duration operator +(Duration left, Duration right) => new(left.Value() + right.Value(), left.Internal());
		public static Duration operator -(Duration left, Duration right) => new(left.Value() - right.Value(), left.Internal());
		public static Duration operator *(Duration duration, double scalar) => new(duration.Value() * scalar, duration.Internal());
		public static Duration operator *(double scalar, Duration duration) => new(scalar * duration.Value(), duration.Internal());
		public static Duration operator /(Duration duration, double scalar) => new(duration.Value() / scalar, duration.Internal());
		public static double operator /(Duration left, Duration right) => left.Value() / right.Value();

		// Comparison operators
		public static bool operator <=(Duration left, Duration right) => left.Value() <= right.Value();
		public static bool operator >=(Duration left, Duration right) => left.Value() >= right.Value();
	}


}
