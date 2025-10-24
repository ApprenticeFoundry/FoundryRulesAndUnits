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
		/// Mass × Speed → Momentum (proper physics)
		/// Example: 10 kg × 5 m/s = 50 kg⋅m/s (momentum)
		/// </summary>
		public static Momentum operator *(Mass mass, Speed speed)
		{
			// Reuse existing unit system - no new instances!
			var momentumValue = mass.BaseValue() * speed.BaseValue(); // kg × (m/s) = kg⋅m/s
			var momentum = mass._unitGroup.CreateUnit<Momentum>();
			momentum.Init(momentumValue, momentum.Internal());
			return momentum;
		}
		
		/// <summary>
		/// Mass × Energy → Energy (for specific energy calculations)
		/// Example: 10 kg × 25 J/kg = 250 J (kinetic energy)
		/// Note: The Energy operand should represent specific energy (J/kg)
		/// </summary>
		public static Energy operator *(Mass mass, Energy energy)
		{
			// For kinetic energy: mass × specific energy = total energy
			var energyValue = mass.BaseValue() * energy.BaseValue(); // kg × (J/kg) = J
			var energy = mass._unitGroup.CreateUnit<Energy>();
			energy.Init(energyValue, energy.Internal());
			return energy;
		}
		
		/// <summary>
		/// Energy × Mass → Energy (commutative version)
		/// </summary>
		public static Energy operator *(Energy energy, Mass mass)
		{
			return mass * energy; // Delegate to Mass × Energy
		}
	}


}