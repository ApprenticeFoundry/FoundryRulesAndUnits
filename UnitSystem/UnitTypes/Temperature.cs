using System;
using System.Collections.Generic;
using FoundryRulesAndUnits.Extensions;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	[UnitType(UnitFamilyName.Temperature, Description = "Temperature measurement")]
	public class Temperature : MeasuredValue
	{
		// UnitFamily comes from UnitTypeAttribute - no need for redundant property override

		/// <summary>
		/// Constructor with UnitGroup injection - use UnitSystem to create instances
		/// </summary>
		public Temperature(UnitGroup unitGroup) : base(unitGroup)
		{
			if (unitGroup.Family != UnitFamilyName.Temperature)
				throw new ArgumentException($"UnitGroup must be for Temperature family, got {unitGroup.Family}");
		}

		// Static factory methods removed - use UnitSystem instead

		// Arithmetic operators
		public static Temperature operator +(Temperature left, Temperature right) 
		{
			var result = new Temperature(left.UnitGroup);
			result.Init(left.Value() + right.Value(), left.Internal());
			return result;
		}
		
		public static Temperature operator -(Temperature left, Temperature right)
		{
			// A difference of temperatures is a DELTA, well-defined only on the absolute scale.
			// The old path subtracted in the base unit and then re-applied the scale offset on
			// display: 100°C − 0°C showed −173.15°C (bug 045, bench shift 2). Deltas now come
			// back in Kelvin — 100°C − 0°C = 100 K, 212°F − 32°F = 100 K — which is also the
			// number every physics formula (Q = m·c·ΔT …) actually wants. A first-class
			// DeltaTemperature family is a design question, deliberately not invented here.
			var result = new Temperature(left.UnitGroup);
			result.Init(left.As("K") - right.As("K"), "K");
			return result;
		}
		
		public static Temperature operator *(Temperature temp, double scalar) 
		{
			var result = new Temperature(temp.UnitGroup);
			result.Init(temp.Value() * scalar, temp.Internal());
			return result;
		}
		
		public static Temperature operator *(double scalar, Temperature temp) 
		{
			var result = new Temperature(temp.UnitGroup);
			result.Init(scalar * temp.Value(), temp.Internal());
			return result;
		}
		
		public static Temperature operator /(Temperature temp, double scalar) 
		{
			var result = new Temperature(temp.UnitGroup);
			result.Init(temp.Value() / scalar, temp.Internal());
			return result;
		}
		
		public static double operator /(Temperature left, Temperature right) => left.Value() / right.Value();
	}


}
