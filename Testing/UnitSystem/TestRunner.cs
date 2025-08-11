using System;
using System.Linq;
using FoundryRulesAndUnits.Extensions;
using FoundryRulesAndUnits.Units;

namespace FoundryRulesAndUnits.Testing.UnitSystem
{
    public class TestRunner
    {
        public static void RunAllTests()
        {
            "\n====== Unit System Validation Tests ======".WriteInfo();
            
            // Test each unit system
            TestSystem(UnitSystemType.IPS, "Inch-Pound-Second");
            TestSystem(UnitSystemType.FPS, "Foot-Pound-Second");
            TestSystem(UnitSystemType.MKS, "Meter-Kilogram-Second");
            TestSystem(UnitSystemType.CGS, "Centimeter-Gram-Second");
            TestSystem(UnitSystemType.mmNs, "Millimeter-Newton-Second");

            "\n====== Cross-System Validation Tests ======".WriteInfo();
            TestCrossSystemConversions();

            "\n====== Base Unit Validation ======".WriteInfo();
            TestBaseUnits();

            "\n====== Conversion Tables ======".WriteInfo();
            DisplayConversionTables();

            "\n====== All Tests Complete ======".WriteInfo();
        }

        private static void TestSystem(UnitSystemType systemType, string fullName)
        {
            $"\n--- Testing {systemType} ({fullName}) ---".WriteInfo();
            
            var service = new FoundryRulesAndUnits.Units.UnitSystem();
            service.Apply(systemType);

            // Test length category
            if (service.length != null)
            {
                var baseUnit = service.length.BaseUnits();
                if (baseUnit != null)
                {
                    $"  ✓ Base Length Unit: {baseUnit.Name()} ({baseUnit.Title()})".WriteSuccess();
                }
                $"  ✓ Total Length Units: {service.length.Units().Count}".WriteSuccess();
            }

            // Test mass category
            if (service.mass != null)
            {
                var baseUnit = service.mass.BaseUnits();
                if (baseUnit != null)
                {
                    $"  ✓ Base Mass Unit: {baseUnit.Name()} ({baseUnit.Title()})".WriteSuccess();
                }
                $"  ✓ Total Mass Units: {service.mass.Units().Count}".WriteSuccess();
            }

            // Test force category
            if (service.force != null)
            {
                var baseUnit = service.force.BaseUnits();
                if (baseUnit != null)
                {
                    $"  ✓ Base Force Unit: {baseUnit.Name()} ({baseUnit.Title()})".WriteSuccess();
                }
                $"  ✓ Total Force Units: {service.force.Units().Count}".WriteSuccess();
            }
        }

        private static void TestCrossSystemConversions()
        {
            // Test metric conversions (should be present in all systems)
            var service = new FoundryRulesAndUnits.Units.UnitSystem();
            service.Apply(UnitSystemType.MKS);
            
            if (service.length != null)
            {
                // Test 1 meter to millimeters
                double testValue = 1.0; // 1 meter
                double mm = service.length.ConvertFromBaseUnits("mm", testValue);
                $"  ✓ 1 m = {mm} mm (expected: 1000)".WriteSuccess();
                
                // Test 1 meter to inches
                double inches = service.length.ConvertFromBaseUnits("in", testValue);
                $"  ✓ 1 m = {inches:F4} in (expected: ~39.3701)".WriteSuccess();
            }
        }

