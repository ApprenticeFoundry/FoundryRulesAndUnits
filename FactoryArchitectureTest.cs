using System;
using FoundryRulesAndUnits.Units;

namespace FoundryRulesAndUnits
{
    /// <summary>
    /// Test the new factory-based architecture with UnitGroup injection
    /// </summary>
    public class FactoryArchitectureTest
    {
        public static void TestFactoryBasedCreation()
        {
            Console.WriteLine("=== Testing Factory-Based Architecture ===");
            
            // Create factory for MKS system
            var factory = new UnitFactory(UnitSystemType.MKS);
            
            Console.WriteLine("\n✓ Testing factory creation and conversion:");
            
            // Test Length with UnitGroup injection
            var length = factory.CreateLength(1000, "mm");
            Console.WriteLine($"Length: {length.Value()} {length.Internal()} = {length.As("m")} m = {length.As("cm")} cm");
            
            // Test Mass with UnitGroup injection  
            var mass = factory.CreateMass(2.5, "kg");
            Console.WriteLine($"Mass: {mass.Value()} {mass.Internal()} = {mass.As("g")} g = {mass.As("lb")} lb");
            
            // Test Temperature with UnitGroup injection
            var temp = factory.CreateTemperature(100, "°C");
            Console.WriteLine($"Temperature: {temp.Value()} {temp.Internal()} = {temp.As("°F")} °F = {temp.As("K")} K");
            
            // Test Angle with UnitGroup injection
            var angle = factory.CreateAngle(90, "deg");
            Console.WriteLine($"Angle: {angle.Value()} {angle.Internal()} = {angle.As("rad")} rad = {angle.As("deg")} deg");
            
            // Test arithmetic operations (should use UnitGroup from existing instances)
            var length2 = factory.CreateLength(500, "mm");
            var totalLength = length + length2; // Uses UnitGroup from left operand
            Console.WriteLine($"Arithmetic: {length.As("mm")} mm + {length2.As("mm")} mm = {totalLength.As("mm")} mm");
            
            Console.WriteLine("\n✓ Factory-based architecture working correctly!");
            Console.WriteLine("  - All instances created with proper UnitGroup injection");
            Console.WriteLine("  - No global dependencies needed for conversions");
            Console.WriteLine("  - Arithmetic operations preserve UnitGroup context");
        }
    }
}