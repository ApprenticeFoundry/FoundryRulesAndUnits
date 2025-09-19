using FoundryRulesAndUnits.Extensions;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	[JsonConverter(typeof(ForceJsonConverter))]
	public class Force : MeasuredValue
	{
		public Force() : base(UnitFamilyName.Force) { }

		public Force(double value, string? units = null) : base(UnitFamilyName.Force)
		{
			Init(value, units);
		}

		// Factory methods
		public static Force FromNewtons(double value) => new(value, "N");
		public static Force FromKilonewtons(double value) => new(value, "kN");
		public static Force FromPoundForce(double value) => new(value, "lbf");
		public static Force FromDynes(double value) => new(value, "dyne");

		// Comparison operators
		public static bool operator <(Force left, Force right) => left.Value() < right.Value();
		public static bool operator >(Force left, Force right) => left.Value() > right.Value();

		// Arithmetic operators
		public static Force operator +(Force left, Force right) => new(left.Value() + right.Value(), left.Internal());
		public static Force operator -(Force left, Force right) => new(left.Value() - right.Value(), left.Internal());
		public static Force operator *(Force force, double scalar) => new(force.Value() * scalar, force.Internal());
		public static Force operator *(double scalar, Force force) => new(scalar * force.Value(), force.Internal());
		public static Force operator /(Force force, double scalar) => new(force.Value() / scalar, force.Internal());
		public static double operator /(Force left, Force right) => left.Value() / right.Value();
	}

	public class ForceJsonConverter : JsonConverter<Force>
	{
		public override Force Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			return MeasuredValue.ReadJSON<Force>(ref reader, typeToConvert);
		}

		public override void Write(Utf8JsonWriter writer, Force dataValue, JsonSerializerOptions options)
		{
			// Writing handled by base serialization
		}
	}
}