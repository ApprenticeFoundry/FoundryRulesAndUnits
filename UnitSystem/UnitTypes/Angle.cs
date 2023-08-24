using FoundryRulesAndUnits.Extensions;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	public class Angle : MeasuredValue
	{
		public static Func<UnitCategory> Category = () =>
		{
			return new UnitCategory("Angle");
		};

		public Angle() :
			base(UnitFamilyName.Angle)
		{
		}


		public Angle(double value, string? units) :
			base(UnitFamilyName.Angle)
		{
			Init(Category(), value, units);
		}

		public Angle Assign(double value, string? units)
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

		public Angle Assign(Angle source)
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

		public Angle Copy()
		{
			return new Angle(Value(), Internal());
		}

		public static Angle FromDegrees(double v)
		{
			return new Angle(v, "D");
		}

		public static Angle FromRadians(double v)
		{
			return new Angle(v, "r");
		}

		public override double As(string units)
		{
			return ConvertAs(Category(), units);
		}

		public Angle Degrees(double value)
		{
			var cat = Category();
			V = cat.ConvertToBaseUnits("d", value);
			return this;
		}

		public static bool operator <(Angle left, Angle right) => left.Value() < right.Value();
		public static bool operator >(Angle left, Angle right) => left.Value() > right.Value();

		public static Angle operator +(Angle left, Angle right) => new(left.Value() + right.Value(), left.Internal());
		public static Angle operator -(Angle left, Angle right) => new(left.Value() - right.Value(), left.Internal());

	}

	public class AngleJsonConverter : JsonConverter<Angle>
	{
		public override Angle Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			return MeasuredValue.ReadJSON<Angle>(ref reader, typeToConvert);
		}

		public override void Write(Utf8JsonWriter writer, Angle dataValue, JsonSerializerOptions options)
		{
			//dataValue.V = 200;
		}
	}

}
