using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	public class DataFlow : MeasuredValue
	{
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
			return new DataFlow(leftValue + rightValue, left.Internal());
		}

		public static DataFlow operator -(DataFlow left, DataFlow right)
		{
			var leftValue = left.As(left.Internal());
			var rightValue = right.As(left.Internal());
			return new DataFlow(leftValue - rightValue, left.Internal());
		}

		public static DataFlow operator *(DataFlow left, double scalar) => new(left.Value() * scalar, left.Internal());
		public static DataFlow operator *(double scalar, DataFlow right) => new(scalar * right.Value(), right.Internal());
		public static DataFlow operator /(DataFlow left, double scalar) => new(left.Value() / scalar, left.Internal());

		public static bool operator >(DataFlow left, DataFlow right) => left.As(left.Internal()) > right.As(left.Internal());
		public static bool operator <(DataFlow left, DataFlow right) => left.As(left.Internal()) < right.As(left.Internal());
		public static bool operator >=(DataFlow left, DataFlow right) => left.As(left.Internal()) >= right.As(left.Internal());
		public static bool operator <=(DataFlow left, DataFlow right) => left.As(left.Internal()) <= right.As(left.Internal());

		#endregion


	}


}
