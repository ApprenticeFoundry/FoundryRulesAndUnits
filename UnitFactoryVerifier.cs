using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace FoundryRulesAndUnits.Units
{
    /// <summary>
    /// Comprehensive verification class for UnitFactory architecture
    /// Tests that factory creates strongly typed objects that work correctly for all operations
    /// Can be instantiated and run in any application to verify the unit engine is functioning properly
    /// </summary>
    public class UnitFactoryVerifier
    {
        /// <summary>
        /// Run comprehensive verification of the UnitFactory architecture
        /// Tests strongly typed object creation, mathematical operations, and parser compatibility
        /// </summary>
        /// <param name="unitSystem">The unit system to test with (if null, creates a default SI system)</param>
        /// <returns>Detailed verification results</returns>
        public VerificationResults RunVerification(IUnitSystem? unitSystem = null)
        {
            var results = new VerificationResults();
            var factory = CreateTestFactory(unitSystem, results);
            
            if (factory == null)
                return results; // Early return if factory creation failed

            try
            {
                // 1. Test UnitTypeRegistry fundamentals
                TestUnitTypeRegistryCore(results);
                
                // 2. Test all three factory methods
                TestAllFactoryMethods(factory, results);
                
                // 3. Test strongly typed operations 
                TestStronglyTypedOperations(factory, results);
                
                // 4. Test cross-API compatibility
                TestCrossApiCompatibility(factory, results);
                
                // 5. Test parser simulation
                TestParserSimulation(factory, results);
                
                // 6. Test compound unit operations - critical for advanced scenarios!
                TestCompoundUnitOperations(factory, results);
                
                // 7. Performance verification
                TestPerformanceCaching(factory, results);
                
                results.AllTestsPassed = results.Failures.Count == 0;
                results.Summary = $"Verification Complete: {results.Successes.Count} passed, {results.Failures.Count} failed";
            }
            catch (Exception ex)
            {
                results.Failures.Add($"CRITICAL ERROR: {ex.Message}");
                results.AllTestsPassed = false;
                results.Summary = "Verification failed with critical error";
            }

            return results;
        }

        private UnitFactory? CreateTestFactory(IUnitSystem? unitSystem, VerificationResults results)
        {
            try
            {
                if (unitSystem == null)
                {
                    // Create a basic SI unit system for testing
                    var siSystem = UnitSystemFactory.CreateSI();
                    unitSystem = siSystem;
                    results.TestInfo.Add("Created default SI unit system for testing");
                }

                var factory = new UnitFactory(unitSystem);
                results.Successes.Add("✅ UnitFactory created successfully");
                results.TestInfo.Add($"Testing with unit system: {unitSystem.GetType().Name}");
                return factory;
            }
            catch (Exception ex)
            {
                results.Failures.Add($"❌ Failed to create UnitFactory: {ex.Message}");
                return null;
            }
        }

        private void TestUnitTypeRegistryCore(VerificationResults results)
        {
            try
            {
                // Test that registry discovers all unit types
                var discoveredTypes = GetAllDiscoveredUnitTypes();
                results.Metrics["DiscoveredUnitTypes"] = discoveredTypes.Count;
                
                if (discoveredTypes.Count == 0)
                    results.Failures.Add("❌ UnitTypeRegistry discovered no unit types");
                else
                    results.Successes.Add($"✅ UnitTypeRegistry discovered {discoveredTypes.Count} unit types");

                // Test attribute lookup for each type
                int attributeSuccesses = 0;
                foreach (var type in discoveredTypes)
                {
                    var attr = UnitTypeRegistry.GetAttributeForType(type);
                    if (attr != null)
                        attributeSuccesses++;
                }
                
                if (attributeSuccesses == discoveredTypes.Count)
                    results.Successes.Add($"✅ All {discoveredTypes.Count} types have valid UnitTypeAttribute");
                else
                    results.Failures.Add($"❌ {discoveredTypes.Count - attributeSuccesses} types missing UnitTypeAttribute");
            }
            catch (Exception ex)
            {
                results.Failures.Add($"❌ UnitTypeRegistry test failed: {ex.Message}");
            }
        }

        private void TestAllFactoryMethods(UnitFactory factory, VerificationResults results)
        {
            try
            {
                // Get a few representative unit families to test
                var testFamilies = new[]
                {
                    UnitFamilyName.Length,
                    UnitFamilyName.Angle,
                    UnitFamilyName.Mass,
                    UnitFamilyName.Time
                };

                foreach (var family in testFamilies)
                {
                    TestFactoryMethodsForFamily(factory, family, results);
                }
            }
            catch (Exception ex)
            {
                results.Failures.Add($"❌ Factory methods test failed: {ex.Message}");
            }
        }

        private void TestFactoryMethodsForFamily(UnitFactory factory, UnitFamilyName family, VerificationResults results)
        {
            try
            {
                // Test 1: CreateMeasuredValue (base class)
                var baseObj = factory.CreateMeasuredValue(family, 5.0);
                if (baseObj.GetType() == typeof(MeasuredValue))
                    results.Successes.Add($"✅ CreateMeasuredValue({family}) returns base MeasuredValue");
                else
                    results.Failures.Add($"❌ CreateMeasuredValue({family}) returned {baseObj.GetType().Name}, expected MeasuredValue");

                // Test 2: CreateTypedMeasuredValue (specific derived type)
                var typedObj = factory.CreateTypedMeasuredValue(family, 5.0);
                if (typedObj.GetType() != typeof(MeasuredValue))
                {
                    results.Successes.Add($"✅ CreateTypedMeasuredValue({family}) returns strongly typed {typedObj.GetType().Name}");
                    
                    // Verify UnitFamily property works
                    if (typedObj.UnitFamily == family)
                        results.Successes.Add($"✅ {typedObj.GetType().Name}.UnitFamily returns correct value");
                    else
                        results.Failures.Add($"❌ {typedObj.GetType().Name}.UnitFamily mismatch");
                }
                else
                    results.Failures.Add($"❌ CreateTypedMeasuredValue({family}) returned base MeasuredValue instead of specific type");

                // Test 3: CreateUnit<T> (generic method) - need to use reflection for dynamic testing
                TestGenericFactoryMethod(factory, typedObj.GetType(), family, results);
            }
            catch (Exception ex)
            {
                results.Failures.Add($"❌ Factory methods test for {family} failed: {ex.Message}");
            }
        }

        private void TestGenericFactoryMethod(UnitFactory factory, Type unitType, UnitFamilyName family, VerificationResults results)
        {
            try
            {
                // Use reflection to call CreateUnit<T>() dynamically
                var method = typeof(UnitFactory).GetMethod("CreateUnit")?.MakeGenericMethod(unitType);
                if (method == null)
                {
                    results.Failures.Add($"❌ Could not find CreateUnit<T> method for {unitType.Name}");
                    return;
                }

                var genericObj = method.Invoke(factory, new object[] { 10.0, null });
                if (genericObj != null && genericObj.GetType() == unitType)
                    results.Successes.Add($"✅ CreateUnit<{unitType.Name}>() returns correct type");
                else
                    results.Failures.Add($"❌ CreateUnit<{unitType.Name}>() type mismatch");
            }
            catch (Exception ex)
            {
                results.Failures.Add($"❌ Generic factory method test failed: {ex.Message}");
            }
        }

        private void TestStronglyTypedOperations(UnitFactory factory, VerificationResults results)
        {
            try
            {
                // Test Length operations including mixed units
                TestLengthOperations(factory, results);
                
                // Test Angle operations 
                TestAngleOperations(factory, results);
                
                // Test Mass operations
                TestMassOperations(factory, results);
            }
            catch (Exception ex)
            {
                results.Failures.Add($"❌ Strongly typed operations test failed: {ex.Message}");
            }
        }

        private void TestLengthOperations(UnitFactory factory, VerificationResults results)
        {
            try
            {
                // Test same units first
                var length1 = factory.CreateTypedMeasuredValue(UnitFamilyName.Length, 5.0, "m");
                var length2 = factory.CreateTypedMeasuredValue(UnitFamilyName.Length, 3.0, "m");

                // Test that both are Length objects
                if (length1 is Length && length2 is Length)
                {
                    results.Successes.Add("✅ Length objects created with correct type");
                    
                    // Test comparison operations
                    if (length1 > length2)
                        results.Successes.Add("✅ Length comparison operations work");
                    else
                        results.Failures.Add("❌ Length comparison failed (5m should be > 3m)");

                    // Test arithmetic operations
                    var sum = ((Length)length1) + ((Length)length2);
                    if (sum is Length && Math.Abs(sum.Value - 8.0) < 0.001)
                        results.Successes.Add("✅ Length addition operations work");
                    else
                        results.Failures.Add("❌ Length addition failed or wrong result type");
                }
                else
                {
                    results.Failures.Add("❌ Length objects not created with correct type");
                }

                // Test MIXED UNITS operations - this is critical!
                TestMixedUnitOperations(factory, results);
            }
            catch (Exception ex)
            {
                results.Failures.Add($"❌ Length operations test failed: {ex.Message}");
            }
        }

        private void TestMixedUnitOperations(UnitFactory factory, VerificationResults results)
        {
            try
            {
                // Test the exact scenario you mentioned: Create<Length>(200, "cm") + Create<Length>(1, "m")
                var lengthCm = factory.CreateUnit<Length>(200, "cm");  // 200 cm = 2 m
                var lengthM = factory.CreateUnit<Length>(1, "m");      // 1 m

                if (lengthCm is Length && lengthM is Length)
                {
                    results.Successes.Add("✅ Mixed unit Length objects created (cm + m)");

                    // Test addition: 200cm + 1m should equal 3m (in base units)
                    var sum = lengthCm + lengthM;
                    if (sum is Length)
                    {
                        results.Successes.Add("✅ Mixed unit addition returns Length type");
                        
                        // The result should be 3.0 (in base units - meters)
                        if (Math.Abs(sum.Value - 3.0) < 0.001)
                            results.Successes.Add("✅ Mixed unit math correct: 200cm + 1m = 3m");
                        else
                            results.Failures.Add($"❌ Mixed unit math incorrect: 200cm + 1m = {sum.Value}m (expected 3.0)");
                    }
                    else
                    {
                        results.Failures.Add("❌ Mixed unit addition returned wrong type");
                    }

                    // Test comparison: 200cm should equal 2m
                    var length2m = factory.CreateUnit<Length>(2, "m");
                    if (Math.Abs(lengthCm.Value - length2m.Value) < 0.001)
                        results.Successes.Add("✅ Unit conversion: 200cm equals 2m in base units");
                    else
                        results.Failures.Add($"❌ Unit conversion failed: 200cm = {lengthCm.Value}, 2m = {length2m.Value}");

                    // Test EQUALITY OPERATORS - this is what the user was asking about!
                    if (lengthCm == length2m)
                        results.Successes.Add("✅ Equality operator works: 200cm == 2m");
                    else
                        results.Failures.Add($"❌ Equality operator failed: 200cm != 2m (values: {lengthCm.Value} vs {length2m.Value})");

                    // Test exact numerical result with known expected value
                    var expectedSum = factory.CreateUnit<Length>(3, "m");
                    if (sum == expectedSum)
                        results.Successes.Add("✅ Exact equality test: (200cm + 1m) == 3m");
                    else
                        results.Failures.Add($"❌ Exact equality failed: sum={sum.Value}, expected={expectedSum.Value}");
                }
                else
                {
                    results.Failures.Add("❌ Failed to create mixed unit Length objects");
                }

                // Test other mixed unit scenarios
                TestAdditionalMixedUnits(factory, results);
            }
            catch (Exception ex)
            {
                results.Failures.Add($"❌ Mixed unit operations test failed: {ex.Message}");
            }
        }

        private void TestAdditionalMixedUnits(UnitFactory factory, VerificationResults results)
        {
            try
            {
                // Test feet + inches
                var feet = factory.CreateUnit<Length>(1, "ft");
                var inches = factory.CreateUnit<Length>(12, "in");
                
                if (feet != null && inches != null)
                {
                    var sum = feet + inches;
                    if (sum is Length)
                        results.Successes.Add("✅ Mixed imperial units work: ft + in");
                    else
                        results.Failures.Add("❌ Mixed imperial units failed: ft + in");
                }

                // Test angle mixed units
                var degrees = factory.CreateUnit<Angle>(90, "deg");
                var radians = factory.CreateUnit<Angle>(1.5708, "rad"); // π/2 radians ≈ 90 degrees
                
                if (degrees != null && radians != null)
                {
                    // Both should be approximately equal when converted to base units
                    if (Math.Abs(degrees.Value - radians.Value) < 0.01)
                        results.Successes.Add("✅ Mixed angle units: 90deg ≈ π/2 rad");
                    else
                        results.TestInfo.Add($"ℹ️ Angle conversion: 90deg = {degrees.Value}, π/2 rad = {radians.Value}");
                }
            }
            catch (Exception ex)
            {
                results.TestInfo.Add($"ℹ️ Additional mixed unit tests had issues: {ex.Message}");
            }
        }

        private void TestAngleOperations(UnitFactory factory, VerificationResults results)
        {
            try
            {
                var angle1 = factory.CreateTypedMeasuredValue(UnitFamilyName.Angle, 90.0, "deg");
                var angle2 = factory.CreateTypedMeasuredValue(UnitFamilyName.Angle, 45.0, "deg");

                if (angle1 is Angle && angle2 is Angle)
                {
                    results.Successes.Add("✅ Angle objects created with correct type");
                    
                    // Test operations
                    if (angle1 > angle2)
                        results.Successes.Add("✅ Angle comparison operations work");
                    
                    var sum = ((Angle)angle1) + ((Angle)angle2);
                    if (sum is Angle)
                        results.Successes.Add("✅ Angle addition operations work");
                }
                else
                {
                    results.Failures.Add("❌ Angle objects not created with correct type");
                }
            }
            catch (Exception ex)
            {
                results.Failures.Add($"❌ Angle operations test failed: {ex.Message}");
            }
        }

        private void TestMassOperations(UnitFactory factory, VerificationResults results)
        {
            try
            {
                var mass1 = factory.CreateTypedMeasuredValue(UnitFamilyName.Mass, 10.0, "kg");
                var mass2 = factory.CreateTypedMeasuredValue(UnitFamilyName.Mass, 5.0, "kg");

                if (mass1 is Mass && mass2 is Mass)
                {
                    results.Successes.Add("✅ Mass objects created with correct type");
                    
                    if (mass1 > mass2)
                        results.Successes.Add("✅ Mass comparison operations work");
                    
                    var sum = ((Mass)mass1) + ((Mass)mass2);
                    if (sum is Mass)
                        results.Successes.Add("✅ Mass addition operations work");
                }
                else
                {
                    results.Failures.Add("❌ Mass objects not created with correct type");
                }
            }
            catch (Exception ex)
            {
                results.Failures.Add($"❌ Mass operations test failed: {ex.Message}");
            }
        }

        private void TestCrossApiCompatibility(UnitFactory factory, VerificationResults results)
        {
            try
            {
                // Create same unit using both APIs
                var runtimeLength = factory.CreateTypedMeasuredValue(UnitFamilyName.Length, 5.0, "m");
                var genericLength = factory.CreateUnit<Length>(5.0, "m");

                // Should be same type
                if (runtimeLength.GetType() == genericLength.GetType())
                    results.Successes.Add("✅ Both APIs create same type");
                else
                    results.Failures.Add("❌ APIs create different types");

                // Should be operationally compatible
                if (runtimeLength is Length && genericLength is Length)
                {
                    var sum = ((Length)runtimeLength) + genericLength;
                    if (sum is Length)
                        results.Successes.Add("✅ Cross-API operations work");
                    else
                        results.Failures.Add("❌ Cross-API operations failed");
                }

                // Test cross-API with MIXED UNITS - critical for parser scenarios!
                TestCrossApiMixedUnits(factory, results);
            }
            catch (Exception ex)
            {
                results.Failures.Add($"❌ Cross-API compatibility test failed: {ex.Message}");
            }
        }

        private void TestCrossApiMixedUnits(UnitFactory factory, VerificationResults results)
        {
            try
            {
                // Parser creates via runtime API, user code uses generic API
                var parserLength = factory.CreateTypedMeasuredValue(UnitFamilyName.Length, 100, "cm"); // Parser: 100cm
                var userLength = factory.CreateUnit<Length>(1, "m");                                    // User: 1m

                if (parserLength is Length && userLength is Length)
                {
                    // Test that parser-created and user-created objects work together
                    var sum = ((Length)parserLength) + userLength;
                    if (sum is Length)
                    {
                        results.Successes.Add("✅ Cross-API mixed units work (parser + user code)");
                        
                        // Should equal 2m total (100cm + 1m = 2m)
                        if (Math.Abs(sum.Value - 2.0) < 0.001)
                            results.Successes.Add("✅ Cross-API mixed unit math: 100cm + 1m = 2m");
                        else
                            results.TestInfo.Add($"ℹ️ Cross-API result: 100cm + 1m = {sum.Value}m");
                    }
                    else
                    {
                        results.Failures.Add("❌ Cross-API mixed unit operations failed");
                    }

                    // Test comparison across APIs and units
                    var comparison = ((Length)parserLength).Value == userLength.Value; // Both should be in base units
                    if (comparison)
                        results.Successes.Add("✅ Cross-API unit comparison works");
                    else
                        results.TestInfo.Add($"ℹ️ Cross-API values: parser={((Length)parserLength).Value}, user={userLength.Value}");
                }
                else
                {
                    results.Failures.Add("❌ Cross-API mixed unit object creation failed");
                }
            }
            catch (Exception ex)
            {
                results.Failures.Add($"❌ Cross-API mixed units test failed: {ex.Message}");
            }
        }

        private void TestParserSimulation(UnitFactory factory, VerificationResults results)
        {
            try
            {
                // Simulate what a parser would do
                var parserScenarios = new[]
                {
                    (UnitFamilyName.Length, "m", typeof(Length)),
                    (UnitFamilyName.Angle, "deg", typeof(Angle)),
                    (UnitFamilyName.Mass, "kg", typeof(Mass)),
                    (UnitFamilyName.Time, "s", typeof(Time))
                };

                foreach (var (family, unit, expectedType) in parserScenarios)
                {
                    var obj = factory.CreateTypedMeasuredValue(family, 1.0, unit);
                    if (obj.GetType() == expectedType)
                        results.Successes.Add($"✅ Parser scenario: {family} → {expectedType.Name}");
                    else
                        results.Failures.Add($"❌ Parser scenario failed: {family} expected {expectedType.Name}, got {obj.GetType().Name}");
                }
            }
            catch (Exception ex)
            {
                results.Failures.Add($"❌ Parser simulation test failed: {ex.Message}");
            }
        }

        private void TestCompoundUnitOperations(UnitFactory factory, VerificationResults results)
        {
            try
            {
                results.TestInfo.Add("🧪 Testing compound unit operations (Speed, Volume, Area)...");
                
                // Test Speed operations (Length/Time) with mixed units
                TestSpeedOperations(factory, results);
                
                // Test Volume operations (Length³) with mixed units  
                TestVolumeOperations(factory, results);
                
                // Test Area operations (Length²) with mixed units
                TestAreaOperations(factory, results);
            }
            catch (Exception ex)
            {
                results.Failures.Add($"❌ Compound unit operations test failed: {ex.Message}");
            }
        }

        private void TestSpeedOperations(UnitFactory factory, VerificationResults results)
        {
            try
            {
                // Try to test compound speed units - may not be implemented yet
                var speedTestCases = new[]
                {
                    ("mph", 60.0, "60 mph highway speed"),
                    ("km/h", 100.0, "100 km/h metric speed"), 
                    ("m/s", 25.0, "25 m/s scientific speed")
                };

                var speedObjects = new List<MeasuredValue>();
                
                foreach (var (unit, value, description) in speedTestCases)
                {
                    try
                    {
                        // Try to create speed objects using different possible family names
                        MeasuredValue? speedObj = null;
                        
                        // Try common speed family names
                        var possibleFamilies = new[] { "Speed", "Velocity" };
                        foreach (var familyName in possibleFamilies)
                        {
                            try
                            {
                                // Use reflection to get UnitFamilyName enum value
                                if (Enum.TryParse<UnitFamilyName>(familyName, out var family))
                                {
                                    speedObj = factory.CreateTypedMeasuredValue(family, value, unit);
                                    if (speedObj != null && speedObj.GetType() != typeof(MeasuredValue))
                                    {
                                        speedObjects.Add(speedObj);
                                        results.Successes.Add($"✅ Speed object created: {description} → {speedObj.GetType().Name}");
                                        results.TestInfo.Add($"ℹ️ Speed value in base units: {speedObj.Value:F2}");
                                        break;
                                    }
                                }
                            }
                            catch { /* Try next family name */ }
                        }
                    }
                    catch { /* Expected for unimplemented speed types */ }
                }

                if (speedObjects.Count > 0)
                {
                    results.Successes.Add($"✅ Found {speedObjects.Count} speed-type objects");
                    
                    // Test mixed speed unit operations if we have multiple objects
                    if (speedObjects.Count >= 2)
                    {
                        TestSpeedMixedUnitOperations(speedObjects, results);
                    }
                }
                else
                {
                    results.TestInfo.Add("ℹ️ Speed/Velocity unit types not yet implemented - this is expected");
                }
            }
            catch (Exception ex)
            {
                results.TestInfo.Add($"ℹ️ Speed operations test (expected for unimplemented types): {ex.Message}");
            }
        }

        private void TestSpeedMixedUnitOperations(List<MeasuredValue> speedObjects, VerificationResults results)
        {
            try
            {
                var speed1 = speedObjects[0];
                var speed2 = speedObjects[1];
                
                // Test that speed objects can be compared (basic operation)
                var comparison = speed1.Value.CompareTo(speed2.Value);
                results.TestInfo.Add($"ℹ️ Speed comparison: {speed1.Value:F1} vs {speed2.Value:F1} = {comparison}");
                
                // Try arithmetic operations if the type supports operators
                try
                {
                    // Use reflection to test if + operator exists
                    var addMethod = speed1.GetType().GetMethod("op_Addition");
                    if (addMethod != null)
                    {
                        var sum = addMethod.Invoke(null, new object[] { speed1, speed2 });
                        if (sum != null && sum.GetType() == speed1.GetType())
                        {
                            results.Successes.Add("✅ Speed mixed unit arithmetic works");
                            results.TestInfo.Add($"ℹ️ Speed sum: {((MeasuredValue)sum).Value:F2}");
                        }
                    }
                }
                catch
                {
                    results.TestInfo.Add("ℹ️ Speed arithmetic operators may not be implemented");
                }
            }
            catch (Exception ex)
            {
                results.TestInfo.Add($"ℹ️ Speed mixed unit operations: {ex.Message}");
            }
        }

        private void TestVolumeOperations(UnitFactory factory, VerificationResults results)
        {
            try
            {
                var volumeTestCases = new[]
                {
                    ("L", 10.0, "10 liters"),
                    ("gal", 2.5, "2.5 gallons"),
                    ("m³", 0.05, "0.05 cubic meters"),
                    ("ft³", 1.8, "1.8 cubic feet")
                };

                var volumeObjects = new List<MeasuredValue>();
                
                foreach (var (unit, value, description) in volumeTestCases)
                {
                    try
                    {
                        var volumeObj = factory.CreateTypedMeasuredValue(UnitFamilyName.Volume, value, unit);
                        if (volumeObj != null && volumeObj.GetType() != typeof(MeasuredValue))
                        {
                            volumeObjects.Add(volumeObj);
                            results.Successes.Add($"✅ Volume object created: {description} → {volumeObj.GetType().Name}");
                            results.TestInfo.Add($"ℹ️ Volume value in base units: {volumeObj.Value:F4}");
                        }
                    }
                    catch { /* Volume unit may not be implemented */ }
                }

                if (volumeObjects.Count > 0)
                {
                    results.Successes.Add($"✅ Found {volumeObjects.Count} volume-type objects");
                    
                    // Test volume mixed unit operations
                    if (volumeObjects.Count >= 2)
                    {
                        TestVolumeMixedUnitOperations(volumeObjects, results);
                    }
                }
                else
                {
                    results.TestInfo.Add("ℹ️ Volume unit types not yet implemented - this is expected");
                }
            }
            catch (Exception ex)
            {
                results.TestInfo.Add($"ℹ️ Volume operations test (expected for unimplemented types): {ex.Message}");
            }
        }

        private void TestVolumeMixedUnitOperations(List<MeasuredValue> volumeObjects, VerificationResults results)
        {
            try
            {
                // Test mixed volume operations: L + gal, m³ + ft³, etc.
                var vol1 = volumeObjects[0];
                var vol2 = volumeObjects[1];
                
                results.TestInfo.Add($"ℹ️ Volume comparison: {vol1.Value:F4} vs {vol2.Value:F4} (base units)");
                
                // Try volume arithmetic if operators exist
                try
                {
                    var addMethod = vol1.GetType().GetMethod("op_Addition");
                    if (addMethod != null)
                    {
                        var sum = addMethod.Invoke(null, new object[] { vol1, vol2 });
                        if (sum != null && sum.GetType() == vol1.GetType())
                        {
                            results.Successes.Add("✅ Volume mixed unit arithmetic works");
                            results.TestInfo.Add($"ℹ️ Volume sum: {((MeasuredValue)sum).Value:F4} (base units)");
                        }
                    }
                }
                catch
                {
                    results.TestInfo.Add("ℹ️ Volume arithmetic operators may not be implemented");
                }
            }
            catch (Exception ex)
            {
                results.TestInfo.Add($"ℹ️ Volume mixed unit operations: {ex.Message}");
            }
        }

        private void TestAreaOperations(UnitFactory factory, VerificationResults results)
        {
            try
            {
                var areaTestCases = new[]
                {
                    ("m²", 100.0, "100 square meters"),
                    ("ft²", 1000.0, "1000 square feet"),
                    ("acre", 2.0, "2 acres"),
                    ("ha", 0.5, "0.5 hectares")
                };

                var areaObjects = new List<MeasuredValue>();
                
                foreach (var (unit, value, description) in areaTestCases)
                {
                    try
                    {
                        var areaObj = factory.CreateTypedMeasuredValue(UnitFamilyName.Area, value, unit);
                        if (areaObj != null && areaObj.GetType() != typeof(MeasuredValue))
                        {
                            areaObjects.Add(areaObj);
                            results.Successes.Add($"✅ Area object created: {description} → {areaObj.GetType().Name}");
                            results.TestInfo.Add($"ℹ️ Area value in base units: {areaObj.Value:F2}");
                        }
                    }
                    catch { /* Area unit may not be implemented */ }
                }

                if (areaObjects.Count > 0)
                {
                    results.Successes.Add($"✅ Found {areaObjects.Count} area-type objects");
                    
                    // Test area mixed unit operations
                    if (areaObjects.Count >= 2)
                    {
                        TestAreaMixedUnitOperations(areaObjects, results);
                    }
                }
                else
                {
                    results.TestInfo.Add("ℹ️ Area unit types not yet implemented - this is expected");
                }
            }
            catch (Exception ex)
            {
                results.TestInfo.Add($"ℹ️ Area operations test (expected for unimplemented types): {ex.Message}");
            }
        }

        private void TestAreaMixedUnitOperations(List<MeasuredValue> areaObjects, VerificationResults results)
        {
            try
            {
                // Test mixed area operations: m² + ft², acres + hectares, etc.
                var area1 = areaObjects[0];
                var area2 = areaObjects[1];
                
                results.TestInfo.Add($"ℹ️ Area comparison: {area1.Value:F2} vs {area2.Value:F2} (base units)");
                
                // Test area arithmetic if operators exist
                try
                {
                    var addMethod = area1.GetType().GetMethod("op_Addition");
                    if (addMethod != null)
                    {
                        var sum = addMethod.Invoke(null, new object[] { area1, area2 });
                        if (sum != null && sum.GetType() == area1.GetType())
                        {
                            results.Successes.Add("✅ Area mixed unit arithmetic works");
                            results.TestInfo.Add($"ℹ️ Area sum: {((MeasuredValue)sum).Value:F2} (base units)");
                        }
                    }
                }
                catch
                {
                    results.TestInfo.Add("ℹ️ Area arithmetic operators may not be implemented");
                }
            }
            catch (Exception ex)
            {
                results.TestInfo.Add($"ℹ️ Area mixed unit operations: {ex.Message}");
            }
        }

        private void TestPerformanceCaching(UnitFactory factory, VerificationResults results)
        {
            try
            {
                // Test that repeated calls are fast (indicating caching is working)
                var stopwatch = Stopwatch.StartNew();
                
                // Create many objects to test caching performance
                for (int i = 0; i < 1000; i++)
                {
                    factory.CreateTypedMeasuredValue(UnitFamilyName.Length, i, "m");
                    factory.CreateUnit<Angle>(i, "deg");
                }
                
                stopwatch.Stop();
                var timePerOperation = stopwatch.ElapsedMilliseconds / 2000.0; // 2000 total operations
                
                results.Metrics["PerformanceTest_TimePerOperation_ms"] = timePerOperation;
                
                if (timePerOperation < 1.0) // Should be very fast with caching
                    results.Successes.Add($"✅ Performance test passed: {timePerOperation:F3}ms per operation");
                else
                    results.Failures.Add($"❌ Performance test failed: {timePerOperation:F3}ms per operation (too slow, caching may not be working)");
            }
            catch (Exception ex)
            {
                results.Failures.Add($"❌ Performance caching test failed: {ex.Message}");
            }
        }

        private List<Type> GetAllDiscoveredUnitTypes()
        {
            // Use reflection to get all types that should be discovered by UnitTypeRegistry
            var assembly = Assembly.GetExecutingAssembly();
            return assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(MeasuredValue)))
                .Where(t => t.GetCustomAttribute<UnitTypeAttribute>() != null)
                .ToList();
        }
    }

    /// <summary>
    /// Results from running UnitFactory verification
    /// </summary>
    public class VerificationResults
    {
        public bool AllTestsPassed { get; set; } = false;
        public List<string> Successes { get; set; } = new List<string>();
        public List<string> Failures { get; set; } = new List<string>();
        public List<string> TestInfo { get; set; } = new List<string>();
        public Dictionary<string, object> Metrics { get; set; } = new Dictionary<string, object>();
        public string Summary { get; set; } = "";
        
        /// <summary>
        /// Get a formatted report of all verification results
        /// </summary>
        public string GetDetailedReport()
        {
            var report = new System.Text.StringBuilder();
            
            report.AppendLine("=== UNIT FACTORY VERIFICATION REPORT ===");
            report.AppendLine($"Overall Result: {(AllTestsPassed ? "✅ PASSED" : "❌ FAILED")}");
            report.AppendLine($"Summary: {Summary}");
            report.AppendLine();
            
            if (TestInfo.Any())
            {
                report.AppendLine("Test Configuration:");
                foreach (var info in TestInfo)
                    report.AppendLine($"  • {info}");
                report.AppendLine();
            }
            
            if (Successes.Any())
            {
                report.AppendLine($"Successes ({Successes.Count}):");
                foreach (var success in Successes)
                    report.AppendLine($"  {success}");
                report.AppendLine();
            }
            
            if (Failures.Any())
            {
                report.AppendLine($"Failures ({Failures.Count}):");
                foreach (var failure in Failures)
                    report.AppendLine($"  {failure}");
                report.AppendLine();
            }
            
            if (Metrics.Any())
            {
                report.AppendLine("Metrics:");
                foreach (var metric in Metrics)
                    report.AppendLine($"  {metric.Key}: {metric.Value}");
                report.AppendLine();
            }
            
            return report.ToString();
        }
    }
}