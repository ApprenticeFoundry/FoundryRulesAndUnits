# Unit Family Ambiguity Architecture - Design Document

## Executive Summary

The FoundryRulesAndUnits system currently faces a fundamental ambiguity challenge where multiple unit families share the same unit symbols (e.g., "s" for both Duration and Time, "deg" for both Angle and Bearing). This creates parser disambiguation challenges and limits semantic precision in engineering applications.

This document outlines the problem, analyzes potential solutions, and provides a roadmap for future implementation when enhanced unit family disambiguation becomes a priority.

## Problem Statement

### Current Limitation
The unit lookup cache uses a "last one wins" approach when multiple unit families define the same symbol, forcing the parser to make arbitrary choices without semantic context.

### Real-World Impact
In engineering applications, this ambiguity matters:
- **CAD Systems**: Geometric angles vs navigation bearings have different meanings
- **Simulation**: Duration vs timestamps affect scheduling algorithms  
- **Control Systems**: Position vs displacement affects control loops
- **Multi-domain Engineering**: Same symbols mean different things in different contexts

## Identified Ambiguity Cases

### Time Domain
- **"s" (seconds)** could represent:
  - `Duration`: Elapsed time intervals (most common)
  - `Time`: Absolute timestamps/moments  
  - `Period`: Repetition cycles (frequency inverse)

### Angular Domain  
- **"deg" (degrees)** could represent:
  - `Angle`: Geometric rotation (most common)
  - `Bearing`: Navigation/compass heading
  - `Azimuth`: Horizontal direction (astronomy/surveying)
  - `Elevation`: Vertical angle (ballistics/astronomy)

### Length Domain
- **"m" (meters)** could represent:
  - `Distance`: Between two points (most common)
  - `Position`: Absolute coordinates
  - `Displacement`: Vector quantity

### Mass Domain
- **"kg" (kilograms)** could represent:
  - `Mass`: Inertial property (most common)
  - `Weight`: Gravitational force in different contexts

## Scale Optimization Challenge

Beyond semantic ambiguity, different engineering domains need different base units for optimal numerical precision:

| Domain | Length Base | Mass Base | Time Base | Rationale |
|--------|-------------|-----------|-----------|-----------|
| Semiconductor | nm | pg | ps | Avoid tiny floating point values |
| Astronomy | ly | solar_mass | year | Avoid extreme scale conversions |
| Civil Engineering | m | kg | s | Traditional scale works well |
| Microelectronics | μm | ng | ns | MEMS scale optimization |
| Nuclear Physics | fm | u | as | Particle scale |
| Geology | km | Mton | My | Geological time/mass scales |

## Architecture Solutions Analysis

### Option 1: Hierarchical Unit Families ⭐ **Recommended**

**Approach**: Expand unit family taxonomy to include semantic context
```csharp
public enum UnitFamilyName 
{
    // Time supergroup
    Time_Duration,    // elapsed time intervals
    Time_Moment,      // absolute timestamps  
    Time_Period,      // repetition cycles
    
    // Angular supergroup  
    Angle_Geometric,  // rotation, geometry
    Angle_Bearing,    // navigation, compass
    Angle_Elevation,  // vertical angles
    
    // Length supergroup
    Length_Distance,  // between two points
    Length_Position,  // absolute coordinates
    Length_Displacement // vector quantity
}
```

**Pros**:
- Semantic clarity and type safety
- No parser ambiguity
- Clean separation of concerns
- Future-proof for domain-specific operations

**Cons**: 
- Larger enum
- Migration effort for existing code

### Option 2: Smart Defaults with Explicit Override ⭐ **Pragmatic**

**Approach**: Use sensible defaults for 90% of cases, explicit API for edge cases
```csharp
// Parser uses defaults
var duration = unitSystem.CreateMeasuredValueFromParsableUnit("s", 60); // → Duration

// Explicit override when needed  
var timestamp = unitSystem.CreateMeasuredValue(UnitFamilyName.Time, 60, "s"); // → Time
```

