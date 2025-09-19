namespace FoundryRulesAndUnits.Units;


/// <summary>
/// Enhanced unit definition with embedded conversion functions
/// Eliminates the need for massive conversion matrices by storing conversions in each unit
/// Uses UnitFamilyName enum for type safety and consistency
/// </summary>
public record UnitDefinition(
    string Symbol,
    string Name,
    UnitFamilyName Family,
    bool IsBaseUnit,
    Func<double, double>? ToBaseUnit = null,     // Convert FROM this unit TO base unit
    Func<double, double>? FromBaseUnit = null    // Convert FROM base unit TO this unit
)
{
    /// <summary>
    /// For base units, both functions are identity (x => x)
    /// </summary>
    public static UnitDefinition BaseUnit(string symbol, string name, UnitFamilyName family) =>
        new(symbol, name, family, true, x => x, x => x);

    /// <summary>
    /// For derived units, provide conversion to/from base unit
    /// </summary>
    public static UnitDefinition DerivedUnit(string symbol, string name, UnitFamilyName family,
        Func<double, double> toBase, Func<double, double> fromBase) =>
        new(symbol, name, family, false, toBase, fromBase);

    /// <summary>
    /// Simple linear conversion (multiplication/division)
    /// Most common case - automatically generates inverse function
    /// </summary>
    public static UnitDefinition LinearUnit(string symbol, string name, UnitFamilyName family, double factor) =>
        new(symbol, name, family, false,
            toBase: x => x * factor,      // e.g., inches to feet: x * (1/12)
            fromBase: x => x / factor);   // e.g., feet to inches: x / (1/12) = x * 12

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
        return FromBaseUnit?.Invoke(value) ?? throw new InvalidOperationException($"No FromBaseUnit function for {targetUnit.Symbol}");
    }

    /// <summary>
    /// Check if this unit can convert to the target unit (same family)
    /// </summary>
    public bool HasSameUnitFamily(UnitDefinition targetUnit) => Family == targetUnit.Family;

}
