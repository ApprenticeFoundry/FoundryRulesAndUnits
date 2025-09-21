using FoundryRulesAndUnits.Extensions;
using System;
using System.Collections.Generic;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	public class Angle : MeasuredValue
	{
		public override UnitFamilyName UnitFamily => UnitFamilyName.Angle;
	// NO backward compatibility constructors - use UnitFactory.CreateAngle() instead

		/// <summary>
		/// Constructor with UnitGroup injection - use UnitFactory to create instances
		/// </summary>
		public Angle(UnitGroup unitGroup) : base(unitGroup)
		{
			if (unitGroup.Family != UnitFamilyName.Angle)
				throw new ArgumentException($"UnitGroup must be for Angle family, got {unitGroup.Family}");
		}

		public Angle Assign(double value, string? units)
		{
			Init(value, units); // Base class handles everything!
			return this;
		}

		public Angle Assign(Angle source)
		{
			Init(source.Value(), source.U); // Base class handles everything!
			return this;
		}

		public Angle Copy()
		{
			var copy = new Angle(_unitGroup);
			copy.Init(Value(), Internal());
			return copy;
		}

		// Static factory methods removed - use UnitFactory.CreateAngle() instead
		// Example: factory.CreateAngle(90, "deg") or factory.CreateAngle(Math.PI/2, "rad")

		// As() method inherited from MeasuredValue - no override needed!

		public Angle Degrees(double value)
		{
			Init(value, "deg"); // Base class handles validation and conversion!
			return this;
		}

		public static bool operator <(Angle left, Angle right) => left.Value() < right.Value();
		public static bool operator >(Angle left, Angle right) => left.Value() > right.Value();

		public static Angle operator +(Angle left, Angle right)
		{
			var result = new Angle(left._unitGroup);
			result.Init(left.Value() + right.Value(), left.Internal());
			return result;
		}
		
		public static Angle operator -(Angle left, Angle right)
		{
			var result = new Angle(left._unitGroup);
			result.Init(left.Value() - right.Value(), left.Internal());
			return result;
		}

	}



}
