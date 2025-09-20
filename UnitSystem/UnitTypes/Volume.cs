using System;
using System.Collections.Generic;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	public class Volume : MeasuredValue
	{
		/// <summary>
		/// Gets the UnitFamily for Volume measurements
		/// </summary>
		public override UnitFamilyName UnitFamily => UnitFamilyName.Volume;

		/// <summary>
		/// Constructor with UnitGroup injection - preferred
		/// </summary>
		public Volume(UnitGroup unitGroup) : base(unitGroup)
		{
			if (unitGroup.Family != UnitFamilyName.Volume)
				throw new ArgumentException($"UnitGroup family must be {UnitFamilyName.Volume}", nameof(unitGroup));
		}

		/// <summary>
		/// Backward compatibility constructor for JSON deserialization
		/// </summary>
		public Volume(double value, string units) : base()
		{
			V = value;
			I = units;
			U = units;
		}

		// Static factory methods removed - use UnitFactory instead

		// Arithmetic operators
		public static Volume operator +(Volume left, Volume right) 
		{
			var result = new Volume(left.UnitGroup);
			result.Init(left.Value() + right.Value(), left.Internal());
			return result;
		}
		
		public static Volume operator -(Volume left, Volume right) 
		{
			var result = new Volume(left.UnitGroup);
			result.Init(left.Value() - right.Value(), left.Internal());
			return result;
		}
		
		public static Volume operator *(Volume volume, double scalar) 
		{
			var result = new Volume(volume.UnitGroup);
			result.Init(volume.Value() * scalar, volume.Internal());
			return result;
		}
		
		public static Volume operator *(double scalar, Volume volume) 
		{
			var result = new Volume(volume.UnitGroup);
			result.Init(scalar * volume.Value(), volume.Internal());
			return result;
		}
		
		public static Volume operator /(Volume volume, double scalar) 
		{
			var result = new Volume(volume.UnitGroup);
			result.Init(volume.Value() / scalar, volume.Internal());
			return result;
		}
		
		public static double operator /(Volume left, Volume right) => left.Value() / right.Value();
	}


}
