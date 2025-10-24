using FoundryRulesAndUnits.Extensions;
using System;
using System.Collections.Generic;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	[UnitType(UnitFamilyName.Mass, Description = "Mass measurement")]
	public class Mass : MeasuredValue
	{
		// UnitFamily comes from UnitTypeAttribute - no need for redundant property override


		/// <summary>
		/// Constructor with UnitGroup injection - preferred
		/// </summary>
		public Mass(UnitGroup unitGroup) : base(unitGroup)
		{
			if (unitGroup.Family != UnitFamilyName.Mass)
				throw new ArgumentException($"UnitGroup family must be {UnitFamilyName.Mass}", nameof(unitGroup));
		}



		public Mass Assign(Mass source)
		{
			Init(source.Value(), source.U); // Base class handles everything!
			return this;
		}

		public Mass Copy()
		{
			var copy = new Mass(UnitGroup);
			copy.Init(Value(), Internal());
			return copy;
		}

		// Static factory methods removed - use UnitSystem instead

		// As() method inherited from MeasuredValue - no override needed!

		public static bool operator <(Mass left, Mass right) => left.Value() < right.Value();
		public static bool operator >(Mass left, Mass right) => left.Value() > right.Value();
		public static bool operator ==(Mass left, Mass right) => Math.Abs(left.Value() - right.Value()) < 1e-10;
		public static bool operator !=(Mass left, Mass right) => !(left == right);
		
		// Override Equals and GetHashCode to be consistent with == operator
		public override bool Equals(object? obj) => obj is Mass other && this == other;
		public override int GetHashCode() => Value().GetHashCode();

		public static Mass operator +(Mass left, Mass right) 
		{
			var result = new Mass(left.UnitGroup);
			result.Init(left.Value() + right.Value(), left.Internal());
			return result;
		}
		
		public static Mass operator -(Mass left, Mass right) 
		{
			var result = new Mass(left.UnitGroup);
			result.Init(left.Value() - right.Value(), left.Internal());
			return result;
		}
		
		public static Mass operator *(double left, Mass right) 
		{
			var result = new Mass(right.UnitGroup);
			result.Init(left * right.Value(), right.Internal());
			return result;
		}

		public static Mass operator *(Mass left, double right)
		{
			var result = new Mass(left.UnitGroup);
			result.Init(left.Value() * right, left.Internal());
			return result;
		}
		
		public static Mass operator /(Mass left, double right) 
		{
			var result = new Mass(left.UnitGroup);
			result.Init(left.Value() / right, left.Internal());
			return result;
		}

		public static double operator /(Mass left, Mass right) => left.Value() / right.Value();
		
		// ============================================================================
		// PHASE 4B: CROSS-FAMILY OPERATIONS - Physics calculations
		// ============================================================================
		
		/// <summary>
		/// Mass × Energy/Mass (specific energy) → Energy  
		/// This handles kinetic energy: Mass × Speed² → Energy
		/// Example: 10 kg × 25 J/kg = 250 J
		/// </summary>
		public static MeasuredValue operator *(Mass left, MeasuredValue right)
		{
			// Check if right operand is Energy family (from Speed²)
			if (right.GetType().GetProperty("UnitFamily")?.GetValue(right)?.ToString() == "Energy")
			{
				var unitSystem = new UnitSystem(left.UnitGroup.SystemType);
				var energyValue = left.BaseValue() * right.BaseValue(); // kg × (J/kg) = J
				return unitSystem.CreateMeasuredValue(UnitFamilyName.Energy, energyValue, "J");
			}
			
			// Fallback to base implementation
			throw new InvalidOperationException($"Operator '*' cannot be applied to operands of type 'Mass' and '{right.GetType().Name}'");
		}
		
		/// <summary>
		/// MeasuredValue × Mass → Energy (commutative version)
		/// </summary>
		public static MeasuredValue operator *(MeasuredValue left, Mass right)
		{
			return right * left; // Delegate to Mass × MeasuredValue
		}
	}


}