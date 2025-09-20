using System;
using System.Collections.Generic;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	public class Power : MeasuredValue
	{
		#region Constructors and Factory Methods

		// UnitGroup injection constructor (preferred for new code)
		public Power(UnitGroup unitGroup) : base(unitGroup) 
		{ 
			if (unitGroup.Family != UnitFamilyName.Power)
				throw new ArgumentException($"Expected UnitGroup for Power, got {unitGroup.Family}");
		}

		public Power(UnitGroup unitGroup, double value, string? units = null) : base(unitGroup)
		{
			if (unitGroup.Family != UnitFamilyName.Power)
				throw new ArgumentException($"Expected UnitGroup for Power, got {unitGroup.Family}");
			Init(value, units);
		}

		/// <summary>


		/// Backward compatibility constructor for JSON deserialization


		/// </summary>


		public Power(double value, string units) : base()


		{


			V = value;


			I = units;


			U = units;


		}

		#endregion

		// Factory methods
		public static Power FromWatts(double value) => new(value, "W");
		public static Power FromKilowatts(double value) => new(value, "kW");
		public static Power FromMegawatts(double value) => new(value, "MW");
		public static Power FromHorsepower(double value) => new(value, "hp");
		public static Power FromBTUPerHour(double value) => new(value, "BTU/h");

		// Arithmetic operators
		public static Power operator +(Power left, Power right) => new(left.Value() + right.Value(), left.Internal());
		public static Power operator -(Power left, Power right) => new(left.Value() - right.Value(), left.Internal());
		public static Power operator *(Power power, double scalar) => new(power.Value() * scalar, power.Internal());
		public static Power operator *(double scalar, Power power) => new(scalar * power.Value(), power.Internal());
		public static Power operator /(Power power, double scalar) => new(power.Value() / scalar, power.Internal());
		public static double operator /(Power left, Power right) => left.Value() / right.Value();
	}


}