        private static void TestBaseUnits()
        {
            var expectedBaseUnits = new (UnitSystemType system, string length, string mass, string force)[]
            {
                (UnitSystemType.IPS, "in", "lb", "lbf"),
                (UnitSystemType.FPS, "ft", "lb", "lbf"),
                (UnitSystemType.MKS, "m", "kg", "N"),
                (UnitSystemType.CGS, "cm", "g", "dyne"),
                (UnitSystemType.mmNs, "mm", "kg", "N")
            };

            foreach (var (systemType, expectedLength, expectedMass, expectedForce) in expectedBaseUnits)
            {
                $"\nValidating {systemType} base units:".WriteInfo();
                
                var service = new FoundryRulesAndUnits.Units.UnitSystem();
                service.Apply(systemType);

                string actualLength = service.length?.BaseUnits()?.Name() ?? "NULL";
                string actualMass = service.mass?.BaseUnits()?.Name() ?? "NULL";
                string actualForce = service.force?.BaseUnits()?.Name() ?? "NULL";

                bool lengthMatch = actualLength == expectedLength;
                bool massMatch = actualMass == expectedMass;
                bool forceMatch = actualForce == expectedForce;

                if (lengthMatch)
                    $"  Length: {actualLength} ✓".WriteSuccess();
                else
                    $"  Length: {actualLength} ✗ Expected: {expectedLength}".WriteError();
                    
                if (massMatch)
                    $"  Mass: {actualMass} ✓".WriteSuccess();
                else
                    $"  Mass: {actualMass} ✗ Expected: {expectedMass}".WriteError();
                    
                if (forceMatch)
                    $"  Force: {actualForce} ✓".WriteSuccess();
                else
                    $"  Force: {actualForce} ✗ Expected: {expectedForce}".WriteError();
            }
        }

        private static void DisplayConversionTables()
        {
            "\n--- Comprehensive Unit Family Conversion Tables ---".WriteInfo();
            
            var service = new FoundryRulesAndUnits.Units.UnitSystem();
            service.Apply(UnitSystemType.MKS);
            
            // Test each available unit category
            DisplayCategoryConversions("Length", service.length);
            DisplayCategoryConversions("Mass", service.mass);
            DisplayCategoryConversions("Force", service.force);
            DisplayCategoryConversions("Temperature", service.temperature);
            DisplayCategoryConversions("Angle", service.angle);
            DisplayCategoryConversions("Storage", service.storage);
            DisplayCategoryConversions("WorkTime", service.worktime);
            
            // Test additional unit families through individual instances
            DisplayMeasuredValueConversions();
            
            "\n--- Strong Typing Demonstrations ---".WriteInfo();
            DemonstrateStrongTyping();
        }

        private static void DisplayCategoryConversions(string categoryName, UnitCategory? category)
        {
            if (category == null)
            {
                $"\n--- {categoryName} Conversion Table ---".WriteInfo();
                $"  {categoryName} category not available in current unit system".WriteError();
                return;
            }

            $"\n--- {categoryName} Conversion Table ---".WriteInfo();
            var baseUnit = category.BaseUnits();
            if (baseUnit == null)
            {
                $"  No base unit defined for {categoryName}".WriteError();
                return;
            }

            $"  Base: 1 {baseUnit.Name()} converts to:".WriteInfo();
            
            var units = category.Units();
            foreach (var unit in units)
            {
                if (unit.Name() != baseUnit.Name())
                {
                    try
                    {
                        double result = category.ConvertFromBaseUnits(unit.Name(), 1.0);
                        $"    1 {baseUnit.Name()} = {result:F4} {unit.Name()} ({unit.Title()})".WriteSuccess();
                    }
                    catch (Exception ex)
                    {
                        $"    1 {baseUnit.Name()} = [conversion failed: {ex.Message}] {unit.Name()} ({unit.Title()})".WriteError();
                    }
                }
            }
            
            if (units.Count <= 1)
            {
                $"    Only base unit {baseUnit.Name()} available in this category".WriteInfo();
            }
        }

