using FoundryRulesAndUnits.Extensions;
using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FoundryRulesAndUnits.Units
{

	public interface IMeasuredValue
	{
		void SetValue(double value);
		void SetDisplayUnits(string units);
		string Debug();
		string AsString(string units);
		string Format(string format);
		string ToString();
	}


	[System.Serializable]
	public class MeasuredValue : IMeasuredValue
	{
		[JsonPropertyName("V")]
		public double V = 0.0;
		[JsonPropertyName("I")]
		public string I = "";      //internal storage units
		[JsonPropertyName("U")]
		protected string U = "";  //reporting  input and output units
		protected UnitFamilyName F = UnitFamilyName.None;



		public MeasuredValue(UnitFamilyName unitFamily)
		{
			F = unitFamily;
			V = default!;
		}


		public double Init(UnitCategory cat, double value, string? units)
		{
			I = cat.BaseUnits().Name();
			U = units ?? I;
			V = value;

			if (I != U)
				V = cat.ConvertToBaseUnits(U, value);
			return V;
		}

		public double ConvertAs(UnitCategory cat, string units)
		{
			return cat.ConvertFromBaseUnits(units, V);
		}

		public int ValueAsInt() { return (int)V; }
		public double Value() { return V; }
		public string Units() { return U; }
		public string Internal() { return I; }

		public void SetInternal(string units)
		{
			I = units;
		}

		public void SetValue(double value)
		{
			V = value;
		}


		public void SetDisplayUnits(string units)
		{
			U = units;
		}

		public virtual double As(string units)
		{
			return default!;
		}
		public override string ToString()
		{
			return AsString(Units());
		}

		public string Format(string format)
		{
			var value = As(Units());
			return $"{value.ToString(format, CultureInfo.CurrentCulture)} {Units()}";
		}

		public string AsString(string units)
		{
			return $"{As(units)} {units}";
		}

		public string Debug()
		{
			return $"{Value()}({Internal()}) {Units()}";
		}

		public static T ReadJSON<T>(ref Utf8JsonReader reader, Type typeToConvert) where T : MeasuredValue
		{
			double value = 0;
			string units = "";
			string internalUnits = "";

			// $"typeToConvert {typeToConvert} ".WriteLine();

			while (reader.Read())
			{
				if (reader.TokenType == JsonTokenType.EndObject)
				{
					var result = Activator.CreateInstance(typeToConvert, value, internalUnits) as T;
					if (!string.IsNullOrEmpty(units))
						result!.SetDisplayUnits(units);

					return result!;
				}

				var propertyName = reader!.GetString();

				reader.Read();

				switch (propertyName)
				{
					case "I":
					case "i":
						internalUnits = reader.GetString()!;
						break;
					case "U":
					case "u":
						units = reader.GetString()!;
						break;
					case "V":
					case "v":
						value = reader.GetDouble();
						break;
				}
			}
			var answer = Activator.CreateInstance(typeToConvert, value, internalUnits) as T;
			return answer!;
		}

	}


	public class MeasuredValueJsonConverter : JsonConverter<MeasuredValue>
	{
		public override MeasuredValue Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			return MeasuredValue.ReadJSON<MeasuredValue>(ref reader, typeToConvert);
		}

		public override void Write(Utf8JsonWriter writer, MeasuredValue dataValue, JsonSerializerOptions options)
		{
			//dataValue.V = 200;
		}
	}
}