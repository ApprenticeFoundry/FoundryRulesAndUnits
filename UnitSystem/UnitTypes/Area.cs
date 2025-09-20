using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	[JsonConverter(typeof(AreaJsonConverter))]
	public class Area : MeasuredValue
	{
		/// <summary>
		/// Gets the UnitFamily for Area measurements
		/// </summary>
		public override UnitFamilyName UnitFamily => UnitFamilyName.Area;

		/// <summary>
		/// Backward compatibility constructor for JSON deserialization
		/// </summary>

		public Area(double value, string units) : base()

		{

			V = value;

			I = units;

			U = units;

		}

		/// <summary>
		/// Constructor with UnitGroup injection - preferred
		/// </summary>
		public Area(UnitGroup unitGroup) : base(unitGroup)
		{
			if (unitGroup.Family != UnitFamilyName.Area)
				throw new ArgumentException($"UnitGroup family must be {UnitFamilyName.Area}", nameof(unitGroup));
		}

		// Static factory methods removed - use UnitFactory instead

		// Arithmetic operators
		public static Area operator +(Area left, Area right) 
		{
			var result = new Area(left.UnitGroup);
			result.Init(left.Value() + right.Value(), left.Internal());
			return result;
		}
		
		public static Area operator -(Area left, Area right) 
		{
			var result = new Area(left.UnitGroup);
			result.Init(left.Value() - right.Value(), left.Internal());
			return result;
		}
		
		public static Area operator *(Area area, double scalar) 
		{
			var result = new Area(area.UnitGroup);
			result.Init(area.Value() * scalar, area.Internal());
			return result;
		}
		
		public static Area operator *(double scalar, Area area) 
		{
			var result = new Area(area.UnitGroup);
			result.Init(scalar * area.Value(), area.Internal());
			return result;
		}
		
		public static Area operator /(Area area, double scalar) 
		{
			var result = new Area(area.UnitGroup);
			result.Init(area.Value() / scalar, area.Internal());
			return result;
		}
		
		public static double operator /(Area left, Area right) => left.Value() / right.Value();
	}

	public class AreaJsonConverter : JsonConverter<Area>
	{
		public override Area Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			return MeasuredValue.ReadJSON<Area>(ref reader, typeToConvert);
		}

		public override void Write(Utf8JsonWriter writer, Area dataValue, JsonSerializerOptions options)
		{
			// Writing handled by base serialization
		}
	}
}