# ContextWrapper<T> Usage Guide

## Overview

The `ContextWrapper<T>` class from the FoundryRulesAndUnits library provides a standardized pattern for API responses, error handling, and data exchange with built-in metadata support. It wraps your data with contextual information including timestamps, error states, and collection metadata.

## Key Features

- **Standardized API Responses**: Consistent response format across your application
- **Built-in Error Handling**: Integrated success/failure states with error messages  
- **Automatic Timestamps**: Every wrapper includes creation timestamp
- **Collection Support**: Works with single objects, lists, and enumerables
- **JSON Serialization**: Full support for serialization/deserialization
- **Type Safety**: Generic implementation maintains compile-time type checking

## Installation

Add reference to FoundryRulesAndUnits library:

```xml
<PackageReference Include="FoundryRulesAndUnits" Version="[latest-version]" />
```

## Basic Usage Examples

### 1. Creating Success Responses

```csharp
using FoundryRulesAndUnits.Models;

// Static success method
var successResponse = ContextWrapper<MyData>.success("Operation completed successfully");

// Success with data
var dataResponse = new ContextWrapper<MyData>(myDataObject);

// Success with list
var listResponse = new ContextWrapper<List<MyData>>(myDataList);
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

// Access properties
List<MyData> items = wrapper.PayloadAsList();
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
}
```

## Real-World Usage Patterns

### API Controller Pattern

```csharp
[ApiController]
[Route("api/[controller]")]
public class PartsController : ControllerBase
{
    public ContextWrapper<List<PartData>> GetParts()
    {
        try 
        {
            var parts = LoadPartsFromDatabase();
            return new ContextWrapper<List<PartData>>(parts);
        }
        catch (Exception ex)
        {
            return new ContextWrapper<List<PartData>>(ex.Message);
        }
    }

    public ContextWrapper<PartData> GetPart(int id)
    {
        try
        {
            var part = FindPartById(id);
            if (part == null)
                return new ContextWrapper<PartData>("Part not found");
                
            return new ContextWrapper<PartData>(part);
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

    public ContextWrapper<List<SearchResult>> Search(string query)
    {
        if (string.IsNullOrEmpty(query))
            return new ContextWrapper<List<SearchResult>>("Search query cannot be empty");

        var results = PerformSearch(query);
        
        if (!results.Any())
            return new ContextWrapper<List<SearchResult>>(new List<SearchResult>(), "No results found");
            
        return new ContextWrapper<List<SearchResult>>(results);
    }
}
```

### JSON Serialization Example

```csharp
using FoundryRulesAndUnits.Extensions;

// Serialize wrapper to JSON
var wrapper = new ContextWrapper<MyData>(myDataObject);
string json = CodingExtensions.Dehydrate(wrapper, true);

// Deserialize from JSON
var deserializedWrapper = CodingExtensions.HydrateWrapper<MyData>(json, true);

// Access the deserialized data
if (!deserializedWrapper.hasError)
{
    var data = deserializedWrapper.PayloadAsList().FirstOrDefault();
    // Use your data...
}
```

### Client-Side Consumption Pattern

```csharp
public async Task<MyData> GetDataAsync(int id)
{
    var response = await httpClient.GetStringAsync($"/api/data/{id}");
    var wrapper = CodingExtensions.HydrateWrapper<MyData>(response, true);
    
    if (wrapper.hasError)
    {
        throw new ApplicationException($"API Error: {wrapper.message}");
    }
    
    return wrapper.PayloadAsList().FirstOrDefault();
}

public async Task<List<MyData>> GetDataListAsync()
{
    var response = await httpClient.GetStringAsync("/api/data");
    var wrapper = CodingExtensions.HydrateWrapper<List<MyData>>(response, true);
    
    if (wrapper.hasError)
    {
        logger.LogError($"Failed to retrieve data: {wrapper.message}");
        return new List<MyData>();
    }
    
    return wrapper.PayloadAsList();
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
    Task<ContextWrapper<List<T>>> GetAllAsync();
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
- Use ContextWrapper for all API responses to maintain consistency
- Include meaningful error messages when creating error responses
- Use static success/exception methods for simple responses
- Leverage the automatic timestamp feature for audit trails
- Use generic type parameters to maintain type safety

### ❌ **DON'T:**
- Don't mix wrapped and unwrapped responses in the same API
- Don't ignore the hasError property when consuming responses
- Don't put sensitive information in error messages
- Don't create empty wrappers unnecessarily

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
    
    var step2 = TransformData(step1.PayloadAsList().First());
    if (step2.hasError) return new ContextWrapper<FinalResult>(step2.message);
    
    var step3 = SaveResult(step2.PayloadAsList().First());
    if (step3.hasError) return new ContextWrapper<FinalResult>(step3.message);
    
    return new ContextWrapper<FinalResult>(step3.PayloadAsList().First());
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

For more information about the FoundryRulesAndUnits library, see the complete documentation and other utility classes available in the library.