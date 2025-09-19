using System;
using System.Collections.Generic;
using FoundryRulesAndUnits.Units;

namespace FoundryRulesAndUnits.Units;

public interface IUnitSystemSpecification
{
    string SystemName { get; }
    string SystemDescription { get; }
    IReadOnlyList<UnitDefinition> UnitDefinitions { get; }

    /// <summary>
    /// Gets all unit symbols in the system (cached for performance)
    /// </summary>
    List<string> GetAllUnitSymbols();

    /// <summary>
    /// Gets all base units in the system (cached for performance)
    /// </summary>
    List<UnitDefinition> GetAllBaseUnits();

    /// <summary>
    /// Gets a dictionary mapping each unit family to its base unit (cached for performance)
    /// </summary>
    Dictionary<UnitFamilyName, UnitDefinition> GetBaseUnitsByFamily();

    /// <summary>
    /// Gets a dictionary mapping each unit family to all units in that family (cached for performance)
    /// </summary>
    Dictionary<UnitFamilyName, List<UnitDefinition>> GetAllUnitsByFamily();

    /// <summary>
    /// Gets a dictionary mapping each unit symbol to its unit family (cached for performance)
    /// </summary>
    Dictionary<string, UnitFamilyName> GetSymbolToFamilyMap();
}
