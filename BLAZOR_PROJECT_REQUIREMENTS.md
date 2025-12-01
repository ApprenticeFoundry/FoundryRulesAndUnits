# Blazor Project Requirements for Foundry Platform

This document specifies the **minimal requirements** for `.csproj` files that participate in the Foundry platform, particularly when exporting Blazor components to consuming projects.

## Overview

Projects in this platform may contain:
- **C# classes and interfaces** (models, services, utilities)
- **Blazor Razor components** (`.razor` files with UI markup)

The SDK and configuration requirements differ based on what the project exports.

---

## Project Types

### Type 1: Library with NO Blazor Components

If your project contains **only C# code** (no `.razor` files), use the standard SDK:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFrameworks>net9.0</TargetFrameworks>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <LangVersion>12.0</LangVersion>
  </PropertyGroup>
</Project>
```

**Example:** `FoundryRulesAndUnits` - contains only models, interfaces, and extensions.

---

### Type 2: Library WITH Blazor Components

If your project contains **any `.razor` files** that will be consumed by other projects, you **MUST** use the Razor SDK:

```xml
<Project Sdk="Microsoft.NET.Sdk.Razor">
  <PropertyGroup>
    <TargetFrameworks>net9.0</TargetFrameworks>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <AddRazorSupportForMvc>true</AddRazorSupportForMvc>
    <LangVersion>12.0</LangVersion>
  </PropertyGroup>

  <ItemGroup>
    <FrameworkReference Include="Microsoft.AspNetCore.App" />
  </ItemGroup>
</Project>
```

**Examples:** 
- `FoundryWorldsAndDrawings` - contains Canvas2D, Canvas3D, ViewerThreeD components
- `FoundryMentorModeler` - contains MentorTreeView, MentorTreeItem components

---

## Critical Requirements

### 1. SDK Selection (MOST IMPORTANT)

| Project Contains | Required SDK |
|-----------------|--------------|
| Only C# code | `Microsoft.NET.Sdk` |
| Any `.razor` files | `Microsoft.NET.Sdk.Razor` |

⚠️ **WARNING:** Using `Microsoft.NET.Sdk` for projects with `.razor` files will cause:
- Components not being compiled as Blazor components
- Consumer projects showing warning: `Found markup element with unexpected name 'ComponentName'`
- Runtime error: `'@onclick' is not a valid attribute name`

### 2. AddRazorSupportForMvc

Required for Razor component projects:
```xml
<AddRazorSupportForMvc>true</AddRazorSupportForMvc>
```

### 3. Framework Reference

Required for projects using ASP.NET Core features (Blazor, DI, etc.):
```xml
<ItemGroup>
  <FrameworkReference Include="Microsoft.AspNetCore.App" />
</ItemGroup>
```

### 4. _Imports.razor File

Projects with `.razor` files should include an `_Imports.razor` at the project root with common usings:

```razor
@using System.Net.Http
@using Microsoft.AspNetCore.Components
@using Microsoft.AspNetCore.Components.Forms
@using Microsoft.AspNetCore.Components.Web
@using Microsoft.JSInterop

@using YourProject.Namespace
```

---

## Component Code-Behind Pattern

When creating Blazor components with code-behind files, use the `@inherits` pattern:

**ComponentName.razor:**
```razor
@inherits ComponentNameBase
@namespace YourProject.Shared

<!-- Component markup here -->
```

**ComponentName.razor.cs:**
```csharp
namespace YourProject.Shared;

public class ComponentNameBase : ComponentBase
{
    // Component logic here
}
```

⚠️ **Do NOT use `partial class`** for the code-behind when the component will be exported to other projects. The `@inherits` pattern ensures proper Razor compilation.

---

## Namespace Consistency

Ensure the `@namespace` directive in `.razor` files matches the namespace in the `.razor.cs` code-behind:

```razor
@namespace FoundryMentorModeler.Shared  // In .razor file
```

```csharp
namespace FoundryMentorModeler.Shared;  // In .razor.cs file
```

---

## Common Package References

Projects with Blazor components typically need:

```xml
<ItemGroup>
  <PackageReference Include="Microsoft.AspNetCore.Components.Web" Version="9.0.9" />
  <PackageReference Include="BlazorComponentBus" Version="2.2.0" />  <!-- If using pub/sub -->
</ItemGroup>
```

---

## Checklist for New Projects

- [ ] Choose correct SDK based on whether project has `.razor` files
- [ ] Add `<AddRazorSupportForMvc>true</AddRazorSupportForMvc>` if using Razor SDK
- [ ] Add `<FrameworkReference Include="Microsoft.AspNetCore.App" />` if needed
- [ ] Create `_Imports.razor` with common usings
- [ ] Use `@inherits` pattern for component code-behind
- [ ] Ensure namespace consistency between `.razor` and `.razor.cs` files

---

## Troubleshooting

### "Found markup element with unexpected name"
**Cause:** Project uses wrong SDK or consumer is missing `@using` directive  
**Fix:** Change to `Microsoft.NET.Sdk.Razor` and ensure consumer has proper `@using`

### "'@onclick' is not a valid attribute name"
**Cause:** Razor directives being rendered as literal text instead of being processed  
**Fix:** Ensure project uses `Microsoft.NET.Sdk.Razor`

### Component not found at runtime
**Cause:** Namespace mismatch or missing `@using` in consumer  
**Fix:** Check `@namespace` matches code-behind namespace, add `@using` to consumer's `_Imports.razor`

---

*Document created: December 1, 2025*  
*Applies to: FoundryRulesAndUnits, FoundryWorldsAndDrawings, FoundryMentorModeler*
