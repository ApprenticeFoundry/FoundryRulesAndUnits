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
		/// <summary>
		/// Speed × Time → Length (Distance = Speed × Time)
		/// Example: 10 m/s × 5s = 50m
		/// </summary>
		public static Length operator *(Speed left, Time right)
		{
			var lengthValue = left.BaseValue() * right.BaseValue(); // (m/s) × s = m
			var length = left._unitGroup.CreateUnit<Length>();
			length.Init(lengthValue, length.Internal());
			return length;
		}
		
		/// <summary>
		/// Speed × Duration → Length (Distance = Speed × Time)
		/// Example: 10 m/s × 5s = 50m
		/// </summary>
		public static Length operator *(Speed left, Duration right)
		{
			var lengthValue = left.BaseValue() * right.BaseValue(); // (m/s) × s = m
			var length = left._unitGroup.CreateUnit<Length>();
			length.Init(lengthValue, length.Internal());
			return length;
		}
		
		/// <summary>
		/// Speed × Speed → Energy (Speed² for kinetic energy calculations)
		/// Example: (5 m/s) × (5 m/s) = 25 m²/s² (specific energy)
		/// This is used in kinetic energy: KE = ½ × Mass × Speed²
		/// </summary>
		public static Energy operator *(Speed left, Speed right)
		{
			var speedSquaredValue = left.BaseValue() * right.BaseValue(); // (m/s) × (m/s) = m²/s²
			// m²/s² has the same units as J/kg (specific energy)
			var energy = left._unitGroup.CreateUnit<Energy>();
			energy.Init(speedSquaredValue, energy.Internal());
			return energy;
		}
		
		/// <summary>
		/// Time × Speed → Length (commutative version)
		/// Example: 5s × 10 m/s = 50m
		/// </summary>
		public static Length operator *(Time time, Speed speed)
		{
			return speed * time; // Delegate to Speed × Time
		}
		
		/// <summary>
		/// Duration × Speed → Length (commutative version)
		/// Example: 5s × 10 m/s = 50m
		/// </summary>
		public static Length operator *(Duration duration, Speed speed)
		{
			return speed * duration; // Delegate to Speed × Duration
		}
		
		/// <summary>
		/// Speed × Mass → Momentum (commutative version of Mass × Speed)
		/// Example: 5 m/s × 10 kg = 50 kg⋅m/s (momentum)
		/// </summary>
		public static Momentum operator *(Speed speed, Mass mass)
		{
			return mass * speed; // Delegate to Mass × Speed
		}
		
		#endregion
	}
}
