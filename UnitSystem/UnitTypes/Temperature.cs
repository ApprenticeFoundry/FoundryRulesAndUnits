using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using FoundryRulesAndUnits.Extensions;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	public class Temperature : MeasuredValue
	{

		public static Func<UnitCategory> Category = () =>
		{
			return new UnitCategory("Temperature");
		};

		public Temperature() :
			base(UnitFamilyName.Temperature)
		{
			//$"Temperature constructor".WriteInfo();
		}

		public Temperature(double value, string? units = null) :
			base(UnitFamilyName.Temperature)
		{
			Init(Category(), value, units);
		}

		public Temperature Assign(double value, string? units=null)
		{
			if (units == I || units == null)
			{
				V = value;
			}
			else
			{
				Init(Category(), value, units);
			}
			return this;
		}

		public Temperature Assign(Temperature source)
		{
			if (source.I == I)
			{
				V = source.Value();
			}
			else
			{
				Init(Category(), source.Value(), source.U);
			}
			return this;
		}
		public override double As(string units)
		{
			return ConvertAs(Category(), units);
		}

		public static Temperature operator +(Temperature left, Temperature right) => new(left.Value() + right.Value(), left.Internal());
		public static Temperature operator -(Temperature left, Temperature right) => new(left.Value() - right.Value(), left.Internal());
	
	
		public class TemperatureJsonConverter : JsonConverter<Temperature>
		{
			public override Temperature Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			{
				return MeasuredValue.ReadJSON<Temperature>(ref reader, typeToConvert);
			}

			public override void Write(Utf8JsonWriter writer, Temperature dataValue, JsonSerializerOptions options)
			{
				//dataValue.V = 200;
			}
		}

	}
}
