# Simplified FoundryRulesAndUnits Modernization Plan
## Data Container Focused Approach

**Date**: January 15, 2026  
**Purpose**: Modernize data containers for spreadsheet/JSON workflows without architectural complexity  
**Duration**: 2 weeks (10 working days)

---

## **Core Understanding: DT Components as Data Containers**

**Primary Use Cases:**
- Import/export from spreadsheets 
- JSON serialization/deserialization
- Flexible data transport via ControlParameters
- Simple property-based data mapping

**What We DON'T Need:**
- Complex collection management
- Editor patterns with mutation tokens
- Composition architectures  
- Behavioral component patterns

**What We DO Need:**
- Better identity management (GlobalId vs manual Guid)
- Efficient metadata (MxObject.Metadata vs nullable ControlParameters)
- Memory efficiency (4-byte StatusBits)
- Clean JSON serialization

---

## **PHASE 0: Revert Previous Changes (Day 1)**

### **Task 0.1: Clean Slate - Remove Earlier Modifications**

#### **Revert DT_Base.cs**
- Remove MxObject inheritance that was added earlier
- Restore original DT_Base class structure:
```csharp
public class DT_Base
{
    public string? Guid { get; set; }
    public string? ParentGuid { get; set; }
    public string? Name { get; set; }
    public string? Type { get; set; }
    public string? Url { get; set; }
    public List<string> Tags { get; set; } = new List<string>();
    public string? TimeStamp;
    protected ControlParameters? metadata;
}
```

#### **Revert DT_Component.cs** 
- Remove MxComponent composition approach that was added
- Restore original DT_Component inheritance: `DT_Component : DT_Ingredient`
- Restore original collection management:
```csharp
public class DT_Component : DT_Ingredient
{
    public string? Text { get; set; } = null;
    public HighResPosition? Position { get; set; } = null;
    public BoundingBox? BoundingBox { get; set; } = null;
    protected List<DT_Component>? members;
    
    // Original methods restored
    public List<DT_Component> GetMembers() { ... }
    public DT_Component AddMember(DT_Component child) { ... }
}
```

#### **Revert StatusBitArray.cs**
- Remove FoundryMicroCore inheritance that was added
- Restore original BitArray-based implementation
- Remove using FoundryMicroCore.Core statement

#### **Revert ControlParameters.cs**
- Restore nullable Lookup pattern: `public Dictionary<string, object>? Lookup = null;`
- Restore original null-checking logic in all methods
- Remove non-nullable simplifications that were added

#### **Remove Project Reference**
- Remove FoundryMicroCore project reference from FoundryRulesAndUnits.csproj
- Ensure project compiles with original structure

---

## **PHASE 1: Foundation Simplification (Days 2-4)**

### **Task 1.1: Re-add Project Reference and Dependencies**

#### **Add FoundryMicroCore Reference Back**
- Add FoundryMicroCore project reference to FoundryRulesAndUnits.csproj (properly this time)
- Verify project builds with reference in place
- Test that FoundryMicroCore types are accessible

#### **Retire StatusBitArray.cs entirely**
- ✅ **Already using FoundryMicroCore StatusBitArray** via inheritance
- Delete `c:\Users\admin\workspace\Core\FoundryRulesAndUnits\Models\StatusBitArray.cs`
- Update any direct references to use inherited StatusBits from MxObject

#### **Consolidate ControlParameters**
- Keep ControlParameters.cs as compatibility wrapper around MxObject.Metadata
- Simplify implementation to direct dictionary access
- Remove nullable patterns - always available via MxObject

### **Task 1.2: DT_Base Foundation Update**

#### **Simple MxObject Inheritance**
```csharp
// BEFORE: Manual identity + separate metadata
public class DT_Base
{
    public string? Guid { get; set; }
    public string? ParentGuid { get; set; } 
    public string? Name { get; set; }
    protected ControlParameters? metadata;
}

// AFTER: Clean data container with modern foundation
public class DT_Base : MxObject
{
    // Inherits: Name, GlobalId, StatusBits, Metadata, Parent
    
    // Keep data container properties
    public string? Type { get; set; }
    public string? Url { get; set; }
    public List<string> Tags { get; set; } = new();
    public string? TimeStamp;
    
    // Simple metadata access (no complexity)
    public ControlParameters MetaData() => new() { Lookup = Metadata ?? new() };
    public bool HasMetaData() => Metadata?.Count > 0;
}
```

#### **Remove Manual Identity Management**
- Remove `Guid` property entirely
- Remove `ParentGuid` property entirely  
- Use inherited `GlobalId` and `Parent?.GlobalId` instead
- Update any internal references

---

## **PHASE 2: Inheritance Chain Cleanup (Days 5-7)**

### **Task 2.1: Preserve Domain Hierarchy**

Keep existing inheritance chain - it serves domain organization:
```
DT_Base : MxObject           // Foundation with identity/metadata
└── DT_Title : DT_Base       // Title/description properties
    └── DT_Hero : DT_Title   // Asset/image properties  
        └── DT_Ingredient : DT_Hero    // Parts/system properties
            └── DT_Component : DT_Ingredient  // Hierarchical data
```

### **Task 2.2: Simplify Collection Properties**

