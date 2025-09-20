using FoundryRulesAndUnits.Units;

namespace FoundryRulesAndUnits.Examples
{
    /// <summary>
    /// Simple demonstration of the new generic Create<T> method
    /// </summary>
    public static class GenericCreateDemo
    {
        public static void RunDemo()
        {
            Console.WriteLine("=== Generic Create<T> Method Demo ===\n");
            
            var system = IUnitSystem.SI();
            
            // Strongly-typed creation - no casting needed!
            var length = system.Create<Length>(10.5, "m");
            var mass = system.Create<Mass>(2.3, "kg");
            var temperature = system.Create<Temperature>(25.0, "°C");
            var voltage = system.Create<Voltage>(120.0, "V");
            var angle = system.Create<Angle>(45.0, "deg");
            
            Console.WriteLine("Created measurements using Create<T>:");
            Console.WriteLine($"Length: {length.Value()} {length.Units()} = {length.As("ft"):F2} ft");
            Console.WriteLine($"Mass: {mass.Value()} {mass.Units()} = {mass.As("lb"):F2} lb");
            Console.WriteLine($"Temperature: {temperature.Value()} {temperature.Units()} = {temperature.As("°F"):F1} °F");
            Console.WriteLine($"Voltage: {voltage.Value()} {voltage.Units()} = {voltage.As("mV"):F0} mV");
            Console.WriteLine($"Angle: {angle.Value()} {angle.Units()} = {angle.As("rad"):F3} radians");
            
            Console.WriteLine("\n=== Comparison: Three Ways to Create ===\n");
            
            // Method 1: Specific factory method (most explicit)
            var length1 = system.CreateLength(100, "cm");
            Console.WriteLine($"Method 1 (CreateLength): {length1.GetType().Name} = {length1.Value()} {length1.Units()}");
            
            // Method 2: Generic Create<T> (strongly typed, flexible)
            var length2 = system.Create<Length>(100, "cm");
            Console.WriteLine($"Method 2 (Create<Length>): {length2.GetType().Name} = {length2.Value()} {length2.Units()}");
            
            // Method 3: CreateMeasuredValue (weakly typed, most flexible)
            var length3 = system.CreateMeasuredValue(UnitFamilyName.Length, 100, "cm");
            Console.WriteLine($"Method 3 (CreateMeasuredValue): {length3.GetType().Name} = {length3.Value()} {length3.Units()}");
            
            Console.WriteLine("\n=== Dynamic Type Creation Example ===\n");
            
            // Example of using generics dynamically
            CreateAndDisplay<Mass>(system, 5.5, "kg");
            CreateAndDisplay<Speed>(system, 100, "km/h");
            CreateAndDisplay<Power>(system, 1500, "W");
            CreateAndDisplay<Resistance>(system, 470, "Ω");
        }
        
        private static void CreateAndDisplay<T>(IUnitSystem system, double value, string units) 
            where T : MeasuredValue
        {
            var measurement = system.Create<T>(value, units);
            Console.WriteLine($"Created {typeof(T).Name}: {measurement.Value()} {measurement.Units()}");
        }
    }
}