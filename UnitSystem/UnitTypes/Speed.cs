using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	[JsonConverter(typeof(SpeedJsonConverter))]
	public class Speed : MeasuredValue
	{
		public Speed() : base(UnitFamilyName.Speed) { }

		public Speed(double value, string? units = null) : base(UnitFamilyName.Speed)
		{
			Init(value, units);
		}

		// Factory methods
		public static Speed FromMetersPerSecond(double value) => new(value, "m/s");
		public static Speed FromKilometersPerHour(double value) => new(value, "km/h");
		public static Speed FromMilesPerHour(double value) => new(value, "mph");
		public static Speed FromFeetPerSecond(double value) => new(value, "ft/s");
		public static Speed FromKnots(double value) => new(value, "kn");

		// Arithmetic operators
		public static Speed operator +(Speed left, Speed right) => new(left.Value() + right.Value(), left.Internal());
		public static Speed operator -(Speed left, Speed right) => new(left.Value() - right.Value(), left.Internal());
		public static Speed operator *(Speed speed, double scalar) => new(speed.Value() * scalar, speed.Internal());
		public static Speed operator *(double scalar, Speed speed) => new(scalar * speed.Value(), speed.Internal());
		public static Speed operator /(Speed speed, double scalar) => new(speed.Value() / scalar, speed.Internal());
		public static double operator /(Speed left, Speed right) => left.Value() / right.Value();
	}

	public class SpeedJsonConverter : JsonConverter<Speed>
	{
		public override Speed Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			return MeasuredValue.ReadJSON<Speed>(ref reader, typeToConvert);
		}

		public override void Write(Utf8JsonWriter writer, Speed dataValue, JsonSerializerOptions options)
		{
			// Writing handled by base serialization
		}
	}
}
