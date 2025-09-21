using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]

	public class DataStorage : MeasuredValue
	{
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
			return new DataStorage(leftValue + rightValue, left.Internal());
		}

		public static DataStorage operator -(DataStorage left, DataStorage right)
		{
			var leftValue = left.As(left.Internal());
			var rightValue = right.As(left.Internal());
			return new DataStorage(leftValue - rightValue, left.Internal());
		}

		public static DataStorage operator *(DataStorage left, double scalar) => new(left.Value() * scalar, left.Internal());
		public static DataStorage operator *(double scalar, DataStorage right) => new(scalar * right.Value(), right.Internal());
		public static DataStorage operator /(DataStorage left, double scalar) => new(left.Value() / scalar, left.Internal());

		public static bool operator >(DataStorage left, DataStorage right) => left.As(left.Internal()) > right.As(left.Internal());
		public static bool operator <(DataStorage left, DataStorage right) => left.As(left.Internal()) < right.As(left.Internal());
		public static bool operator >=(DataStorage left, DataStorage right) => left.As(left.Internal()) >= right.As(left.Internal());
		public static bool operator <=(DataStorage left, DataStorage right) => left.As(left.Internal()) <= right.As(left.Internal());

		#endregion


	}


}
