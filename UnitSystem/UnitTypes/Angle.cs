using FoundryRulesAndUnits.Extensions;
using System;
using System.Collections.Generic;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	[UnitType(UnitFamilyName.Angle, Description = "Angular measurement")]
	public class Angle : MeasuredValue
	{
		// UnitFamily comes from UnitTypeAttribute - no need for redundant property override
		// NO backward compatibility constructors - use UnitSystem.CreateUnit<Angle() instead

		/// <summary>
		/// Constructor with UnitGroup injection - use UnitSystem to create instances
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

		// Static factory methods removed - use UnitSystem.CreateUnit<Angle() instead
		// Example: factory.CreateAngle(90, "deg") or factory.CreateAngle(Math.PI/2, "rad")

		// As() method inherited from MeasuredValue - no override needed!

		public Angle Degrees(double value)
		{
			Init(value, "deg"); // Base class handles validation and conversion!
			return this;
		}

		public static bool operator <(Angle left, Angle right) => left.Value() < right.Value();
		public static bool operator >(Angle left, Angle right) => left.Value() > right.Value();
		public static bool operator ==(Angle left, Angle right) => Math.Abs(left.Value() - right.Value()) < 1e-10;
		public static bool operator !=(Angle left, Angle right) => !(left == right);
		
		// Override Equals and GetHashCode to be consistent with == operator
		public override bool Equals(object? obj) => obj is Angle other && this == other;
		public override int GetHashCode() => Value().GetHashCode();

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

		// Scalar multiplication operators - both directions needed
		public static Angle operator *(double left, Angle right)
		{
			var result = new Angle(right._unitGroup);
			result.Init(left * right.Value(), right.Internal());
			return result;
		}

		public static Angle operator *(Angle left, double right)
		{
			var result = new Angle(left._unitGroup);
			result.Init(left.Value() * right, left.Internal());
			return result;
		}

	}



}
