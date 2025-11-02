# ContextWrapper Factory Methods - Preferred Usage Guide

The **static factory methods** are the recommended way to create `ContextWrapper<T>` instances. They provide crystal-clear intent and eliminate ambiguity.

## 🎯 Why Use Factory Methods?

- **🔍 Clear Intent**: `Error()` vs `Ok()` vs `Empty()` - no guessing
- **🛡️ Type Safety**: Eliminates the string ambiguity problem 
- **📖 Self-Documenting**: Code reads like plain English
- **🔧 IntelliSense**: Shows all available options when you type `ContextWrapper<T>.`

## 📋 Complete Factory Method Reference

### ❌ Error Creation

```csharp
// Create error wrapper (no payload)
return ContextWrapper<DocumentDTO>.Error("Document not found");
return ContextWrapper<string>.Error("Invalid file format");
return ContextWrapper<User>.Error("Authentication failed");
```

**Result**: `hasError = true`, `message = "your message"`, `length = 0`

### ✅ Success with Single Item

```csharp
// Create success wrapper with one item
return ContextWrapper<DocumentDTO>.Ok(document);
return ContextWrapper<string>.Ok("Hello World");
return ContextWrapper<User>.Ok(currentUser);

// With optional success message
return ContextWrapper<DocumentDTO>.Ok(document, "Document loaded successfully");
```

**Result**: `hasError = false`, `length = 1`, item in `payload`

### ✅ Success with Multiple Items

```csharp
// Create success wrapper with collection
var users = await GetUsersAsync();
return ContextWrapper<User>.Ok(users);

var messages = new[] { "Hello", "World", "!" };
return ContextWrapper<string>.Ok(messages);

// With optional success message  
return ContextWrapper<User>.Ok(users, "Found 5 users");
```

**Result**: `hasError = false`, `length = items.Count()`, all items in `payload`

### 🗂️ Empty Success

```csharp
// Create empty wrapper (no payload, no error)
return ContextWrapper<DocumentDTO>.Empty();
return ContextWrapper<string>.Empty();
```

**Result**: `hasError = false`, `length = 0`, empty `payload`

### ⚠️ Deprecated (Special Case)

```csharp
// Mark code that needs migration
return ContextWrapper<User>.Deprecated(user, "Migrate to new Result<T> pattern");
```

**Result**: Success wrapper with `isDeprecated = true` flag

## 🚀 Real-World Examples

### API Controller Pattern

```csharp
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<ContextWrapper<User>> GetUser(int id)
    {
        // Input validation
        if (id <= 0)
            return ContextWrapper<User>.Error("Invalid user ID");
        
        // Database lookup
        var user = await _userService.GetByIdAsync(id);
        if (user == null)
            return ContextWrapper<User>.Error($"User {id} not found");
        
        // Success
        return ContextWrapper<User>.Ok(user);
    }
    
    [HttpGet]
    public async Task<ContextWrapper<User>> GetAllUsers()
    {
        var users = await _userService.GetAllAsync();
        
        if (!users.Any())
            return ContextWrapper<User>.Empty(); // No users found, but not an error
        
        return ContextWrapper<User>.Ok(users, $"Found {users.Count()} users");
    }
}
```

### Service Layer Pattern

```csharp
public class DocumentService
{
    public async Task<ContextWrapper<string>> GetDocumentContentAsync(string path)
    {
        // Validation
        if (string.IsNullOrWhiteSpace(path))
            return ContextWrapper<string>.Error("Document path is required");
        
        if (!File.Exists(path))
            return ContextWrapper<string>.Error($"File not found: {path}");
        
        try
        {
            // Read file
            var content = await File.ReadAllTextAsync(path);
            
            if (string.IsNullOrEmpty(content))
                return ContextWrapper<string>.Empty(); // File exists but is empty
            
            return ContextWrapper<string>.Ok(content, "Document loaded successfully");
        }
        catch (Exception ex)
        {
            return ContextWrapper<string>.Error($"Error reading file: {ex.Message}");
        }
    }
}
```

### Multi-Service Orchestration

```csharp
public class OrderService
{
    public async Task<ContextWrapper<Order>> CreateOrderAsync(int userId, List<int> productIds)
    {
        // Get user
        var userResult = await _userService.GetUserAsync(userId);
        if (userResult.hasError)
            return ContextWrapper<Order>.Error($"User error: {userResult.message}");
        
        // Get products
        var productsResult = await _productService.GetProductsAsync(productIds);
        if (productsResult.hasError)
            return ContextWrapper<Order>.Error($"Product error: {productsResult.message}");
        
        if (productsResult.IsEmpty())
            return ContextWrapper<Order>.Error("No valid products found");
        
        // Create order
        var order = new Order 
        { 
            User = userResult.payload.First(),
            Products = productsResult.PayloadAsList(),
            CreatedAt = DateTime.UtcNow
        };
        
        await _orderRepository.SaveAsync(order);
        
        return ContextWrapper<Order>.Ok(order, "Order created successfully");
    }
}
```

## 💡 Best Practices

### ✅ DO: Use Factory Methods

```csharp
// ✅ Clear and explicit
return ContextWrapper<string>.Error("Invalid input");
return ContextWrapper<User>.Ok(user);
return ContextWrapper<Document>.Empty();
```

### ❌ AVOID: Ambiguous Constructors

```csharp
// ❌ Don't use for error-only cases (no longer compiles anyway)
// return new ContextWrapper<string>("error message"); // REMOVED!

// ⚠️ These work but are less clear than factory methods
return new ContextWrapper<User>(user);           // OK but Ok(user) is clearer
return new ContextWrapper<User>();               // OK but Empty() is clearer
```

### 🎯 String Payloads - Always Use Factory Methods

```csharp
// ✅ Crystal clear for string data
return ContextWrapper<string>.Ok("actual data");
return ContextWrapper<string>.Error("error message");

// ⚠️ This works but intent is unclear:
return new ContextWrapper<string>("text"); // Is this payload? (Yes, but not obvious)
```

## 🔄 Migration from Constructors

If you have existing code using constructors, here's how to migrate:

```csharp
// OLD WAY (still works, but less clear)
return new ContextWrapper<User>(user);
return new ContextWrapper<User>();
// return new ContextWrapper<User>("error"); // ← This never worked anyway

// NEW WAY (preferred)
return ContextWrapper<User>.Ok(user);
return ContextWrapper<User>.Empty();
return ContextWrapper<User>.Error("error");
```

## 📦 Package Info

- **Package**: `ApprenticeFoundryRulesAndUnits`
- **Version**: 10.0.0+
- **Namespace**: `FoundryRulesAndUnits.Models`

## 🎉 Summary

Factory methods make your code:
- **More readable** - Intent is obvious
- **Less error-prone** - No ambiguity with string types
- **More discoverable** - IntelliSense shows options
- **Self-documenting** - No comments needed to explain intent

**Recommendation**: Use factory methods for all new code! 🚀