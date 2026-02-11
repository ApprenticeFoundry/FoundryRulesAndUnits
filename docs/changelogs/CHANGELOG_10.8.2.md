# FoundryRulesAndUnits 10.8.2 Release Notes

**Release Date**: December 4, 2025  
**Package**: `ApprenticeFoundryRulesAndUnits` version 10.8.2  
**Target Framework**: .NET 9.0

---

## 🎯 What's New in 10.8.2

### **🐛 Bug Fix: MeasuredValueDisplayExtensions**

Fixed incorrect display value conversion in `MeasuredValueDisplayExtensions` to use the `As()` method for accurate unit representation.

**The Problem (v10.8.1)**:
Display formatting was not properly converting values to the target units before formatting, potentially showing incorrect values when the internal storage units differed from the display units.

**The Fix (v10.8.2)**:
The extension methods now correctly use `As()` to convert values to the requested display units before applying formatting.

```csharp
// Now correctly converts to display units before formatting
var length = unitSystem.CreateLength(1000, "mm");
string display = length.ToDisplayString("m");  // Correctly shows "1 m" not "1000 m"
```

### **📚 Documentation: Blazor Project Requirements**

Added comprehensive Blazor project requirements documentation for the Foundry platform, providing guidance for developers building Blazor applications with FoundryRulesAndUnits.

---

## 🔧 Changes

### **Bug Fixes**
- ✅ **MeasuredValueDisplayExtensions**: Fixed display value conversion to use `As()` method for accurate unit representation

### **Documentation**
- ✅ Added Blazor project requirements documentation for Foundry platform

---

## 📦 Upgrade Instructions

### From 10.8.1

**Update Package Reference**:
```xml
<PackageReference Include="ApprenticeFoundryRulesAndUnits" Version="10.8.2" />
```

**Compatibility**:
- ✅ **Fully backward compatible** - No breaking changes
- ✅ Drop-in replacement for 10.8.1
- ✅ Bug fix improves display accuracy

---

## 📋 Complete Change Summary

| Type | Description |
|------|-------------|
| 🐛 Bug Fix | Fixed `MeasuredValueDisplayExtensions` to use `As()` for accurate unit conversion |
| 📚 Docs | Added Blazor project requirements documentation |

---

## 🔗 Resources

- **Package**: https://www.nuget.org/packages/ApprenticeFoundryRulesAndUnits/
- **Repository**: https://github.com/SteveStrong/FoundryRulesAndUnits

---

**Previous Version**: 10.8.1  
**Upgrade Recommended**: Yes - Bug fix for display formatting accuracy  
**Breaking Changes**: None  
**Migration Effort**: Zero - Drop-in replacement
