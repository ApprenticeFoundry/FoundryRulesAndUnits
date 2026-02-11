# JSON Converter Reintroduction Guide

## Background

JSON converters were temporarily removed during the factory pattern refactoring to focus on getting the core unit system architecture solid. This document outlines how to properly reintroduce them when needed.

## Key Design Requirements

### 1. Compact Data Format (Critical!)
The original design used single-character property names for storage efficiency:
```json
{
  "V": 5.0,     // Value (internal, in base units)
  "I": "m",     // Internal units (base unit symbol)
  "U": "ft",    // User/display units (original input units)
  "F": "Length" // Family (optional, can be inferred from type)
}
```

**Benefits:**
- 50-70% smaller JSON payload vs verbose naming
- Critical for measurement-heavy APIs and data storage
- Faster serialization/deserialization
- Reduced network bandwidth and database footprint

### 2. Factory Pattern Integration
New converters MUST use the factory pattern, not direct constructors:

**Wrong (bypasses factory system):**
```csharp
public override Length Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
{
    // DON'T DO THIS - bypasses UnitGroup injection
    return new Length(value, units);
}
```

**Correct (uses factory system):**
```csharp
public override Length Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
{
    var V = /* parse value */;
    var U = /* parse user units */;
    
    // Use factory pattern with proper UnitGroup injection
    var unitSystem = GetCurrentUnitSystem(options); // From context
    return unitSystem.CreateLength(V, U);
}
```

### 3. Unit System Context Handling

#### Option A: Global Context (Recommended)
Store unit system in JsonSerializerOptions context:
```csharp
var options = new JsonSerializerOptions();
options.SetUnitSystem(IUnitSystem.MKS()); // Set global context
var measurement = JsonSerializer.Deserialize<Length>(json, options);
```

#### Option B: Embed System ID (Alternative)
Add minimal system identifier to JSON:
```json
{
  "V": 5.0,
  "I": "m", 
  "U": "ft",
  "S": "M"    // System: M=MKS, F=FPS, S=SI, etc. (1 char)
}
```

#### Option C: Default System Assumption
Always assume MKS during deserialization, let application handle conversion.

## Implementation Strategy

### Phase 1: Base Converter Infrastructure
1. Create `MeasuredValueJsonConverter<T>` base class
2. Handle V/I/U parsing logic once
3. Provide abstract factory method for each measurement type

### Phase 2: Unit System Context
1. Extend JsonSerializerOptions with unit system context
2. Create helper methods for setting/getting current system
3. Test with different unit systems

### Phase 3: Specific Converters
Create converters for each measurement type:
- `LengthJsonConverter`
- `MassJsonConverter`
- `TemperatureJsonConverter`
- etc.

### Phase 4: Validation & Testing
1. Round-trip testing (serialize → deserialize → verify)
2. Cross-system compatibility (MKS → FPS → back)
3. Performance benchmarking vs direct constructors
4. Backward compatibility with existing JSON data

## Code Template

### Base Converter Pattern
```csharp
public abstract class MeasuredValueJsonConverter<T> : JsonConverter<T> 
    where T : MeasuredValue
{
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Parse compact V/I/U format
        var data = ParseCompactFormat(ref reader);
        
        // Get unit system from context
        var unitSystem = GetUnitSystem(options);
        
        // Use factory method (implemented by derived class)
        return CreateFromFactory(unitSystem, data.V, data.U);
    }
    
    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        // Write compact V/I/U format
        WriteCompactFormat(writer, value);
    }
    
    protected abstract T CreateFromFactory(IUnitSystem unitSystem, double value, string units);
}

public class LengthJsonConverter : MeasuredValueJsonConverter<Length>
{
    protected override Length CreateFromFactory(IUnitSystem unitSystem, double value, string units)
    {
        return unitSystem.CreateLength(value, units);
    }
}
```

### Registration Pattern
```csharp
// In system setup
var options = new JsonSerializerOptions();
options.Converters.Add(new LengthJsonConverter());
options.Converters.Add(new MassJsonConverter());
options.SetUnitSystem(IUnitSystem.MKS()); // Global context
```

## Migration Considerations

### Existing Data
If there's existing JSON data using V/I/U format:
1. New converters must maintain compatibility
2. Test deserialization of old data
3. Validate that values/units are preserved correctly

### ID-Based Systems
The mention of "IDS for length and IDS for unit system" suggests:
- Unit families might have numeric IDs instead of strings
- Unit systems might have compact identifiers
- Consider these in the converter design:

```json
{
  "V": 5.0,
  "I": 1,      // Internal unit ID instead of "m"
  "U": 12,     // User unit ID instead of "ft"  
  "F": 2,      // Family ID instead of "Length"
  "S": 1       // System ID instead of "MKS"
}
```

## Performance Requirements

The reintroduced converters must:
- Maintain or improve current serialization speed
- Preserve compact data format benefits
- Not add significant memory overhead
- Work efficiently with large arrays of measurements

## Testing Checklist

Before reintroduction:
- [ ] Round-trip serialization works correctly
- [ ] Factory pattern is used (no direct constructors)
- [ ] UnitGroup injection occurs properly
- [ ] Unit conversions work after deserialization
- [ ] Performance meets requirements
- [ ] Compact format is preserved
- [ ] Unit system context handled correctly
- [ ] Backward compatibility with existing data
- [ ] Cross-system serialization works (MKS → FPS)
- [ ] Error handling for invalid JSON

## Priority Assessment

Reintroduce JSON converters when:
1. **High Priority**: APIs need to serialize measurements
2. **High Priority**: Database storage requires JSON persistence  
3. **Medium Priority**: Configuration files contain measurements
4. **Medium Priority**: Inter-service communication needed
5. **Low Priority**: General JSON support desired

## Notes

- Keep the single-character property names (V/I/U) for storage efficiency
- Always use factory pattern for proper UnitGroup injection
- Consider unit system context early in design
- Test thoroughly with existing data if any exists
- Document the compact format for API consumers