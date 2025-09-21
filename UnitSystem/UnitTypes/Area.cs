using System;
using System.Collections.Generic;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	[UnitType(UnitFamilyName.Area, Description = "Area measurement")]
	public class Area : MeasuredValue
	{
		// UnitFamily comes from UnitTypeAttribute - no need for redundant property override
		// NO backward compatibility constructors - use UnitFactory.CreateArea() instead

		/// <summary>
		/// Constructor with UnitGroup injection - use UnitFactory to create instances
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

		// Static factory methods removed - use UnitFactory.CreateArea() instead

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
		
		public static Area operator /(Area left, double scalar) 
		{
			var result = new Area(left._unitGroup);
			result.Init(left.Value() / scalar, left.Internal());
			return result;
		}
		
		public static double operator /(Area left, Area right) => left.Value() / right.Value();
	}


}