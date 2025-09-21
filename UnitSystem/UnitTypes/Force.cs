using FoundryRulesAndUnits.Extensions;
using System;
using System.Collections.Generic;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	public class Force : MeasuredValue
	{
		override public UnitFamilyName UnitFamily => UnitFamilyName.Force;
		/// <summary>
		/// Constructor with UnitGroup injection - preferred
		/// </summary>
		public Force(UnitGroup unitGroup) : base(unitGroup)
		{
			if (unitGroup.Family != UnitFamilyName.Force)
				throw new ArgumentException($"UnitGroup family must be {UnitFamilyName.Force}", nameof(unitGroup));
		}

		}

		// Static factory methods removed - use UnitFactory instead

		// Comparison operators
		public static bool operator <(Force left, Force right) => left.Value() < right.Value();
		public static bool operator >(Force left, Force right) => left.Value() > right.Value();

		// Arithmetic operators
		public static Force operator +(Force left, Force right) 
		{
			var result = new Force(left.UnitGroup);
			result.Init(left.Value() + right.Value(), left.Internal());
			return result;
		}
		
		public static Force operator -(Force left, Force right) 
		{
			var result = new Force(left.UnitGroup);
			result.Init(left.Value() - right.Value(), left.Internal());
			return result;
		}
		
		public static Force operator *(Force force, double scalar) 
		{
			var result = new Force(force.UnitGroup);
			result.Init(force.Value() * scalar, force.Internal());
			return result;
		}
		
		public static Force operator *(double scalar, Force force) 
		{
			var result = new Force(force.UnitGroup);
			result.Init(scalar * force.Value(), force.Internal());
			return result;
		}
		
		public static Force operator /(Force force, double scalar) 
		{
			var result = new Force(force.UnitGroup);
			result.Init(force.Value() / scalar, force.Internal());
			return result;
		}
		
		public static double operator /(Force left, Force right) => left.Value() / right.Value();
	}


}