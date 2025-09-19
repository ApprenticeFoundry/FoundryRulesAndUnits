using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	[JsonConverter(typeof(DurationJsonConverter))]
	public class Duration : MeasuredValue
	{
		public Duration() : base(UnitFamilyName.Duration) { }

		public Duration(double value, string? units = null) : base(UnitFamilyName.Duration)
		{
			Init(value, units);
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

	public class DurationJsonConverter : JsonConverter<Duration>
	{
		public override Duration Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			return MeasuredValue.ReadJSON<Duration>(ref reader, typeToConvert);
		}

		public override void Write(Utf8JsonWriter writer, Duration dataValue, JsonSerializerOptions options)
		{
			// Writing handled by base serialization
		}
	}
}
