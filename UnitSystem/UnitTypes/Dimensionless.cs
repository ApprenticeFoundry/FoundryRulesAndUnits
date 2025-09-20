using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	[JsonConverter(typeof(DimensionlessJsonConverter))]
	public class Dimensionless : MeasuredValue
	{
		#region Constructors and Factory Methods

		// UnitGroup injection constructor (preferred for new code)
		public Dimensionless(UnitGroup unitGroup) : base(unitGroup) 
		{ 
			if (unitGroup.Family != UnitFamilyName.None)
				throw new ArgumentException($"Expected UnitGroup for None (Dimensionless), got {unitGroup.Family}");
		}

		public Dimensionless(UnitGroup unitGroup, double value, string? units = null) : base(unitGroup)
		{
			if (unitGroup.Family != UnitFamilyName.None)
				throw new ArgumentException($"Expected UnitGroup for None (Dimensionless), got {unitGroup.Family}");
			Init(value, units ?? "dimensionless");
		}

		/// <summary>
		/// Backward compatibility constructor for JSON deserialization
		/// </summary>
		public Dimensionless(double value, string units) : base()
		{
			V = value;
			I = units;
			U = units;
		}

		// Factory methods for common dimensionless values
		public static Dimensionless FromValue(double value) => new(value, "dimensionless");
		public static Dimensionless FromRatio(double value) => new(value, "ratio");
		public static Dimensionless FromFactor(double value) => new(value, "factor");
		public static Dimensionless FromScalar(double value) => new(value, "scalar");
		public static Dimensionless FromCount(double value) => new(value, "count");
		public static Dimensionless FromEach(double value) => new(value, "each");
		public static Dimensionless FromUnity(double value) => new(value, "1");

		#endregion

		#region Unit Conversion

		// As() method inherited from MeasuredValue with UnitGroup conversion

		#endregion

		#region Operators

		public static Dimensionless operator +(Dimensionless left, Dimensionless right)
		{
			var leftValue = left.As(left.Internal());
			var rightValue = right.As(left.Internal());
			return new Dimensionless(leftValue + rightValue, left.Internal());
		}

		public static Dimensionless operator -(Dimensionless left, Dimensionless right)
		{
			var leftValue = left.As(left.Internal());
			var rightValue = right.As(left.Internal());
			return new Dimensionless(leftValue - rightValue, left.Internal());
		}

		public static Dimensionless operator *(Dimensionless left, Dimensionless right)
		{
			return new Dimensionless(left.Value() * right.Value(), left.Internal());
		}

		public static Dimensionless operator /(Dimensionless left, Dimensionless right)
		{
			return new Dimensionless(left.Value() / right.Value(), left.Internal());
		}

		public static Dimensionless operator *(Dimensionless left, double scalar) => new(left.Value() * scalar, left.Internal());
		public static Dimensionless operator *(double scalar, Dimensionless right) => new(scalar * right.Value(), right.Internal());
		public static Dimensionless operator /(Dimensionless left, double scalar) => new(left.Value() / scalar, left.Internal());

		public static bool operator >(Dimensionless left, Dimensionless right) => left.As(left.Internal()) > right.As(left.Internal());
		public static bool operator <(Dimensionless left, Dimensionless right) => left.As(left.Internal()) < right.As(left.Internal());
		public static bool operator >=(Dimensionless left, Dimensionless right) => left.As(left.Internal()) >= right.As(left.Internal());
		public static bool operator <=(Dimensionless left, Dimensionless right) => left.As(left.Internal()) <= right.As(left.Internal());

		#endregion


	}

	#region JSON Converter

	public class DimensionlessJsonConverter : JsonConverter<Dimensionless>
	{
		public override Dimensionless Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
			{
				var root = doc.RootElement;
				var value = root.GetProperty("value").GetDouble();
				var units = root.GetProperty("units").GetString();
				return new Dimensionless(value, units);
			}
		}

		public override void Write(Utf8JsonWriter writer, Dimensionless value, JsonSerializerOptions options)
		{
			writer.WriteStartObject();
			writer.WriteNumber("value", value.Value());
			writer.WriteString("units", value.Internal());
			writer.WriteEndObject();
		}
	}

	#endregion
}