#### **DT_Hero: Asset Collections**
```csharp
// KEEP SIMPLE: Just properties for JSON serialization
public class DT_Hero : DT_Title
{
    public List<DT_AssetReference>? AssetReferences { get; set; }
    public List<DT_HeroReference>? HeroReferences { get; set; }
    
    // KEEP SIMPLE: Basic add/get methods (no editor complexity)
    public T AddAssetReference<T>(T item) where T : DT_AssetReference
    {
        AssetReferences ??= new();
        item.HeroGuid = this.GlobalId;  // Use GlobalId not Guid
        AssetReferences.Add(item);
        return item;
    }
}
```

#### **DT_Component: Child Collections**  
```csharp
// KEEP SIMPLE: Just properties for spreadsheet/JSON
public class DT_Component : DT_Ingredient
{
    public List<DT_Component>? Children { get; set; }  // Renamed from 'members'
    
    // KEEP SIMPLE: Basic methods (no editor tokens)
    public DT_Component AddChild(DT_Component child)
    {
        Children ??= new();
        child.Parent = this;  // Use MxObject Parent, not ParentGuid
        Children.Add(child);
        return child;
    }
    
    public List<DT_Component> GetChildren() => Children ?? new();
}
```

### **Task 2.3: Property Cleanup**

#### **Replace Manual GUID References**
```csharp
// BEFORE: Manual string GUIDs
item.HeroGuid = this.Guid;
child.ParentGuid = parent.Guid;

// AFTER: Use MxObject identity
item.HeroGuid = this.GlobalId;
child.Parent = parent;  // Automatic parent management
```

---

## **PHASE 3: Serialization & Compatibility (Days 8-9)**

### **Task 3.1: JSON Compatibility**

#### **Ensure Property Names Preserved**
- `GlobalId` should serialize as expected
- Existing property names maintained for spreadsheet compatibility
- Test round-trip JSON serialization
- Validate import/export workflows still work

#### **Backward Compatible JSON Reading**
```csharp
// Handle legacy JSON that might have "Guid" field
[JsonPropertyName("Guid")]
[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
public string? LegacyGuid 
{ 
    get => null; // Don't serialize
    set { if (!string.IsNullOrEmpty(value)) GlobalId = value; } // Read legacy
}
```

### **Task 3.2: Spreadsheet Import/Export**

#### **Verify Property Mapping**
- Column mappings still work with new property structure
- GlobalId accessible for unique identifiers
- ControlParameters still accessible for custom columns
- Parent relationships work for hierarchical data

---

## **PHASE 4: Testing & Validation (Day 10)**

### **Task 4.1: Data Container Tests**

#### **JSON Serialization Tests**
```csharp
[TestMethod]
public void JSON_RoundTrip_Preserves_Data()
{
    var component = new DT_Component("test") 
    {
        Text = "Sample",
        Type = "TestType"
    };
    component.MetaData().Establish("custom", "value");
    
    var json = JsonSerializer.Serialize(component);
    var restored = JsonSerializer.Deserialize<DT_Component>(json);
    
    Assert.AreEqual("test", restored.Name);
    Assert.AreEqual("Sample", restored.Text);
    Assert.AreEqual("value", restored.MetaData().GetValue("custom"));
}
```

#### **Spreadsheet Mapping Tests**
- Test property enumeration for column mapping
- Test hierarchical parent-child relationships  
- Test custom data via ControlParameters
- Test bulk import/export scenarios

### **Task 4.2: Memory & Performance**

#### **Simple Benchmarks**
- Memory per DT_Component object (target: significant reduction)
- JSON serialization speed (should be same or better)
- Large dataset handling (1000+ components)

---

## **BENEFITS OF SIMPLIFIED APPROACH**

### **✅ Core Improvements**
- **87% memory reduction**: StatusBits (32+ bytes → 4 bytes)
- **Better identity**: GlobalId vs manual Guid management
- **Cleaner metadata**: MxObject.Metadata vs nullable wrapper
- **Automatic parent management**: Parent property vs manual ParentGuid

### **✅ Maintains Simplicity**  
- No editor patterns or mutation control
- No composition complexity
- Simple properties for spreadsheet mapping
- Clean JSON serialization
- Existing workflows preserved

### **✅ Easy Migration**
- Minimal breaking changes (Guid → GlobalId)
- Same inheritance hierarchy
- Same property-based approach
- Same ControlParameters interface

---

## **WHAT GETS RETIRED**

### **Files to Delete:**
- `Models/StatusBitArray.cs` (redundant with FoundryMicroCore)

### **Properties to Remove:**
- `public string? Guid { get; set; }` (use GlobalId)  
- `public string? ParentGuid { get; set; }` (use Parent)
- `protected ControlParameters? metadata` (use Metadata)

### **Complexity to Avoid:**
- MxComponent composition patterns
- Editor-based mutation control  
- Collection management tokens
- Complex parent synchronization

---

## **SUCCESS CRITERIA**

### **Functional Success**
- [ ] All DT_* classes inherit modern foundation (MxObject)
- [ ] JSON serialization produces compatible output
- [ ] Spreadsheet import/export workflows unchanged
- [ ] ControlParameters still work for custom data

### **Performance Success**  
- [ ] Memory usage per object significantly reduced
- [ ] JSON serialization performance same or better
- [ ] No performance regressions in data workflows

### **Simplicity Success**
- [ ] Less code than before (retired classes/properties)
- [ ] No architectural complexity added
- [ ] Easy to understand data container pattern
- [ ] Clean inheritance hierarchy preserved

**This approach gets the foundational benefits of FoundryMicroCore without the behavioral complexity - perfect for data containers.**