**Default Mapping**:
- `"s"` → `Duration` (90% of engineering use cases)
- `"deg"` → `Angle` (geometric applications most common)  
- `"m"` → `Length` (distance measurement default)
- `"Hz"` → `Frequency` (temporal frequency default)

**Pros**:
- Minimal parser changes
- Backward compatible
- Covers majority use cases simply
- Clear upgrade path

**Cons**:
- Still has ambiguity edge cases
- Requires documentation of defaults

### Option 3: Context-Aware Disambiguation 

**Approach**: Parser analyzes surrounding expression for semantic clues
```csharp
// Context suggests geometric angle
"cos(90deg)" → Angle family

// Context suggests duration  
"wait for 30s" → Duration family

// Context suggests navigation
"heading 045deg" → Bearing family
```

**Pros**:
- Intuitive behavior
- Minimal explicit disambiguation needed
- Leverages natural language patterns

**Cons**:
- Complex parser logic
- Context may be ambiguous
- Performance overhead
- Debugging complexity

### Option 4: Scale-Aware Unit Systems

**Approach**: Different `UnitSystemType` values optimize for domain scales
```csharp
public enum UnitSystemType
{
    MKS,           // General engineering
    Nano,          // Semiconductor
    Astronomical,  // Space/astronomy  
    Nuclear,       // Particle physics
    Geological     // Earth sciences
}
```

**Pros**:
- Numerical stability
- Domain-appropriate precision
- Performance optimization
- Natural engineering workflow

**Cons**:
- Multiple system specifications to maintain
- Cross-domain interoperability complexity

## Recommended Implementation Strategy

### Phase 1: Smart Defaults (Short Term)
1. Implement default family mappings for common ambiguous symbols
2. Maintain existing API for backward compatibility  
3. Add explicit override methods for edge cases
4. Document default behaviors clearly

### Phase 2: Hierarchical Families (Medium Term)  
1. Expand `UnitFamilyName` enum with semantic hierarchy
2. Update unit system specifications with new families
3. Provide migration utilities for existing code
4. Maintain backward compatibility with aliases

### Phase 3: Scale-Aware Systems (Long Term)
1. Implement domain-specific unit system types
2. Optimize base units for numerical stability per domain
3. Add cross-domain conversion utilities
4. Provide domain selection guidance

## Research Requirements

Before implementation, investigate:

1. **Parser Integration Patterns**
   - How to provide disambiguation hints to expression parser
   - Performance impact of multi-family lookups
   - Error handling for ambiguous cases

2. **User Experience Design**
   - When to prompt for disambiguation vs use defaults
   - API design for explicit family specification
   - Migration path for existing codebases

3. **Domain Analysis**
   - Survey of unit usage patterns in target engineering domains
   - Base unit optimization for numerical stability
   - Cross-domain interoperability requirements

4. **Performance Evaluation**
   - Lookup performance with multi-family dictionaries
   - Memory overhead of hierarchical families
   - Caching strategies for disambiguation results

## Implementation Notes

### Current Architecture Impact
- `BuildUnitLookupCache()` method in `UnitSystem.cs` contains "last wins" behavior
- Parser relies on single-family-per-symbol assumption
- No disambiguation infrastructure currently exists

### Migration Considerations
- Existing code using ambiguous symbols will need review
- Default mappings should match current implicit behaviors
- Explicit override API should be optional, not required

### Testing Strategy
- Comprehensive test suite for all ambiguity cases
- Performance benchmarks for lookup operations
- Migration testing for backward compatibility
- Domain-specific validation with engineering examples

## Decision Timeline

This enhancement should be prioritized when:
1. Multi-domain engineering applications require semantic precision
2. Parser ambiguity creates user confusion or errors
3. Numerical precision issues arise from inappropriate base units
4. Cross-team collaboration requires clear unit semantics

Until then, current single-family approach is adequate for most use cases.

## References

- `UnitSystem.cs`: Current implementation with architectural notes
- `UnitSystemInvestigationTests.cs`: Comprehensive unit behavior validation
- Parser integration discussions and requirements analysis

---

*Document Version: 1.0*  
*Last Updated: October 3, 2025*  
*Author: Architecture Discussion with GitHub Copilot*  
*Status: Research and Planning Phase*