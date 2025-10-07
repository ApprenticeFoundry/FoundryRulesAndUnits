using System;
using System.Collections.Generic;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	[UnitType(UnitFamilyName.Area, Description = "Area measurement")]
	public class Area : MeasuredValue
	{
		// UnitFamily comes from UnitTypeAttribute - no need for redundant property override
		// NO backward compatibility constructors - use UnitSystem.CreateUnit<Area() instead

		/// <summary>
		/// Constructor with UnitGroup injection - use UnitSystem to create instances
		/// </summary>
		public Area(UnitGroup unitGroup) : base(unitGroup)
		{
			if (unitGroup.Family != UnitFamilyName.Area)
				throw new ArgumentException($"UnitGroup must be for Area family, got {unitGroup.Family}");
		}

		public Area Assign(double value, string? units)
		{
			Init(value, units); // Base class handles everything!
			return this;
		}

		public Area Assign(Area source)
		{
			Init(source.Value(), source.U); // Base class handles everything!
			return this;
		}

		public Area Copy()
		{
			var copy = new Area(_unitGroup);
			copy.Init(Value(), Internal());
			return copy;
		}

		// Static factory methods removed - use UnitSystem.CreateUnit<Area() instead

		// Comparison operators
		public static bool operator <(Area left, Area right) => left.Value() < right.Value();
		public static bool operator >(Area left, Area right) => left.Value() > right.Value();

		// Arithmetic operators
		public static Area operator +(Area left, Area right) 
		{
			var result = new Area(left._unitGroup);
			result.Init(left.Value() + right.Value(), left.Internal());
			return result;
		}
		
		public static Area operator -(Area left, Area right) 
		{
			var result = new Area(left._unitGroup);
			result.Init(left.Value() - right.Value(), left.Internal());
			return result;
		}
		
		public static Area operator *(double scalar, Area right) 
		{
			var result = new Area(right._unitGroup);
			result.Init(scalar * right.Value(), right.Internal());
			return result;
		}

		public static Area operator *(Area left, double scalar)
		{
			var result = new Area(left._unitGroup);
			result.Init(left.Value() * scalar, left.Internal());
			return result;
		}
		
		public static Area operator /(Area left, double scalar) 
		{
			var result = new Area(left._unitGroup);
			result.Init(left.Value() / scalar, left.Internal());
			return result;
		}
		
		public static double operator /(Area left, Area right) => left.Value() / right.Value();
		
		// ============================================================================
		// PHASE 4A: CROSS-FAMILY OPERATIONS - Area-specific operators  
		// ============================================================================
		
		/// <summary>
		/// Area × Length → Volume
		/// Example: 15 m² × 3m = 45 m³
		/// </summary>
		public static MeasuredValue operator *(Area left, Length right)
		{
			var unitSystem = new UnitSystem(left._unitGroup.SystemType);
			var volumeValue = left.BaseValue() * right.BaseValue(); // m² × m = m³
			return unitSystem.CreateMeasuredValue(UnitFamilyName.Volume, volumeValue, "m3");
		}
		
		/// <summary>
		/// Area ÷ Length → Length (Width = Area ÷ Length) 
		/// Example: 15 m² ÷ 3m = 5m
		/// </summary>
		public static MeasuredValue operator /(Area left, Length right)
		{
			var unitSystem = new UnitSystem(left._unitGroup.SystemType);
			var lengthValue = left.BaseValue() / right.BaseValue(); // m² ÷ m = m
			return unitSystem.CreateMeasuredValue(UnitFamilyName.Length, lengthValue, "m");
		}
	}


}