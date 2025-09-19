using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using FoundryRulesAndUnits.Extensions;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	[JsonConverter(typeof(TemperatureJsonConverter))]
	public class Temperature : MeasuredValue
	{
		public Temperature() : base(UnitFamilyName.Temperature) { }

		public Temperature(double value, string? units = null) : base(UnitFamilyName.Temperature)
		{
			Init(value, units);
		}

		// Factory methods
		public static Temperature FromCelsius(double value) => new(value, "°C");
		public static Temperature FromFahrenheit(double value) => new(value, "°F");
		public static Temperature FromKelvin(double value) => new(value, "K");
		public static Temperature FromRankine(double value) => new(value, "°R");

		// Arithmetic operators
		public static Temperature operator +(Temperature left, Temperature right) => new(left.Value() + right.Value(), left.Internal());
		public static Temperature operator -(Temperature left, Temperature right) => new(left.Value() - right.Value(), left.Internal());
		public static Temperature operator *(Temperature temp, double scalar) => new(temp.Value() * scalar, temp.Internal());
		public static Temperature operator *(double scalar, Temperature temp) => new(scalar * temp.Value(), temp.Internal());
		public static Temperature operator /(Temperature temp, double scalar) => new(temp.Value() / scalar, temp.Internal());
		public static double operator /(Temperature left, Temperature right) => left.Value() / right.Value();
	}

	public class TemperatureJsonConverter : JsonConverter<Temperature>
	{
		public override Temperature Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			return MeasuredValue.ReadJSON<Temperature>(ref reader, typeToConvert);
		}

		public override void Write(Utf8JsonWriter writer, Temperature dataValue, JsonSerializerOptions options)
		{
			// Writing handled by base serialization
		}
	}
}
