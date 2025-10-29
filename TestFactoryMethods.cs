using FoundryRulesAndUnits.Models;
using System;
using System.Collections.Generic;

namespace FoundryRulesAndUnits.Tests
{
    /// <summary>
    /// Quick verification of new ContextWrapper static factory methods
    /// Run this to test the new API before publishing
    /// </summary>
    public class TestFactoryMethods
    {
        public class SampleData
        {
            public string Name { get; set; } = "";
            public int Value { get; set; }
        }

        public static void RunTests()
        {
            Console.WriteLine("Testing ContextWrapper Static Factory Methods\n");
            Console.WriteLine("==============================================\n");

            // Test 1: Error factory method
            var error = ContextWrapper<SampleData>.Error("Something went wrong");
            Console.WriteLine($"✅ Error Factory: hasError={error.hasError}, message='{error.message}', length={error.length}");
            
            // Test 2: Ok with single item
            var singleData = new SampleData { Name = "Test", Value = 42 };
            var okSingle = ContextWrapper<SampleData>.Ok(singleData);
            Console.WriteLine($"✅ Ok(single): hasError={okSingle.hasError}, length={okSingle.length}, name={okSingle.payload.First().Name}");
            
            // Test 3: Ok with multiple items
            var listData = new List<SampleData>
            {
                new SampleData { Name = "Item1", Value = 1 },
                new SampleData { Name = "Item2", Value = 2 },
                new SampleData { Name = "Item3", Value = 3 }
            };
            var okMultiple = ContextWrapper<SampleData>.Ok(listData);
            Console.WriteLine($"✅ Ok(list): hasError={okMultiple.hasError}, length={okMultiple.length}");
            
            // Test 4: Empty factory method
            var empty = ContextWrapper<SampleData>.Empty();
            Console.WriteLine($"✅ Empty: hasError={empty.hasError}, length={empty.length}");
            
            // Test 5: AsErrorFor type conversion
            var typedError = error.AsErrorFor<string>();
            Console.WriteLine($"✅ AsErrorFor: hasError={typedError.hasError}, message='{typedError.message}', type={typedError.payloadType}");
            
            // Test 6: SetError fluent method
            var wrapper = ContextWrapper<SampleData>.Ok(singleData);
            wrapper.SetError("Additional error");
            Console.WriteLine($"✅ SetError: hasError={wrapper.hasError}, message='{wrapper.message}'");

            // Test 7: CRITICAL - ContextWrapper<string> ambiguity resolution
            Console.WriteLine("\n🎯 CRITICAL TEST: ContextWrapper<string> (previously ambiguous!)");
            var stringError = ContextWrapper<string>.Error("This is an error message");
            Console.WriteLine($"✅ String Error: hasError={stringError.hasError}, message='{stringError.message}', length={stringError.length}");
            
            var stringPayload = ContextWrapper<string>.Ok("This is actual data");
            Console.WriteLine($"✅ String Payload: hasError={stringPayload.hasError}, length={stringPayload.length}, data='{stringPayload.payload.First()}'");

            var stringList = ContextWrapper<string>.Ok(new[] { "Item1", "Item2", "Item3" });
            Console.WriteLine($"✅ String List: hasError={stringList.hasError}, length={stringList.length}");

            Console.WriteLine("\n==============================================");
            Console.WriteLine("All factory method tests completed!");
        }

        // Example usage in a typical service method
        public static ContextWrapper<SampleData> GetDataById(int id)
        {
            // Simulate database lookup
            if (id < 0)
                return ContextWrapper<SampleData>.Error("Invalid ID");
            
            if (id == 0)
                return ContextWrapper<SampleData>.Empty();
            
            var data = new SampleData { Name = $"Item{id}", Value = id };
            return ContextWrapper<SampleData>.Ok(data);
        }

        // Example multi-service orchestration
        public static ContextWrapper<string> ProcessData(int dataId)
        {
            var dataResult = GetDataById(dataId);
            
            // Type conversion on error
            if (dataResult.hasError)
                return dataResult.AsErrorFor<string>();
            
            if (dataResult.IsEmpty())
                return ContextWrapper<string>.Error("No data found");
            
            // Process and return success
            var processed = $"Processed: {dataResult.payload.First().Name}";
            return ContextWrapper<string>.Ok(processed);
        }
    }
}
