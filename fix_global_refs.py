#!/usr/bin/env python3
import os
import re

# Files with GlobalUnitSystem references
files_with_global = [
    "UnitSystem/UnitTypes/Current.cs",
    "UnitSystem/UnitTypes/DataFlow.cs", 
    "UnitSystem/UnitTypes/DataStorage.cs",
    "UnitSystem/UnitTypes/Percent.cs",
    "UnitSystem/UnitTypes/QuanityFlow.cs",
    "UnitSystem/UnitTypes/Resistance.cs",
    "UnitSystem/UnitTypes/Capacitance.cs",
    "UnitSystem/UnitTypes/Heading.cs"
]

for file_path in files_with_global:
    if os.path.exists(file_path):
        with open(file_path, 'r', encoding='utf-8') as f:
            content = f.read()
        
        # Replace As() method overrides that use GlobalUnitSystem
        content = re.sub(
            r'\s*public override double As\(string units\)\s*\{\s*return GlobalUnitSystem\.Convert\(Value\(\), Internal\(\), units\);\s*\}',
            '\n\t\t// As() method inherited from MeasuredValue with UnitGroup conversion',
            content,
            flags=re.MULTILINE
        )
        
        # Replace other GlobalUnitSystem.Convert calls
        content = re.sub(
            r'GlobalUnitSystem\.Convert\(([^)]+)\)',
            r'_unitGroup.Convert(\1)',
            content
        )
        
        with open(file_path, 'w', encoding='utf-8') as f:
            f.write(content)
        print(f"Fixed GlobalUnitSystem references in {file_path}")
    else:
        print(f"File not found: {file_path}")