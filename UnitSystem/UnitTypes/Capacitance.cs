using System;
using System.Collections.Generic;

namespace FoundryRulesAndUnits.Units;

[System.Serializable]
[UnitType(UnitFamilyName.Capacitance, Description = "Electrical capacitance measurement")]
public class Capacitance : MeasuredValue
{
	// UnitFamily comes from UnitTypeAttribute - no need for redundant property override
	
	#region Constructors and Factory Methods

	/// <summary>
	/// Constructor with UnitGroup injection - preferred for new code
	/// </summary>
	public Capacitance(UnitGroup unitGroup) : base(unitGroup)
	{
		if (unitGroup.Family != UnitFamilyName.Capacitance)
			throw new ArgumentException($"Expected UnitGroup for Capacitance, got {unitGroup.Family}");
	}

	#endregion

	#region Operators

	public static Capacitance operator +(Capacitance left, Capacitance right)
	{
		var result = new Capacitance(left.UnitGroup);
		result.Init(left.Value() + right.Value(), left.Internal());
		return result;
	}

	public static Capacitance operator -(Capacitance left, Capacitance right)
	{
		var result = new Capacitance(left.UnitGroup);
		result.Init(left.Value() - right.Value(), left.Internal());
		return result;
	}

	public static Capacitance operator *(Capacitance left, double scalar)
	{
		var result = new Capacitance(left.UnitGroup);
		result.Init(left.Value() * scalar, left.Internal());
		return result;
	}

	public static Capacitance operator *(double scalar, Capacitance right)
	{
		var result = new Capacitance(right.UnitGroup);
		result.Init(scalar * right.Value(), right.Internal());
		return result;
	}

	public static Capacitance operator /(Capacitance left, double scalar)
	{
		var result = new Capacitance(left.UnitGroup);
		result.Init(left.Value() / scalar, left.Internal());
		return result;
	}

	public static double operator /(Capacitance left, Capacitance right) => left.Value() / right.Value();

	public static bool operator >(Capacitance left, Capacitance right) => left.As(left.Internal()) > right.As(left.Internal());
	public static bool operator <(Capacitance left, Capacitance right) => left.As(left.Internal()) < right.As(left.Internal());
	public static bool operator >=(Capacitance left, Capacitance right) => left.As(left.Internal()) >= right.As(left.Internal());
	public static bool operator <=(Capacitance left, Capacitance right) => left.As(left.Internal()) <= right.As(left.Internal());

	#endregion


}


