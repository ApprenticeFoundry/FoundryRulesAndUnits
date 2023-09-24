using FoundryRulesAndUnits.Extensions;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	public class Heading : MeasuredValue
	{
		public static Func<UnitCategory> Category = () =>
		{
			return new UnitCategory("Heading");
		};

		public Heading() :
			base(UnitFamilyName.Heading)
		{
		}


		public Heading(double value, string? units=null) :
			base(UnitFamilyName.Heading)
		{
			Init(Category(), value, units);
		}

		public Heading Assign(double value, string? units)
		{
			if (units == I)
			{
				V = value;
			}
			else
			{
				Init(Category(), value, units);
			}
			return this;
		}

		public Heading Assign(Heading source)
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

		public Heading Copy()
		{
			return new Heading(Value(), Internal());
		}

		public static Heading FromDegrees(double v)
		{
			return new Heading(v, "deg");
		}

		public static Heading FromRadians(double v)
		{
			return new Heading(v, "rad");
		}

		public override double As(string units)
		{
			return ConvertAs(Category(), units);
		}

		public Heading Degrees(double value)
		{
			var cat = Category();
			V = cat.ConvertToBaseUnits("deg", value);
			return this;
		}

		public static bool operator <(Heading left, Heading right) => left.Value() < right.Value();
		public static bool operator >(Heading left, Heading right) => left.Value() > right.Value();

		public static Heading operator +(Heading left, Heading right) => new(left.Value() + right.Value(), left.Internal());
		public static Heading operator -(Heading left, Heading right) => new(left.Value() - right.Value(), left.Internal());

	}

	public class HeadingJsonConverter : JsonConverter<Heading>
	{
		public override Heading Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			return MeasuredValue.ReadJSON<Heading>(ref reader, typeToConvert);
		}

		public override void Write(Utf8JsonWriter writer, Heading dataValue, JsonSerializerOptions options)
		{
			//dataValue.V = 200;
		}
	}

}
