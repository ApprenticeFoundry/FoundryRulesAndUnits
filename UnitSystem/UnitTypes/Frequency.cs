using System;
using System.Collections.Generic;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	public class Frequency : MeasuredValue
	{
		override public UnitFamilyName UnitFamily => UnitFamilyName.Frequency;
		#region Constructors and Factory Methods

		// UnitGroup injection constructor (preferred for new code)
		public Frequency(UnitGroup unitGroup) : base(unitGroup)
		{
			if (unitGroup.Family != UnitFamilyName.Frequency)
				throw new ArgumentException($"Expected UnitGroup for Frequency, got {unitGroup.Family}");
		}


		// Arithmetic operators
		public static Frequency operator +(Frequency left, Frequency right) => new(left.Value() + right.Value(), left.Internal());
		public static Frequency operator -(Frequency left, Frequency right) => new(left.Value() - right.Value(), left.Internal());
		public static Frequency operator *(Frequency frequency, double scalar) => new(frequency.Value() * scalar, frequency.Internal());
		public static Frequency operator *(double scalar, Frequency frequency) => new(scalar * frequency.Value(), frequency.Internal());
		public static Frequency operator /(Frequency frequency, double scalar) => new(frequency.Value() / scalar, frequency.Internal());
		public static double operator /(Frequency left, Frequency right) => left.Value() / right.Value();
	}


}
