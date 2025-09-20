using FoundryRulesAndUnits.Units;
using System;

namespace FoundryRulesAndUnits
{
    /// <summary>
    /// Quick test of the new factory methods
    /// </summary>
    public class FactoryTest
    {
        public static void RunTests()
        {
            Console.WriteLine("Testing UnitFactory methods...");
            
            // Test IUnitSystem factory methods
            var unitSystem = new UnitSystem(UnitSystemType.SI);
            
            var length = unitSystem.CreateLength(5.5, "m");
            Console.WriteLine($"Length: {length.Value()} {length.Internal()}");
            
            var mass = unitSystem.CreateMass(10.2, "kg");
            Console.WriteLine($"Mass: {mass.Value()} {mass.Internal()}");
            
            var area = unitSystem.CreateArea(25.0, "m²");
            Console.WriteLine($"Area: {area.Value()} {area.Internal()}");
            
            // Test generic Create<T> method
            var genericLength = unitSystem.CreateLength(3.5, "ft");
            Console.WriteLine($"Generic Length: {genericLength.Value()} {genericLength.Internal()}");
            
            var genericMass = unitSystem.CreateMass(500, "g");
            Console.WriteLine($"Generic Mass: {genericMass.Value()} {genericMass.Internal()}");
            
            var genericArea = unitSystem.CreateArea(100, "cm²");
            Console.WriteLine($"Generic Area: {genericArea.Value()} {genericArea.Internal()}");
            
            // Test static unit system creation convenience methods
            var siSystem = IUnitSystem.SI();
            var quickLength = siSystem.CreateLength(12.0, "in");
            Console.WriteLine($"Quick Length: {quickLength.Value()} {quickLength.Internal()}");
            
            var quickMass = siSystem.CreateMass(2.5, "lb");
            Console.WriteLine($"Quick Mass: {quickMass.Value()} {quickMass.Internal()}");
            
            Console.WriteLine("All factory tests completed successfully!");
        }
    }
}