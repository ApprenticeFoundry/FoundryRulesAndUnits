namespace FoundryRulesAndUnits.Units;


/// <summary>
/// Enhanced unit definition with embedded conversion functions
/// Eliminates the need for massive conversion matrices by storing conversions in each unit
/// Uses UnitFamilyName enum for type safety and consistency
/// 
/// ARCHITECTURE: Single Definition Per Unit
/// - Symbol: ASCII canonical form for internal storage and lookup (e.g., "ohm", "kohm")
/// - UnicodeSymbol: Optional Unicode form for display (e.g., "Ω", "kΩ")
/// - Parser normalizes Unicode input to ASCII Symbol for lookup
/// - Display formatter uses UnicodeSymbol when available, falls back to Symbol
/// </summary>
public record UnitDefinition(
    string Symbol,
    string Name,
    UnitFamilyName Family,
    bool IsBaseUnit,
    Func<double, double>? ToBaseUnit = null,     // Convert FROM this unit TO base unit
    Func<double, double>? FromBaseUnit = null,   // Convert FROM base unit TO this unit
    string? UnicodeSymbol = null                  // Optional Unicode display form
)
{
    /// <summary>
    /// For base units, both functions are identity (x => x)
    /// </summary>
    /// <param name="symbol">ASCII canonical form (e.g., "ohm")</param>
    /// <param name="name">Human-readable name</param>
    /// <param name="family">Unit family</param>
    /// <param name="unicodeSymbol">Optional Unicode display form (e.g., "Ω")</param>
    public static UnitDefinition BaseUnit(string symbol, string name, UnitFamilyName family, string? unicodeSymbol = null) =>
        new(symbol, name, family, true, x => x, x => x, unicodeSymbol);

    /// <summary>
    /// For derived units, provide conversion to/from base unit
    /// </summary>
    /// <param name="symbol">ASCII canonical form</param>
    /// <param name="name">Human-readable name</param>
    /// <param name="family">Unit family</param>
    /// <param name="toBase">Function to convert to base unit</param>
    /// <param name="fromBase">Function to convert from base unit</param>
    /// <param name="unicodeSymbol">Optional Unicode display form</param>
    public static UnitDefinition DerivedUnit(string symbol, string name, UnitFamilyName family,
        Func<double, double> toBase, Func<double, double> fromBase, string? unicodeSymbol = null) =>
        new(symbol, name, family, false, toBase, fromBase, unicodeSymbol);

    /// <summary>
    /// Simple linear conversion (multiplication/division)
    /// Most common case - automatically generates inverse function
    /// </summary>
    /// <param name="symbol">ASCII canonical form</param>
    /// <param name="name">Human-readable name</param>
    /// <param name="family">Unit family</param>
    /// <param name="factor">Conversion factor to base unit</param>
    /// <param name="unicodeSymbol">Optional Unicode display form</param>
    public static UnitDefinition LinearUnit(string symbol, string name, UnitFamilyName family, double factor, string? unicodeSymbol = null) =>
        new(symbol, name, family, false,
            x => x * factor,      // e.g., inches to feet: x * (1/12)
            x => x / factor,      // e.g., feet to inches: x / (1/12) = x * 12
            unicodeSymbol);

    /// <summary>
    /// Convert any value from this unit to any other unit in the same family
    /// This replaces the entire conversion matrix lookup system!
    /// </summary>
    public double ConvertToBase(double value)
    {
        return ToBaseUnit?.Invoke(value) ?? throw new InvalidOperationException($"No ToBaseUnit function for {Symbol}");
    }

    public double ConvertFromBase(double value)
    {
        if (FromBaseUnit == null) throw new InvalidOperationException($"No FromBaseUnit function for {Symbol}");

        // Only a read-out in a NON-base unit has completed a round trip worth cleaning. Storing
        // INTO the base unit must stay exact, because everything else is computed from it:
        // snapping there truncated 9 deg to 0.157079632679 rad, which reads back 8.99999999997.
        var converted = FromBaseUnit.Invoke(value);
        return IsBaseUnit ? converted : SnapConversionResidue(converted);
    }

    /// <summary>
    /// Significant digits a converted value is trusted to. A double carries ~15.95 decimal
    /// digits, so 12 leaves three orders of magnitude of headroom for residue accumulated by a
    /// SUM before the conversion, while still being finer than any physical measurement this
    /// framework carries (12 digits is picometres on a kilometre).
    /// </summary>
    private const int ConversionSignificantDigits = 12;

    /// <summary>
    /// Erase the floating-point residue a hub-and-spoke conversion leaves behind.
    ///
    /// Every non-base unit reaches its sibling through the family base — value x factor, then
    /// / factor — and exact values do not survive two IEEE 754 operations. `6 U` stored as
    /// 6 * 0.04445 m reads back as 6.000000000000001 U; `3 in` reads back as 2.9999999999999996 in.
    ///
    /// The residue is ~1e-16 relative, far below any measurement, but it is NOT harmless,
    /// because the rounding family amplifies it into whole units. Measured 2026-09-07 in the
    /// Foundry Framework Lab: a 42 U rack holding seven 6 U chassis - exactly full - summed to
    /// 42.000000000000014 U, and `ceiling` reported 43 U used, so the rack failed its own
    /// height constraint. `floor` fails the same way downward, reporting 5 U for a 6 U load.
    /// No formula-level function can fix that, because the residue is already in the operands
    /// before the function is called. It has to be gone by the time a value is read in a unit.
    ///
    /// Snapping the ANSWER is honest: two values that differ only past the 12th significant
    /// digit were never distinguishable as measurements, and the conversion is what introduced
    /// the difference. Base-unit arithmetic is untouched - this is the boundary where a value
    /// is read back out in a named unit, nothing else.
    /// </summary>
    public static double SnapConversionResidue(double value)
    {
        if (value == 0.0 || !double.IsFinite(value)) return value;

        // Digits after the point that leave ConversionSignificantDigits significant ones.
        var magnitude = (int)Math.Floor(Math.Log10(Math.Abs(value)));
        var decimals = ConversionSignificantDigits - 1 - magnitude;

        // Outside Math.Round's domain the value is either enormous or denormal-small; in both
        // cases there is no residue worth chasing, and clamping would corrupt real digits.
        if (decimals is < 0 or > 15) return value;

        return Math.Round(value, decimals, MidpointRounding.AwayFromZero);
    }

    /// <summary>
    /// Check if this unit can convert to the target unit (same family)
    /// </summary>
    public bool HasSameUnitFamily(UnitDefinition targetUnit) => Family == targetUnit.Family;

    /// <summary>
    /// Get the display symbol (Unicode if available, otherwise ASCII)
    /// </summary>
    public string DisplaySymbol => UnicodeSymbol ?? Symbol;

    /// <summary>
    /// Get the code generation symbol (always ASCII for human typing)
    /// </summary>
    public string CodeSymbol => Symbol;

    /// <summary>
    /// Check if this unit has a Unicode display representation
    /// </summary>
    public bool HasUnicodeRepresentation => !string.IsNullOrEmpty(UnicodeSymbol);

}
