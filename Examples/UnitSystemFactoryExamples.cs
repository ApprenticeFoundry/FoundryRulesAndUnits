using FoundryRulesAndUnits.Units;

namespace FoundryRulesAndUnits.Examples
{
    /// <summary>
    /// Demonstration of the new simplified factory methods for creating measurements
    /// These methods make it much easier to create measurements without dealing with UnitGroup injection manually
    /// </summary>
    public static class UnitSystemFactoryExamples
    {
        /// <summary>
        /// Example showing the old vs new way of creating measurements
        /// </summary>
        public static void BasicFactoryUsage()
        {
            // NEW: Simple and clean - create a unit system and use it directly
            var siSystem = IUnitSystem.SI();
            
            // Create measurements using the unit system factory methods
            var length = siSystem.CreateLength(10.5, "m");
            var mass = siSystem.CreateMass(2.3, "kg");
            var temperature = siSystem.CreateTemperature(25.0, "°C");
            var speed = siSystem.CreateSpeed(100, "km/h");
            
            // Convert between units easily
            Console.WriteLine($"Length: {length.As("ft")} ft");  
            Console.WriteLine($"Mass: {mass.As("lb")} lb");
            Console.WriteLine($"Temperature: {temperature.As("°F")} °F");
            Console.WriteLine($"Speed: {speed.As("mph")} mph");
        }

        /// <summary>
        /// Example showing different unit systems
        /// </summary>
        public static void DifferentUnitSystems()
        {
            // Create different unit systems
            var siSystem = IUnitSystem.SI();
            var fpsSystem = IUnitSystem.FPS();
            var ipsSystem = IUnitSystem.IPS();
            
            // Same measurement in different systems
            var siLength = siSystem.CreateLength(1.0, "m");
            var fpsLength = fpsSystem.CreateLength(1.0, "ft");  
            var ipsLength = ipsSystem.CreateLength(1.0, "in");
            
            Console.WriteLine($"SI: {siLength.Value()} {siLength.Units()}");
            Console.WriteLine($"FPS: {fpsLength.Value()} {fpsLength.Units()}");
            Console.WriteLine($"IPS: {ipsLength.Value()} {ipsLength.Units()}");
        }

        /// <summary>
        /// Example showing electrical measurements
        /// </summary>
        public static void ElectricalMeasurements()
        {
            var system = IUnitSystem.SI();
            
            // Create electrical measurements
            var voltage = system.CreateVoltage(120, "V");
            var current = system.CreateCurrent(10, "A");
            var resistance = system.CreateResistance(12, "Ω");
            var power = system.CreatePower(1200, "W");
            
            Console.WriteLine($"Voltage: {voltage.As("mV")} mV");
            Console.WriteLine($"Current: {current.As("mA")} mA");
            Console.WriteLine($"Resistance: {resistance.As("kΩ")} kΩ");
            Console.WriteLine($"Power: {power.As("kW")} kW");
        }

        /// <summary>
        /// Example showing how to work with a specific system over time
        /// </summary>
        public static void PersistentSystemUsage()
        {
            // Create a system and keep it around
            var engineeringSystem = IUnitSystem.MKS();
            
            // Use it to create multiple measurements
            var measurements = new List<MeasuredValue>
            {
                engineeringSystem.CreateLength(100, "mm"),
                engineeringSystem.CreateMass(5.5, "kg"),
                engineeringSystem.CreateForce(250, "N"),
                engineeringSystem.CreatePower(1500, "W")
            };
            
            // Process measurements
            foreach (var measurement in measurements)
            {
                Console.WriteLine($"{measurement.GetType().Name}: {measurement.Value()} {measurement.Units()}");
            }
        }

