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
        return FromBaseUnit?.Invoke(value) ?? throw new InvalidOperationException($"No FromBaseUnit function for {Symbol}");
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
