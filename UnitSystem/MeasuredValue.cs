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


    [JsonDerivedType(typeof(Temperature))]
	[JsonDerivedType(typeof(Length))]
	[JsonDerivedType(typeof(Angle))]
	[JsonDerivedType(typeof(Area))]
	[JsonDerivedType(typeof(Volume))]
	[JsonDerivedType(typeof(Current))]
	[JsonDerivedType(typeof(Time))]
	[JsonDerivedType(typeof(Speed))]
	[JsonDerivedType(typeof(DataFlow))]
	[JsonDerivedType(typeof(DataStorage))]
	[JsonDerivedType(typeof(Frequency))]
	[JsonDerivedType(typeof(Percent))]
	[JsonDerivedType(typeof(Duration))]
	[JsonDerivedType(typeof(Distance))]
	[System.Serializable]
	public class MeasuredValue : IMeasuredValue
	{
		[JsonPropertyName("V")]
		public double V = 0.0;
		[JsonPropertyName("I")]
		public string I = "";      //internal storage units
		[JsonIgnore]
		public string U = "";  //reporting  input and output units
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
			{
				var (success, result) = cat.ConvertToBaseUnits(U, value);
				if (success)
					V = result;
			}
			else
			{
				V = cat.ConvertToBaseUnits(U, value).value;
			}
			return V;
		}

		public double ConvertAs(UnitCategory cat, string units)
		{
			var result = cat.ConvertFromBaseUnits(units, V);
			return result.value;
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

		/// <summary>
		/// Checks if this MeasuredValue is compatible with another for mathematical operations
		/// </summary>
		/// <param name="other">The other MeasuredValue to check compatibility with</param>
		/// <returns>True if the units are compatible (same family)</returns>
		public bool IsCompatibleWith(MeasuredValue other)
		{
			// Check if they're the same unit family (both Angle, both Length, etc.)
			return other != null && this.GetType() == other.GetType();
		}

		/// <summary>
		/// Gets the unit family name for this measured value
		/// </summary>
		/// <returns>The unit family (Angle, Length, Mass, etc.)</returns>
		public string GetUnitFamily()
		{
			return this.GetType().Name;
		}

		/// <summary>
		/// Validates if a mathematical operation with another MeasuredValue is valid
		/// </summary>
		/// <param name="operation">The operation (+, -, *, /)</param>
		/// <param name="other">The other MeasuredValue</param>
		/// <returns>Validation result with success/failure and explanatory message</returns>
		public UnitOperationResult ValidateOperation(string operation, MeasuredValue other)
		{
			if (other == null)
			{
				return UnitOperationResult.Invalid("Cannot perform operation with null value");
			}

			var thisType = this.GetType();
			var otherType = other.GetType();

			return operation switch
			{
				"+" or "-" when thisType == otherType => 
					UnitOperationResult.Valid($"Can {operation} same unit types: {thisType.Name}", thisType),
				
				"+" or "-" when thisType != otherType => 
					UnitOperationResult.Invalid($"Cannot {operation} different unit types: {thisType.Name} {operation} {otherType.Name}"),
				
				"*" when thisType == typeof(Length) && otherType == typeof(Length) => 
					UnitOperationResult.Valid("Length * Length = Area", typeof(Area)),
				
				"/" when thisType == typeof(Length) && otherType == typeof(Time) => 
					UnitOperationResult.Valid("Length / Time = Speed", typeof(Speed)),
				
				"/" when thisType == otherType => 
					UnitOperationResult.Valid($"{thisType.Name} / {otherType.Name} = dimensionless ratio", typeof(Dimensionless)),
				
				"*" or "/" => 
					UnitOperationResult.Warning($"Multiplication/division of {thisType.Name} * {otherType.Name} - result type unclear"),
				
				_ => UnitOperationResult.Invalid($"Unknown operation: {operation}")
			};
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

	/// <summary>
	/// Result of unit operation validation
	/// </summary>
	public class UnitOperationResult
	{
		public bool IsValid { get; set; }
		public bool IsWarning { get; set; }
		public string Message { get; set; }
		public Type? ResultType { get; set; }

		public static UnitOperationResult Valid(string message, Type? resultType = null) =>
			new() { IsValid = true, Message = message, ResultType = resultType };

		public static UnitOperationResult Invalid(string message) =>
			new() { IsValid = false, Message = message };

		public static UnitOperationResult Warning(string message) =>
			new() { IsValid = true, IsWarning = true, Message = message };
	}
}