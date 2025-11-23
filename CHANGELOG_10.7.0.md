# FoundryRulesAndUnits 10.7.0 Release Notes

**Release Date**: November 23, 2025  
**Package**: `ApprenticeFoundryRulesAndUnits` version 10.7.0  
**Target Framework**: .NET 9.0

---

## 🎯 What's New in 10.7.0

### **🏭 ContextWrapper Factory Methods** ⭐ MAJOR FEATURE

Added static factory methods to `ContextWrapper<T>` that provide **crystal-clear intent** and eliminate the long-standing string payload ambiguity problem.

#### **New Static Factory Methods**

```csharp
// ✅ Error creation - explicit and unambiguous
ContextWrapper<T>.Error("Error message")

// ✅ Success with data - clear intent
ContextWrapper<T>.Ok(singleItem)
ContextWrapper<T>.Ok(itemList)

// ✅ Empty success - no payload, no error
ContextWrapper<T>.Empty()

// ✅ Deprecation tracking - mark code for migration
ContextWrapper<T>.Deprecated(item, "Migrate to new pattern")
```

#### **Problem Solved: String Payload Ambiguity**

**Before (v10.6.0 and earlier)**:
```csharp
// 🤔 AMBIGUOUS - Is this an error or payload?
var wrapper = new ContextWrapper<string>("some text");
```

**After (v10.7.0)**:
```csharp
// ❌ COMPILE ERROR - Dangerous constructor removed!
// var wrapper = new ContextWrapper<string>("text");

// ✅ CRYSTAL CLEAR - Factory methods are explicit
var error = ContextWrapper<string>.Error("Error message");
var data = ContextWrapper<string>.Ok("Actual data");
```

#### **Benefits**

- **🔍 Clear Intent**: `Error()` vs `Ok()` vs `Empty()` - no guessing
- **🛡️ Type Safety**: Eliminates string ambiguity completely
- **📖 Self-Documenting**: Code reads like plain English
- **🔧 Better IntelliSense**: Shows all available options
- **🎯 Prevents Bugs**: Impossible to confuse error messages with data

#### **Real-World Example**

```csharp
[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<ContextWrapper<User>> GetUser(int id)
    {
        // Input validation - crystal clear this is an error
        if (id <= 0)
            return ContextWrapper<User>.Error("Invalid user ID");
        
        // Database lookup
        var user = await _userService.GetByIdAsync(id);
        if (user == null)
            return ContextWrapper<User>.Error($"User {id} not found");
        
        // Success - crystal clear this is data
        return ContextWrapper<User>.Ok(user);
    }
}
```

---

## 🔧 Breaking Changes

### **Removed Dangerous Constructor**

The ambiguous single-string constructor has been **removed**:

```csharp
// ❌ REMOVED - No longer compiles
public ContextWrapper(string error)
```

**Migration Path**:
```csharp
// OLD (no longer works)
return new ContextWrapper<string>("error message");

// NEW (required)
return ContextWrapper<string>.Error("error message");
```

### **Constructors That Still Work**

All other constructors remain **fully functional**:

```csharp
// ✅ These still work perfectly
new ContextWrapper<T>()                    // Empty wrapper
new ContextWrapper<T>(item)                // Single item payload
new ContextWrapper<T>(items)               // Collection payload
new ContextWrapper<T>(item, "error")       // Item with error message
```

---

## 📚 Documentation Updates

### **New Documentation**

- ✅ **CONTEXTWRAPPER_FACTORY_METHODS.md** - Comprehensive guide to factory methods
- ✅ **STRING_PAYLOAD_MIGRATION.md** - Migration guide for string payloads
- ✅ **README.md** - Updated with factory method examples and quick reference table

### **Updated Documentation**

- ✅ **CONTEXTWRAPPER_USAGE_GUIDE.md** - Added factory method patterns
- ✅ **README.md** - Updated version to 10.7.0, added ContextWrapper section
- ✅ **NUGETREADME.md** - Updated version references to 10.7.0

---

## 🎉 Key Benefits of This Release

### **1. Eliminates Ambiguity**
No more guessing whether `new ContextWrapper<string>("text")` is an error or data - the dangerous constructor is gone!

