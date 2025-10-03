using System;
using System.Collections.Generic;
using System.Linq;
using FoundryRulesAndUnits.Units;

namespace FoundryRulesAndUnits.Units;

/// <summary>
/// Defines compatibility relationships between unit families
/// Enables mixing compatible families in equations (e.g., Length + Distance)
/// </summary>
public class UnitFamilyCompatibility
{
    /// <summary>
    /// Compatibility groups - families within a group can be mixed in equations
    /// </summary>
    private static readonly Dictionary<string, List<UnitFamilyName>> CompatibilityGroups = new()
    {
        // Spatial measurements - all represent distances/positions in space
        ["Spatial"] = new List<UnitFamilyName>
        {
            UnitFamilyName.Length,
            UnitFamilyName.Distance
        },
        
        // Temporal measurements - all represent time quantities
        ["Temporal"] = new List<UnitFamilyName>
        {
            UnitFamilyName.Duration,
            UnitFamilyName.Time
        },
        
        // Angular measurements - all represent rotational quantities
        ["Angular"] = new List<UnitFamilyName>
        {
            UnitFamilyName.Angle,
            UnitFamilyName.Bearing
        },
        
        // Mass measurements - all represent mass/weight quantities (only Mass exists currently)
        ["Mass"] = new List<UnitFamilyName>
        {
            UnitFamilyName.Mass
        },
        
        // Electrical quantities that might be compatible in some contexts
        ["Electrical"] = new List<UnitFamilyName>
        {
            UnitFamilyName.Voltage,
            UnitFamilyName.Current,
            UnitFamilyName.Power,
            UnitFamilyName.Energy,
            UnitFamilyName.Resistance,
            UnitFamilyName.Capacitance,
            UnitFamilyName.ElectricCharge,
            UnitFamilyName.Conductance,
            UnitFamilyName.Inductance
        }
    };

    /// <summary>
    /// Result family preferences - when mixing families, which one should the result be?
    /// </summary>
    private static readonly Dictionary<string, UnitFamilyName> GroupResultFamilies = new()
    {
        ["Spatial"] = UnitFamilyName.Length,      // Default to Length for spatial results
        ["Temporal"] = UnitFamilyName.Duration,   // Default to Duration for temporal results
        ["Angular"] = UnitFamilyName.Angle,       // Default to Angle for angular results
        ["Mass"] = UnitFamilyName.Mass           // Default to Mass for mass results
    };

    /// <summary>
    /// Check if two unit families are compatible for mathematical operations
    /// </summary>
    public static bool AreCompatible(UnitFamilyName family1, UnitFamilyName family2)
    {
        // Same family is always compatible
        if (family1 == family2) return true;
        
        // Check if both families are in the same compatibility group
        foreach (var group in CompatibilityGroups.Values)
        {
            if (group.Contains(family1) && group.Contains(family2))
            {
                return true;
            }
        }
        
        return false;
    }

    /// <summary>
    /// Get the result family when combining two compatible families
    /// Uses preference rules to determine the most appropriate result type
    /// </summary>
    public static UnitFamilyName GetResultFamily(UnitFamilyName family1, UnitFamilyName family2)
    {
        if (!AreCompatible(family1, family2))
        {
            throw new ArgumentException($"Incompatible unit families: {family1} and {family2}");
        }
        
        // If same family, keep it
        if (family1 == family2) return family1;
        
        // Find which group they belong to and return the preferred result family
        foreach (var (groupName, families) in CompatibilityGroups)
        {
            if (families.Contains(family1) && families.Contains(family2))
            {
                return GroupResultFamilies[groupName];
            }
        }
        
        // Fallback (shouldn't reach here if AreCompatible works correctly)
        return family1;
    }

    /// <summary>
    /// Get all compatible families for a given family
    /// </summary>
    public static List<UnitFamilyName> GetCompatibleFamilies(UnitFamilyName family)
    {
        foreach (var group in CompatibilityGroups.Values)
        {
            if (group.Contains(family))
            {
                return group.ToList();
            }
        }
        
        // If not in any group, only compatible with itself
        return new List<UnitFamilyName> { family };
    }

    /// <summary>
    /// Get the compatibility group name for a family (useful for debugging/UI)
    /// </summary>
    public static string? GetCompatibilityGroup(UnitFamilyName family)
    {
        foreach (var (groupName, families) in CompatibilityGroups)
        {
            if (families.Contains(family))
            {
                return groupName;
            }
        }
        return null;
    }
}