using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	[JsonConverter(typeof(FrequencyJsonConverter))]
	public class Frequency : MeasuredValue
	{
		#region Constructors and Factory Methods

		// UnitGroup injection constructor (preferred for new code)
		public Frequency(UnitGroup unitGroup) : base(unitGroup) 
		{ 
			if (unitGroup.Family != UnitFamilyName.Frequency)
				throw new ArgumentException($"Expected UnitGroup for Frequency, got {unitGroup.Family}");
		}

		public Frequency(UnitGroup unitGroup, double value, string? units = null) : base(unitGroup)
		{
			if (unitGroup.Family != UnitFamilyName.Frequency)
				throw new ArgumentException($"Expected UnitGroup for Frequency, got {unitGroup.Family}");
			Init(value, units);
		}

		/// <summary>


		/// Backward compatibility constructor for JSON deserialization


		/// </summary>


		public Frequency(double value, string units) : base()


		{


			V = value;


			I = units;


			U = units;


		}

		#endregion

		// Factory methods
		public static Frequency FromHertz(double value) => new(value, "Hz");
		public static Frequency FromKilohertz(double value) => new(value, "kHz");
		public static Frequency FromMegahertz(double value) => new(value, "MHz");
		public static Frequency FromGigahertz(double value) => new(value, "GHz");
		public static Frequency FromRPM(double value) => new(value, "rpm");

		// Arithmetic operators
		public static Frequency operator +(Frequency left, Frequency right) => new(left.Value() + right.Value(), left.Internal());
		public static Frequency operator -(Frequency left, Frequency right) => new(left.Value() - right.Value(), left.Internal());
		public static Frequency operator *(Frequency frequency, double scalar) => new(frequency.Value() * scalar, frequency.Internal());
		public static Frequency operator *(double scalar, Frequency frequency) => new(scalar * frequency.Value(), frequency.Internal());
		public static Frequency operator /(Frequency frequency, double scalar) => new(frequency.Value() / scalar, frequency.Internal());
		public static double operator /(Frequency left, Frequency right) => left.Value() / right.Value();
	}

	public class FrequencyJsonConverter : JsonConverter<Frequency>
	{
		public override Frequency Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			return MeasuredValue.ReadJSON<Frequency>(ref reader, typeToConvert);
		}

		public override void Write(Utf8JsonWriter writer, Frequency dataValue, JsonSerializerOptions options)
		{
			// Writing handled by base serialization
		}
	}
}
