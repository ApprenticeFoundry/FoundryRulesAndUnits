using System;
using System.Collections.Generic;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	[UnitType(UnitFamilyName.Speed, Description = "Speed/velocity measurement")]
	public class Speed : MeasuredValue
	{
		// UnitFamily comes from UnitTypeAttribute - no need for redundant property override
		#region Constructors and Factory Methods

		/// <summary>
		/// Initializes a new instance of the Speed class with the specified UnitGroup.
		/// </summary>
		/// <param name="unitGroup">The unit group to use for this Speed measurement.</param>
		public Speed(UnitGroup unitGroup) : base(unitGroup)
		{
			if (unitGroup.Family != UnitFamilyName.Speed)
				throw new ArgumentException($"Expected UnitGroup for Speed, got {unitGroup.Family}");
		}


		// Arithmetic operators
		public static Speed operator +(Speed left, Speed right) 
		{
			var result = new Speed(left.UnitGroup);
			result.Init(left.Value() + right.Value(), left.Internal());
			return result;
		}
		
		public static Speed operator -(Speed left, Speed right) 
		{
			var result = new Speed(left.UnitGroup);
			result.Init(left.Value() - right.Value(), left.Internal());
			return result;
		}
		
		public static Speed operator *(Speed speed, double scalar) 
		{
			var result = new Speed(speed.UnitGroup);
			result.Init(speed.Value() * scalar, speed.Internal());
			return result;
		}
		
		public static Speed operator *(double scalar, Speed speed) 
		{
			var result = new Speed(speed.UnitGroup);
			result.Init(scalar * speed.Value(), speed.Internal());
			return result;
		}
		
		public static Speed operator /(Speed speed, double scalar) 
		{
			var result = new Speed(speed.UnitGroup);
			result.Init(speed.Value() / scalar, speed.Internal());
			return result;
		}
		
		public static double operator /(Speed left, Speed right) => left.Value() / right.Value();
		
		#endregion
	}
}
