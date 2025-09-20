using System;
using System.Collections.Generic;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	public class Speed : MeasuredValue
	{
		#region Constructors and Factory Methods

		// UnitGroup injection constructor (preferred for new code)
		public Speed(UnitGroup unitGroup) : base(unitGroup)
		{
			if (unitGroup.Family != UnitFamilyName.Speed)
				throw new ArgumentException($"Expected UnitGroup for Speed, got {unitGroup.Family}");
		}

		public Speed(UnitGroup unitGroup, double value, string? units = null) : base(unitGroup)
		{
			if (unitGroup.Family != UnitFamilyName.Speed)
				throw new ArgumentException($"Expected UnitGroup for Speed, got {unitGroup.Family}");
			Init(value, units);
		}

		/// <summary>


		/// Backward compatibility constructor for JSON deserialization


		/// </summary>


		public Speed(double value, string units) : base()


		{


			V = value;


			I = units;


			U = units;


		}

		#endregion

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
	}


}
