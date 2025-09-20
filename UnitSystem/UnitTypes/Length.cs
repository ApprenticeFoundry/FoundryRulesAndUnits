using FoundryRulesAndUnits.Extensions;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	public class Length : MeasuredValue
	{
		/// <summary>
		/// Gets the UnitFamily for Length measurements
		/// </summary>
		public override UnitFamilyName UnitFamily => UnitFamilyName.Length;

		/// <summary>
		/// Static property for factory methods to determine unit family without instantiation
		/// </summary>
		public static UnitFamilyName StaticUnitFamily => UnitFamilyName.Length;

		/// <summary>
		/// Backward compatibility constructor for JSON deserialization
		/// </summary>
		public Length(double value, string units) : base()
		{
			V = value;
			I = units;
			U = units;
		}

		/// <summary>
		/// Backward compatibility constructor - single value assumes base unit
		/// </summary>
		public Length(double value) : base()
		{
			V = value;
			I = "m"; // Default to meters
			U = "m";
		}		/// <summary>
		/// Constructor with UnitGroup injection - preferred for factory pattern
		/// </summary>
		public Length(UnitGroup unitGroup) : base(unitGroup)
		{
			if (unitGroup.Family != UnitFamilyName.Length)
				throw new ArgumentException($"UnitGroup family must be {UnitFamilyName.Length}", nameof(unitGroup));
		}

		public Length Assign(double value, string? units)
		{
			Init(value, units); // Base class handles everything!
			return this;
		}

		public Length Assign(Length source)
		{
			Init(source.Value(), source.U); // Base class handles everything!
			return this;
		}

		public Length Copy()
		{
			return new Length(Value(), Internal());
		}

		public static Length FromKilometers(double v)
		{
			return new Length(v, "km");
		}

		public static Length FromMeters(double v)
		{
			return new Length(v, "m");
		}

		public static Length FromMillimeters(double v)
		{
			return new Length(v, "mm");
		}

		public static Length FromInches(double v)
		{
			return new Length(v, "in");
		}

		public static Length FromFeet(double v)
		{
			return new Length(v, "ft");
		}

		// As() method inherited from MeasuredValue - no override needed!

		// Legacy compatibility methods
		public double AsPixels() => As("px"); // Convert to pixels using unit system
		
		public static bool operator <(Length left, Length right) => left.Value() < right.Value();
		public static bool operator >(Length left, Length right) => left.Value() > right.Value();

		public static Length operator +(Length left, Length right) => new(left.Value() + right.Value(), left.Internal());
		public static Length operator -(Length left, Length right) => new(left.Value() - right.Value(), left.Internal());
		public static Length operator *(double left, Length right) => new(left * right.Value(), right.Internal());
		public static Length operator /(Length left, double right) => new(left.Value() / right, left.Internal());

		public static double operator /(Length left, Length right) => left.Value() / right.Value();

		// Area and Volume operations (cross-unit calculations)
		public static Area operator *(Length left, Length right) => new(left.Value() * right.Value(), "m2");
		public static Volume operator *(Area left, Length right) => new(left.Value() * right.Value(), "m3");
		public static Volume operator *(Length left, Area right) => new(left.Value() * right.Value(), "m3");
	}

	public class LengthJsonConverter : JsonConverter<Length>
	{
		public override Length Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			return MeasuredValue.ReadJSON<Length>(ref reader, typeToConvert);
		}

		public override void Write(Utf8JsonWriter writer, Length dataValue, JsonSerializerOptions options)
		{
			//dataValue.V = 200;
		}
	}

}
