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

		// Static factory methods removed - use UnitFactory instead

		// As() method inherited from MeasuredValue - no override needed!

		public static bool operator <(Mass left, Mass right) => left.Value() < right.Value();
		public static bool operator >(Mass left, Mass right) => left.Value() > right.Value();

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
		
		public static Mass operator /(Mass left, double right) 
		{
			var result = new Mass(left.UnitGroup);
			result.Init(left.Value() / right, left.Internal());
			return result;
		}

		public static double operator /(Mass left, Mass right) => left.Value() / right.Value();
	}


}