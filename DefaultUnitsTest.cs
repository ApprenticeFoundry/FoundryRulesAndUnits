using FoundryRulesAndUnits.Units;

namespace FoundryRulesAndUnits
{
    public class DefaultUnitsTest
    {
        public static void TestDefaultUnits()
        {
            // Test MKS system (default)
            var mksSystem = IUnitSystem.MKS();
            
            Console.WriteLine("=== MKS System (Meters-Kilograms-Seconds) ===");
            
            // Test with default units (should use base units)
            var length1 = mksSystem.CreateLength(5.0);        // Should default to meters
            var mass1 = mksSystem.CreateMass(10.0);           // Should default to kg
            var time1 = mksSystem.CreateTime(2.0);            // Should default to seconds
            
            Console.WriteLine($"Length: {length1.Value()} {length1.Internal()} (display: {length1})");
            Console.WriteLine($"Mass: {mass1.Value()} {mass1.Internal()} (display: {mass1})");
            Console.WriteLine($"Time: {time1.Value()} {time1.Internal()} (display: {time1})");
            
            // Test with explicit units
            var length2 = mksSystem.CreateLength(5.0, "ft");   // 5 feet, stored as meters internally
            var mass2 = mksSystem.CreateMass(10.0, "lb");      // 10 pounds, stored as kg internally
            
            Console.WriteLine($"Length (ft): {length2.Value()} {length2.Internal()} (display: {length2})");
            Console.WriteLine($"Mass (lb): {mass2.Value()} {mass2.Internal()} (display: {mass2})");
            
            Console.WriteLine();
            Console.WriteLine("=== FPS System (Feet-Pounds-Seconds) ===");
            
            // Test FPS system
            var fpsSystem = IUnitSystem.FPS();
            
            var length3 = fpsSystem.CreateLength(5.0);        // Should default to feet
            var mass3 = fpsSystem.CreateMass(10.0);           // Should default to pounds
            
            Console.WriteLine($"Length: {length3.Value()} {length3.Internal()} (display: {length3})");
            Console.WriteLine($"Mass: {mass3.Value()} {mass3.Internal()} (display: {mass3})");
        }
    }
}