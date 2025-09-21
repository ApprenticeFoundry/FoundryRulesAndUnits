using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	[UnitType(UnitFamilyName.None, Description = "Dimensionless measurement")]
	public class Dimensionless : MeasuredValue
	{
		// UnitFamily comes from UnitTypeAttribute - no need for redundant property override
		
		#region Constructors and Factory Methods

		/// <summary>
		/// Constructor with UnitGroup injection - preferred for new code
		/// </summary>
		public Dimensionless(UnitGroup unitGroup) : base(unitGroup)
		{
			if (unitGroup.Family != UnitFamilyName.None)
				throw new ArgumentException($"Expected UnitGroup for None (Dimensionless), got {unitGroup.Family}");
		}


		#endregion

		#region Unit Conversion

		// As() method inherited from MeasuredValue with UnitGroup conversion

		#endregion

		#region Operators

		public static Dimensionless operator +(Dimensionless left, Dimensionless right)
		{
			var result = new Dimensionless(left.UnitGroup);
			result.Init(left.Value() + right.Value(), left.Internal());
			return result;
		}

		public static Dimensionless operator -(Dimensionless left, Dimensionless right)
		{
			var result = new Dimensionless(left.UnitGroup);
			result.Init(left.Value() - right.Value(), left.Internal());
			return result;
		}

		public static Dimensionless operator *(Dimensionless left, double scalar)
		{
			var result = new Dimensionless(left.UnitGroup);
			result.Init(left.Value() * scalar, left.Internal());
			return result;
		}

		public static Dimensionless operator *(double scalar, Dimensionless right)
		{
			var result = new Dimensionless(right.UnitGroup);
			result.Init(scalar * right.Value(), right.Internal());
			return result;
		}

		public static Dimensionless operator /(Dimensionless left, double scalar)
		{
			var result = new Dimensionless(left.UnitGroup);
			result.Init(left.Value() / scalar, left.Internal());
			return result;
		}

		public static double operator /(Dimensionless left, Dimensionless right) => left.Value() / right.Value();

		// Special Dimensionless operators for cross-type operations
		public static Dimensionless operator *(Dimensionless left, Dimensionless right)
		{
			var result = new Dimensionless(left.UnitGroup);
			result.Init(left.Value() * right.Value(), left.Internal());
			return result;
		}

		public static bool operator >(Dimensionless left, Dimensionless right) => left.As(left.Internal()) > right.As(left.Internal());
		public static bool operator <(Dimensionless left, Dimensionless right) => left.As(left.Internal()) < right.As(left.Internal());
		public static bool operator >=(Dimensionless left, Dimensionless right) => left.As(left.Internal()) >= right.As(left.Internal());
		public static bool operator <=(Dimensionless left, Dimensionless right) => left.As(left.Internal()) <= right.As(left.Internal());

		#endregion


	}


}