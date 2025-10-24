using System;
using System.Collections.Generic;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	[UnitType(UnitFamilyName.Speed, Description = "Speed/velocity measurement")]
	public class Speed : MeasuredValue
	{
		// UnitFamily comes from UnitTypeAttribute - no need for redundant property override
		#region Constructors and Factory Methods

		/// <summary>
		/// Initializes a new instance of the Speed class with the specified UnitGroup.
		/// </summary>
		/// <param name="unitGroup">The unit group to use for this Speed measurement.</param>
		public Speed(UnitGroup unitGroup) : base(unitGroup)
		{
			if (unitGroup.Family != UnitFamilyName.Speed)
				throw new ArgumentException($"Expected UnitGroup for Speed, got {unitGroup.Family}");
		}


		// Arithmetic operators
		public static Speed operator +(Speed left, Speed right) 
		{
			var result = new Speed(left.UnitGroup);
			result.Init(left.Value() + right.Value(), left.Internal());
			return result;
		}
		
		public static Speed operator -(Speed left, Speed right) 
		{
			var result = new Speed(left.UnitGroup);
			result.Init(left.Value() - right.Value(), left.Internal());
			return result;
		}
		
		public static Speed operator *(Speed speed, double scalar) 
		{
			var result = new Speed(speed.UnitGroup);
			result.Init(speed.Value() * scalar, speed.Internal());
			return result;
		}
		
		public static Speed operator *(double scalar, Speed speed) 
		{
			var result = new Speed(speed.UnitGroup);
			result.Init(scalar * speed.Value(), speed.Internal());
			return result;
		}
		
		public static Speed operator /(Speed speed, double scalar) 
		{
			var result = new Speed(speed.UnitGroup);
			result.Init(speed.Value() / scalar, speed.Internal());
			return result;
		}
		
		public static double operator /(Speed left, Speed right) => left.Value() / right.Value();
		
		// ============================================================================
		// PHASE 4A: CROSS-FAMILY OPERATIONS - Speed-specific operators
		// ============================================================================
		
		/// <summary>
		/// Speed × Time → Length (Distance = Speed × Time)
		/// Example: 10 m/s × 5s = 50m
		/// </summary>
		public static MeasuredValue operator *(Speed left, Time right)
		{
			var unitSystem = new UnitSystem(left._unitGroup.SystemType);
			var lengthValue = left.BaseValue() * right.BaseValue(); // (m/s) × s = m
			return unitSystem.CreateMeasuredValue(UnitFamilyName.Length, lengthValue, "m");
		}
		
		/// <summary>
		/// Speed × Duration → Length (Distance = Speed × Time)
		/// Example: 10 m/s × 5s = 50m
		/// </summary>
		public static MeasuredValue operator *(Speed left, Duration right)
		{
			var unitSystem = new UnitSystem(left._unitGroup.SystemType);
			var lengthValue = left.BaseValue() * right.BaseValue(); // (m/s) × s = m
			return unitSystem.CreateMeasuredValue(UnitFamilyName.Length, lengthValue, "m");
		}
		
		/// <summary>
		/// Speed × Speed → Energy/Mass (Speed² for kinetic energy calculations)
		/// Example: (5 m/s) × (5 m/s) = 25 m²/s² (specific energy)
		/// This is used in kinetic energy: KE = ½ × Mass × Speed²
		/// </summary>
		public static MeasuredValue operator *(Speed left, Speed right)
		{
			var unitSystem = new UnitSystem(left._unitGroup.SystemType);
			var speedSquaredValue = left.BaseValue() * right.BaseValue(); // (m/s) × (m/s) = m²/s²
			// m²/s² has the same units as J/kg (specific energy)
			// For now, return as Energy to enable Mass × Speed² → Energy
			return unitSystem.CreateMeasuredValue(UnitFamilyName.Energy, speedSquaredValue, "J/kg");
		}
		
		#endregion
	}
}
