using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	[JsonConverter(typeof(VolumeJsonConverter))]
	public class Volume : MeasuredValue
	{
		public Volume() : base(UnitFamilyName.Volume) { }

		public Volume(double value, string? units = null) : base(UnitFamilyName.Volume)
		{
			Init(value, units);
		}

		// Factory methods
		public static Volume FromLiters(double value) => new(value, "L");
		public static Volume FromMilliliters(double value) => new(value, "mL");
		public static Volume FromGallons(double value) => new(value, "gal");
		public static Volume FromCubicMeters(double value) => new(value, "m³");
		public static Volume FromCubicInches(double value) => new(value, "in³");
		public static Volume FromCubicFeet(double value) => new(value, "ft³");

		// Arithmetic operators
		public static Volume operator +(Volume left, Volume right) => new(left.Value() + right.Value(), left.Internal());
		public static Volume operator -(Volume left, Volume right) => new(left.Value() - right.Value(), left.Internal());
		public static Volume operator *(Volume volume, double scalar) => new(volume.Value() * scalar, volume.Internal());
		public static Volume operator *(double scalar, Volume volume) => new(scalar * volume.Value(), volume.Internal());
		public static Volume operator /(Volume volume, double scalar) => new(volume.Value() / scalar, volume.Internal());
		public static double operator /(Volume left, Volume right) => left.Value() / right.Value();
	}

	public class VolumeJsonConverter : JsonConverter<Volume>
	{
		public override Volume Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			return MeasuredValue.ReadJSON<Volume>(ref reader, typeToConvert);
		}

		public override void Write(Utf8JsonWriter writer, Volume dataValue, JsonSerializerOptions options)
		{
			// Writing handled by base serialization
		}
	}
}
