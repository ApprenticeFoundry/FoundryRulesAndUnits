# FoundryRulesAndUnits 10.8.1 Release Notes

**Release Date**: November 28, 2025  
**Package**: `ApprenticeFoundryRulesAndUnits` version 10.8.1  
**Target Framework**: .NET 9.0

---

## 🎯 What's New in 10.8.1

### **🔧 RecomputeBoundary Persistence Fix**

Fixed the `ClearAllStaleFlags()` method to preserve the `RecomputeBoundary` flag, ensuring it remains a persistent opt-in flag rather than being cleared after each boundary computation.

#### **What Changed**

**StatusBitArray.ClearAllStaleFlags() Behavior**:
```csharp
// ✅ NEW BEHAVIOR - RecomputeBoundary persists
public void ClearAllStaleFlags()
{
    IsTransformStale = false;
    IsMaterialStale = false;
    IsGeometryStale = false;
    IsStructureStale = false;
    IsDataStale = false;
    // RecomputeBoundary is NOT cleared - it's a persistent opt-in flag
}

// ❌ OLD BEHAVIOR - RecomputeBoundary was incorrectly cleared
public void ClearAllStaleFlags()
{
    IsTransformStale = false;
    IsMaterialStale = false;
    IsGeometryStale = false;
    IsStructureStale = false;
    IsDataStale = false;
    RecomputeBoundary = false;  // ← This was incorrect
}
```

#### **Why This Matters**

**Persistent Opt-In Pattern**:
- Shapes call `SetRecomputeBoundary()` once to opt-in to JavaScript world position calculations
- This flag should persist across all frames, not be cleared after each boundary computation
- Clearing it would break dependent shapes (like pipes, distance text) that need boundaries every frame

**Before (Broken)**:
1. Pipe calls `SetRecomputeBoundary()` to opt-in ✅
2. JavaScript computes boundary and returns it ✅
3. Scene3D calls `ClearAllStaleFlags()` ❌ **Clears RecomputeBoundary!**
4. Next frame: Boundary not computed because flag is gone 💥

**After (Fixed)**:
1. Pipe calls `SetRecomputeBoundary()` to opt-in ✅
2. JavaScript computes boundary and returns it ✅
3. Scene3D calls `ClearAllStaleFlags()` ✅ **RecomputeBoundary preserved**
4. Next frame: Boundary computed again as expected ✅

---

## 📦 Installation

```bash
dotnet add package ApprenticeFoundryRulesAndUnits --version 10.8.1
```

---

## 🔄 Migration Guide

**No breaking changes** - this is a bug fix release.

If you were experiencing issues with distance text or pipes showing incorrect values after the first frame, this fix resolves that problem automatically.

---

## 🐛 Bug Fixes

- **StatusBitArray**: Fixed `ClearAllStaleFlags()` to not clear `RecomputeBoundary` flag, ensuring shapes that opt-in to boundary calculation continue to receive updates every frame

---

## 📝 Full Changelog

See [CHANGELOG_10.8.0.md](./CHANGELOG_10.8.0.md) for previous release notes.

---

## 👨‍💻 Author

**Stephen Strong**  
Apprentice Foundry  
https://apprenticefoundry.github.io/

---

## 📄 License

MIT License - see LICENSE file for details
