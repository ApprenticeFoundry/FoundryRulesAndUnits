using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	[JsonConverter(typeof(PowerJsonConverter))]
	public class Power : MeasuredValue
	{
		public Power() : base(UnitFamilyName.Power) { }

		public Power(double value, string? units = null) : base(UnitFamilyName.Power)
		{
			Init(value, units);
		}

		// Factory methods
		public static Power FromWatts(double value) => new(value, "W");
		public static Power FromKilowatts(double value) => new(value, "kW");
		public static Power FromMegawatts(double value) => new(value, "MW");
		public static Power FromHorsepower(double value) => new(value, "hp");
		public static Power FromBTUPerHour(double value) => new(value, "BTU/h");

		// Arithmetic operators
		public static Power operator +(Power left, Power right) => new(left.Value() + right.Value(), left.Internal());
		public static Power operator -(Power left, Power right) => new(left.Value() - right.Value(), left.Internal());
		public static Power operator *(Power power, double scalar) => new(power.Value() * scalar, power.Internal());
		public static Power operator *(double scalar, Power power) => new(scalar * power.Value(), power.Internal());
		public static Power operator /(Power power, double scalar) => new(power.Value() / scalar, power.Internal());
		public static double operator /(Power left, Power right) => left.Value() / right.Value();
	}

	public class PowerJsonConverter : JsonConverter<Power>
	{
		public override Power Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			return MeasuredValue.ReadJSON<Power>(ref reader, typeToConvert);
		}

		public override void Write(Utf8JsonWriter writer, Power dataValue, JsonSerializerOptions options)
		{
			// Writing handled by base serialization
		}
	}
}
