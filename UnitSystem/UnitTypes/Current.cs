using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	[UnitType(UnitFamilyName.Current, Description = "Electrical current measurement")]
	public class Current : MeasuredValue
	{
		// UnitFamily comes from UnitTypeAttribute - no need for redundant property override
		
		#region Constructors and Factory Methods

		/// <summary>
		/// Constructor with UnitGroup injection - preferred for new code
		/// </summary>
		public Current(UnitGroup unitGroup) : base(unitGroup) 
		{ 
			if (unitGroup.Family != UnitFamilyName.Current)
				throw new ArgumentException($"Expected UnitGroup for Current, got {unitGroup.Family}");
		}



		#endregion

		#region Unit Conversion
		// As() method inherited from MeasuredValue with UnitGroup conversion

		#endregion

		#region Operators

		public static Current operator +(Current left, Current right)
		{
			var result = new Current(left.UnitGroup);
			result.Init(left.Value() + right.Value(), left.Internal());
			return result;
		}

		public static Current operator -(Current left, Current right)
		{
			var result = new Current(left.UnitGroup);
			result.Init(left.Value() - right.Value(), left.Internal());
			return result;
		}

		public static Current operator *(Current left, double scalar)
		{
			var result = new Current(left.UnitGroup);
			result.Init(left.Value() * scalar, left.Internal());
			return result;
		}

		public static Current operator *(double scalar, Current right)
		{
			var result = new Current(right.UnitGroup);
			result.Init(scalar * right.Value(), right.Internal());
			return result;
		}

		public static Current operator /(Current left, double scalar)
		{
			var result = new Current(left.UnitGroup);
			result.Init(left.Value() / scalar, left.Internal());
			return result;
		}

		public static double operator /(Current left, Current right) => left.Value() / right.Value();

		public static bool operator >(Current left, Current right) => left.As(left.Internal()) > right.As(left.Internal());
		public static bool operator <(Current left, Current right) => left.As(left.Internal()) < right.As(left.Internal());
		public static bool operator >=(Current left, Current right) => left.As(left.Internal()) >= right.As(left.Internal());
		public static bool operator <=(Current left, Current right) => left.As(left.Internal()) <= right.As(left.Internal());

		#endregion


	}
}
