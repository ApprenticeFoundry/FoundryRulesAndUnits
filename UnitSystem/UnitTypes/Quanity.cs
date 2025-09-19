using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	[JsonConverter(typeof(QuantityJsonConverter))]
	public class Quantity : MeasuredValue
	{
		public Quantity() : base(UnitFamilyName.Quantity) { }

		public Quantity(double value, string? units = null) : base(UnitFamilyName.Quantity)
		{
			Init(value, units);
		}

		// Factory methods
		public static Quantity FromEach(double value) => new(value, "ea");
		public static Quantity FromItems(double value) => new(value, "items");
		public static Quantity FromPieces(double value) => new(value, "pcs");
		public static Quantity FromUnits(double value) => new(value, "units");


		// Arithmetic operators with integers
		public static Quantity operator +(Quantity left, int right) => new(left.Value() + right, left.Internal());
		public static Quantity operator -(Quantity left, int right) => new(left.Value() - right, left.Internal());

		// Arithmetic operators with other quantities
		public static Quantity operator +(Quantity left, Quantity right) => new(left.Value() + right.Value(), left.Internal());
		public static Quantity operator -(Quantity left, Quantity right) => new(left.Value() - right.Value(), left.Internal());
		public static Quantity operator *(Quantity quantity, double scalar) => new(quantity.Value() * scalar, quantity.Internal());
		public static Quantity operator *(double scalar, Quantity quantity) => new(scalar * quantity.Value(), quantity.Internal());
		public static Quantity operator /(Quantity quantity, double scalar) => new(quantity.Value() / scalar, quantity.Internal());
		public static double operator /(Quantity left, Quantity right) => left.Value() / right.Value();

		// Special operator for creating flow rates
		public static QuantityFlow operator /(Quantity left, Time right) => new(left.Value() / right.Value(), "ea/s");
	}

	public class QuantityJsonConverter : JsonConverter<Quantity>
	{
		public override Quantity Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			return MeasuredValue.ReadJSON<Quantity>(ref reader, typeToConvert);
		}

		public override void Write(Utf8JsonWriter writer, Quantity dataValue, JsonSerializerOptions options)
		{
			// Writing handled by base serialization
		}
	}
}
