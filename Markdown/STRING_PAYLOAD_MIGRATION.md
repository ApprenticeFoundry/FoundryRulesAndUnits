# ContextWrapper<string> Migration Guide

## The String Payload Ambiguity Problem - SOLVED! ✅

This document explains the long-standing `ContextWrapper<string>` ambiguity problem and how the new static factory methods completely solve it.

---

## 🚨 The Problem

### What Was Wrong?

For years, `ContextWrapper<string>` had a **critical ambiguity** issue:

```csharp
// AMBIGUOUS - Is this an error or a payload?
var wrapper = new ContextWrapper<string>("some text");
```

Looking at this code, **it's impossible to determine** whether:
1. ✅ This is a **SUCCESS** with payload = `"some text"`
2. ❌ This is an **ERROR** with message = `"some text"`

### Why Did This Happen?

The problem stems from constructor overloading:

```csharp
// Constructor 1: Create wrapper with payload
public ContextWrapper(T? obj, string error = "")

// Constructor 2: Create wrapper with error message
public ContextWrapper(string error)

// When T = string, BOTH constructors accept a single string parameter!
// The compiler can't tell which one you meant.
```

### Real-World Impact

This affected any service method returning string data:

```csharp
// ❌ BROKEN PATTERN - Don't do this!
public ContextWrapper<string> GetUserEmail(int userId)
{
    if (userId < 0)
        return new ContextWrapper<string>("Invalid user ID");  // Is this an error?
    
    var email = _db.GetEmail(userId);
    if (email == null)
        return new ContextWrapper<string>("user@example.com"); // Is this data?
    
    return new ContextWrapper<string>(email);  // What about this?
}

// QUESTION: Which lines are errors and which are data?
// ANSWER: You can't tell without reading the database code!
```

**Result**: Developers avoided `ContextWrapper<string>` entirely, breaking consistency.

---

## ✅ The Solution: Static Factory Methods

### New API - Crystal Clear Intent

```csharp
// UNAMBIGUOUS - This is definitely an error
var error = ContextWrapper<string>.Error("File not found");
// hasError = true, message = "File not found", length = 0

// UNAMBIGUOUS - This is definitely data
var data = ContextWrapper<string>.Ok("Hello World");
// hasError = false, payload contains "Hello World", length = 1
```

### Real-World Example - Fixed

```csharp
// ✅ FIXED PATTERN - Crystal clear!
public ContextWrapper<string> GetUserEmail(int userId)
{
    if (userId < 0)
        return ContextWrapper<string>.Error("Invalid user ID");  // Explicit error ❌
    
    var email = _db.GetEmail(userId);
    if (email == null)
        return ContextWrapper<string>.Error("Email not found"); // Explicit error ❌
    
    return ContextWrapper<string>.Ok(email);  // Explicit success ✅
}

// NOW: Every line is crystal clear!
// - Error() = Error state
// - Ok() = Success state
```

---

## 📋 Migration Checklist

### Step 1: Find All ContextWrapper<string> Usage

Search your codebase for:
- `new ContextWrapper<string>(`
- `ContextWrapper<string> result =`
- Service methods returning `ContextWrapper<string>`

### Step 2: Replace with Factory Methods

#### For Error Cases:
```csharp
// ❌ BEFORE (ambiguous)
return new ContextWrapper<string>("Error: not found");

// ✅ AFTER (explicit)
return ContextWrapper<string>.Error("Error: not found");
```

#### For Success Cases:
```csharp
// ❌ BEFORE (ambiguous)
return new ContextWrapper<string>(userName);

// ✅ AFTER (explicit)
return ContextWrapper<string>.Ok(userName);
```

#### For List/Collection Cases:
```csharp
// ❌ BEFORE
return new ContextWrapper<string>(nameList);

// ✅ AFTER
return ContextWrapper<string>.Ok(nameList);
```

#### For Empty Cases:
```csharp
// ❌ BEFORE
return new ContextWrapper<string>();

// ✅ AFTER
return ContextWrapper<string>.Empty();
```

### Step 3: Update Consumer Code (Usually No Changes Needed!)

The consuming code typically doesn't need changes because the wrapper structure is the same:

```csharp
// Consumer code works exactly the same
var result = GetUserEmail(userId);

if (result.hasError)
{
    Console.WriteLine($"Error: {result.message}");
}
else if (result.length > 0)
{
    var email = result.payload.First();
    Console.WriteLine($"Email: {email}");
}
```

---

## 🎯 Benefits Summary

### Before Factory Methods:
- ❌ `new ContextWrapper<string>("text")` - Ambiguous
- ❌ Required code comments to clarify intent
- ❌ Prone to bugs when reading/maintaining code
- ❌ Developers avoided `ContextWrapper<string>`
- ❌ No IntelliSense help

### After Factory Methods:
- ✅ `Error("text")` vs `Ok("text")` - Explicit
- ✅ Self-documenting code
- ✅ Impossible to misinterpret
- ✅ Consistent API across all types
- ✅ IntelliSense shows available options

---

## 🔧 Complete API Reference

### Factory Methods (RECOMMENDED)

```csharp
// Error creation
ContextWrapper<string>.Error("error message")

// Success with single item
ContextWrapper<string>.Ok("single string")

// Success with multiple items
ContextWrapper<string>.Ok(new[] { "item1", "item2", "item3" })

// Empty success (no payload, no error)
ContextWrapper<string>.Empty()
```

### Legacy Constructors (Still Work, But Ambiguous)

```csharp
// These still work but are AMBIGUOUS for string payloads:
new ContextWrapper<string>("text")           // ⚠️ Error or data? Can't tell!
new ContextWrapper<string>(item, "error")    // ⚠️ Confusing parameter order
```

**RECOMMENDATION**: Use factory methods for all new code!

---

## 🧪 Testing Your Migration

Add these test cases to verify correct migration:

```csharp
[Test]
public void StringWrapper_Error_ShouldHaveError()
{
    var result = ContextWrapper<string>.Error("Test error");
    
    Assert.IsTrue(result.hasError);
    Assert.AreEqual("Test error", result.message);
    Assert.AreEqual(0, result.length);
}

[Test]
public void StringWrapper_Ok_ShouldHavePayload()
{
    var result = ContextWrapper<string>.Ok("Test data");
    
    Assert.IsFalse(result.hasError);
    Assert.AreEqual(1, result.length);
    Assert.AreEqual("Test data", result.payload.First());
}

[Test]
public void StringWrapper_OkList_ShouldHaveMultipleItems()
{
    var items = new[] { "A", "B", "C" };
    var result = ContextWrapper<string>.Ok(items);
    
    Assert.IsFalse(result.hasError);
    Assert.AreEqual(3, result.length);
    CollectionAssert.AreEqual(items, result.PayloadAsList());
}
```

---

## 📚 Additional Resources

- See `TestFactoryMethods.cs` for complete examples
- See `CONTEXTWRAPPER_USAGE_GUIDE.md` for full API documentation
- See README.md for quick reference

---

**Version**: 10.0.0  
**Date**: October 29, 2025  
**Breaking Changes**: None - Factory methods are additive, all existing code continues to work
