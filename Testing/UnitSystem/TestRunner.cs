using System;
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
    }
}
