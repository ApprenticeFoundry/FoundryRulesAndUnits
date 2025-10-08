# ContextWrapper<T> Usage Guide

## Overview

The `ContextWrapper<T>` class from the FoundryRulesAndUnits library (v9.1.0) provides a standardized pattern for API responses, error handling, and data exchange with built-in metadata support. It wraps your data with contextual information including timestamps, error states, and collection metadata.

**Current Version**: 9.1.0 targeting .NET 9.0  
**JSON Support**: System.Text.Json with FoundryRulesAndUnits.Extensions  
**Architecture**: Collection-based internal storage with type-safe access methods

## Key Features

- **Standardized API Responses**: Consistent response format across your application
- **Built-in Error Handling**: Integrated success/failure states with error messages  
- **Automatic Timestamps**: Every wrapper includes creation timestamp
- **Collection Support**: Works with single objects, lists, and enumerables
- **JSON Serialization**: Full support for serialization/deserialization
- **Type Safety**: Generic implementation maintains compile-time type checking

## Important Design Principle

**ContextWrapper<T> ALWAYS contains a collection internally via the `payload` property.**

This means:
- ✅ Use `ContextWrapper<MyData>` - NOT `ContextWrapper<List<MyData>>`
- ✅ Single items become single-item collections automatically  
- ✅ Collections are stored directly as collections
- ✅ Access data via `wrapper.payload` (ICollection<T>) or `wrapper.PayloadAsList()` (List<T>)
- ✅ Check `wrapper.length` to see how many items are in the collection

## Installation

Add reference to FoundryRulesAndUnits library:

```xml
<PackageReference Include="ApprenticeFoundryRulesAndUnits" Version="9.1.0" />
```

**Note**: The package ID is `ApprenticeFoundryRulesAndUnits` (see project file for current version).

## Basic Usage Examples

### 1. Creating Success Responses

```csharp
using FoundryRulesAndUnits.Models;

// Static success method
var successResponse = ContextWrapper<MyData>.success("Operation completed successfully");

// Success with single data item
var dataResponse = new ContextWrapper<MyData>(myDataObject);

// Success with multiple items (pass collection directly, NOT List<T>)
var listResponse = new ContextWrapper<MyData>(myDataList);
```

### 2. Creating Error Responses

```csharp
// Static error method
var errorResponse = ContextWrapper<MyData>.exception("Something went wrong");

// Error constructor
var error = new ContextWrapper<MyData>("Validation failed");

// Error with specific failure type
var failure = new ContextWrapper<Failure>(new Failure 
{ 
    Status = false, 
    Message = "Database connection failed" 
});
```

### 3. Accessing Wrapper Data

```csharp
var wrapper = new ContextWrapper<MyData>(myDataObject);

// Access properties - payload is always a collection
ICollection<MyData> items = wrapper.payload; // Direct access to collection
List<MyData> itemsList = wrapper.PayloadAsList(); // Convenience method for List<T>
bool hasError = wrapper.hasError;
string message = wrapper.message;
DateTime timestamp = wrapper.dateTime;
int itemCount = wrapper.length;
string type = wrapper.payloadType;

// Check for errors
if (wrapper.hasError)
{
    Console.WriteLine($"Error: {wrapper.message}");
}
else
{
    Console.WriteLine($"Success: Found {wrapper.length} items");
    // Process all items in the collection - use either approach
    foreach (var item in wrapper.payload) // Direct access
    {
        // Process each item...
    }
    
    // Or use the convenience method
    foreach (var item in wrapper.PayloadAsList())
    {
        // Process each item...
    }
}
```

## Real-World Usage Patterns

### API Controller Pattern

```csharp
[ApiController]
[Route("api/[controller]")]
public class PartsController : ControllerBase
{
    public ContextWrapper<PartData> GetParts()
    {
        try 
        {
            var parts = LoadPartsFromDatabase(); // Returns List<PartData>
            return new ContextWrapper<PartData>(parts); // Pass collection directly
        }
        catch (Exception ex)
        {
            return new ContextWrapper<PartData>(ex.Message);
        }
    }

    public ContextWrapper<PartData> GetPart(int id)
    {
        try
        {
            var part = FindPartById(id);
            if (part == null)
                return new ContextWrapper<PartData>("Part not found");
                
            return new ContextWrapper<PartData>(part); // Single item becomes collection
        }
        catch (Exception ex)
        {
            return new ContextWrapper<PartData>($"Error retrieving part: {ex.Message}");
        }
    }
}
```

### Service Layer Pattern

