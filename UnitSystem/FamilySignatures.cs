using System;
using System.Collections.Generic;
using System.Linq;

namespace FoundryRulesAndUnits.Units;

/// <summary>
/// The one missing declaration (SPEC_DIMENSIONAL_PEDIGREE §3.4): each unit family's
/// dimensional signature, stated once. Everything downstream — closed multiplication and
/// division, the reverse index, AS-function validation — derives from this table.
///
/// NAMES OUTRANK STRUCTURE. Families deliberately absent from this table are nominal-only:
/// they carry meaning the exponent axes cannot express, never compose directly, and are
/// never auto-promoted.
///   - WorkTime          — semantically true, dimensionally false (a work day is 8 hours)
///   - Angle, Bearing    — dimensionless by physics; meaning is entirely nominal (no angle
///                         axis by design — Torque-vs-Energy is answered by intent, §11)
///   - AngularVelocity, AngularAcceleration — identity depends on the absent angle axis;
///                         structurally they would collide with Frequency (rad/s vs Hz)
///   - Percent, Verdict  — zero signature; must never name a raw scalar
///   - Temperature       — affine conversions do not compose linearly (Kelvin-only entry
///                         is open question §12.2)
///   - Radioactivity     — Bq vs Hz is a purely semantic distinction; nominal-only
///   - AmountOfSubstance, Illuminance, LuminousFlux, LuminousIntensity — would need mole /
///                         candela axes the 8-lane design does not carry
/// </summary>
public static class FamilySignatures
{
    private static readonly Dictionary<UnitFamilyName, DimensionalSignature> Table = new()
    {
        // ── the coherent core ────────────────────────────────────────────────
        [UnitFamilyName.Length]       = DimensionalSignature.Of(length: 1),
        [UnitFamilyName.Distance]     = DimensionalSignature.Of(length: 1),   // twin of Length (base km — scale handled by pedigree)
        [UnitFamilyName.Mass]         = DimensionalSignature.Of(mass: 1),
        [UnitFamilyName.Time]         = DimensionalSignature.Of(time: 1),
        [UnitFamilyName.Duration]     = DimensionalSignature.Of(time: 1),     // twin of Time
        [UnitFamilyName.Area]         = DimensionalSignature.Of(length: 2),
        [UnitFamilyName.Volume]       = DimensionalSignature.Of(length: 3),
        [UnitFamilyName.Speed]        = DimensionalSignature.Of(length: 1, time: -1),
        [UnitFamilyName.Acceleration] = DimensionalSignature.Of(length: 1, time: -2),
        [UnitFamilyName.Force]        = DimensionalSignature.Of(length: 1, mass: 1, time: -2),
        [UnitFamilyName.Pressure]     = DimensionalSignature.Of(length: -1, mass: 1, time: -2),
        [UnitFamilyName.Stiffness]    = DimensionalSignature.Of(mass: 1, time: -2),           // N/m — bug 030/031 (spring constants)
        [UnitFamilyName.Damping]      = DimensionalSignature.Of(mass: 1, time: -1),           // Ns/m — bug 030/031 (damping coefficients)
        [UnitFamilyName.Energy]       = DimensionalSignature.Of(length: 2, mass: 1, time: -2),
        [UnitFamilyName.Torque]       = DimensionalSignature.Of(length: 2, mass: 1, time: -2), // == Energy: AMBIGUOUS on purpose
        [UnitFamilyName.Power]        = DimensionalSignature.Of(length: 2, mass: 1, time: -3),
        [UnitFamilyName.Momentum]     = DimensionalSignature.Of(length: 1, mass: 1, time: -1),
        [UnitFamilyName.Inertia]      = DimensionalSignature.Of(length: 2, mass: 1),
        [UnitFamilyName.Frequency]    = DimensionalSignature.Of(time: -1),

        // ── electrical ───────────────────────────────────────────────────────
        [UnitFamilyName.Current]              = DimensionalSignature.Of(current: 1),
        [UnitFamilyName.ElectricCharge]       = DimensionalSignature.Of(time: 1, current: 1),
        [UnitFamilyName.Voltage]              = DimensionalSignature.Of(length: 2, mass: 1, time: -3, current: -1),
        [UnitFamilyName.Resistance]           = DimensionalSignature.Of(length: 2, mass: 1, time: -3, current: -2),
        [UnitFamilyName.Conductance]          = DimensionalSignature.Of(length: -2, mass: -1, time: 3, current: 2),
        [UnitFamilyName.Capacitance]          = DimensionalSignature.Of(length: -2, mass: -1, time: 4, current: 2),
        [UnitFamilyName.Inductance]           = DimensionalSignature.Of(length: 2, mass: 1, time: -2, current: -2),
        [UnitFamilyName.MagneticFlux]         = DimensionalSignature.Of(length: 2, mass: 1, time: -2, current: -1),
        [UnitFamilyName.MagneticFluxDensity]  = DimensionalSignature.Of(mass: 1, time: -2, current: -1),

        // ── radiation dose (Gy vs Sv: a second deliberate ambiguity — named, not structural) ──
        [UnitFamilyName.AbsorbedDose]   = DimensionalSignature.Of(length: 2, time: -2),
        [UnitFamilyName.EquivalentDose] = DimensionalSignature.Of(length: 2, time: -2),

        // ── counting and commerce ────────────────────────────────────────────
        [UnitFamilyName.Quantity]        = DimensionalSignature.Of(quantity: 1),
        [UnitFamilyName.QuantityFlow]    = DimensionalSignature.Of(quantity: 1, time: -1),
        [UnitFamilyName.Currency]        = DimensionalSignature.Of(currency: 1),
        [UnitFamilyName.CostPerQuantity] = DimensionalSignature.Of(currency: 1, quantity: -1),
        [UnitFamilyName.CostPerTime]     = DimensionalSignature.Of(currency: 1, time: -1),
        [UnitFamilyName.CostPerMass]     = DimensionalSignature.Of(currency: 1, mass: -1),
        [UnitFamilyName.CostPerLength]   = DimensionalSignature.Of(currency: 1, length: -1),
        [UnitFamilyName.CostPerArea]     = DimensionalSignature.Of(currency: 1, length: -2),
        [UnitFamilyName.CostPerVolume]   = DimensionalSignature.Of(currency: 1, length: -3),
        [UnitFamilyName.CostPerEnergy]   = DimensionalSignature.Of(currency: 1, length: -2, mass: -1, time: 2),

        // ── data ─────────────────────────────────────────────────────────────
        [UnitFamilyName.DataStorage] = DimensionalSignature.Of(data: 1),
        [UnitFamilyName.DataFlow]    = DimensionalSignature.Of(data: 1, time: -1),
    };

