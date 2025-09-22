using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	[UnitType(UnitFamilyName.DataFlow, Description = "Data flow rate measurement")]
	public class DataFlow : MeasuredValue
	{
		// UnitFamily comes from UnitTypeAttribute - no need for redundant property override
		#region Constructors and Factory Methods

		// UnitGroup injection constructor (preferred for new code)
		public DataFlow(UnitGroup unitGroup) : base(unitGroup) 
		{ 
			if (unitGroup.Family != UnitFamilyName.DataFlow)
				throw new ArgumentException($"Expected UnitGroup for DataFlow, got {unitGroup.Family}");
		}



		#endregion



		#region Operators

		public static DataFlow operator +(DataFlow left, DataFlow right)
		{
			var leftValue = left.As(left.Internal());
			var rightValue = right.As(left.Internal());
			var result = new DataFlow(left.UnitGroup);
			result.Init(leftValue + rightValue, left.Internal());
			return result;
		}

		public static DataFlow operator -(DataFlow left, DataFlow right)
		{
			var leftValue = left.As(left.Internal());
			var rightValue = right.As(left.Internal());
			var result = new DataFlow(left.UnitGroup);
			result.Init(leftValue - rightValue, left.Internal());
			return result;
		}

		public static DataFlow operator *(DataFlow left, double scalar)
		{
			var result = new DataFlow(left.UnitGroup);
			result.Init(left.Value() * scalar, left.Internal());
			return result;
		}

		public static DataFlow operator *(double scalar, DataFlow right)
		{
			var result = new DataFlow(right.UnitGroup);
			result.Init(scalar * right.Value(), right.Internal());
			return result;
		}

		public static DataFlow operator /(DataFlow left, double scalar)
		{
			var result = new DataFlow(left.UnitGroup);
			result.Init(left.Value() / scalar, left.Internal());
			return result;
		}

		public static bool operator >(DataFlow left, DataFlow right) => left.As(left.Internal()) > right.As(left.Internal());
		public static bool operator <(DataFlow left, DataFlow right) => left.As(left.Internal()) < right.As(left.Internal());
		public static bool operator >=(DataFlow left, DataFlow right) => left.As(left.Internal()) >= right.As(left.Internal());
		public static bool operator <=(DataFlow left, DataFlow right) => left.As(left.Internal()) <= right.As(left.Internal());

		#endregion


	}


}
