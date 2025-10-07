using System;
using System.Text.RegularExpressions;
using FoundryRulesAndUnits.Units;

namespace FoundryRulesAndUnits.Units
{
    /// <summary>
    /// Parser for variable declaration syntax:
    /// Simple: dim1|m : 100 in
    /// Explicit: dim1:distance(km) : 5000 ft
    /// </summary>
    public class VariableDeclarationParser
    {
        /// <summary>
        /// Represents a parsed variable declaration
        /// </summary>
        public class VariableDeclaration
        {
            public string VariableName { get; set; } = "";
            public string? DisplayUnits { get; set; }
            public UnitFamilyName? ExplicitFamily { get; set; }
            public string? FamilyDisplayUnits { get; set; }
            public double InputValue { get; set; }
            public string InputUnits { get; set; } = "";
            public bool IsExplicitFamily => ExplicitFamily.HasValue;
        }

        // Regex patterns for parsing
        private static readonly Regex SimplePattern = new Regex(
            @"^(?<varName>\w+)(\|(?<displayUnits>\w+))?\s*:\s*(?<value>[\d.]+)\s*(?<inputUnits>\w+)$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex ExplicitPattern = new Regex(
            @"^(?<varName>\w+):(?<family>\w+)\((?<familyUnits>\w+)\)\s*:\s*(?<value>[\d.]+)\s*(?<inputUnits>\w+)$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Parse a variable declaration string into structured components
        /// </summary>
        /// <param name="declaration">The variable declaration string</param>
        /// <returns>Parsed variable declaration or null if invalid</returns>
        public static VariableDeclaration? Parse(string declaration)
        {
            if (string.IsNullOrWhiteSpace(declaration))
                return null;

            declaration = declaration.Trim();

            // Try explicit family pattern first: dim1:distance(km) : 5000 ft
            var explicitMatch = ExplicitPattern.Match(declaration);
            if (explicitMatch.Success)
            {
                var familyName = explicitMatch.Groups["family"].Value.ToLower();
                var familyEnum = ParseFamilyName(familyName);
                
                if (familyEnum == UnitFamilyName.None)
                    return null; // Invalid family name

                return new VariableDeclaration
                {
                    VariableName = explicitMatch.Groups["varName"].Value,
                    ExplicitFamily = familyEnum,
                    FamilyDisplayUnits = explicitMatch.Groups["familyUnits"].Value,
                    DisplayUnits = explicitMatch.Groups["familyUnits"].Value, // Use family units as display
                    InputValue = double.Parse(explicitMatch.Groups["value"].Value),
                    InputUnits = explicitMatch.Groups["inputUnits"].Value
                };
            }

            // Try simple pattern: dim1|m : 100 in
            var simpleMatch = SimplePattern.Match(declaration);
            if (simpleMatch.Success)
            {
                return new VariableDeclaration
                {
                    VariableName = simpleMatch.Groups["varName"].Value,
                    DisplayUnits = simpleMatch.Groups["displayUnits"].Value,
                    InputValue = double.Parse(simpleMatch.Groups["value"].Value),
                    InputUnits = simpleMatch.Groups["inputUnits"].Value
                };
            }

            return null; // No pattern matched
        }

        /// <summary>
        /// Create a MeasuredValue from a parsed variable declaration
        /// </summary>
        /// <param name="declaration">Parsed variable declaration</param>
        /// <param name="unitSystem">Unit system to use for creation</param>
        /// <returns>Configured MeasuredValue</returns>
        public static MeasuredValue CreateMeasuredValue(VariableDeclaration declaration, UnitSystem unitSystem)
        {
            MeasuredValue result;

            if (declaration.IsExplicitFamily)
            {
                // Create with explicit family
                result = unitSystem.CreateMeasuredValue(
                    declaration.ExplicitFamily!.Value,
                    declaration.InputValue,
                    declaration.InputUnits);
                    
                // Set display units from family specification
                if (!string.IsNullOrEmpty(declaration.FamilyDisplayUnits))
                {
                    result.SetDisplayUnits(declaration.FamilyDisplayUnits);
                }
            }
            else
            {
                // Create with smart defaults - let unit system determine family from input units
                result = unitSystem.CreateMeasuredValueFromParsableUnit(declaration.InputUnits, declaration.InputValue);
                
                // Set display units if specified
                if (!string.IsNullOrEmpty(declaration.DisplayUnits))
                {
                    result.SetDisplayUnits(declaration.DisplayUnits);
                }
            }

            return result;
        }

        /// <summary>
        /// Convert family name string to enum
        /// </summary>
        private static UnitFamilyName ParseFamilyName(string familyName)
        {
            return familyName.ToLower() switch
            {
                "length" => UnitFamilyName.Length,
                "distance" => UnitFamilyName.Distance,
                "time" => UnitFamilyName.Time,
                "duration" => UnitFamilyName.Duration,
                "angle" => UnitFamilyName.Angle,
                "bearing" => UnitFamilyName.Bearing,
                "mass" => UnitFamilyName.Mass,
                "speed" => UnitFamilyName.Speed,
                "area" => UnitFamilyName.Area,
                "volume" => UnitFamilyName.Volume,
                "force" => UnitFamilyName.Force,
                "power" => UnitFamilyName.Power,
                "voltage" => UnitFamilyName.Voltage,
                "current" => UnitFamilyName.Current,
                "temperature" => UnitFamilyName.Temperature,
                _ => UnitFamilyName.None
            };
        }

        /// <summary>
        /// Generate human-readable description of a variable declaration
        /// </summary>
        public static string Describe(VariableDeclaration declaration)
        {
            var description = $"Variable '{declaration.VariableName}'";
            
            if (declaration.IsExplicitFamily)
            {
                description += $" (Family: {declaration.ExplicitFamily})";
                description += $" displays as {declaration.FamilyDisplayUnits}";
            }
            else if (!string.IsNullOrEmpty(declaration.DisplayUnits))
            {
                description += $" displays as {declaration.DisplayUnits}";
            }
            
            description += $", initialized with {declaration.InputValue} {declaration.InputUnits}";
            
            return description;
        }
    }
}