```csharp
public class DataService
{
    public ContextWrapper<ProcessResult> ProcessData(InputData input)
    {
        // Validation
        if (input == null)
            return ContextWrapper<ProcessResult>.exception("Input cannot be null");
            
        if (!IsValidInput(input))
            return new ContextWrapper<ProcessResult>("Invalid input data");

        try
        {
            var result = PerformProcessing(input);
            return new ContextWrapper<ProcessResult>(result);
        }
        catch (Exception ex)
        {
            return ContextWrapper<ProcessResult>.exception($"Processing failed: {ex.Message}");
        }
    }

    public ContextWrapper<SearchResult> Search(string query)
    {
        if (string.IsNullOrEmpty(query))
            return new ContextWrapper<SearchResult>("Search query cannot be empty");

        var results = PerformSearch(query); // Returns List<SearchResult>
        
        if (!results.Any())
            return new ContextWrapper<SearchResult>(new List<SearchResult>(), "No results found");
            
        return new ContextWrapper<SearchResult>(results); // Pass collection directly
    }
}
```

### JSON Serialization Example

```csharp
using FoundryRulesAndUnits.Extensions;
using System.Text.Json;

// Serialize wrapper to JSON (using System.Text.Json)
var wrapper = new ContextWrapper<MyData>(myDataObject);
string json = JsonSerializer.Serialize(wrapper, new JsonSerializerOptions { WriteIndented = true });

// Deserialize from JSON
var deserializedWrapper = JsonSerializer.Deserialize<ContextWrapper<MyData>>(json);

// Alternative: Using FoundryRulesAndUnits extensions (if available)
// string json = CodingExtensions.Dehydrate(wrapper, true);
// var deserializedWrapper = CodingExtensions.HydrateWrapper<MyData>(json, true);

// Access the deserialized data
if (deserializedWrapper != null && !deserializedWrapper.hasError)
{
    var data = deserializedWrapper.PayloadAsList().FirstOrDefault();
    // Use your data...
}
```

### Client-Side Consumption Pattern

```csharp
public async Task<MyData?> GetDataAsync(int id)
{
    var response = await httpClient.GetStringAsync($"/api/data/{id}");
    var wrapper = JsonSerializer.Deserialize<ContextWrapper<MyData>>(response);
    
    if (wrapper == null || wrapper.hasError)
    {
        throw new ApplicationException($"API Error: {wrapper?.message ?? "Unknown error"}");
    }
    
    return wrapper.payload.FirstOrDefault(); // Direct access to collection
}

public async Task<List<MyData>> GetDataListAsync()
{
    var response = await httpClient.GetStringAsync("/api/data");
    var wrapper = JsonSerializer.Deserialize<ContextWrapper<MyData>>(response);
    
    if (wrapper == null || wrapper.hasError)
    {
        logger.LogError($"Failed to retrieve data: {wrapper?.message ?? "Unknown error"}");
        return new List<MyData>();
    }
    
    // payload is already a collection - convert to List if needed
    return wrapper.payload.ToList(); // Or use wrapper.PayloadAsList() convenience method
}
```

## Advanced Usage Patterns

### Fluent Response Building

```csharp
public static class ContextWrapperExtensions
{
    public static ContextWrapper<T> WithTimestamp<T>(this ContextWrapper<T> wrapper, DateTime timestamp)
    {
        wrapper.dateTime = timestamp;
        return wrapper;
    }
    
    public static ContextWrapper<T> WithMessage<T>(this ContextWrapper<T> wrapper, string message)
    {
        wrapper.message = message;
        return wrapper;
    }
}

// Usage
var response = new ContextWrapper<MyData>(data)
    .WithMessage("Data retrieved successfully")
    .WithTimestamp(DateTime.UtcNow);
```

### Repository Pattern Integration

```csharp
public interface IRepository<T>
{
    Task<ContextWrapper<T>> GetByIdAsync(int id);
    Task<ContextWrapper<T>> GetAllAsync(); // Returns T, not List<T>
    Task<ContextWrapper<T>> CreateAsync(T entity);
    Task<ContextWrapper<T>> UpdateAsync(T entity);
    Task<ContextWrapper<Success>> DeleteAsync(int id);
}

public class Repository<T> : IRepository<T> where T : class
{
    public async Task<ContextWrapper<T>> GetByIdAsync(int id)
    {
        try
        {
            var entity = await context.Set<T>().FindAsync(id);
            if (entity == null)
                return new ContextWrapper<T>("Entity not found");
                
            return new ContextWrapper<T>(entity);
        }
        catch (Exception ex)
        {
            return new ContextWrapper<T>($"Database error: {ex.Message}");
        }
    }
    
    public async Task<ContextWrapper<Success>> DeleteAsync(int id)
    {
        try
        {
            var entity = await context.Set<T>().FindAsync(id);
            if (entity == null)
                return new ContextWrapper<Success>("Entity not found");
                
            context.Set<T>().Remove(entity);
            await context.SaveChangesAsync();
            
            return ContextWrapper<Success>.success("Entity deleted successfully");
        }
        catch (Exception ex)
        {
            return ContextWrapper<Success>.exception($"Delete failed: {ex.Message}");
        }
    }
}
```

