using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	[JsonConverter(typeof(VoltageJsonConverter))]
	public class Voltage : MeasuredValue
	{
		#region Constructors and Factory Methods

		public Voltage() : base(UnitFamilyName.Voltage) { }

		public Voltage(double value, string? units = null) : base(UnitFamilyName.Voltage)
		{
			Init(value, units);
		}

		// Factory methods for common voltage units
		public static Voltage FromVolts(double value) => new(value, "V");
		public static Voltage FromKilovolts(double value) => new(value, "kV");
		public static Voltage FromMillivolts(double value) => new(value, "mV");
		public static Voltage FromMicrovolts(double value) => new(value, "μV");
		public static Voltage FromMegavolts(double value) => new(value, "MV");

		#endregion

		#region Unit Conversion

		public override double As(string units)
		{
			return UnitSystemService.Instance.Convert(Value(), Internal(), units);
		}

		#endregion

		#region Operators

		public static Voltage operator +(Voltage left, Voltage right)
		{
			var leftValue = left.As(left.Internal());
			var rightValue = right.As(left.Internal());
			return new Voltage(leftValue + rightValue, left.Internal());
		}

		public static Voltage operator -(Voltage left, Voltage right)
		{
			var leftValue = left.As(left.Internal());
			var rightValue = right.As(left.Internal());
			return new Voltage(leftValue - rightValue, left.Internal());
		}

		public static Voltage operator *(Voltage left, double scalar) => new(left.Value() * scalar, left.Internal());
		public static Voltage operator *(double scalar, Voltage right) => new(scalar * right.Value(), right.Internal());
		public static Voltage operator /(Voltage left, double scalar) => new(left.Value() / scalar, left.Internal());

		public static bool operator >(Voltage left, Voltage right) => left.As(left.Internal()) > right.As(left.Internal());
		public static bool operator <(Voltage left, Voltage right) => left.As(left.Internal()) < right.As(left.Internal());
		public static bool operator >=(Voltage left, Voltage right) => left.As(left.Internal()) >= right.As(left.Internal());
		public static bool operator <=(Voltage left, Voltage right) => left.As(left.Internal()) <= right.As(left.Internal());

		#endregion

		#region Legacy Compatibility

		[Obsolete("Use factory methods like FromVolts() for new code. This method is maintained for backward compatibility.")]
		public static Func<UnitCategory> Category = () => new UnitCategory("Voltage");

		#endregion
	}

	#region JSON Converter

	public class VoltageJsonConverter : JsonConverter<Voltage>
	{
		public override Voltage Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
			{
				var root = doc.RootElement;
				var value = root.GetProperty("value").GetDouble();
				var units = root.GetProperty("units").GetString();
				return new Voltage(value, units);
			}
		}

		public override void Write(Utf8JsonWriter writer, Voltage value, JsonSerializerOptions options)
		{
			writer.WriteStartObject();
			writer.WriteNumber("value", value.Value());
			writer.WriteString("units", value.Internal());
			writer.WriteEndObject();
		}
	}

	#endregion
}
