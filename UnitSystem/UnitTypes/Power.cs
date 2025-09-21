using System;
using System.Collections.Generic;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	public class Power : MeasuredValue
	{
		override public UnitFamilyName UnitFamily => UnitFamilyName.Power;
		#region Constructors and Factory Methods

		// UnitGroup injection constructor (preferred for new code)
		public Power(UnitGroup unitGroup) : base(unitGroup)
		{
			if (unitGroup.Family != UnitFamilyName.Power)
				throw new ArgumentException($"Expected UnitGroup for Power, got {unitGroup.Family}");
		}

	
		// Arithmetic operators
		public static Power operator +(Power left, Power right) => new(left.Value() + right.Value(), left.Internal());
		public static Power operator -(Power left, Power right) => new(left.Value() - right.Value(), left.Internal());
		public static Power operator *(Power power, double scalar) => new(power.Value() * scalar, power.Internal());
		public static Power operator *(double scalar, Power power) => new(scalar * power.Value(), power.Internal());
		public static Power operator /(Power power, double scalar) => new(power.Value() / scalar, power.Internal());
		public static double operator /(Power left, Power right) => left.Value() / right.Value();
	}


}
