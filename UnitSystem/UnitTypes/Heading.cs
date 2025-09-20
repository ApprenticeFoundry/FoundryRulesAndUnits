using FoundryRulesAndUnits.Extensions;
using System;
using System.Collections.Generic;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	public class Heading : MeasuredValue
	{
		#region Constructors and Factory Methods

		// UnitGroup injection constructor (preferred for new code)
		public Heading(UnitGroup unitGroup) : base(unitGroup) 
		{ 
			if (unitGroup.Family != UnitFamilyName.Heading)
				throw new ArgumentException($"Expected UnitGroup for Heading, got {unitGroup.Family}");
		}

		public Heading(UnitGroup unitGroup, double value, string? units = null) : base(unitGroup)
		{
			if (unitGroup.Family != UnitFamilyName.Heading)
				throw new ArgumentException($"Expected UnitGroup for Heading, got {unitGroup.Family}");
			Init(value, units);
		}

		/// <summary>


		/// Backward compatibility constructor for JSON deserialization


		/// </summary>


		public Heading(double value, string units) : base()


		{


			V = value;


			I = units;


			U = units;


		}

		// Factory methods for common heading units
		public static Heading FromDegrees(double value) => new(value, "deg");
		public static Heading FromRadians(double value) => new(value, "rad");
		public static Heading FromGradians(double value) => new(value, "grad");

		#endregion

		#region Unit Conversion
		// As() method inherited from MeasuredValue with UnitGroup conversion

		#endregion

		#region Legacy Methods (Maintained for Compatibility)

		public Heading Assign(double value, string? units)
		{
			if (units == I)
			{
				V = value;
			}
			else
			{
				Init(value, units);
			}
			return this;
		}

		public Heading Assign(Heading source)
		{
			if (source.I == I)
			{
				V = source.Value();
			}
			else
			{
				Init(source.Value(), source.U);
			}
			return this;
		}

		public Heading Copy()
		{
			return new Heading(Value(), Internal());
		}

		public Heading Degrees(double value)
		{
			V = _unitGroup.Convert(value, "deg", Internal());
			return this;
		}

		#endregion

		#region Operators

		public static Heading operator +(Heading left, Heading right)
		{
			var leftValue = left.As(left.Internal());
			var rightValue = right.As(left.Internal());
			return new Heading(leftValue + rightValue, left.Internal());
		}

		public static Heading operator -(Heading left, Heading right)
		{
			var leftValue = left.As(left.Internal());
			var rightValue = right.As(left.Internal());
			return new Heading(leftValue - rightValue, left.Internal());
		}

		public static Heading operator *(Heading left, double scalar) => new(left.Value() * scalar, left.Internal());
		public static Heading operator *(double scalar, Heading right) => new(scalar * right.Value(), right.Internal());
		public static Heading operator /(Heading left, double scalar) => new(left.Value() / scalar, left.Internal());

		public static bool operator >(Heading left, Heading right) => left.As(left.Internal()) > right.As(left.Internal());
		public static bool operator <(Heading left, Heading right) => left.As(left.Internal()) < right.As(left.Internal());
		public static bool operator >=(Heading left, Heading right) => left.As(left.Internal()) >= right.As(left.Internal());
		public static bool operator <=(Heading left, Heading right) => left.As(left.Internal()) <= right.As(left.Internal());

		#endregion


	}


}
