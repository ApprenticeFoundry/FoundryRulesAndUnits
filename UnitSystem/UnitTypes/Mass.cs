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
		public Mass() : base(UnitFamilyName.Mass)
		{
		}

		public Mass(double value, string? units = null) : base(UnitFamilyName.Mass)
		{
			Init(value, units); // Base class handles everything!
		}

		public Mass Assign(double value, string? units)
		{
			Init(value, units); // Base class handles everything!
			return this;
		}

		public Mass Assign(Mass source)
		{
			Init(source.Value(), source.U); // Base class handles everything!
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

		public static Mass FromTons(double v)
		{
			return new Mass(v, "t");
		}

		// As() method inherited from MeasuredValue - no override needed!

		public static bool operator <(Mass left, Mass right) => left.Value() < right.Value();
		public static bool operator >(Mass left, Mass right) => left.Value() > right.Value();

		public static Mass operator +(Mass left, Mass right) => new(left.Value() + right.Value(), left.Internal());
		public static Mass operator -(Mass left, Mass right) => new(left.Value() - right.Value(), left.Internal());
		public static Mass operator *(double left, Mass right) => new(left * right.Value(), right.Internal());
		public static Mass operator /(Mass left, double right) => new(left.Value() / right, left.Internal());

		public static double operator /(Mass left, Mass right) => left.Value() / right.Value();
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