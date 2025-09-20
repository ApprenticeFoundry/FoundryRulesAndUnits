using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	[JsonConverter(typeof(ResistanceJsonConverter))]
	public class Resistance : MeasuredValue
	{
		#region Constructors and Factory Methods

		public Resistance() : base(UnitFamilyName.Resistance) { }


		public Resistance(double value, string? units = null) : base(UnitFamilyName.Resistance)
		{
			Init(value, units);
		}

		// Factory methods for common resistance units
		public static Resistance FromOhms(double value) => new(value, "Ω");
		public static Resistance FromKiloOhms(double value) => new(value, "kΩ");
		public static Resistance FromMegaOhms(double value) => new(value, "MΩ");
		public static Resistance FromGigaOhms(double value) => new(value, "GΩ");
		public static Resistance FromMilliOhms(double value) => new(value, "mΩ");
		public static Resistance FromMicroOhms(double value) => new(value, "μΩ");

		#endregion

		#region Unit Conversion

		public override double As(string units)
		{
			return GlobalUnitSystem.Convert(Value(), Internal(), units);
		}

		#endregion

		#region Operators

		public static Resistance operator +(Resistance left, Resistance right)
		{
			var leftValue = left.As(left.Internal());
			var rightValue = right.As(left.Internal());
			return new Resistance(leftValue + rightValue, left.Internal());
		}

		public static Resistance operator -(Resistance left, Resistance right)
		{
			var leftValue = left.As(left.Internal());
			var rightValue = right.As(left.Internal());
			return new Resistance(leftValue - rightValue, left.Internal());
		}

		public static Resistance operator *(Resistance left, double scalar) => new(left.Value() * scalar, left.Internal());
		public static Resistance operator *(double scalar, Resistance right) => new(scalar * right.Value(), right.Internal());
		public static Resistance operator /(Resistance left, double scalar) => new(left.Value() / scalar, left.Internal());

		public static bool operator >(Resistance left, Resistance right) => left.As(left.Internal()) > right.As(left.Internal());
		public static bool operator <(Resistance left, Resistance right) => left.As(left.Internal()) < right.As(left.Internal());
		public static bool operator >=(Resistance left, Resistance right) => left.As(left.Internal()) >= right.As(left.Internal());
		public static bool operator <=(Resistance left, Resistance right) => left.As(left.Internal()) <= right.As(left.Internal());

		#endregion


	}

	#region JSON Converter

	public class ResistanceJsonConverter : JsonConverter<Resistance>
	{
		public override Resistance Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
			{
				var root = doc.RootElement;
				var value = root.GetProperty("value").GetDouble();
				var units = root.GetProperty("units").GetString();
				return new Resistance(value, units);
			}
		}

		public override void Write(Utf8JsonWriter writer, Resistance value, JsonSerializerOptions options)
		{
			writer.WriteStartObject();
			writer.WriteNumber("value", value.Value());
			writer.WriteString("units", value.Internal());
			writer.WriteEndObject();
		}
	}

	#endregion
}