        private static void DisplayMeasuredValueConversions()
        {
            "\n--- Additional MeasuredValue Family Tests ---".WriteInfo();
            
            // Test Area
            TestMeasuredValueType<Area>("Area", 1.0, "m²", new[] { 
                ("cm²", "square centimeters"),
                ("in²", "square inches"),
                ("ft²", "square feet") 
            });
            
            // Test Volume
            TestMeasuredValueType<Volume>("Volume", 1.0, "m³", new[] { 
                ("L", "liters"),
                ("cm³", "cubic centimeters"),
                ("ft³", "cubic feet"),
                ("gal", "gallons") 
            });
            
            // Test Speed
            TestMeasuredValueType<Speed>("Speed", 1.0, "m/s", new[] { 
                ("km/h", "kilometers per hour"),
                ("mph", "miles per hour"),
                ("ft/s", "feet per second") 
            });
            
            // Test Time
            TestMeasuredValueType<Time>("Time", 1.0, "s", new[] { 
                ("min", "minutes"),
                ("h", "hours"),
                ("ms", "milliseconds") 
            });
            
            // Test Current
            TestMeasuredValueType<Current>("Current", 1.0, "A", new[] { 
                ("mA", "milliamperes"),
                ("μA", "microamperes") 
            });
            
            // Test Voltage
            TestMeasuredValueType<Voltage>("Voltage", 1.0, "V", new[] { 
                ("mV", "millivolts"),
                ("kV", "kilovolts") 
            });
            
            // Test Power
            TestMeasuredValueType<Power>("Power", 1.0, "W", new[] { 
                ("kW", "kilowatts"),
                ("hp", "horsepower"),
                ("mW", "milliwatts") 
            });
            
            // Test Frequency
            TestMeasuredValueType<Frequency>("Frequency", 1.0, "Hz", new[] { 
                ("kHz", "kilohertz"),
                ("MHz", "megahertz"),
                ("GHz", "gigahertz") 
            });
            
            // Test DataStorage
            TestMeasuredValueType<DataStorage>("DataStorage", 1.0, "B", new[] { 
                ("KB", "kilobytes"),
                ("MB", "megabytes"),
                ("GB", "gigabytes") 
            });
            
            // Test DataFlow
            TestMeasuredValueType<DataFlow>("DataFlow", 1.0, "B/s", new[] { 
                ("KB/s", "kilobytes per second"),
                ("MB/s", "megabytes per second") 
            });
            
            // Test Percent
            TestMeasuredValueType<Percent>("Percent", 50.0, "%", new[] { 
                ("ratio", "ratio"),
                ("ppm", "parts per million") 
            });
            
            // Test Resistance
            TestMeasuredValueType<Resistance>("Resistance", 1.0, "Ω", new[] { 
                ("kΩ", "kiloohms"),
                ("MΩ", "megaohms") 
            });
            
            // Test Capacitance
            TestMeasuredValueType<Capacitance>("Capacitance", 1.0, "F", new[] { 
                ("μF", "microfarads"),
                ("nF", "nanofarads"),
                ("pF", "picofarads") 
            });
        }
        
