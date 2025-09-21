using System;
using System.Collections.Generic;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	[UnitType(UnitFamilyName.Resistance, Description = "Electrical resistance measurement")]
	public class Resistance : MeasuredValue
	{
		// UnitFamily comes from UnitTypeAttribute - no need for redundant property override
		#region Constructors and Factory Methods

		/// <summary>
		/// Initializes a new instance of the Resistance class with the specified UnitGroup.
		/// </summary>
		/// <param name="unitGroup">The unit group to use for this Resistance measurement.</param>
		public Resistance(UnitGroup unitGroup) : base(unitGroup)
		{
			if (unitGroup.Family != UnitFamilyName.Resistance)
				throw new ArgumentException($"Expected UnitGroup for Resistance, got {unitGroup.Family}");
		}


		#endregion

		#region Unit Conversion
		// As() method inherited from MeasuredValue with UnitGroup conversion

		#endregion

		#region Operators

		public static Resistance operator +(Resistance left, Resistance right)
		{
			var result = new Resistance(left.UnitGroup);
			result.Init(left.Value() + right.Value(), left.Internal());
			return result;
		}

		public static Resistance operator -(Resistance left, Resistance right)
		{
			var result = new Resistance(left.UnitGroup);
			result.Init(left.Value() - right.Value(), left.Internal());
			return result;
		}

		public static Resistance operator *(Resistance left, double scalar)
		{
			var result = new Resistance(left.UnitGroup);
			result.Init(left.Value() * scalar, left.Internal());
			return result;
		}

		public static Resistance operator *(double scalar, Resistance right)
		{
			var result = new Resistance(right.UnitGroup);
			result.Init(scalar * right.Value(), right.Internal());
			return result;
		}

		public static Resistance operator /(Resistance left, double scalar)
		{
			var result = new Resistance(left.UnitGroup);
			result.Init(left.Value() / scalar, left.Internal());
			return result;
		}

		public static bool operator >(Resistance left, Resistance right) => left.As(left.Internal()) > right.As(left.Internal());
		public static bool operator <(Resistance left, Resistance right) => left.As(left.Internal()) < right.As(left.Internal());
		public static bool operator >=(Resistance left, Resistance right) => left.As(left.Internal()) >= right.As(left.Internal());
		public static bool operator <=(Resistance left, Resistance right) => left.As(left.Internal()) <= right.As(left.Internal());

		#endregion


	}


}
