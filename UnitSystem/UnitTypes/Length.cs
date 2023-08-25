using FoundryRulesAndUnits.Extensions;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	public class Length : MeasuredValue
	{
		public static Func<UnitCategory> Category = () =>
		{
			return new UnitCategory("Length");
		};

		public Length() :
			base(UnitFamilyName.Length)
		{

		}

		public Length(double value, string? units=null) :
			base(UnitFamilyName.Length)
		{
			Init(Category(), value, units);
		}

		public Length Assign(double value, string? units=null)
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

		public Length Assign(Length source)
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

		public int AsPixels()
		{
			return (int)ConvertAs(Category(), "px");
		}

		public double FromPixels(int pixels)
		{
			return Value() * pixels / AsPixels();
		}

		public Length Sqrt()
		{
			this.V = Math.Sqrt(V);
			return this;
		}

		public Length Sq()
		{
			this.V = V * V;
			return this;
		}

		public Length Copy()
		{
			return new Length(Value(), Internal());
		}

		public double Diff(Length other)
		{
			return Value() - other.Value();
		}
		public double Diff(double other)
		{
			return Value() - other;
		}
		public double Sum(Length other)
		{
			return Value() + other.Value();
		}

		public double Sum(double other)
		{
			return Value() + other;
		}

		public static Length FromKilometers(double v)
		{
			return new Length(v, "km");
		}

		public static Length FromMeters(double v)
		{
			return new Length(v, "m");
		}

		public override double As(string units)
		{
			return ConvertAs(Category(), units);
		}

		public static Length operator +(Length left, double right) => new(left.Value() + right, left.Internal());
		public static Length operator -(Length left, double right) => new(left.Value() - right, left.Internal());

		public static Length operator +(Length left, Length right) => new(left.Value() + right.Value(), left.Internal());
		public static Length operator -(Length left, Length right) => new(left.Value() - right.Value(), left.Internal());

		public static bool operator <(Length left, Length right) => left.Value() < right.Value();
		public static bool operator >(Length left, Length right) => left.Value() > right.Value();

		public static Length operator *(double left, Length right) => new(left * right.Value(), right.Internal());
		public static Area operator *(Length left, Length right) => new(left.Value() * right.Value(), "m2");

		public static double operator /(Length left, Length right) => left.Value() /  right.Value();
		public static Length operator /(Length left, double right) => new(left.Value()/ right, left.Internal());

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
