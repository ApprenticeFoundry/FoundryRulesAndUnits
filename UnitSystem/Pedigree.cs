using System;

namespace FoundryRulesAndUnits.Units;

/// <summary>
/// The dimensional pedigree that travels with a value through evaluation
/// (SPEC_DIMENSIONAL_PEDIGREE §4). Two machine words, value type, zero allocation:
///   Signature      — what KIND of thing this is (the prime factor form)
///   CanonicalScale — multiplier converting the carried raw number into canonical units
///                    (m·kg·s·K·A·ea·USD·B), independent of the active UnitSystemType.
///
/// CanonicalScale == 0 is the UNTRACKED sentinel (0 is an impossible scale), so
/// default(Pedigree) means "no dimensional information" — never an error, it simply
/// propagates untracked. Shadow-mode invariant: carrying a pedigree never changes a
/// value, a status, or a message.
///
/// STONE 1 NOTE: scale resolution per unit system (Distance/km → ×1000, IPS in → ×0.0254)
/// lands with phase 3 (scale correction). Until then tracked pedigrees carry scale 1.0 —
/// the signature side is fully live, the scale side is a placeholder.
/// </summary>
public readonly struct Pedigree : IEquatable<Pedigree>
{
    public DimensionalSignature Signature { get; }
    public double CanonicalScale { get; }

    public static readonly Pedigree Untracked = default;

    /// <summary>A tracked, dimensionless pedigree — the pedigree of a bare number.</summary>
    public static readonly Pedigree Dimensionless = new(DimensionalSignature.Dimensionless, 1.0);

    public bool IsTracked => CanonicalScale != 0.0;

    public Pedigree(DimensionalSignature signature, double canonicalScale = 1.0)
    {
        Signature = signature;
        CanonicalScale = canonicalScale;
    }

    /// <summary>
    /// Pedigree for a value of the given family: tracked when the family has a declared
    /// signature, untracked for nominal-only families (WorkTime, Bearing, Percent, ...).
    /// </summary>
    public static Pedigree ForFamily(UnitFamilyName family)
        => FamilySignatures.TryGet(family, out var signature)
            ? new Pedigree(signature, 1.0)
            : Untracked;

    /// <summary>
    /// The scale-aware form (phase 3 begins, 2026-08-30): the pedigree of a value stored in the
    /// family's base unit under the given unit system, with <see cref="CanonicalScale"/> supplying
    /// the base→canonical multiplier (1.0 for every coherent MKS family; 1/3600 for USD/hr, …).
    /// </summary>
    public static Pedigree ForFamily(UnitFamilyName family, UnitSystemType system)
        => FamilySignatures.TryGet(family, out var signature)
            ? new Pedigree(signature, Units.CanonicalScale.Of(family, system))   // the static table, not this struct's property
            : Untracked;

    /// <summary>a × b — signatures add, scales multiply. Untracked is absorbing.</summary>
    public Pedigree Multiply(Pedigree other)
        => IsTracked && other.IsTracked
            ? new Pedigree(Signature.Multiply(other.Signature), CanonicalScale * other.CanonicalScale)
            : Untracked;

    /// <summary>a ÷ b — signatures subtract, scales divide. Untracked is absorbing.</summary>
    public Pedigree Divide(Pedigree other)
        => IsTracked && other.IsTracked
            ? new Pedigree(Signature.Divide(other.Signature), CanonicalScale / other.CanonicalScale)
            : Untracked;

    public bool Equals(Pedigree other)
        => Signature.Equals(other.Signature) && CanonicalScale.Equals(other.CanonicalScale);
    public override bool Equals(object? obj) => obj is Pedigree other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(Signature, CanonicalScale);

    /// <summary>Lazy — display/reporting only, never the evaluation hot path.</summary>
    public override string ToString()
        => IsTracked ? Signature.ToCanonical() : "untracked";
}