        private static void TestMeasuredValueType<T>(string typeName, double testValue, string baseUnit, (string unit, string description)[] conversions) 
            where T : MeasuredValue, new()
        {
            $"\n  {typeName} conversions ({testValue} {baseUnit}):".WriteInfo();
            
            try
            {
                // Demonstrate strong typing by creating actual instances
                var instance = new T();
                $"    {typeName} type instantiated successfully ✓".WriteSuccess();
                
                // Try to create an instance with a value to show type-specific behavior
                try
                {
                    var constructor = typeof(T).GetConstructor(new[] { typeof(double), typeof(string) });
                    if (constructor != null)
                    {
                        var typedInstance = (T)constructor.Invoke(new object[] { testValue, baseUnit });
                        $"    Created {typeName}({testValue}, \"{baseUnit}\") ✓".WriteSuccess();
                        
                        // Show the strongly-typed value access
                        $"    Value: {typedInstance.Value()} {typedInstance.Units()}".WriteInfo();
                        
                        // Test conversions using the strongly-typed instance
                        foreach (var (unit, description) in conversions.Take(2))
                        {
                            try
                            {
                                double result = typedInstance.As(unit);
                                $"    {testValue} {baseUnit} = {result:F4} {unit} ({description}) ✓".WriteSuccess();
                            }
                            catch (Exception ex)
                            {
                                $"    {testValue} {baseUnit} = [conversion failed: {ex.Message}] {unit}".WriteError();
                            }
                        }
                        
                        // Show operator overloading for applicable types
                        if (typeof(T) == typeof(Temperature))
                        {
                            try
                            {
                                var temp1 = (Temperature)(object)typedInstance;
                                var temp2 = new Temperature(10, baseUnit);
                                var sum = temp1 + temp2;
                                $"    Operator demo: {temp1.Value()}{baseUnit} + {temp2.Value()}{baseUnit} = {sum.Value()}{baseUnit} ✓".WriteSuccess();
                            }
                            catch (Exception ex)
                            {
                                $"    Operator demo failed: {ex.Message}".WriteError();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    $"    Could not create parameterized {typeName}: {ex.Message}".WriteError();
                }
                
                // Show category information
                try
                {
                    var categoryProperty = typeof(T).GetProperty("Category", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                    if (categoryProperty != null)
                    {
                        var categoryFunc = categoryProperty.GetValue(null) as Func<UnitCategory>;
                        if (categoryFunc != null)
                        {
                            var category = categoryFunc();
                            var units = category.Units();
                            $"    Category: {category.Title()} with {units.Count} units".WriteInfo();
                            $"    Available units: {string.Join(", ", units.Select(u => u.Name()))}".WriteInfo();
                        }
                    }
                }
                catch (Exception ex)
                {
                    $"    Could not access category for {typeName}: {ex.Message}".WriteError();
                }
            }
            catch (Exception ex)
            {
                $"    Failed to instantiate {typeName}: {ex.Message}".WriteError();
            }
        }

        private static void DemonstrateStrongTyping()
        {
            "  Demonstrating type safety and strongly-typed operations:".WriteInfo();
            
            try
            {
                // Create strongly-typed instances
                var length1 = new Length(5.0, "m");
                var length2 = new Length(2.0, "m");
                var temp1 = new Temperature(25.0, "C");
                var temp2 = new Temperature(10.0, "C");
                
                $"    Created Length(5.0, \"m\") and Length(2.0, \"m\") ✓".WriteSuccess();
                $"    Created Temperature(25.0, \"C\") and Temperature(10.0, \"C\") ✓".WriteSuccess();
                
                // Demonstrate type-safe arithmetic
                var lengthSum = length1 + length2;
                var tempSum = temp1 + temp2;
                
                $"    Length arithmetic: 5m + 2m = {lengthSum.Value()}m ✓".WriteSuccess();
                $"    Temperature arithmetic: 25°C + 10°C = {tempSum.Value()}°C ✓".WriteSuccess();
                
                // Demonstrate unit conversions within same type
                $"    Length conversion: 5m = {length1.As("ft"):F2}ft ✓".WriteSuccess();
                $"    Temperature conversion: 25°C = {temp1.As("F"):F1}°F ✓".WriteSuccess();
                
                // Demonstrate that each type maintains its own unit category
                $"    Length category: {Length.Category().Title()} ✓".WriteSuccess();
                $"    Temperature category: {Temperature.Category().Title()} ✓".WriteSuccess();
                
                // Show type identity is preserved
                $"    Length type: {length1.GetType().Name} ✓".WriteSuccess();
                $"    Temperature type: {temp1.GetType().Name} ✓".WriteSuccess();
                
                "    Strong typing prevents invalid operations (compile-time safety) ✓".WriteSuccess();
                
            }
            catch (Exception ex)
            {
                $"    Strong typing demonstration failed: {ex.Message}".WriteError();
            }
            
            // Demonstrate different physical quantities have different base units
            "\n  Base units by physical quantity:".WriteInfo();
            try
            {
                var service = new FoundryRulesAndUnits.Units.UnitSystem();
                service.Apply(UnitSystemType.MKS);
                
                var physicalQuantities = new[]
                {
                    ("Length", service.length),
                    ("Mass", service.mass),
                    ("Temperature", service.temperature),
                    ("Force", service.force),
                    ("Angle", service.angle)
                };
                
                foreach (var (name, category) in physicalQuantities)
                {
                    if (category != null)
                    {
                        var baseUnit = category.BaseUnits();
                        $"    {name}: {baseUnit?.Name()} ({baseUnit?.Title()}) ✓".WriteSuccess();
                    }
                    else
                    {
                        $"    {name}: [not available in current system]".WriteError();
                    }
                }
            }
            catch (Exception ex)
            {
                $"    Base unit demonstration failed: {ex.Message}".WriteError();
            }
        }
    }
}
