using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	[JsonConverter(typeof(DistanceJsonConverter))]
	public class Distance : MeasuredValue
	{
		#region Constructors and Factory Methods

		// UnitGroup injection constructor (preferred for new code)
		public Distance(UnitGroup unitGroup) : base(unitGroup) 
		{ 
			if (unitGroup.Family != UnitFamilyName.Length)
				throw new ArgumentException($"Expected UnitGroup for Length, got {unitGroup.Family}");
		}

		public Distance(UnitGroup unitGroup, double value, string? units = null) : base(unitGroup)
		{
			if (unitGroup.Family != UnitFamilyName.Length)
				throw new ArgumentException($"Expected UnitGroup for Length, got {unitGroup.Family}");
			Init(value, units);
		}

		/// <summary>


		/// Backward compatibility constructor for JSON deserialization


		/// </summary>


		public Distance(double value, string units) : base()


		{


			V = value;


			I = units;


			U = units;


		}

		#endregion

		// Factory methods
		public static Distance FromMeters(double value) => new(value, "m");
		public static Distance FromKilometers(double value) => new(value, "km");
		public static Distance FromMiles(double value) => new(value, "mi");
		public static Distance FromFeet(double value) => new(value, "ft");
		public static Distance FromInches(double value) => new(value, "in");

		// Utility methods
		public double Diff(Length other) => Value() - other.Value();
		public double Diff(double other) => Value() - other;
		public double Sum(Length other) => Value() + other.Value();
		public double Sum(double other) => Value() + other;

		// Arithmetic operators
		public static Distance operator +(Distance left, Distance right) => new(left.Value() + right.Value(), left.Internal());
		public static Distance operator -(Distance left, Distance right) => new(left.Value() - right.Value(), left.Internal());
		public static Distance operator +(Distance left, double right) => new(left.Value() + right, left.Internal());
		public static Distance operator -(Distance left, double right) => new(left.Value() - right, left.Internal());
		public static Distance operator *(Distance distance, double scalar) => new(distance.Value() * scalar, distance.Internal());
		public static Distance operator *(double scalar, Distance distance) => new(scalar * distance.Value(), distance.Internal());
		public static Distance operator /(Distance distance, double scalar) => new(distance.Value() / scalar, distance.Internal());
		public static double operator /(Distance left, Distance right) => left.Value() / right.Value();
	}

	public class DistanceJsonConverter : JsonConverter<Distance>
	{
		public override Distance Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			return MeasuredValue.ReadJSON<Distance>(ref reader, typeToConvert);
		}

		public override void Write(Utf8JsonWriter writer, Distance dataValue, JsonSerializerOptions options)
		{
			// Writing handled by base serialization
		}
	}
}
