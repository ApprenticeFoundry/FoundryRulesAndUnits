using System;
using System.Collections.Generic;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	[UnitType(UnitFamilyName.Quantity, Description = "Quantity measurement")]
	public class Quantity : MeasuredValue
	{
		// UnitFamily comes from UnitTypeAttribute - no need for redundant property override
		#region Constructors and Factory Methods

		// UnitGroup injection constructor (preferred for new code)
		public Quantity(UnitGroup unitGroup) : base(unitGroup)
		{
			if (unitGroup.Family != UnitFamilyName.Quantity)
				throw new ArgumentException($"Expected UnitGroup for Quantity, got {unitGroup.Family}");
		}


		#endregion



		// Arithmetic operators with integers
		public static Quantity operator +(Quantity left, int right)
		{
			var result = new Quantity(left.UnitGroup);
			result.Init(left.Value() + right, left.Internal());
			return result;
		}

		public static Quantity operator -(Quantity left, int right)
		{
			var result = new Quantity(left.UnitGroup);
			result.Init(left.Value() - right, left.Internal());
			return result;
		}

		// Arithmetic operators with other quantities
		public static Quantity operator +(Quantity left, Quantity right)
		{
			var result = new Quantity(left.UnitGroup);
			result.Init(left.Value() + right.Value(), left.Internal());
			return result;
		}

		public static Quantity operator -(Quantity left, Quantity right)
		{
			var result = new Quantity(left.UnitGroup);
			result.Init(left.Value() - right.Value(), left.Internal());
			return result;
		}

		public static Quantity operator *(Quantity quantity, double scalar)
		{
			var result = new Quantity(quantity.UnitGroup);
			result.Init(quantity.Value() * scalar, quantity.Internal());
			return result;
		}

		public static Quantity operator *(double scalar, Quantity quantity)
		{
			var result = new Quantity(quantity.UnitGroup);
			result.Init(scalar * quantity.Value(), quantity.Internal());
			return result;
		}

		public static Quantity operator /(Quantity quantity, double scalar)
		{
			var result = new Quantity(quantity.UnitGroup);
			result.Init(quantity.Value() / scalar, quantity.Internal());
			return result;
		}

		public static double operator /(Quantity left, Quantity right) => left.Value() / right.Value();

		// Special operator for creating flow rates - will need proper QuantityFlow UnitGroup
		public static QuantityFlow operator /(Quantity left, Time right)
		{
			// This needs a proper QuantityFlow UnitGroup - placeholder for now
			var flowGroup = left.UnitGroup; // Will need to fix this properly
			var result = new QuantityFlow(flowGroup);
			result.Init(left.Value() / right.Value(), "ea/s");
			return result;
		}
	}


}
