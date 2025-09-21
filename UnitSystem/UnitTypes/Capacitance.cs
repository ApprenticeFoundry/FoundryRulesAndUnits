using System;
using System.Collections.Generic;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	public class Capacitance : MeasuredValue
	{
		#region Constructors and Factory Methods

		// UnitGroup injection constructor (preferred for new code)
		public Capacitance(UnitGroup unitGroup) : base(unitGroup) 
		{ 
			if (unitGroup.Family != UnitFamilyName.Capacitance)
				throw new ArgumentException($"Expected UnitGroup for Capacitance, got {unitGroup.Family}");
		}



		#region Unit Conversion
		// As() method inherited from MeasuredValue with UnitGroup conversion

		#endregion

		#region Operators

		public static Capacitance operator +(Capacitance left, Capacitance right)
		{
			var leftValue = left.As(left.Internal());
			var rightValue = right.As(left.Internal());
			return new Capacitance(leftValue + rightValue, left.Internal());
		}

		public static Capacitance operator -(Capacitance left, Capacitance right)
		{
			var leftValue = left.As(left.Internal());
			var rightValue = right.As(left.Internal());
			return new Capacitance(leftValue - rightValue, left.Internal());
		}

		public static Capacitance operator *(Capacitance left, double scalar) => new(left.Value() * scalar, left.Internal());
		public static Capacitance operator *(double scalar, Capacitance right) => new(scalar * right.Value(), right.Internal());
		public static Capacitance operator /(Capacitance left, double scalar) => new(left.Value() / scalar, left.Internal());

		public static bool operator >(Capacitance left, Capacitance right) => left.As(left.Internal()) > right.As(left.Internal());
		public static bool operator <(Capacitance left, Capacitance right) => left.As(left.Internal()) < right.As(left.Internal());
		public static bool operator >=(Capacitance left, Capacitance right) => left.As(left.Internal()) >= right.As(left.Internal());
		public static bool operator <=(Capacitance left, Capacitance right) => left.As(left.Internal()) <= right.As(left.Internal());

		#endregion


	}


}
