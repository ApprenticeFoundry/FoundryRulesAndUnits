using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	[JsonConverter(typeof(DataStorageJsonConverter))]
	public class DataStorage : MeasuredValue
	{
		#region Constructors and Factory Methods

		// UnitGroup injection constructor (preferred for new code)
		public DataStorage(UnitGroup unitGroup) : base(unitGroup) 
		{ 
			if (unitGroup.Family != UnitFamilyName.DataStorage)
				throw new ArgumentException($"Expected UnitGroup for DataStorage, got {unitGroup.Family}");
		}

		public DataStorage(UnitGroup unitGroup, double value, string? units = null) : base(unitGroup)
		{
			if (unitGroup.Family != UnitFamilyName.DataStorage)
				throw new ArgumentException($"Expected UnitGroup for DataStorage, got {unitGroup.Family}");
			Init(value, units);
		}

		/// <summary>


		/// Backward compatibility constructor for JSON deserialization


		/// </summary>


		public DataStorage(double value, string units) : base()


		{


			V = value;


			I = units;


			U = units;


		}

		// Factory methods for common data storage units
		public static DataStorage FromBytes(double value) => new(value, "B");
		public static DataStorage FromKilobytes(double value) => new(value, "KB");
		public static DataStorage FromMegabytes(double value) => new(value, "MB");
		public static DataStorage FromGigabytes(double value) => new(value, "GB");
		public static DataStorage FromTerabytes(double value) => new(value, "TB");
		public static DataStorage FromPetabytes(double value) => new(value, "PB");
		public static DataStorage FromKibibytes(double value) => new(value, "KiB");
		public static DataStorage FromMebibytes(double value) => new(value, "MiB");
		public static DataStorage FromGibibytes(double value) => new(value, "GiB");
		public static DataStorage FromTebibytes(double value) => new(value, "TiB");

		#endregion

		#region Unit Conversion
		// As() method inherited from MeasuredValue with UnitGroup conversion

		#endregion

		#region Operators

		public static DataStorage operator +(DataStorage left, DataStorage right)
		{
			var leftValue = left.As(left.Internal());
			var rightValue = right.As(left.Internal());
			return new DataStorage(leftValue + rightValue, left.Internal());
		}

		public static DataStorage operator -(DataStorage left, DataStorage right)
		{
			var leftValue = left.As(left.Internal());
			var rightValue = right.As(left.Internal());
			return new DataStorage(leftValue - rightValue, left.Internal());
		}

		public static DataStorage operator *(DataStorage left, double scalar) => new(left.Value() * scalar, left.Internal());
		public static DataStorage operator *(double scalar, DataStorage right) => new(scalar * right.Value(), right.Internal());
		public static DataStorage operator /(DataStorage left, double scalar) => new(left.Value() / scalar, left.Internal());

		public static bool operator >(DataStorage left, DataStorage right) => left.As(left.Internal()) > right.As(left.Internal());
		public static bool operator <(DataStorage left, DataStorage right) => left.As(left.Internal()) < right.As(left.Internal());
		public static bool operator >=(DataStorage left, DataStorage right) => left.As(left.Internal()) >= right.As(left.Internal());
		public static bool operator <=(DataStorage left, DataStorage right) => left.As(left.Internal()) <= right.As(left.Internal());

		#endregion


	}

	#region JSON Converter

	public class DataStorageJsonConverter : JsonConverter<DataStorage>
	{
		public override DataStorage Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
			{
				var root = doc.RootElement;
				var value = root.GetProperty("value").GetDouble();
				var units = root.GetProperty("units").GetString();
				return new DataStorage(value, units);
			}
		}

		public override void Write(Utf8JsonWriter writer, DataStorage value, JsonSerializerOptions options)
		{
			writer.WriteStartObject();
			writer.WriteNumber("value", value.Value());
			writer.WriteString("units", value.Internal());
			writer.WriteEndObject();
		}
	}

	#endregion
}