## Best Practices

### ✅ **DO:**
- Use `ContextWrapper<T>` for all API responses to maintain consistency
- Access data via `wrapper.payload` directly (it's already a collection)
- Use `wrapper.PayloadAsList()` when you specifically need a `List<T>`
- Use single generic type parameter (`MyData`), never `List<MyData>`
- Include meaningful error messages when creating error responses
- Use static success/exception methods for simple responses
- Leverage the automatic timestamp feature for audit trails
- Check `wrapper.length` to determine collection size

### ❌ **DON'T:**
- Don't use `ContextWrapper<List<T>>` - use `ContextWrapper<T>` instead
- Don't mix wrapped and unwrapped responses in the same API
- Don't ignore the hasError property when consuming responses
- Don't put sensitive information in error messages
- Don't create empty wrappers unnecessarily
- Don't forget that `payload` is already a collection - no conversion needed

## Error Handling Strategies

### Validation Pattern
```csharp
public ContextWrapper<UserData> ValidateAndProcess(UserInput input)
{
    var validationErrors = new List<string>();
    
    if (string.IsNullOrEmpty(input.Name))
        validationErrors.Add("Name is required");
        
    if (input.Age < 0)
        validationErrors.Add("Age must be positive");
        
    if (validationErrors.Any())
        return new ContextWrapper<UserData>(string.Join("; ", validationErrors));
        
    // Process valid input
    var result = ProcessInput(input);
    return new ContextWrapper<UserData>(result);
}
```

### Chain of Operations Pattern
```csharp
public ContextWrapper<FinalResult> ProcessChain(InputData input)
{
    var step1 = ValidateInput(input);
    if (step1.hasError) return new ContextWrapper<FinalResult>(step1.message);
    
    var step2 = TransformData(step1.payload.First()); // Direct access to collection
    if (step2.hasError) return new ContextWrapper<FinalResult>(step2.message);
    
    var step3 = SaveResult(step2.payload.First());
    if (step3.hasError) return new ContextWrapper<FinalResult>(step3.message);
    
    return new ContextWrapper<FinalResult>(step3.payload.First());
}
```

## Integration with Other Libraries

The ContextWrapper class integrates seamlessly with:

- **ASP.NET Core**: Return from controllers for consistent API responses
- **Entity Framework**: Wrap database operation results
- **AutoMapper**: Map between wrapped DTOs and domain objects
- **FluentValidation**: Combine with validation results
- **Newtonsoft.Json**: Full serialization support (use CodingExtensions)

## JSON Schema Example

When serialized, a ContextWrapper produces JSON like this:

```json
{
  "dateTime": "2025-09-25T10:30:00.000Z",
  "length": 2,
  "payloadType": "MyData",
  "payload": [
    { "id": 1, "name": "Item 1" },
    { "id": 2, "name": "Item 2" }
  ],
  "hasError": false,
  "message": ""
}
```

Error response example:
```json
{
  "dateTime": "2025-09-25T10:30:00.000Z",
  "length": 0,
  "payloadType": "MyData",
  "payload": [],
  "hasError": true,
  "message": "Validation failed: Name is required"
}
```

## Performance Considerations

- ContextWrapper adds minimal overhead to your responses
- Automatic timestamp generation uses `DateTime.UtcNow`
- Collections are converted to arrays during enumerable construction
- JSON serialization leverages System.Text.Json for optimal performance

## Summary

The ContextWrapper<T> class provides a robust, standardized approach to API responses and data exchange. It combines data payload with essential metadata, error handling, and timestamps in a type-safe, serializable package. This pattern promotes consistency across your application and simplifies error handling for consuming code.

**Version 9.1.0 Benefits:**
- Native .NET 9.0 support with System.Text.Json
- Enhanced performance and memory efficiency
- Simplified serialization patterns
- Type-safe collection handling

For more information about the FoundryRulesAndUnits library, see the complete documentation and other utility classes available in the library.