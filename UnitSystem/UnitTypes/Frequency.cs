using System;
using System.Collections.Generic;

namespace FoundryRulesAndUnits.Units;

	[System.Serializable]
	[UnitType(UnitFamilyName.Frequency, Description = "Frequency measurement")]
	public class Frequency : MeasuredValue
	{
		// UnitFamily comes from UnitTypeAttribute - no need for redundant property override
		
		#region Constructors and Factory Methods

		/// <summary>
		/// Constructor with UnitGroup injection - preferred for new code
		/// </summary>
		public Frequency(UnitGroup unitGroup) : base(unitGroup)
		{
			if (unitGroup.Family != UnitFamilyName.Frequency)
				throw new ArgumentException($"Expected UnitGroup for Frequency, got {unitGroup.Family}");
		}

		#endregion

		#region Operators

		public static Frequency operator +(Frequency left, Frequency right)
		{
			var result = new Frequency(left.UnitGroup);
			result.Init(left.Value() + right.Value(), left.Internal());
			return result;
		}

		public static Frequency operator -(Frequency left, Frequency right)
		{
			var result = new Frequency(left.UnitGroup);
			result.Init(left.Value() - right.Value(), left.Internal());
			return result;
		}

		public static Frequency operator *(Frequency left, double scalar)
		{
			var result = new Frequency(left.UnitGroup);
			result.Init(left.Value() * scalar, left.Internal());
			return result;
		}

		public static Frequency operator *(double scalar, Frequency right)
		{
			var result = new Frequency(right.UnitGroup);
			result.Init(scalar * right.Value(), right.Internal());
			return result;
		}

		public static Frequency operator /(Frequency left, double scalar)
		{
			var result = new Frequency(left.UnitGroup);
			result.Init(left.Value() / scalar, left.Internal());
			return result;
		}

		public static double operator /(Frequency left, Frequency right) => left.Value() / right.Value();

		#endregion
	}



