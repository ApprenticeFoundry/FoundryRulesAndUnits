using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	[JsonConverter(typeof(TimeJsonConverter))]
	public class Time : MeasuredValue
	{
		public Time() : base(UnitFamilyName.Time) { }

		public Time(double value, string? units = null) : base(UnitFamilyName.Time)
		{
			Init(value, units);
		}

		// Static properties
		public static Time Zero => new(0, "s");

		// Factory methods
		public static Time FromSeconds(double value) => new(value, "s");
		public static Time FromMinutes(double value) => new(value, "min");
		public static Time FromHours(double value) => new(value, "hr");
		public static Time FromDays(double value) => new(value, "day");
		public static Time FromWeeks(double value) => new(value, "week");
		public static Time FromYears(double value) => new(value, "year");

		// Arithmetic operators
		public static Time operator +(Time left, Time right) => new(left.Value() + right.Value(), left.Internal());
		public static Time operator -(Time left, Time right) => new(left.Value() - right.Value(), left.Internal());
		public static Time operator *(Time time, double scalar) => new(time.Value() * scalar, time.Internal());
		public static Time operator *(double scalar, Time time) => new(scalar * time.Value(), time.Internal());
		public static Time operator /(Time time, double scalar) => new(time.Value() / scalar, time.Internal());
		public static double operator /(Time left, Time right) => left.Value() / right.Value();
	}

	public class TimeJsonConverter : JsonConverter<Time>
	{
		public override Time Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			return MeasuredValue.ReadJSON<Time>(ref reader, typeToConvert);
		}

		public override void Write(Utf8JsonWriter writer, Time dataValue, JsonSerializerOptions options)
		{
			// Writing handled by base serialization
		}
	}
}
