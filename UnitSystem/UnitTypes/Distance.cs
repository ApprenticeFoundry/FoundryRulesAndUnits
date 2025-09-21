using System;
using System.Collections.Generic;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	public class Distance : MeasuredValue
	{
		override public UnitFamilyName UnitFamily => UnitFamilyName.Distance;
		#region Constructors and Factory Methods

		// UnitGroup injection constructor (preferred for new code)
		public Distance(UnitGroup unitGroup) : base(unitGroup)
		{
			if (unitGroup.Family != UnitFamilyName.Length)
				throw new ArgumentException($"Expected UnitGroup for Length, got {unitGroup.Family}");
		}


		// Utility methods
		public double Diff(Length other) => Value() - other.Value();
		public double Diff(double other) => Value() - other;
		public double Sum(Length other) => Value() + other.Value();
		public double Sum(double other) => Value() + other;

		// Arithmetic operators
		public static Distance operator +(Distance left, Distance right) => new(left.Value() + right.Value(), left.Internal());
		public static Distance operator -(Distance left, Distance right) => new(left.Value() - right.Value(), left.Internal());
		public static Distance operator +(Distance left, double right) => new(left.Value() + right, left.Internal());
		public static Distance operator -(Distance left, double right) => new(left.Value() - right, left.Internal());
		public static Distance operator *(Distance distance, double scalar) => new(distance.Value() * scalar, distance.Internal());
		public static Distance operator *(double scalar, Distance distance) => new(scalar * distance.Value(), distance.Internal());
		public static Distance operator /(Distance distance, double scalar) => new(distance.Value() / scalar, distance.Internal());
		public static double operator /(Distance left, Distance right) => left.Value() / right.Value();
	}


}
