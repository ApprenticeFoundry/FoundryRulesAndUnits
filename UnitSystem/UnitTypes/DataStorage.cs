using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	[UnitType(UnitFamilyName.DataStorage, Description = "Data storage measurement")]
	public class DataStorage : MeasuredValue
	{
		// UnitFamily comes from UnitTypeAttribute - no need for redundant property override
		#region Constructors and Factory Methods

		// UnitGroup injection constructor (preferred for new code)
		public DataStorage(UnitGroup unitGroup) : base(unitGroup) 
		{ 
			if (unitGroup.Family != UnitFamilyName.DataStorage)
				throw new ArgumentException($"Expected UnitGroup for DataStorage, got {unitGroup.Family}");
		}


		#endregion

		#region Unit Conversion
		// As() method inherited from MeasuredValue with UnitGroup conversion

		#endregion

		#region Operators

		public static DataStorage operator +(DataStorage left, DataStorage right)
		{
			var leftValue = left.As(left.Internal());
			var rightValue = right.As(left.Internal());
			var result = new DataStorage(left.UnitGroup);
			result.Init(leftValue + rightValue, left.Internal());
			return result;
		}

		public static DataStorage operator -(DataStorage left, DataStorage right)
		{
			var leftValue = left.As(left.Internal());
			var rightValue = right.As(left.Internal());
			var result = new DataStorage(left.UnitGroup);
			result.Init(leftValue - rightValue, left.Internal());
			return result;
		}

		public static DataStorage operator *(DataStorage left, double scalar)
		{
			var result = new DataStorage(left.UnitGroup);
			result.Init(left.Value() * scalar, left.Internal());
			return result;
		}

		public static DataStorage operator *(double scalar, DataStorage right)
		{
			var result = new DataStorage(right.UnitGroup);
			result.Init(scalar * right.Value(), right.Internal());
			return result;
		}

		public static DataStorage operator /(DataStorage left, double scalar)
		{
			var result = new DataStorage(left.UnitGroup);
			result.Init(left.Value() / scalar, left.Internal());
			return result;
		}

		public static bool operator >(DataStorage left, DataStorage right) => left.As(left.Internal()) > right.As(left.Internal());
		public static bool operator <(DataStorage left, DataStorage right) => left.As(left.Internal()) < right.As(left.Internal());
		public static bool operator >=(DataStorage left, DataStorage right) => left.As(left.Internal()) >= right.As(left.Internal());
		public static bool operator <=(DataStorage left, DataStorage right) => left.As(left.Internal()) <= right.As(left.Internal());

		#endregion


	}


}
