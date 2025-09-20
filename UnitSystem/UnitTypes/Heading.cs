using FoundryRulesAndUnits.Extensions;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	[JsonConverter(typeof(HeadingJsonConverter))]
	public class Heading : MeasuredValue
	{
		#region Constructors and Factory Methods

		public Heading() : base(UnitFamilyName.Heading) { }

		public Heading(double value, string? units = null) : base(UnitFamilyName.Heading)
		{
			Init(value, units);
		}

		// Factory methods for common heading units
		public static Heading FromDegrees(double value) => new(value, "deg");
		public static Heading FromRadians(double value) => new(value, "rad");
		public static Heading FromGradians(double value) => new(value, "grad");

		#endregion

		#region Unit Conversion

		public override double As(string units)
		{
			return UnitSystemService.Instance.Convert(Value(), Internal(), units);
		}

		#endregion

		#region Legacy Methods (Maintained for Compatibility)

		public Heading Assign(double value, string? units)
		{
			if (units == I)
			{
				V = value;
			}
			else
			{
				Init(value, units);
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
				Init(source.Value(), source.U);
			}
			return this;
		}

		public Heading Copy()
		{
			return new Heading(Value(), Internal());
		}

		public Heading Degrees(double value)
		{
			V = UnitSystemService.Instance.Convert(value, "deg", Internal());
			return this;
		}

		#endregion

		#region Operators

		public static Heading operator +(Heading left, Heading right)
		{
			var leftValue = left.As(left.Internal());
			var rightValue = right.As(left.Internal());
			return new Heading(leftValue + rightValue, left.Internal());
		}

		public static Heading operator -(Heading left, Heading right)
		{
			var leftValue = left.As(left.Internal());
			var rightValue = right.As(left.Internal());
			return new Heading(leftValue - rightValue, left.Internal());
		}

		public static Heading operator *(Heading left, double scalar) => new(left.Value() * scalar, left.Internal());
		public static Heading operator *(double scalar, Heading right) => new(scalar * right.Value(), right.Internal());
		public static Heading operator /(Heading left, double scalar) => new(left.Value() / scalar, left.Internal());

		public static bool operator >(Heading left, Heading right) => left.As(left.Internal()) > right.As(left.Internal());
		public static bool operator <(Heading left, Heading right) => left.As(left.Internal()) < right.As(left.Internal());
		public static bool operator >=(Heading left, Heading right) => left.As(left.Internal()) >= right.As(left.Internal());
		public static bool operator <=(Heading left, Heading right) => left.As(left.Internal()) <= right.As(left.Internal());

		#endregion

		#region Legacy Compatibility

		[Obsolete("Use factory methods like FromDegrees() for new code. This method is maintained for backward compatibility.")]
		public static Func<UnitCategory> Category = () => new UnitCategory("Heading");

		#endregion
	}

	#region JSON Converter

	public class HeadingJsonConverter : JsonConverter<Heading>
	{
		public override Heading Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
			{
				var root = doc.RootElement;
				var value = root.GetProperty("value").GetDouble();
				var units = root.GetProperty("units").GetString();
				return new Heading(value, units);
			}
		}

		public override void Write(Utf8JsonWriter writer, Heading value, JsonSerializerOptions options)
		{
			writer.WriteStartObject();
			writer.WriteNumber("value", value.Value());
			writer.WriteString("units", value.Internal());
			writer.WriteEndObject();
		}
	}

	#endregion
}
