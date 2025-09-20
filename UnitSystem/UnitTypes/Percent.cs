using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	[JsonConverter(typeof(PercentJsonConverter))]
	public class Percent : MeasuredValue
	{
		#region Constructors and Factory Methods

		// UnitGroup injection constructor (preferred for new code)
		public Percent(UnitGroup unitGroup) : base(unitGroup) 
		{ 
			if (unitGroup.Family != UnitFamilyName.Percent)
				throw new ArgumentException($"Expected UnitGroup for Percent, got {unitGroup.Family}");
		}

		public Percent(UnitGroup unitGroup, double value, string? units = null) : base(unitGroup)
		{
			if (unitGroup.Family != UnitFamilyName.Percent)
				throw new ArgumentException($"Expected UnitGroup for Percent, got {unitGroup.Family}");
			Init(value, units);
		}

		/// <summary>


		/// Backward compatibility constructor for JSON deserialization


		/// </summary>


		public Percent(double value, string units) : base()


		{


			V = value;


			I = units;


			U = units;


		}

		// Factory methods for common percentage representations
		public static Percent FromPercent(double value) => new(value, "%");
		public static Percent FromDecimal(double value) => new(value * 100, "%"); // Convert 0.75 -> 75%
		public static Percent FromRatio(double value) => new(value * 100, "%");   // Alias for FromDecimal
		public static Percent FromFraction(double numerator, double denominator) => new((numerator / denominator) * 100, "%");

		#endregion

		#region Unit Conversion
		// As() method inherited from MeasuredValue with UnitGroup conversion

		#endregion

		#region Operators

		public static Percent operator +(Percent left, Percent right)
		{
			var leftValue = left.As(left.Internal());
			var rightValue = right.As(left.Internal());
			return new Percent(leftValue + rightValue, left.Internal());
		}

		public static Percent operator -(Percent left, Percent right)
		{
			var leftValue = left.As(left.Internal());
			var rightValue = right.As(left.Internal());
			return new Percent(leftValue - rightValue, left.Internal());
		}

		public static Percent operator *(Percent left, double scalar) => new(left.Value() * scalar, left.Internal());
		public static Percent operator *(double scalar, Percent right) => new(scalar * right.Value(), right.Internal());
		public static Percent operator /(Percent left, double scalar) => new(left.Value() / scalar, left.Internal());

		public static bool operator >(Percent left, Percent right) => left.As(left.Internal()) > right.As(left.Internal());
		public static bool operator <(Percent left, Percent right) => left.As(left.Internal()) < right.As(left.Internal());
		public static bool operator >=(Percent left, Percent right) => left.As(left.Internal()) >= right.As(left.Internal());
		public static bool operator <=(Percent left, Percent right) => left.As(left.Internal()) <= right.As(left.Internal());

		#endregion


	}

	#region JSON Converter

	public class PercentJsonConverter : JsonConverter<Percent>
	{
		public override Percent Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
			{
				var root = doc.RootElement;
				var value = root.GetProperty("value").GetDouble();
				var units = root.GetProperty("units").GetString();
				return new Percent(value, units);
			}
		}

		public override void Write(Utf8JsonWriter writer, Percent value, JsonSerializerOptions options)
		{
			writer.WriteStartObject();
			writer.WriteNumber("value", value.Value());
			writer.WriteString("units", value.Internal());
			writer.WriteEndObject();
		}
	}

	#endregion
}