    /// <summary>
    /// Reverse index: signature → every family that claims it (SPEC §3.5). Built once from
    /// the table. Returns ALL claimants — filtering to parser-accessible candidates is the
    /// promotion layer's concern, not the index's.
    /// </summary>
    private static readonly Dictionary<DimensionalSignature, List<UnitFamilyName>> ReverseIndex =
        Table.GroupBy(kv => kv.Value)
             .ToDictionary(g => g.Key, g => g.Select(kv => kv.Key).ToList());

    private static readonly IReadOnlyList<UnitFamilyName> NoCandidates = Array.Empty<UnitFamilyName>();

    /// <summary>
    /// The family's dimensional signature, if it has one. False for nominal-only families
    /// (WorkTime, Bearing, Percent, ...) — absence means "never compose, never promote".
    /// </summary>
    public static bool TryGet(UnitFamilyName family, out DimensionalSignature signature)
        => Table.TryGetValue(family, out signature);

    /// <summary>
    /// All families claiming this signature. Empty for Anonymous signatures — and ALWAYS
    /// empty for dimensionless, so a bare number can never acquire a family from here.
    /// </summary>
    public static IReadOnlyList<UnitFamilyName> CandidatesFor(DimensionalSignature signature)
    {
        if (signature.IsDimensionless) return NoCandidates;
        return ReverseIndex.TryGetValue(signature, out var families) ? families : NoCandidates;
    }

    /// <summary>Families that participate in the algebra (have a declared signature).</summary>
    public static IEnumerable<UnitFamilyName> IncludedFamilies => Table.Keys;
}
