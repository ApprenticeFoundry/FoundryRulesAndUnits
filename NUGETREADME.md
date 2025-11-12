# NuGet Publishing Guide - Personal Deployment Notes

## Version 10.3.0 Deployment Checklist

### Pre-Deployment Steps
- [ ] Update FoundryRulesAndUnits.csproj version to 10.3.0
- [ ] Update README.md version references
- [ ] Update all markdown documentation 
- [ ] Review CHANGELOG_10.3.0.md
- [ ] Verify all unit tests pass
- [ ] Check build warnings are acceptable

## Build and Pack Commands

```bash
# Clean previous builds
dotnet clean --configuration Release

# Build in Release mode  
dotnet build --configuration Release

# Run tests to verify everything works
dotnet test --configuration Release

# Create NuGet package
dotnet pack --configuration Release
```

## Package Information

- **Package ID**: `ApprenticeFoundryRulesAndUnits`
- **Current Version**: 10.4.0
- **Target Framework**: .NET 9.0
- **License**: MIT
- **Repository**: https://github.com/SteveStrong/FoundryRulesAndUnits

## Package Location After Build
The `.nupkg` file will be created at:
`./bin/Release/ApprenticeFoundryRulesAndUnits.10.4.0.nupkg`

## NuGet.org Publishing Steps

### Manual Upload Process
1. **Go to NuGet.org**
   - Navigate to https://www.nuget.org/
   - Login with Microsoft account

2. **Upload Package**
   - Click "Upload" button in top navigation
   - Click "Browse" and select: `./bin/Release/ApprenticeFoundryRulesAndUnits.10.3.0.nupkg`
   - Review package details (version, dependencies, etc.)
   - Add release notes if needed
   - Click "Submit"

3. **Verify Upload**
   - Package should appear at: https://www.nuget.org/packages/ApprenticeFoundryRulesAndUnits/10.3.0
   - May take a few minutes to be available for download

### Alternative: Command Line (if API key is set up)
```bash
dotnet nuget push ./bin/Release/ApprenticeFoundryRulesAndUnits.10.3.0.nupkg --source https://api.nuget.org/v3/index.json
```

## Package Links

- **NuGet Package**: https://www.nuget.org/packages/ApprenticeFoundryRulesAndUnits/
- **GitHub Repository**: https://github.com/SteveStrong/FoundryRulesAndUnits
- **Documentation**: https://apprenticefoundry.github.io/

## Post-Deployment Checklist
- [ ] Verify package appears on nuget.org
- [ ] Test installation in a clean test project
- [ ] Update any dependent projects to use new version
- [ ] Tag the git repository with v10.3.0

## Installation (for users)

```xml
<PackageReference Include="ApprenticeFoundryRulesAndUnits" Version="10.3.0" />
```

Or via Package Manager Console:
```powershell
Install-Package ApprenticeFoundryRulesAndUnits -Version 10.3.0
```

## Quick Test Installation
```bash
# Create test project to verify package works
mkdir nuget-test
cd nuget-test
dotnet new console
dotnet add package ApprenticeFoundryRulesAndUnits --version 10.3.0
# Add simple test code and run
dotnet run
```