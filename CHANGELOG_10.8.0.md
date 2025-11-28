# FoundryRulesAndUnits 10.8.0 Release Notes

**Release Date**: November 28, 2025  
**Package**: `ApprenticeFoundryRulesAndUnits` version 10.8.0  
**Target Framework**: .NET 9.0

---

## 🎯 What's New in 10.8.0

### **🧹 StatusBitArray Architecture Simplification** ⭐ MAJOR REFACTOR

Removed over-engineered bitwise operations and simplified the StatusBitArray API by eliminating redundant methods that provided "only X stale" semantics. The new architecture relies on simple if-else-if patterns in consuming code, which naturally provide the same semantics without complex bit masking.

#### **What Changed**

**Serialization Upgrade: 5 bits → 32 bits**
- **Old**: `StaleBits` property serialized only bits 3-7 (stale flags) via `GetStaleBits()`
- **New**: `StatusBits` property serializes all 32 bits via `GetAllBits()`
- **JSON Property**: Changed from `"staleBits"` to `"statusBits"`
- **Reason**: Provides more flexibility on JavaScript side for future features

**Removed Over-Engineered Methods**:
```csharp
// ❌ REMOVED - Unnecessary complexity
GetStaleBits()           // Used bit masking to extract only stale flags
GetStaleCount()          // Counted how many stale flags were set
IsOnlyTransformStale()   // Checked if ONLY transform was stale
IsOnlyMaterialStale()    // Checked if ONLY material was stale  
IsOnlyGeometryStale()    // Checked if ONLY geometry was stale

// ❌ REMOVED - Bit constants
STALE_MASK      // 0b11111000 - mask for bits 3-7
TRANSFORM_BIT   // 0b00001000 - bit 3
MATERIAL_BIT    // 0b00010000 - bit 4
GEOMETRY_BIT    // 0b00100000 - bit 5
STRUCTURE_BIT   // 0b01000000 - bit 6
DATA_BIT        // 0b10000000 - bit 7
```

**What Remains** (Clean, Simple API):
```csharp
// ✅ KEPT - Individual flag properties
IsTransformStale    // Is GPU transform cache stale?
IsMaterialStale     // Is GPU material cache stale?
IsGeometryStale     // Is GPU geometry cache stale?
IsStructureStale    // Is GPU structure cache stale?
IsDataStale         // Is GPU data cache stale?

// ✅ KEPT - Setter methods
SetTransformStale(bool value)
SetMaterialStale(bool value)
ClearAllStaleFlags()

// ✅ NEW - Full bit serialization
GetAllBits()        // Returns all 32 bits as single integer
StatusBits          // Property: [JsonPropertyName("statusBits")]
```

---

## 💡 Why This Change?

### **Problem: Over-Engineering**

The old `IsOnly*Stale()` methods were solving a problem that didn't need solving:

```csharp
// OLD: Complex bit masking logic
public bool IsOnlyTransformStale() 
{
    var staleBits = GetStaleBits();
    return staleBits == TRANSFORM_BIT; // Only bit 3 set?
}

// Usage in consuming code
if (mesh.IsOnlyTransformStale()) 
{
    operations.TransformUpdates.Add(mesh);  // Fast path
}
else if (mesh.IsOnlyMaterialStale()) 
{
    operations.MaterialUpdates.Add(mesh);   // Medium path
}
// ... etc
```

### **Solution: Simple if-else-if Pattern**

The same logic works perfectly with **simple boolean checks**:

```csharp
// NEW: Clean, readable, no bit masking needed
if (mesh.IsGeometryStale())           // Check most expensive first
{
    operations.GeometryUpdates.Add(mesh);   // Slow path (rebuild mesh)
}
else if (mesh.IsMaterialStale())      // Check medium expense second
{
    operations.MaterialUpdates.Add(mesh);   // Medium path (shader update)
}
else if (mesh.IsTransformStale())     // Check cheapest last
{
    operations.TransformUpdates.Add(mesh);  // Fast path (matrix only)
}
```

**Key Insight**: The `else if` chain **automatically ensures "only X stale"** semantics! Once you find `IsGeometryStale() == true`, you skip the other checks. This is the same behavior as the old `IsOnlyGeometryStale()` method, but **simpler and more readable**.

---

## 🔧 Breaking Changes

### **Removed Methods**

If your code was using these methods, migrate to simple boolean checks:

```csharp
// ❌ OLD - No longer compiles
if (obj.IsOnlyTransformStale()) { ... }
if (obj.IsOnlyMaterialStale()) { ... }
if (obj.IsOnlyGeometryStale()) { ... }
var count = obj.GetStaleCount();
var staleBits = obj.GetStaleBits();

// ✅ NEW - Use simple if-else-if pattern
if (obj.IsGeometryStale()) { /* Handle geometry */ }
else if (obj.IsMaterialStale()) { /* Handle material */ }
else if (obj.IsTransformStale()) { /* Handle transform */ }
else if (obj.IsStructureStale()) { /* Handle structure */ }
else if (obj.IsDataStale()) { /* Handle data */ }
```

