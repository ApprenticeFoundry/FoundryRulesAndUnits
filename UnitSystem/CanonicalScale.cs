namespace FoundryRulesAndUnits.Units;

/// <summary>
/// The scale side of the dimensional pedigree (SPEC_DIMENSIONAL_PEDIGREE §4, phase 3 — "scale
/// correction"): the multiplier that takes a family's BASE-unit value in a given unit system to
/// the canonical frame (m · kg · s · K · A · ea · USD · B). Most MKS families are already
/// coherent with the canonical frame (m, kg, s, N, Pa, J, USD, USD/kg …) and scale by 1. The
/// exceptions are the families whose base unit was chosen for human convenience rather than
/// coherence — Distance in km, CostPerTime in USD per HOUR — and they are exactly the families
/// the spec's "×1000 mis-tag" example is about: a value stored in USD/hr multiplied by seconds
/// is off by 3600 unless this table is consulted.
///
/// The table below is MKS. Every other system is worked out from it: one of that system's base
/// units, in the MKS base unit, then the MKS scale. Added 2026-08-30 for the first live promotion
/// path (FoundryFrameworkLab Drafts/DESIGN-datatable-and-carrier-functions.md Q6).
/// </summary>
public static class CanonicalScale
{
    private static readonly System.Collections.Concurrent.ConcurrentDictionary<(UnitFamilyName, UnitSystemType), double> _otherSystems = new();

    public static double Of(UnitFamilyName family, UnitSystemType system) =>
        system == UnitSystemType.MKS ? OfMks(family) : _otherSystems.GetOrAdd((family, system), static key => Derive(key.Item1, key.Item2));

    private static double Derive(UnitFamilyName family, UnitSystemType system)
    {
        var groups = UnitSystem.Shared(system).Current.GetUnitGroups();
        var mksBase = UnitSystem.Shared().GetBaseUnitForFamily(family);
        if (mksBase.Length == 0 || !groups.TryGetValue(family, out var group)) return 1.0;

        // The difference between two values, so that an offset (Temperature) does not count as scale.
        var own = group.BaseUnit.Symbol;
        return (group.Convert(1.0, own, mksBase) - group.Convert(0.0, own, mksBase)) * OfMks(family);
    }

    private static double OfMks(UnitFamilyName family)
    {
        return family switch
        {
            UnitFamilyName.Distance      => 1000.0,          // base km        → m
            UnitFamilyName.CostPerTime   => 1.0 / 3600.0,    // base USD/hr    → USD/s
            UnitFamilyName.CostPerVolume => 1000.0,          // base USD/L     → USD/m³
            UnitFamilyName.CostPerEnergy => 1.0 / 3.6e6,     // base USD/kWh   → USD/J
            _                            => 1.0,
        };
    }
}
