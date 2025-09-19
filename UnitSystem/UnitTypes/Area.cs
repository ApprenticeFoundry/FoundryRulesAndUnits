using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	[JsonConverter(typeof(AreaJsonConverter))]
	public class Area : MeasuredValue
	{
		public Area() : base(UnitFamilyName.Area) { }

		public Area(double value, string? units = null) : base(UnitFamilyName.Area)
		{
			Init(value, units);
		}

		// Factory methods
		public static Area FromSquareMeters(double value) => new(value, "m²");
		public static Area FromSquareKilometers(double value) => new(value, "km²");
		public static Area FromSquareFeet(double value) => new(value, "ft²");
		public static Area FromSquareInches(double value) => new(value, "in²");
		public static Area FromAcres(double value) => new(value, "ac");
		public static Area FromHectares(double value) => new(value, "ha");

		// Arithmetic operators
		public static Area operator +(Area left, Area right) => new(left.Value() + right.Value(), left.Internal());
		public static Area operator -(Area left, Area right) => new(left.Value() - right.Value(), left.Internal());
		public static Area operator *(Area area, double scalar) => new(area.Value() * scalar, area.Internal());
		public static Area operator *(double scalar, Area area) => new(scalar * area.Value(), area.Internal());
		public static Area operator /(Area area, double scalar) => new(area.Value() / scalar, area.Internal());
		public static double operator /(Area left, Area right) => left.Value() / right.Value();
	}

	public class AreaJsonConverter : JsonConverter<Area>
	{
		public override Area Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			return MeasuredValue.ReadJSON<Area>(ref reader, typeToConvert);
		}

		public override void Write(Utf8JsonWriter writer, Area dataValue, JsonSerializerOptions options)
		{
			// Writing handled by base serialization
		}
	}
}