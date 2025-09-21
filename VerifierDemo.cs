using System;
using FoundryRulesAndUnits.Units;

namespace FoundryRulesAndUnits
{
    /// <summary>
    /// Demonstration of how to use the UnitFactoryVerifier
    /// Shows simple usage patterns for verifying the unit architecture
    /// </summary>
    public static class VerifierDemo
    {
        /// <summary>
        /// Run a basic verification test with default SI unit system
        /// </summary>
        public static void RunBasicVerification()
        {
            "🔧 Starting UnitFactory Architecture Verification...".WriteInfo();
            "".WriteInfo();

            var verifier = new UnitFactoryVerifier();
            var results = verifier.RunVerification();

            // Display results
            results.GetDetailedReport().WriteInfo();

            if (results.AllTestsPassed)
            {
                "🎉 All tests passed! The unit factory architecture is working correctly.".WriteInfo();
            }
            else
            {
                "⚠️ Some tests failed. Check the report above for details.".WriteInfo();
            }
        }

        /// <summary>
        /// Run verification with a custom unit system
        /// </summary>
        public static void RunVerificationWithCustomSystem(IUnitSystem unitSystem)
        {
            $"🔧 Verifying UnitFactory with {unitSystem.GetType().Name}...".WriteInfo();
            "".WriteInfo();

            var verifier = new UnitFactoryVerifier();
            var results = verifier.RunVerification(unitSystem);

            results.GetDetailedReport().WriteInfo();
        }

        /// <summary>
        /// Quick verification that just returns true/false
        /// Useful for integration tests or health checks
        /// </summary>
        public static bool QuickHealthCheck()
        {
            var verifier = new UnitFactoryVerifier();
            var results = verifier.RunVerification();
            return results.AllTestsPassed;
        }

        /// <summary>
        /// Example of using verification results programmatically
        /// </summary>
        public static void ExampleProgrammaticUsage()
        {
            var verifier = new UnitFactoryVerifier();
            var results = verifier.RunVerification();

            // Check specific metrics
            if (results.Metrics.ContainsKey("DiscoveredUnitTypes"))
            {
                var typeCount = results.Metrics["DiscoveredUnitTypes"];
                $"Discovered {typeCount} unit types".WriteInfo();
            }

            // Check performance
            if (results.Metrics.ContainsKey("PerformanceTest_TimePerOperation_ms"))
            {
                var timePerOp = results.Metrics["PerformanceTest_TimePerOperation_ms"];
                $"Performance: {timePerOp}ms per operation".WriteInfo();
            }

            // Count specific results
            $"Total successes: {results.Successes.Count}".WriteInfo();
            $"Total failures: {results.Failures.Count}".WriteInfo();

            // Use in application logic
            if (results.AllTestsPassed)
            {
                // Safe to proceed with unit operations
                "✅ Unit system verified - safe to proceed".WriteInfo();
            }
            else
            {
                // Handle verification failure
                "❌ Unit system issues detected".WriteInfo();
                foreach (var failure in results.Failures)
                {
                    $"  - {failure}".WriteInfo();
                }
            }
        }
    }
}