using System;
using System.Collections.Generic;

namespace FoundryRulesAndUnits.Units;

/// <summary>
/// Which family owns a unit symbol that more than one family registers.
///
/// Symbols are registered per family with no collision detection, so when two families claim the
/// same text the lookup cache simply keeps whichever was registered LAST. That is registration
/// order deciding meaning: `F` is Fahrenheit (Temperature) and farads (Capacitance), farads is
/// registered later, so every `|F` parameter silently resolved to farads — the fountain model's
/// `WaterTemp|F: 68` read as 68 farads rather than 68 degrees.
///
/// This table states the answer instead of letting file order imply it. It is SYMBOL-level, not
/// family-level: declaring Temperature the owner of `F` costs Capacitance only that one symbol,
/// leaving uF/nF/pF/mF — which never collided — reachable exactly as before. That is the
/// difference from the family-level `isParserAccessible` switch in UnitSystemSpecificationBase,
/// which is the right tool when an ENTIRE family should be function-only (Distance, Bearing,
/// Time, WorkTime) and too blunt when a single symbol is contested.
///
/// The language service reads this same table (Kn.Notation.Syntax's UnitTable) so KN016 never
/// reports a conflict the runtime has already decided. One rule, one place — the two used to
/// disagree, and the editor kept flagging `F` after the runtime had stopped being ambiguous.
/// </summary>
public static class UnitSymbolOwnership
{
    /// <summary>Contested symbol → the family that keeps it when parsed bare (`|F`).</summary>
    public static readonly IReadOnlyDictionary<string, UnitFamilyName> Owners =
        new Dictionary<string, UnitFamilyName>(StringComparer.Ordinal)
        {
            // Temperature beats the electrical families on both letters. Steve, 2026-09-15:
            // people working with these models "will be worried about temperature way before"
            // capacitance — and a sweep of every .kn in the workspace found `|F` used for
            // temperature and never once for farads. Celsius/coulomb is the identical pair.
            ["F"] = UnitFamilyName.Temperature, // Fahrenheit, not farads    (Capacitance)
            ["C"] = UnitFamilyName.Temperature, // Celsius,    not coulombs  (ElectricCharge)

            // Still undeclared, deliberately — each needs a modelling call, not a default:
            //   rad      Angle vs AbsorbedDose        (SI)
            //   rpm/rps  Frequency vs AngularVelocity (MKS)
            // Adding one is a single line here; nothing else changes.
        };

    /// <summary>The family that owns this symbol, or null if no one has claimed it.</summary>
    public static UnitFamilyName? OwnerOf(string symbol) =>
        Owners.TryGetValue(symbol, out var owner) ? owner : null;

    /// <summary>
    /// True when <paramref name="family"/> must NOT take this symbol because another family owns
    /// it. False for an unclaimed symbol, so an undeclared collision keeps its old behaviour
    /// rather than silently losing a unit.
    /// </summary>
    public static bool IsClaimedByAnother(string symbol, UnitFamilyName family) =>
        Owners.TryGetValue(symbol, out var owner) && owner != family;
}