### **Property Renamed**

```csharp
// ❌ OLD - Property removed
int staleBits = obj.StaleBits;  // Only bits 3-7

// ✅ NEW - Upgraded property
int statusBits = obj.StatusBits;  // All 32 bits
```

### **JSON Property Changed**

```json
// ❌ OLD JSON
{
  "staleBits": 24  
}

// ✅ NEW JSON
{
  "statusBits": 16777240
}
```

**Note**: JavaScript side must update to use `statusBits` instead of `staleBits`.

---

## 📊 Performance Impact

### **Zero Performance Loss**

The if-else-if pattern is **identical in performance** to the old IsOnly* methods:

- **Old approach**: Check `IsOnlyTransformStale()` → internally calls `GetStaleBits()` → bit masking → comparison
- **New approach**: Check `IsGeometryStale()` → bit check → if false, check next condition

Both approaches perform **the same number of bit operations**. The new approach is just **more straightforward**.

### **Improved Serialization Flexibility**

Sending all 32 bits instead of just 5 bits provides:
- **Future-proofing**: JavaScript can access any flag without C# changes
- **Debugging**: Full state visible in JSON payloads
- **Flexibility**: JavaScript can implement new features using existing flags

---

## 🎉 Migration Benefits

### **1. Simpler Code**
```csharp
// Before: What does this even do?
if (GetStaleBits() == (TRANSFORM_BIT | MATERIAL_BIT)) { ... }

// After: Crystal clear intent
if (IsTransformStale() || IsMaterialStale()) { ... }
```

### **2. Easier to Test**
```csharp
// Before: Set up complex bit state
obj.SetBit(3, true);  // Transform
obj.SetBit(4, false); // Material
obj.SetBit(5, false); // Geometry
Assert.True(obj.IsOnlyTransformStale());

// After: Direct, readable tests
obj.SetTransformStale(true);
obj.SetMaterialStale(false);
obj.SetGeometryStale(false);
Assert.True(obj.IsTransformStale());
Assert.False(obj.IsMaterialStale());
```

### **3. Better IntelliSense**
- Individual flag properties show up with clear names
- No need to remember bit positions or masks
- Self-documenting API surface

### **4. Natural Priority Ordering**
```csharp
// Check most expensive operations first
// If geometry needs rebuild, no point checking transform
if (IsGeometryStale())        // Slow: full mesh rebuild
{
    RebuildMesh();
}
else if (IsMaterialStale())   // Medium: shader update
{
    UpdateMaterial();
}
else if (IsTransformStale())  // Fast: matrix only
{
    UpdateTransform();
}
```

The if-else-if chain **implicitly gives you the "only" semantics** - once you handle geometry, you don't waste time checking transform.

---

## 📚 Documentation Updates

### **Updated Files**

- ✅ **README.md** - Updated StatusBitArray section to reflect simplified API
- ✅ **CHANGELOG_10.8.0.md** - This file - comprehensive migration guide
- ✅ **FoundryRulesAndUnits.csproj** - Version bumped to 10.8.0

### **Architecture Notes**

The StatusBitArray simplification aligns with the broader "pre-sorting" architecture:
- **C# (FoundryWorldsAndDrawings)**: Uses if-else-if pattern to pre-sort objects by update type
- **C# sends**: `SceneOperations` DTO with typed buckets (TransformUpdates, MaterialUpdates, etc.)
- **JavaScript receives**: Pre-sorted batches and applies minimal updates per type
- **Result**: No sorting or bit checking in JavaScript - C# already did the work

---

## 🔄 Version History

- **10.8.0** (Nov 28, 2025): StatusBitArray simplification - removed IsOnly*Stale() methods, upgraded to 32-bit serialization
- **10.7.0** (Nov 23, 2025): ContextWrapper factory methods - eliminated string ambiguity
- **10.6.0**: StatusBitArray expanded to 32 bits with domain-grouped organization
- **10.4.1**: Unit family ambiguity resolution with two-tier architecture
- **10.0.0**: Major refactor to IUnitSystem interface with UnitGroup injection

---

## 📦 NuGet Package

```xml
<PackageReference Include="ApprenticeFoundryRulesAndUnits" Version="10.8.0" />
```

---

## 🎯 Summary

Version 10.8.0 **simplifies without sacrificing functionality**. The old IsOnly*Stale() methods were solving a non-problem with complex bit masking. The new approach uses **simple if-else-if patterns** that are:

- ✅ **Easier to read** - No bit masking knowledge required
- ✅ **Easier to test** - Direct flag checks
- ✅ **Equally performant** - Same number of operations
- ✅ **More flexible** - All 32 bits sent to JavaScript
- ✅ **Self-documenting** - Natural priority ordering (expensive → cheap)

The architecture remains robust: C# pre-sorts by checking flags in priority order, JavaScript receives pre-sorted batches and applies type-specific updates. **Zero regressions, maximum clarity**.
