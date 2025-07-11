using FoundryRulesAndUnits.Extensions;
using FoundryRulesAndUnits.Testing.UnitSystem;

namespace FoundryRulesAndUnits.Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            "=== FoundryRulesAndUnits Unit System Demo ===".WriteInfo();
            "\nRunning comprehensive unit system tests with visual conversion tables...\n".WriteInfo();
            
            TestRunner.RunAllTests();
            
            "\n=== Demo Complete ===".WriteInfo();
            "Press any key to exit...".WriteInfo();
            Console.ReadKey();
        }
    }
}
