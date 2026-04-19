
using FoundryRulesAndUnits.Units;
using System;
using System.Collections.Generic;
using System.Linq;

public class UnitGroup
{
    public UnitFamilyName Family { get; private set; }
    public UnitSystemType SystemType { get; private set; }
    public UnitDefinition BaseUnit { get; private set; }
    public List<UnitDefinition> Members { get; private set; }
    public bool IsParserAccessible { get; private set; }  // Parser accessibility at family level
    public UnitFamilyName? AlternativeFamily { get; private set; }  // Non-parsable alternative family

    public UnitGroup(UnitFamilyName family, UnitSystemType systemType, UnitDefinition baseUnit, List<UnitDefinition> members, bool isParserAccessible = true, UnitFamilyName? alternativeFamily = null)
    {
        Family = family;
        SystemType = systemType;
        BaseUnit = baseUnit;
        Members = members;
        IsParserAccessible = isParserAccessible;
        AlternativeFamily = alternativeFamily;
    }

    /// <summary>
    /// Check if a unit symbol is valid for this unit group
    /// </summary>
    public bool IsValidUnit(string unitSymbol)
    {
        return Members.Any(u => u.Symbol == unitSymbol) || BaseUnit.Symbol == unitSymbol;
    }

    /// <summary>
    /// Check if this unit group is compatible with another family for arithmetic operations
    /// Compatible means same family or alternative family relationship
    /// </summary>
    public bool IsCompatibleWith(UnitFamilyName otherFamily)
    {
        return Family == otherFamily || AlternativeFamily == otherFamily;
    }

    /// <summary>
    /// Convert between units within this group
    /// </summary>
    public double Convert(double value, string fromUnit, string toUnit)
    {
        fromUnit = UnitSystem.NormalizeUnit(fromUnit);
        toUnit   = UnitSystem.NormalizeUnit(toUnit);
        if (fromUnit == toUnit) return value;

        var fromDef = Members.FirstOrDefault(u => u.Symbol == fromUnit) ?? 
                     (BaseUnit.Symbol == fromUnit ? BaseUnit : null);
        var toDef = Members.FirstOrDefault(u => u.Symbol == toUnit) ?? 
                   (BaseUnit.Symbol == toUnit ? BaseUnit : null);

        if (fromDef == null) throw new ArgumentException($"Unknown unit: {fromUnit}");
        if (toDef == null) throw new ArgumentException($"Unknown unit: {toUnit}");

        // Convert using the embedded functions: from -> base -> to
        var valueInBase = fromDef.ToBaseUnit?.Invoke(value) ?? value;
        return toDef.FromBaseUnit?.Invoke(valueInBase) ?? valueInBase;
    }
}