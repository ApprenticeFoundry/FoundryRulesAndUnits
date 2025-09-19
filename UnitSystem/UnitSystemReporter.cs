using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FoundryRulesAndUnits.Extensions;

namespace FoundryRulesAndUnits.Units
{
    /// <summary>
    /// Generates comprehensive reports showing base units and all supported conversions
    /// for each unit system. Useful for documentation, debugging, and system validation.
    /// </summary>
    public class UnitSystemReporter
    {
        /// <summary>
        /// Generates a complete report for all unit systems showing base units and conversions
        /// </summary>
        public static string GenerateCompleteReport()
        {
            var report = new StringBuilder();
            
            report.AppendLine("═══════════════════════════════════════════════════════════════");
            report.AppendLine("                    UNIT SYSTEM COMPREHENSIVE REPORT");
            report.AppendLine("═══════════════════════════════════════════════════════════════");
            report.AppendLine();
            
            // Test each unit system
            var systems = new[]
            {
                new { Type = UnitSystemType.MKS, Name = "MKS (Meter-Kilogram-Second)", Description = "SI Standard - General Engineering" },
                new { Type = UnitSystemType.IPS, Name = "IPS (Inch-Pound-Second)", Description = "Imperial - Manufacturing/Mechanical" },
                new { Type = UnitSystemType.FPS, Name = "FPS (Foot-Pound-Second)", Description = "Imperial - Civil/Construction" },
                new { Type = UnitSystemType.CGS, Name = "CGS (Centimeter-Gram-Second)", Description = "Metric - Chemistry/Small Scale" },
                new { Type = UnitSystemType.mmNs, Name = "mmNs (Millimeter-Newton-Second)", Description = "Metric - Precision/CAD Systems" }
            };

            foreach (var system in systems)
            {
                report.AppendLine(GenerateSystemReport(system.Type, system.Name, system.Description));
                report.AppendLine();
            }
            
            report.AppendLine("═══════════════════════════════════════════════════════════════");
            report.AppendLine("                         REPORT COMPLETE");
            report.AppendLine("═══════════════════════════════════════════════════════════════");
            
            return report.ToString();
        }
        
        /// <summary>
        /// Generates a detailed report for a specific unit system
        /// </summary>
        public static string GenerateSystemReport(UnitSystemType systemType, string systemName, string description)
        {
            var report = new StringBuilder();
            
            // Create and apply the unit system
            var unitSystem = new UnitSystem();
            unitSystem.Apply(systemType);
            
            report.AppendLine($"┌─ {systemName} ─────────────────────────────────────────┐");
            report.AppendLine($"│ {description.PadRight(55)} │");
            report.AppendLine($"└─────────────────────────────────────────────────────────────┘");
            report.AppendLine();
            
            // Report on each unit category
            ReportCategory(report, "LENGTH", unitSystem.length);
            ReportCategory(report, "MASS", unitSystem.mass);
            ReportCategory(report, "TIME", unitSystem.time);
            ReportCategory(report, "ANGLE", unitSystem.angle);
            ReportCategory(report, "FORCE", unitSystem.force);
            ReportCategory(report, "TEMPERATURE", unitSystem.temperature);
            
            return report.ToString();
        }
        
        /// <summary>
        /// Reports details for a specific unit category
        /// </summary>
        private static void ReportCategory(StringBuilder report, string categoryName, UnitCategory? category)
        {
            if (category == null)
            {
                report.AppendLine($"  {categoryName}: NOT CONFIGURED");
                return;
            }
            
            var baseUnit = category.BaseUnits();
            report.AppendLine($"  {categoryName}:");
            report.AppendLine($"    Base Unit: {baseUnit.Name()}");
            
            // Show all available units
            var units = category.Units().OrderBy(u => u.Name()).ToList();
            report.AppendLine($"    Available Units ({units.Count}): {string.Join(", ", units.Select(u => u.Name()))}");
            
            // Show conversion examples
            report.AppendLine($"    Sample Conversions from {baseUnit.Name()}:");
            
            var testValue = 1.0;
            foreach (var unit in units.Take(6)) // Show first 6 conversions to avoid clutter
            {
                if (unit.Name() != baseUnit.Name())
                {
                    var conversion = category.ConvertFromBaseUnits(unit.Name(), testValue);
                    if (conversion.success)
                    {
                        report.AppendLine($"      1 {baseUnit.Name()} = {conversion.value:G6} {unit.Name()}");
                    }
                }
            }
            
            // Show total conversions available
            var conversions = category.Conversions();
            report.AppendLine($"    Total Conversions Available: {conversions.Count}");
            report.AppendLine();
        }
        
        /// <summary>
        /// Generates a conversion matrix for a specific unit category
        /// </summary>
        public static string GenerateConversionMatrix(UnitCategory category, string categoryName)
        {
            var report = new StringBuilder();
            var units = category.Units().OrderBy(u => u.Name()).Take(8).ToList(); // Limit to prevent huge matrices
            
            report.AppendLine($"CONVERSION MATRIX: {categoryName}");
            report.AppendLine($"Base Unit: {category.BaseUnits().Name()}");
            report.AppendLine();
            
            // Header row
            report.Append("FROM\\TO".PadRight(8));
            foreach (var unit in units)
            {
                report.Append($"{unit.Name(),10}");
            }
            report.AppendLine();
            
            // Conversion rows
            foreach (var fromUnit in units)
            {
                report.Append($"{fromUnit.Name()}".PadRight(8));
                
                foreach (var toUnit in units)
                {
                    if (fromUnit.Name() == toUnit.Name())
                    {
                        report.Append("      1.0 ");
                    }
                    else
                    {
                        var conversion = category.Convert(fromUnit.Name(), toUnit.Name(), 1.0);
                        if (conversion.success)
                        {
                            report.Append($"{conversion.value,10:G6}");
                        }
                        else
                        {
                            report.Append("    N/A   ");
                        }
                    }
                }
                report.AppendLine();
            }
            
            return report.ToString();
        }
        
