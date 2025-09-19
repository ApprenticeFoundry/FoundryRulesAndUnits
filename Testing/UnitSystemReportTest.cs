using System;
using FoundryRulesAndUnits.Extensions;
using FoundryRulesAndUnits.Units;

namespace FoundryRulesAndUnits.Testing
{
    /// <summary>
    /// Test program to demonstrate the Unit System Reporter functionality
    /// </summary>
    class UnitSystemReportTest
    {
        static void Main()
        {
            Console.WriteLine("🔧 UNIT SYSTEM REPORTER DEMONSTRATION");
            Console.WriteLine("=====================================");
            Console.WriteLine();
            
            try
            {
                // Option 1: Complete report for all systems
                Console.WriteLine("1. GENERATING COMPLETE REPORT...");
                Console.WriteLine();
                var completeReport = UnitSystemReporter.GenerateCompleteReport();
                Console.WriteLine(completeReport);
                
                Console.WriteLine("\n" + new string('=', 80));
                Console.WriteLine();
                
                // Option 2: Detailed report for one system
                Console.WriteLine("2. DETAILED MKS SYSTEM REPORT...");
                Console.WriteLine();
                var mksReport = UnitSystemReporter.GenerateSystemReport(
                    UnitSystemType.MKS, 
                    "MKS (Meter-Kilogram-Second)", 
                    "SI Standard - General Engineering"
                );
                Console.WriteLine(mksReport);
                
                Console.WriteLine("\n" + new string('=', 80));
                Console.WriteLine();
                
                // Option 3: Conversion matrix for length units
                Console.WriteLine("3. LENGTH CONVERSION MATRIX (MKS)...");
                Console.WriteLine();
                var unitSystem = new FoundryRulesAndUnits.Units.UnitSystem();
                unitSystem.Apply(UnitSystemType.MKS);
                if (unitSystem.length != null)
                {
                    var matrixReport = UnitSystemReporter.GenerateConversionMatrix(unitSystem.length, "LENGTH");
                    Console.WriteLine(matrixReport);
                }
                
                Console.WriteLine("\n" + new string('=', 80));
                Console.WriteLine();
                
                // Option 4: Validation report
                Console.WriteLine("4. SYSTEM VALIDATION REPORT...");
                Console.WriteLine();
                var validationReport = UnitSystemReporter.GenerateValidationReport();
                Console.WriteLine(validationReport);
                
                Console.WriteLine("\n🎉 REPORTER DEMONSTRATION COMPLETE!");
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ ERROR: {ex.Message}");
                Console.WriteLine($"Details: {ex}");
            }
            
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}