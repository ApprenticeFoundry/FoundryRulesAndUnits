using FoundryRulesAndUnits.Extensions;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	public class Force : MeasuredValue
	{
		public static Func<UnitCategory> Category = () =>
		{
			return new UnitCategory("Force");
		};

		public Force() :
			base(UnitFamilyName.Force)
		{
		}

		public Force(double value, string? units = null) :
			base(UnitFamilyName.Force)
		{
			Init(Category(), value, units);
		}

		public Force Assign(double value, string? units)
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

		public Force Assign(Force source)
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

		public Force Copy()
		{
			return new Force(Value(), Internal());
		}

		public static Force FromNewtons(double v)
		{
			return new Force(v, "N");
		}

		public static Force FromKilonewtons(double v)
		{
			return new Force(v, "kN");
		}

		public static Force FromPoundForce(double v)
		{
			return new Force(v, "lbf");
		}

		public static Force FromDynes(double v)
		{
			return new Force(v, "dyne");
		}

		public override double As(string units)
		{
			return ConvertAs(Category(), units);
		}

		public Force Newtons(double value)
		{
			var cat = Category();
			var result = cat.ConvertToBaseUnits("N", value);
			if (result.success)
				V = result.value;
			else
				V = value;
			return this;
		}

		public static bool operator <(Force left, Force right) => left.Value() < right.Value();
		public static bool operator >(Force left, Force right) => left.Value() > right.Value();

		public static Force operator +(Force left, Force right) => new(left.Value() + right.Value(), left.Internal());
		public static Force operator -(Force left, Force right) => new(left.Value() - right.Value(), left.Internal());
	}

	public class ForceJsonConverter : JsonConverter<Force>
	{
		public override Force Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			return MeasuredValue.ReadJSON<Force>(ref reader, typeToConvert);
		}

		public override void Write(Utf8JsonWriter writer, Force dataValue, JsonSerializerOptions options)
		{
			//dataValue.V = 200;
		}
	}
}