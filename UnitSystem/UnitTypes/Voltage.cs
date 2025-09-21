using System;
using System.Collections.Generic;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	[UnitType(UnitFamilyName.Voltage, Description = "Electrical voltage measurement")]
	public class Voltage : MeasuredValue
	{
		// UnitFamily comes from UnitTypeAttribute - no need for redundant property override
		#region Constructors and Factory Methods

		/// <summary>
		/// Initializes a new instance of the Voltage class with the specified UnitGroup.
		/// </summary>
		/// <param name="unitGroup">The unit group to use for this Voltage measurement.</param>
		public Voltage(UnitGroup unitGroup) : base(unitGroup)
		{
			if (unitGroup.Family != UnitFamilyName.Voltage)
				throw new ArgumentException($"Expected UnitGroup for Voltage, got {unitGroup.Family}");
		}

		#endregion


		#region Operators

		public static Voltage operator +(Voltage left, Voltage right)
		{
			var result = new Voltage(left.UnitGroup);
			var leftValue = left.As(left.Internal());
			var rightValue = right.As(left.Internal());
			result.Init(leftValue + rightValue, left.Internal());
			return result;
		}

		public static Voltage operator -(Voltage left, Voltage right)
		{
			var result = new Voltage(left.UnitGroup);
			var leftValue = left.As(left.Internal());
			var rightValue = right.As(left.Internal());
			result.Init(leftValue - rightValue, left.Internal());
			return result;
		}

		public static Voltage operator *(Voltage left, double scalar) 
		{
			var result = new Voltage(left.UnitGroup);
			result.Init(left.Value() * scalar, left.Internal());
			return result;
		}
		
		public static Voltage operator *(double scalar, Voltage right) 
		{
			var result = new Voltage(right.UnitGroup);
			result.Init(scalar * right.Value(), right.Internal());
			return result;
		}
		
		public static Voltage operator /(Voltage left, double scalar) 
		{
			var result = new Voltage(left.UnitGroup);
			result.Init(left.Value() / scalar, left.Internal());
			return result;
		}

		public static bool operator >(Voltage left, Voltage right) => left.As(left.Internal()) > right.As(left.Internal());
		public static bool operator <(Voltage left, Voltage right) => left.As(left.Internal()) < right.As(left.Internal());
		public static bool operator >=(Voltage left, Voltage right) => left.As(left.Internal()) >= right.As(left.Internal());
		public static bool operator <=(Voltage left, Voltage right) => left.As(left.Internal()) <= right.As(left.Internal());

		#endregion


	}


}
