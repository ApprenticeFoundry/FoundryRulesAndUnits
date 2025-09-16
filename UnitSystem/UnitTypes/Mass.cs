using FoundryRulesAndUnits.Extensions;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	public class Mass : MeasuredValue
	{
		public static Func<UnitCategory> Category = () =>
		{
			return new UnitCategory("Mass");
		};

		public Mass() :
			base(UnitFamilyName.Mass)
		{
		}

		public Mass(double value, string? units = null) :
			base(UnitFamilyName.Mass)
		{
			Init(Category(), value, units);
		}

		public Mass Assign(double value, string? units)
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

		public Mass Assign(Mass source)
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

		public Mass Copy()
		{
			return new Mass(Value(), Internal());
		}

		public static Mass FromKilograms(double v)
		{
			return new Mass(v, "kg");
		}

		public static Mass FromGrams(double v)
		{
			return new Mass(v, "g");
		}

		public static Mass FromPounds(double v)
		{
			return new Mass(v, "lb");
		}

		public static Mass FromOunces(double v)
		{
			return new Mass(v, "oz");
		}

		public override double As(string units)
		{
			return ConvertAs(Category(), units);
		}

		public Mass Kilograms(double value)
		{
			var cat = Category();
			var result = cat.ConvertToBaseUnits("kg", value);
			if (result.success)
				V = result.value;
			else
				V = value;
			return this;
		}

		public static bool operator <(Mass left, Mass right) => left.Value() < right.Value();
		public static bool operator >(Mass left, Mass right) => left.Value() > right.Value();

		public static Mass operator +(Mass left, Mass right) => new(left.Value() + right.Value(), left.Internal());
		public static Mass operator -(Mass left, Mass right) => new(left.Value() - right.Value(), left.Internal());
	}

	public class MassJsonConverter : JsonConverter<Mass>
	{
		public override Mass Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			return MeasuredValue.ReadJSON<Mass>(ref reader, typeToConvert);
		}

		public override void Write(Utf8JsonWriter writer, Mass dataValue, JsonSerializerOptions options)
		{
			//dataValue.V = 200;
		}
	}
}