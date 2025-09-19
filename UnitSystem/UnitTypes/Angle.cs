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
		public Angle() : base(UnitFamilyName.Angle)
		{
		}

		public Angle(double value, string? units = null) : base(UnitFamilyName.Angle)
		{
			Init(value, units); // Base class handles everything!
		}

		public Angle Assign(double value, string? units)
		{
			Init(value, units); // Base class handles everything!
			return this;
		}

		public Angle Assign(Angle source)
		{
			Init(source.Value(), source.U); // Base class handles everything!
			return this;
		}

		public Angle Copy()
		{
			return new Angle(Value(), Internal());
		}

		public static Angle FromDegrees(double v)
		{
			return new Angle(v, "deg");
		}

		public static Angle FromRadians(double v)
		{
			return new Angle(v, "rad");
		}

		// As() method inherited from MeasuredValue - no override needed!

		public Angle Degrees(double value)
		{
			Init(value, "deg"); // Base class handles validation and conversion!
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
