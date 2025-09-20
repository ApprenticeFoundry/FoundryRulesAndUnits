using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	[JsonConverter(typeof(CurrentJsonConverter))]
	public class Current : MeasuredValue
	{
		#region Constructors and Factory Methods

		// UnitGroup injection constructor (preferred for new code)
		public Current(UnitGroup unitGroup) : base(unitGroup) 
		{ 
			if (unitGroup.Family != UnitFamilyName.Current)
				throw new ArgumentException($"Expected UnitGroup for Current, got {unitGroup.Family}");
		}

		public Current(UnitGroup unitGroup, double value, string? units = null) : base(unitGroup)
		{
			if (unitGroup.Family != UnitFamilyName.Current)
				throw new ArgumentException($"Expected UnitGroup for Current, got {unitGroup.Family}");
			Init(value, units);
		}

		/// <summary>


		/// Backward compatibility constructor for JSON deserialization


		/// </summary>


		public Current(double value, string units) : base()


		{


			V = value;


			I = units;


			U = units;


		}

		// Factory methods for common current units
		public static Current FromAmperes(double value) => new(value, "A");
		public static Current FromAmps(double value) => new(value, "A"); // Alias for Amperes
		public static Current FromKiloamperes(double value) => new(value, "kA");
		public static Current FromMilliamperes(double value) => new(value, "mA");
		public static Current FromMicroamperes(double value) => new(value, "μA");
		public static Current FromNanoamperes(double value) => new(value, "nA");

		#endregion

		#region Unit Conversion
		// As() method inherited from MeasuredValue with UnitGroup conversion

		#endregion

		#region Operators

		public static Current operator +(Current left, Current right)
		{
			var leftValue = left.As(left.Internal());
			var rightValue = right.As(left.Internal());
			return new Current(leftValue + rightValue, left.Internal());
		}

		public static Current operator -(Current left, Current right)
		{
			var leftValue = left.As(left.Internal());
			var rightValue = right.As(left.Internal());
			return new Current(leftValue - rightValue, left.Internal());
		}

		public static Current operator *(Current left, double scalar) => new(left.Value() * scalar, left.Internal());
		public static Current operator *(double scalar, Current right) => new(scalar * right.Value(), right.Internal());
		public static Current operator /(Current left, double scalar) => new(left.Value() / scalar, left.Internal());

		public static bool operator >(Current left, Current right) => left.As(left.Internal()) > right.As(left.Internal());
		public static bool operator <(Current left, Current right) => left.As(left.Internal()) < right.As(left.Internal());
		public static bool operator >=(Current left, Current right) => left.As(left.Internal()) >= right.As(left.Internal());
		public static bool operator <=(Current left, Current right) => left.As(left.Internal()) <= right.As(left.Internal());

		#endregion


	}

	#region JSON Converter

	public class CurrentJsonConverter : JsonConverter<Current>
	{
		public override Current Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
			{
				var root = doc.RootElement;
				var value = root.GetProperty("value").GetDouble();
				var units = root.GetProperty("units").GetString();
				return new Current(value, units);
			}
		}

		public override void Write(Utf8JsonWriter writer, Current value, JsonSerializerOptions options)
		{
			writer.WriteStartObject();
			writer.WriteNumber("value", value.Value());
			writer.WriteString("units", value.Internal());
			writer.WriteEndObject();
		}
	}

	#endregion
}
