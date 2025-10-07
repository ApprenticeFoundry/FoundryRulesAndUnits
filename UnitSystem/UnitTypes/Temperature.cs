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
			var result = new Temperature(left.UnitGroup);
			result.Init(left.Value() - right.Value(), left.Internal());
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