        /// <summary>
        /// Example showing advanced factory usage
        /// </summary>
        public static void AdvancedFactoryUsage()
        {
            var system = IUnitSystem.SI();
            
            // Get the factory for more advanced operations
            var factory = system.GetFactory();
            
            // Create measurements using the factory directly (more advanced usage)
            var distance1 = factory.CreateDistance(100, "km");
            var distance2 = factory.CreateDistance(50, "mi");
            
            // Use generic measurement creation (weakly typed)
            var genericLength = system.CreateMeasuredValue(UnitFamilyName.Length, 25.4, "mm");
            var genericMass = system.CreateMeasuredValue(UnitFamilyName.Mass, 1.0, "lb");
            
            Console.WriteLine($"Distance 1: {distance1.As("mi")} miles");
            Console.WriteLine($"Distance 2: {distance2.As("km")} km");
            Console.WriteLine($"Generic length: {genericLength.As("in")} inches");
            Console.WriteLine($"Generic mass: {genericMass.As("kg")} kg");
        }

        /// <summary>
        /// Example showing the new strongly-typed generic Create method
        /// </summary>
        public static void GenericCreateUsage()
        {
            var system = IUnitSystem.SI();
            
            // NEW: Strongly-typed generic creation - best of both worlds!
            var length = system.Create<Length>(10.5, "m");        // Returns Length, not MeasuredValue
            var mass = system.Create<Mass>(2.3, "kg");            // Returns Mass, not MeasuredValue  
            var temperature = system.Create<Temperature>(25, "°C"); // Returns Temperature, not MeasuredValue
            var voltage = system.Create<Voltage>(120, "V");       // Returns Voltage, not MeasuredValue
            
            // No casting needed - already strongly typed!
            Console.WriteLine($"Length: {length.As("ft")} ft");
            Console.WriteLine($"Mass: {mass.As("lb")} lb");
            Console.WriteLine($"Temperature: {temperature.As("°F")} °F");
            Console.WriteLine($"Voltage: {voltage.As("mV")} mV");
            
            // Works with any measurement type
            var angle = system.Create<Angle>(45, "deg");
            var area = system.Create<Area>(100, "m²");
            var power = system.Create<Power>(1500, "W");
            
            Console.WriteLine($"Angle: {angle.As("rad")} radians");
            Console.WriteLine($"Area: {area.As("ft²")} ft²");
            Console.WriteLine($"Power: {power.As("hp")} hp");
            
            // Can also use with the factory directly
            var factory = system.GetFactory();
            var speed = factory.Create<Speed>(100, "km/h");
            var force = factory.Create<Force>(250, "N");
            
            Console.WriteLine($"Speed: {speed.As("mph")} mph");
            Console.WriteLine($"Force: {force.As("lbf")} lbf");
        }

        /// <summary>
        /// Comparison showing the before and after
        /// </summary>
        public static void BeforeAndAfterComparison()
        {
            Console.WriteLine("=== BEFORE (Complex) ===");
            Console.WriteLine("// Old way - complex setup required");
            Console.WriteLine("var factory = new UnitFactory(UnitSystemType.SI);");
            Console.WriteLine("var length = factory.CreateLength(10, \"m\");");
            Console.WriteLine();
            
            Console.WriteLine("=== AFTER (Simple) ===");
            Console.WriteLine("// New way - simple and clean");
            Console.WriteLine("var system = IUnitSystem.SI();");
            Console.WriteLine("var length = system.CreateLength(10, \"m\");");
            Console.WriteLine();
            
            // Actually demonstrate both
            Console.WriteLine("=== ACTUAL EXECUTION ===");
            
            // Old way (still works)
            var factory = new UnitFactory(UnitSystemType.SI);
            var oldLength = factory.CreateLength(10, "m");
            Console.WriteLine($"Old way result: {oldLength.Value()} {oldLength.Units()}");
            
            // New way (much simpler)
            var system = IUnitSystem.SI();
            var newLength = system.CreateLength(10, "m");
            Console.WriteLine($"New way result: {newLength.Value()} {newLength.Units()}");
        }
    }
}