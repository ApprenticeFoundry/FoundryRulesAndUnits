using System;

namespace FoundryRulesAndUnits.Units;

/// <summary>
/// A three-valued measured quantity on the verdict scale: -1 (False), 0 (Undecided), 1 (True).
///
/// Unlike binary bool, Verdict is a first-class measured value — system-independent,
/// with no unit conversion. The stored value IS the verdict.
///
/// Useful for legal, compliance, and business evaluation where criteria may be
/// confirmed (1), refuted (-1), or still under review (0).
///
/// Logical operators follow Kleene's strong three-valued logic:
///   AND → Min(a, b)  — one False (-1) overrides all
///   OR  → Max(a, b)  — one True  (1) resolves all
///   NOT → negate     — reverses the sign
/// </summary>
[System.Serializable]
[UnitType(UnitFamilyName.Verdict, Description = "Three-valued verdict: -1 false, 0 undecided, 1 true")]
public class Verdict : MeasuredValue
{
    #region Constants

    public const double TrueValue      =  1.0;
    public const double UndecidedValue =  0.0;
    public const double FalseValue     = -1.0;

    #endregion

    #region Constructor

    public Verdict(UnitGroup unitGroup) : base(unitGroup)
    {
        if (unitGroup.Family != UnitFamilyName.Verdict)
            throw new ArgumentException($"Expected UnitGroup for Verdict, got {unitGroup.Family}");
    }

    #endregion

    #region State inspection

    public bool IsTrue      => V == TrueValue;
    public bool IsFalse     => V == FalseValue;
    public bool IsUndecided => V == UndecidedValue;

    #endregion

    #region Fluent assignment

    public Verdict Assign(double verdictValue)
    {
        var clamped = verdictValue > 0 ? TrueValue : verdictValue < 0 ? FalseValue : UndecidedValue;
        Init(clamped, "verdict");
        return this;
    }

    #endregion

    #region Kleene logical operators

    /// <summary>AND: Min(a, b) — one False dominates.</summary>
    public static Verdict operator &(Verdict left, Verdict right)
    {
        var result = new Verdict(left._unitGroup);
        result.Init(Math.Min(left.V, right.V), "verdict");
        return result;
    }

    /// <summary>OR: Max(a, b) — one True resolves.</summary>
    public static Verdict operator |(Verdict left, Verdict right)
    {
        var result = new Verdict(left._unitGroup);
        result.Init(Math.Max(left.V, right.V), "verdict");
        return result;
    }

    /// <summary>NOT: negate — reverses the sign.</summary>
    public static Verdict operator !(Verdict operand)
    {
        var result = new Verdict(operand._unitGroup);
        result.Init(-operand.V, "verdict");
        return result;
    }

    #endregion

    #region Comparison operators

    public static bool operator ==(Verdict left, Verdict right) => left.V == right.V;
    public static bool operator !=(Verdict left, Verdict right) => left.V != right.V;

    public override bool Equals(object? obj) => obj is Verdict other && V == other.V;
    public override int GetHashCode() => V.GetHashCode();

    #endregion

    #region Display

    public override string ToString() => V switch
    {
         1.0 => "True",
        -1.0 => "False",
        _    => "Undecided"
    };

    #endregion
}
