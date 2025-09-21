using FoundryRulesAndUnits.Extensions;
using System;
using System.Diagnostics.CodeAnalysis;
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
		
		// Unit inspection methods
		double BaseValue();
		string BaseUnits();
		string DisplayUnits();
		string InternalRepresentation();
		string BaseUnitInfo();
		string DisplayInfo();
		string ConversionInfo();
		string DetailedDebug();
	}


	[JsonDerivedType(typeof(Angle))]
	[JsonDerivedType(typeof(Area))]
	[JsonDerivedType(typeof(Capacitance))]
	[JsonDerivedType(typeof(Current))]
	[JsonDerivedType(typeof(DataFlow))]
	[JsonDerivedType(typeof(DataStorage))]
	[JsonDerivedType(typeof(Dimensionless))]
	[JsonDerivedType(typeof(Distance))]
	[JsonDerivedType(typeof(Duration))]
	[JsonDerivedType(typeof(Force))]
	[JsonDerivedType(typeof(Frequency))]
	[JsonDerivedType(typeof(Heading))]
	[JsonDerivedType(typeof(Length))]
	[JsonDerivedType(typeof(Mass))]
	[JsonDerivedType(typeof(Percent))]
	[JsonDerivedType(typeof(Power))]
	[JsonDerivedType(typeof(Quantity))]
	[JsonDerivedType(typeof(QuantityFlow))]
	[JsonDerivedType(typeof(Resistance))]
	[JsonDerivedType(typeof(Speed))]
	[JsonDerivedType(typeof(Temperature))]
	[JsonDerivedType(typeof(Time))]
	[JsonDerivedType(typeof(Voltage))]
	[JsonDerivedType(typeof(Volume))]
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

		// UnitGroup injection - contains all unit conversion logic for this family
		protected UnitGroup _unitGroup;

		/// <summary>
		/// Gets the UnitFamily for this measurement type
		/// Override in derived classes to specify the correct family
		/// </summary>
		public virtual UnitFamilyName UnitFamily => UnitFamilyName.None;

		/// <summary>
		/// Constructor with UnitGroup injection - preferred for new code
		/// Use UnitFactory to create instances with proper UnitGroup injection
		/// </summary>
		public MeasuredValue(UnitGroup unitGroup)
		{
			_unitGroup = unitGroup ?? throw new ArgumentNullException(nameof(unitGroup));
			F = unitGroup.Family;
			V = default!;
			// Set internal units to base unit from injected UnitGroup
			I = unitGroup.BaseUnit.Symbol;
			U = I; // Default display units to base units
		}

		/// <summary>
		/// Parameterless constructor for JSON deserialization only
		/// WARNING: UnitGroup will be null - conversion methods will have limited functionality
		/// </summary>
		protected MeasuredValue()
		{
			_unitGroup = null!; // Will be null for JSON deserialization
			F = UnitFamilyName.None;
			V = default!;
			I = "";
			U = "";
		}
			/// <summary>
		/// Initialize with value and units using injected UnitGroup
		/// </summary>
		public double Init(double value, string? units = null)
		{
			// Use injected UnitGroup - all family conversions handled here
			units = units ?? _unitGroup.BaseUnit.Symbol;

			// Validate unit belongs to this family
			if (!_unitGroup.IsValidUnit(units))
				throw new ArgumentException($"{units} is not a valid unit for {F}");

			U = units;
			I = _unitGroup.BaseUnit.Symbol;

			// Convert to base units for internal storage using UnitGroup
			if (I != U)
			{
				V = _unitGroup.Convert(value, U, I);
			}
			else
			{
				V = value;
			}
			return V;
		}

		/// <summary>
		/// Convert current value to specified units using injected UnitGroup
		/// </summary>
		public virtual double As(string units)
		{
			if (_unitGroup != null)
			{
				// Use injected UnitGroup - all family conversions handled here
				return _unitGroup.Convert(V, I, units);
			}
			else
			{
				// Enhanced fallback for common conversions when UnitGroup not available
				if (units == I) return V;
				
				// Try to create a temporary unit system for conversion
				try
				{
					var unitSystem = new UnitSystem();
					return unitSystem.Convert(V, I, units);
				}
				catch
				{
					// If all else fails, throw the original error
					throw new InvalidOperationException($"Unit conversion requires UnitGroup injection. Cannot convert from {I} to {units}");
				}
			}
		}

		public int ValueAsInt() { return (int)V; }
		public double Value() { return V; }
		public string Units() { return U; }
		public string Internal() { return I; }

		/// <summary>
		/// Get the base unit value (V field) - the numeric value stored in base units
		/// </summary>
		/// <returns>The numeric value in base units for mental math calculations</returns>
		public double BaseValue() { return V; }

		/// <summary>
		/// Get the base unit symbol (I field) - the unit symbol for internal storage
		/// </summary>
		/// <returns>The base unit symbol (e.g., 'm', 'km', 'ft', 'in')</returns>
		public string BaseUnits() { return I; }

		/// <summary>
		/// Get the display unit symbol (U field) - the unit symbol for user display
		/// </summary>
		/// <returns>The display unit symbol as originally entered</returns>
		public string DisplayUnits() { return U; }

		/// <summary>
		/// Get a detailed internal representation showing all V/I/U values
		/// </summary>
		/// <returns>Formatted string showing Value, Internal units, and User units</returns>
		public string InternalRepresentation()
		{
			return $"V={V:G}, I='{I}', U='{U}'";
		}

		/// <summary>
		/// Get base unit information for mental math - shows value and base unit clearly
		/// </summary>
		/// <returns>Human-readable string for doing mental calculations</returns>
		public string BaseUnitInfo()
		{
			return $"{V:G} {I}";
		}

		/// <summary>
		/// Get display information - shows value and display unit clearly
		/// </summary>
		/// <returns>Human-readable string as entered by user</returns>
		public string DisplayInfo()
		{
			return $"{As(U):G} {U}";
		}

		/// <summary>
		/// Get conversion information showing the relationship between display and base units
		/// </summary>
		/// <returns>Conversion details for understanding unit transformations</returns>
		public string ConversionInfo()
		{
			if (U == I)
				return $"{As(U):G} {U} (no conversion needed)";
			
			return $"{As(U):G} {U} = {V:G} {I} (base units)";
		}

		/// <summary>
		/// Get comprehensive debug information including unit family
		/// </summary>
		/// <returns>Complete internal state for debugging</returns>
		public string DetailedDebug()
		{
			return $"Display: {As(U):G} {U} | Base: {V:G} {I} | Family: {GetUnitFamily()} | V/I/U: {V:G}/'{I}'/'{U}'";
		}

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

		// public virtual double As(string units)
		// {
		// 	return default!;
		// }

		/// <summary>
		/// Public property to access the injected UnitGroup
		/// </summary>
		public UnitGroup UnitGroup
		{
			get
			{
				return _unitGroup ?? throw new InvalidOperationException("MeasuredValue must have an injected UnitGroup. Use UnitFactory to create instances.");
			}
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

		[SuppressMessage("Trimming", "IL2070:DynamicallyAccessedMembers", Justification = "Legacy JSON deserialization requires backward compatibility constructors")]
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
		[SuppressMessage("Trimming", "IL2070:DynamicallyAccessedMembers", Justification = "Legacy JSON deserialization")]
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