        /// <summary>
        /// Validates that all expected conversions work correctly
        /// </summary>
        public static string GenerateValidationReport()
        {
            var report = new StringBuilder();
            
            report.AppendLine("═══════════════════════════════════════════════════════════════");
            report.AppendLine("                    UNIT SYSTEM VALIDATION REPORT");
            report.AppendLine("═══════════════════════════════════════════════════════════════");
            report.AppendLine();
            
            var systems = new[] { UnitSystemType.MKS, UnitSystemType.IPS, UnitSystemType.FPS, UnitSystemType.CGS, UnitSystemType.mmNs };
            
            foreach (var systemType in systems)
            {
                var unitSystem = new UnitSystem();
                unitSystem.Apply(systemType);
                
                report.AppendLine($"Validating {systemType} System:");
                
                // Test key conversions that were previously failing
                if (unitSystem.length != null)
                {
                    var testResults = new[]
                    {
                        TestConversion(unitSystem.length, 1.0, "m", "ft", "Length: 1m → ft"),
                        TestConversion(unitSystem.length, 1.0, "m", "in", "Length: 1m → in"),
                        TestConversion(unitSystem.length, 1.0, "ft", "m", "Length: 1ft → m"),
                        TestConversion(unitSystem.length, 12.0, "in", "ft", "Length: 12in → ft")
                    };
                    
                    foreach (var result in testResults)
                    {
                        report.AppendLine($"  {result}");
                    }
                }
                
                report.AppendLine();
            }
            
            return report.ToString();
        }
        
        /// <summary>
        /// Tests a specific conversion and returns a formatted result
        /// </summary>
        private static string TestConversion(UnitCategory category, double value, string fromUnit, string toUnit, string description)
        {
            var result = category.Convert(fromUnit, toUnit, value);
            if (result.success)
            {
                return $"✅ {description}: {value} {fromUnit} = {result.value:G6} {toUnit}";
            }
            else
            {
                return $"❌ {description}: CONVERSION FAILED";
            }
        }
        
        /// <summary>
        /// Generates HTML report suitable for Razor pages
        /// </summary>
        public static string GenerateHtmlReport()
        {
            var report = new StringBuilder();
            
            report.AppendLine("<div class='unit-system-report'>");
            report.AppendLine("<h2>Unit System Comprehensive Report</h2>");
            
            var systems = new[]
            {
                new { Type = UnitSystemType.MKS, Name = "MKS (Meter-Kilogram-Second)", Description = "SI Standard - General Engineering" },
                new { Type = UnitSystemType.IPS, Name = "IPS (Inch-Pound-Second)", Description = "Imperial - Manufacturing/Mechanical" },
                new { Type = UnitSystemType.FPS, Name = "FPS (Foot-Pound-Second)", Description = "Imperial - Civil/Construction" },
                new { Type = UnitSystemType.CGS, Name = "CGS (Centimeter-Gram-Second)", Description = "Metric - Chemistry/Small Scale" },
                new { Type = UnitSystemType.mmNs, Name = "mmNs (Millimeter-Newton-Second)", Description = "Metric - Precision/CAD Systems" }
            };

            foreach (var system in systems)
            {
                report.AppendLine(GenerateHtmlSystemReport(system.Type, system.Name, system.Description));
            }
            
            report.AppendLine("</div>");
            
            return report.ToString();
        }
        
        /// <summary>
        /// Generates HTML report for a specific system
        /// </summary>
        private static string GenerateHtmlSystemReport(UnitSystemType systemType, string systemName, string description)
        {
            var report = new StringBuilder();
            var unitSystem = new UnitSystem();
            unitSystem.Apply(systemType);
            
            report.AppendLine($"<div class='system-section'>");
            report.AppendLine($"<h3>{systemName}</h3>");
            report.AppendLine($"<p class='description'>{description}</p>");
            
            if (unitSystem.length != null)
            {
                report.AppendLine("<div class='category-section'>");
                report.AppendLine("<h4>Length Units</h4>");
                report.AppendLine($"<p><strong>Base Unit:</strong> {unitSystem.length.BaseUnits().Name()}</p>");
                
                var units = unitSystem.length.Units().OrderBy(u => u.Name()).ToList();
                report.AppendLine($"<p><strong>Available Units:</strong> {string.Join(", ", units.Select(u => u.Name()))}</p>");
                
                report.AppendLine("<table class='conversion-table'>");
                report.AppendLine("<tr><th>From</th><th>To</th><th>Conversion</th></tr>");
                
                var baseUnit = unitSystem.length.BaseUnits().Name();
                foreach (var unit in units.Take(8))
                {
                    if (unit.Name() != baseUnit)
                    {
                        var conversion = unitSystem.length.ConvertFromBaseUnits(unit.Name(), 1.0);
                        if (conversion.success)
                        {
                            report.AppendLine($"<tr><td>1 {baseUnit}</td><td>{unit.Name()}</td><td>{conversion.value:G6} {unit.Name()}</td></tr>");
                        }
                    }
                }
                
                report.AppendLine("</table>");
                report.AppendLine("</div>");
            }
            
            report.AppendLine("</div>");
            
            return report.ToString();
        }
    }
}