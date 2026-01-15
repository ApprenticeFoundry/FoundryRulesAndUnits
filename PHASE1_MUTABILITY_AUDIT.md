# Phase 1: Mutability API Audit
## Identifying APIs That Require Controlled Mutability

**Date**: January 15, 2026  
**Purpose**: Systematic identification of uncontrolled mutability patterns in FoundryRulesAndUnits

---

## **Critical Findings: Uncontrolled Mutability Patterns**

### **🔴 HIGH PRIORITY: Collection Manipulation**

#### **DT_Component.cs**
```csharp
// ❌ UNCONTROLLED: Direct list manipulation
protected List<DT_Component>? members;
public DT_Component AddMember(DT_Component child)
{
    members ??= new List<DT_Component>();  // Direct mutation
    child.ParentGuid = this.Guid;          // Manual parent tracking
    members.Add(child);                    // Uncontrolled addition
    return child;
}
public List<DT_Component> GetMembers()     // Returns mutable list
```

#### **DT_Hero.cs** 
```csharp
// ❌ UNCONTROLLED: Asset/Hero reference manipulation
public T AddAssetReference<T>(T item) where T : DT_AssetReference
{
    AssetReferences ??= new List<DT_AssetReference>();
    if (AssetReferences.IndexOf(item) == -1)
    {
        item.HeroGuid = this.Guid;         // Manual GUID assignment
        AssetReferences.Add(item);         // Direct mutation
    }
}
```

#### **DT_ComponentTree.cs**
```csharp
// ❌ UNCONTROLLED: Tree manipulation without validation
public void AddChild(DT_ComponentTree child)
{
    child.Item.ParentGuid = this.Item.Guid;  // Manual parent tracking
    this.Children.Add(child);                // Direct mutation
}
```

### **🟡 MEDIUM PRIORITY: Property Setters**

#### **Universal Pattern: All DT_* Classes**
```csharp
// ❌ UNCONTROLLED: Direct property mutations without validation
public string? Text { get; set; }
public string? Filename { get; set; }
public BoundingBox? BoundingBox { get; set; }
public HighResPosition? Position { get; set; }
```

### **🟡 MEDIUM PRIORITY: Manual Parent/GUID Management**

#### **DT_Base.cs**
```csharp
// ❌ UNCONTROLLED: Manual identity management
public string? Guid { get; set; }          // Should be immutable GlobalId
public string? ParentGuid { get; set; }    // Should be automatic via Parent
```

#### **Pattern Throughout DT_* Classes**
```csharp
// ❌ UNCONTROLLED: Manual parent assignment
child.ParentGuid = this.Guid;
item.HeroGuid = this.Guid;
item.AssetGuid = asset.Guid;
```

### **🟢 LOW PRIORITY: Metadata Management**

#### **DT_Base.cs**
```csharp
// ❌ UNCONTROLLED: Direct metadata manipulation
protected ControlParameters? metadata;
public ControlParameters AddMetaData(string key, string value)
{
    var data = MetaData();
    data.Establish(key, value);           // Direct dictionary mutation
}
```

---

## **Phase 1 Summary: APIs Requiring Replacement**

### **Must Replace (Breaking Changes Required)**
1. **Collection Management**: All `AddMember()`, `AddChild()`, `GetMembers()` patterns
2. **Manual Parent Tracking**: All `child.ParentGuid = parent.Guid` assignments
3. **Direct List Returns**: All methods returning `List<T>` for external mutation

### **Should Replace (API Improvement)**
1. **Property Setters**: Convert critical properties to editor-controlled
2. **Manual GUID Management**: Replace with automatic GlobalId/Parent system
3. **Metadata Manipulation**: Use MxObject.Metadata with controlled access

### **Can Defer (Low Impact)**
1. **Simple Data Properties**: Basic strings/primitives where validation isn't critical
2. **Read-Only Properties**: Properties that are naturally immutable
3. **Legacy Compatibility**: Non-critical properties used for serialization only

---

## **Next Steps for Phase 2: Inheritance Design**

Based on this audit, the inheritance design should focus on:

1. **DT_Component** → Needs MxComponent capabilities (collection management)
2. **DT_Hero** → Needs controlled asset/reference management  
3. **DT_Base** → Foundation class - should inherit from MxObject
4. **Data Classes** (DT_AssetFile, DT_Document, etc.) → Simple data containers, may only need MxObject

---

## **Validation Criteria for Phase 3**

### **Mutability Control Tests**
- [ ] Direct collection mutation should be prevented
- [ ] Parent-child relationships should be automatically managed
- [ ] Property mutations should be tracked/validated where critical
- [ ] Editor pattern should be required for structural changes

### **Backward Compatibility Tests**
- [ ] Existing serialization should continue working
- [ ] Read operations should remain unchanged where possible
- [ ] Legacy properties should be accessible (even if deprecated)

### **Performance Validation Tests**
- [ ] Collection operations should be faster (O(1) vs O(N))
- [ ] Memory usage should be reduced (fewer allocations)
- [ ] Object creation should be comparable or faster