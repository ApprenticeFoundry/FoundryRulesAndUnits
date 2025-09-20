using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	[JsonConverter(typeof(QuantityFlowJsonConverter))]
	public class QuantityFlow : MeasuredValue
	{
		#region Constructors and Factory Methods

		// UnitGroup injection constructor (preferred for new code)
		public QuantityFlow(UnitGroup unitGroup) : base(unitGroup) 
		{ 
			if (unitGroup.Family != UnitFamilyName.QuantityFlow)
				throw new ArgumentException($"Expected UnitGroup for QuantityFlow, got {unitGroup.Family}");
		}

		public QuantityFlow(UnitGroup unitGroup, double value, string? units = null) : base(unitGroup)
		{
			if (unitGroup.Family != UnitFamilyName.QuantityFlow)
				throw new ArgumentException($"Expected UnitGroup for QuantityFlow, got {unitGroup.Family}");
			Init(value, units);
		}

		/// <summary>


		/// Backward compatibility constructor for JSON deserialization


		/// </summary>


		public QuantityFlow(double value, string units) : base()


		{


			V = value;


			I = units;


			U = units;


		}

		// Factory methods for common quantity flow units
		public static QuantityFlow FromUnitsPerSecond(double value) => new(value, "ea/s");
		public static QuantityFlow FromUnitsPerMinute(double value) => new(value, "ea/min");
		public static QuantityFlow FromUnitsPerHour(double value) => new(value, "ea/hr");
		public static QuantityFlow FromItemsPerSecond(double value) => new(value, "items/s");
		public static QuantityFlow FromItemsPerMinute(double value) => new(value, "items/min");
		public static QuantityFlow FromItemsPerHour(double value) => new(value, "items/hr");

		#endregion

		#region Unit Conversion
		// As() method inherited from MeasuredValue with UnitGroup conversion

		#endregion

		#region Operators

		public static QuantityFlow operator +(QuantityFlow left, QuantityFlow right)
		{
			var leftValue = left.As(left.Internal());
			var rightValue = right.As(left.Internal());
			return new QuantityFlow(leftValue + rightValue, left.Internal());
		}

		public static QuantityFlow operator -(QuantityFlow left, QuantityFlow right)
		{
			var leftValue = left.As(left.Internal());
			var rightValue = right.As(left.Internal());
			return new QuantityFlow(leftValue - rightValue, left.Internal());
		}

		public static QuantityFlow operator *(QuantityFlow left, double scalar) => new(left.Value() * scalar, left.Internal());
		public static QuantityFlow operator *(double scalar, QuantityFlow right) => new(scalar * right.Value(), right.Internal());
		public static QuantityFlow operator /(QuantityFlow left, double scalar) => new(left.Value() / scalar, left.Internal());

		// Special cross-unit operations
		public static Quantity operator *(QuantityFlow left, Time right) => new(left.Value() * right.Value(), "ea");

		public static bool operator >(QuantityFlow left, QuantityFlow right) => left.As(left.Internal()) > right.As(left.Internal());
		public static bool operator <(QuantityFlow left, QuantityFlow right) => left.As(left.Internal()) < right.As(left.Internal());
		public static bool operator >=(QuantityFlow left, QuantityFlow right) => left.As(left.Internal()) >= right.As(left.Internal());
		public static bool operator <=(QuantityFlow left, QuantityFlow right) => left.As(left.Internal()) <= right.As(left.Internal());

		#endregion


	}

	#region JSON Converter

	public class QuantityFlowJsonConverter : JsonConverter<QuantityFlow>
	{
		public override QuantityFlow Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
			{
				var root = doc.RootElement;
				var value = root.GetProperty("value").GetDouble();
				var units = root.GetProperty("units").GetString();
				return new QuantityFlow(value, units);
			}
		}

		public override void Write(Utf8JsonWriter writer, QuantityFlow value, JsonSerializerOptions options)
		{
			writer.WriteStartObject();
			writer.WriteNumber("value", value.Value());
			writer.WriteString("units", value.Internal());
			writer.WriteEndObject();
		}
	}

	#endregion
}
