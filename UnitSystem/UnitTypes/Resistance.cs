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

		// UnitGroup injection constructor (preferred for new code)
		public Resistance(UnitGroup unitGroup) : base(unitGroup) 
		{ 
			if (unitGroup.Family != UnitFamilyName.Resistance)
				throw new ArgumentException($"Expected UnitGroup for Resistance, got {unitGroup.Family}");
		}

		public Resistance(UnitGroup unitGroup, double value, string? units = null) : base(unitGroup)
		{
			if (unitGroup.Family != UnitFamilyName.Resistance)
				throw new ArgumentException($"Expected UnitGroup for Resistance, got {unitGroup.Family}");
			Init(value, units);
		}

		/// <summary>


		/// Backward compatibility constructor for JSON deserialization


		/// </summary>


		public Resistance(double value, string units) : base()


		{


			V = value;


			I = units;


			U = units;


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
		// As() method inherited from MeasuredValue with UnitGroup conversion

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
