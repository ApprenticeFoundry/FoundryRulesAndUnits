using System;
using System.Collections.Generic;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	public class Area : MeasuredValue
	{
		/// <summary>
		/// Gets the UnitFamily for Area measurements
		/// </summary>
		public override UnitFamilyName UnitFamily => UnitFamilyName.Area;

		/// <summary>
		/// Backward compatibility constructor for JSON deserialization
		/// </summary>

		public Area(double value, string units) : base()

		{

			V = value;

			I = units;

			U = units;

		}

		/// <summary>
		/// Constructor with UnitGroup injection - preferred
		/// </summary>
		public Area(UnitGroup unitGroup) : base(unitGroup)
		{
			if (unitGroup.Family != UnitFamilyName.Area)
				throw new ArgumentException($"UnitGroup family must be {UnitFamilyName.Area}", nameof(unitGroup));
		}

		// Static factory methods removed - use UnitFactory instead

		// Arithmetic operators
		public static Area operator +(Area left, Area right) 
		{
			var result = new Area(left.UnitGroup);
			result.Init(left.Value() + right.Value(), left.Internal());
			return result;
		}
		
		public static Area operator -(Area left, Area right) 
		{
			var result = new Area(left.UnitGroup);
			result.Init(left.Value() - right.Value(), left.Internal());
			return result;
		}
		
		public static Area operator *(Area area, double scalar) 
		{
			var result = new Area(area.UnitGroup);
			result.Init(area.Value() * scalar, area.Internal());
			return result;
		}
		
		public static Area operator *(double scalar, Area area) 
		{
			var result = new Area(area.UnitGroup);
			result.Init(scalar * area.Value(), area.Internal());
			return result;
		}
		
		public static Area operator /(Area area, double scalar) 
		{
			var result = new Area(area.UnitGroup);
			result.Init(area.Value() / scalar, area.Internal());
			return result;
		}
		
		public static double operator /(Area left, Area right) => left.Value() / right.Value();
	}


}