### **2. Self-Documenting Code**
```csharp
// Before: What is this?
return new ContextWrapper<string>("text");

// After: Intent is obvious
return ContextWrapper<string>.Error("Not found");
return ContextWrapper<string>.Ok("Hello World");
```

### **3. Better Developer Experience**
- IntelliSense shows all factory methods when you type `ContextWrapper<T>.`
- Clear method names guide developers to the right choice
- Compile-time errors prevent misuse

### **4. Consistent Across All Types**
Works perfectly for all types, not just strings:
```csharp
ContextWrapper<DocumentDTO>.Error("Document not found")
ContextWrapper<int>.Ok(42)
ContextWrapper<User>.Ok(userList)
```

---

## 📦 Upgrade Instructions

### From 10.6.x

**Update Package Reference**:
```xml
<PackageReference Include="ApprenticeFoundryRulesAndUnits" Version="10.7.0" />
```

**Migration Steps**:

1. **Search for `new ContextWrapper<string>(`** - These need updates
2. **Replace error cases**: Use `ContextWrapper<string>.Error("message")`
3. **Verify payload cases**: Use `ContextWrapper<string>.Ok("data")` for clarity
4. **Rebuild**: Compiler will catch any remaining issues

**Compatibility**:
- ✅ **Fully backward compatible** for non-string types
- ⚠️ **Breaking change** only for `new ContextWrapper<string>("text")` pattern
- ✅ All existing constructors with typed payloads still work

---

## 🧪 Testing Recommendations

### **Verify Factory Methods**

```csharp
[Test]
public void ContextWrapper_Error_CreatesErrorState()
{
    var result = ContextWrapper<string>.Error("Test error");
    
    Assert.IsTrue(result.hasError);
    Assert.AreEqual("Test error", result.message);
    Assert.AreEqual(0, result.length);
}

[Test]
public void ContextWrapper_Ok_CreatesSuccessState()
{
    var result = ContextWrapper<string>.Ok("Test data");
    
    Assert.IsFalse(result.hasError);
    Assert.AreEqual(1, result.length);
    Assert.AreEqual("Test data", result.payload.First());
}
```

---

## 📋 Complete Feature List

### **ContextWrapper Enhancements**
- ✨ Static factory method: `Error(string message)`
- ✨ Static factory method: `Ok(T item, string message = "")`
- ✨ Static factory method: `Ok(IEnumerable<T> items, string message = "")`
- ✨ Static factory method: `Empty()`
- ✨ Static factory method: `Deprecated(T item, string message = "")`
- 🗑️ Removed: Ambiguous `ContextWrapper(string error)` constructor
- 📖 Comprehensive documentation with real-world examples

### **Existing Features (Unchanged)**
- ✅ UnitSystem with 24+ unit families
- ✅ IUnitSystem interface pattern
- ✅ StatusBitArray with 32-bit capacity
- ✅ Mathematical operations with type inference
- ✅ 6 complete unit systems (SI, MKS, CGS, FPS, IPS, mmNs)
- ✅ JSON serialization support
- ✅ .NET 9.0 target framework

---

## 🔗 Resources

- **Package**: https://www.nuget.org/packages/ApprenticeFoundryRulesAndUnits/
- **Repository**: https://github.com/SteveStrong/FoundryRulesAndUnits
- **Documentation**: See `/Markdown` folder for comprehensive guides

---

## 👨‍💻 Contributors

- **Stephen Strong** - Library maintainer

---

## 📝 Migration Checklist

Use this checklist when upgrading from 10.6.x to 10.7.0:

- [ ] Update package reference to 10.7.0
- [ ] Search codebase for `new ContextWrapper<string>(`
- [ ] Replace error-only cases with `ContextWrapper<string>.Error()`
- [ ] Update payload cases to use `ContextWrapper<string>.Ok()` for clarity
- [ ] Rebuild project and fix any compilation errors
- [ ] Run tests to verify behavior
- [ ] Review and update API documentation if needed

---

**Previous Version**: 10.6.0  
**Upgrade Recommended**: Yes - Eliminates ambiguity and improves code clarity  
**Breaking Changes**: Minimal - Only affects `new ContextWrapper<string>("text")` pattern  
**Migration Effort**: Low - Compiler-assisted with clear error messages
