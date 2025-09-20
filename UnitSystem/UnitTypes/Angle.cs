using FoundryRulesAndUnits.Extensions;
using System;
using System.Collections.Generic;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	public class Angle : MeasuredValue
	{
		/// <summary>
		/// Backward compatibility constructor for JSON deserialization
		/// </summary>
		public Angle(double value, string units) : base()
		{
			V = value;
			I = units;
			U = units;
		}

		/// <summary>
		/// Backward compatibility constructor - single value assumes degrees
		/// </summary>
		public Angle(double value) : base()
		{
			V = value;
			I = "deg"; // Default to degrees
			U = "deg";
		}

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
			return new Angle(Value(), Internal());
		}

		public static Angle FromDegrees(double v)
		{
			return new Angle(v, "deg");
		}

		public static Angle FromRadians(double v)
		{
			return new Angle(v, "rad");
		}

		// As() method inherited from MeasuredValue - no override needed!

		public Angle Degrees(double value)
		{
			Init(value, "deg"); // Base class handles validation and conversion!
			return this;
		}

		public static bool operator <(Angle left, Angle right) => left.Value() < right.Value();
		public static bool operator >(Angle left, Angle right) => left.Value() > right.Value();

		public static Angle operator +(Angle left, Angle right) => new(left.Value() + right.Value(), left.Internal());
		public static Angle operator -(Angle left, Angle right) => new(left.Value() - right.Value(), left.Internal());

	}



}
