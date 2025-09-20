using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	[JsonConverter(typeof(DataFlowJsonConverter))]
	public class DataFlow : MeasuredValue
	{
		#region Constructors and Factory Methods

		public DataFlow() : base(UnitFamilyName.DataFlow) { }

		public DataFlow(double value, string? units = null) : base(UnitFamilyName.DataFlow)
		{
			Init(value, units);
		}

		// Factory methods for common data flow units
		public static DataFlow FromBytesPerSecond(double value) => new(value, "B/s");
		public static DataFlow FromKilobytesPerSecond(double value) => new(value, "KB/s");
		public static DataFlow FromMegabytesPerSecond(double value) => new(value, "MB/s");
		public static DataFlow FromGigabytesPerSecond(double value) => new(value, "GB/s");
		public static DataFlow FromKbps(double value) => new(value, "Kbps");
		public static DataFlow FromMbps(double value) => new(value, "Mbps");
		public static DataFlow FromGbps(double value) => new(value, "Gbps");

		#endregion

		#region Unit Conversion

		public override double As(string units)
		{
			return GlobalUnitSystem.Convert(Value(), Internal(), units);
		}

		#endregion

		#region Operators

		public static DataFlow operator +(DataFlow left, DataFlow right)
		{
			var leftValue = left.As(left.Internal());
			var rightValue = right.As(left.Internal());
			return new DataFlow(leftValue + rightValue, left.Internal());
		}

		public static DataFlow operator -(DataFlow left, DataFlow right)
		{
			var leftValue = left.As(left.Internal());
			var rightValue = right.As(left.Internal());
			return new DataFlow(leftValue - rightValue, left.Internal());
		}

		public static DataFlow operator *(DataFlow left, double scalar) => new(left.Value() * scalar, left.Internal());
		public static DataFlow operator *(double scalar, DataFlow right) => new(scalar * right.Value(), right.Internal());
		public static DataFlow operator /(DataFlow left, double scalar) => new(left.Value() / scalar, left.Internal());

		public static bool operator >(DataFlow left, DataFlow right) => left.As(left.Internal()) > right.As(left.Internal());
		public static bool operator <(DataFlow left, DataFlow right) => left.As(left.Internal()) < right.As(left.Internal());
		public static bool operator >=(DataFlow left, DataFlow right) => left.As(left.Internal()) >= right.As(left.Internal());
		public static bool operator <=(DataFlow left, DataFlow right) => left.As(left.Internal()) <= right.As(left.Internal());

		#endregion


	}

	#region JSON Converter

	public class DataFlowJsonConverter : JsonConverter<DataFlow>
	{
		public override DataFlow Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
			{
				var root = doc.RootElement;
				var value = root.GetProperty("value").GetDouble();
				var units = root.GetProperty("units").GetString();
				return new DataFlow(value, units);
			}
		}

		public override void Write(Utf8JsonWriter writer, DataFlow value, JsonSerializerOptions options)
		{
			writer.WriteStartObject();
			writer.WriteNumber("value", value.Value());
			writer.WriteString("units", value.Internal());
			writer.WriteEndObject();
		}
	}

	#endregion
}
