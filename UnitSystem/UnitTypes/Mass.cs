using FoundryRulesAndUnits.Extensions;
using System;
using System.Collections.Generic;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	public class Mass : MeasuredValue
	{
		/// <summary>
		/// Gets the UnitFamily for Mass measurements
		/// </summary>
		public override UnitFamilyName UnitFamily => UnitFamilyName.Mass;

		/// <summary>
		/// Backward compatibility constructor for JSON deserialization
		/// </summary>

		public Mass(double value, string units) : base()

		{

			V = value;

			I = units;

			U = units;

		}

		/// <summary>
		/// Constructor with UnitGroup injection - preferred
		/// </summary>
		public Mass(UnitGroup unitGroup) : base(unitGroup)
		{
			if (unitGroup.Family != UnitFamilyName.Mass)
				throw new ArgumentException($"UnitGroup family must be {UnitFamilyName.Mass}", nameof(unitGroup));
		}

		public Mass Assign(double value, string? units)
		{
			Init(value, units); // Base class handles everything!
			return this;
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