using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	[JsonConverter(typeof(CapacitanceJsonConverter))]
	public class Capacitance : MeasuredValue
	{
		#region Constructors and Factory Methods

		// UnitGroup injection constructor (preferred for new code)
		public Capacitance(UnitGroup unitGroup) : base(unitGroup) 
		{ 
			if (unitGroup.Family != UnitFamilyName.Capacitance)
				throw new ArgumentException($"Expected UnitGroup for Capacitance, got {unitGroup.Family}");
		}

		public Capacitance(UnitGroup unitGroup, double value, string? units = null) : base(unitGroup)
		{
			if (unitGroup.Family != UnitFamilyName.Capacitance)
				throw new ArgumentException($"Expected UnitGroup for Capacitance, got {unitGroup.Family}");
			Init(value, units);
		}

		/// <summary>


		/// Backward compatibility constructor for JSON deserialization


		/// </summary>


		public Capacitance(double value, string units) : base()


		{


			V = value;


			I = units;


			U = units;


		}

		// Factory methods for common capacitance units
		public static Capacitance FromFarads(double value) => new(value, "F");
		public static Capacitance FromMicrofarads(double value) => new(value, "μF");
		public static Capacitance FromNanofarads(double value) => new(value, "nF");
		public static Capacitance FromPicofarads(double value) => new(value, "pF");
		public static Capacitance FromMillifarads(double value) => new(value, "mF");

		#endregion

		#region Unit Conversion
		// As() method inherited from MeasuredValue with UnitGroup conversion

		#endregion

		#region Operators

		public static Capacitance operator +(Capacitance left, Capacitance right)
		{
			var leftValue = left.As(left.Internal());
			var rightValue = right.As(left.Internal());
			return new Capacitance(leftValue + rightValue, left.Internal());
		}

		public static Capacitance operator -(Capacitance left, Capacitance right)
		{
			var leftValue = left.As(left.Internal());
			var rightValue = right.As(left.Internal());
			return new Capacitance(leftValue - rightValue, left.Internal());
		}

		public static Capacitance operator *(Capacitance left, double scalar) => new(left.Value() * scalar, left.Internal());
		public static Capacitance operator *(double scalar, Capacitance right) => new(scalar * right.Value(), right.Internal());
		public static Capacitance operator /(Capacitance left, double scalar) => new(left.Value() / scalar, left.Internal());

		public static bool operator >(Capacitance left, Capacitance right) => left.As(left.Internal()) > right.As(left.Internal());
		public static bool operator <(Capacitance left, Capacitance right) => left.As(left.Internal()) < right.As(left.Internal());
		public static bool operator >=(Capacitance left, Capacitance right) => left.As(left.Internal()) >= right.As(left.Internal());
		public static bool operator <=(Capacitance left, Capacitance right) => left.As(left.Internal()) <= right.As(left.Internal());

		#endregion


	}

	#region JSON Converter

	public class CapacitanceJsonConverter : JsonConverter<Capacitance>
	{
		public override Capacitance Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
			{
				var root = doc.RootElement;
				var value = root.GetProperty("value").GetDouble();
				var units = root.GetProperty("units").GetString();
				return new Capacitance(value, units);
			}
		}

		public override void Write(Utf8JsonWriter writer, Capacitance value, JsonSerializerOptions options)
		{
			writer.WriteStartObject();
			writer.WriteNumber("value", value.Value());
			writer.WriteString("units", value.Internal());
			writer.WriteEndObject();
		}
	}

	#endregion
}
