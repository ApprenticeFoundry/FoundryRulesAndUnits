#!/usr/bin/env python3
import os
import re

# List of files that need fixing
files_to_fix = [
    "UnitSystem/UnitTypes/Angle.cs",
    "UnitSystem/UnitTypes/Area.cs", 
    "UnitSystem/UnitTypes/Capacitance.cs",
    "UnitSystem/UnitTypes/Current.cs",
    "UnitSystem/UnitTypes/DataFlow.cs",
    "UnitSystem/UnitTypes/DataStorage.cs",
    "UnitSystem/UnitTypes/Dimensionless.cs",
    "UnitSystem/UnitTypes/Distance.cs",
    "UnitSystem/UnitTypes/Duration.cs",
    "UnitSystem/UnitTypes/Frequency.cs",
    "UnitSystem/UnitTypes/Heading.cs",
    "UnitSystem/UnitTypes/Length.cs",
    "UnitSystem/UnitTypes/Mass.cs",
    "UnitSystem/UnitTypes/Percent.cs",
    "UnitSystem/UnitTypes/Power.cs",
    "UnitSystem/UnitTypes/Quanity.cs",
    "UnitSystem/UnitTypes/QuanityFlow.cs",
    "UnitSystem/UnitTypes/Resistance.cs",
    "UnitSystem/UnitTypes/Speed.cs",
    "UnitSystem/UnitTypes/Temperature.cs"
]

# Pattern to match the old constructors
old_pattern = re.compile(
    r'(\s*)(// Backward compatibility constructors\s*\n)?'
    r'\s*public \w+\(\) : base\(UnitFamilyName\.\w+\) { }\s*\n'
    r'\s*public \w+\(double value, string\? units = null\) : base\(UnitFamilyName\.\w+\)\s*\n'
    r'\s*{\s*\n'
    r'\s*Init\(value, units\);\s*\n'
    r'\s*}',
    re.MULTILINE
)

replacement = r'''\1/// <summary>
\1/// Backward compatibility constructor for JSON deserialization
\1/// </summary>
\1public {class_name}(double value, string units) : base()
\1{{
\1\tV = value;
\1\tI = units;
\1\tU = units;
\1}}'''

for file_path in files_to_fix:
    if os.path.exists(file_path):
        with open(file_path, 'r', encoding='utf-8') as f:
            content = f.read()
        
        # Extract class name from file path
        class_name = os.path.basename(file_path).replace('.cs', '')
        if class_name == 'Quanity':
            class_name = 'Quantity'  # Fix the typo
        elif class_name == 'QuanityFlow':
            class_name = 'QuantityFlow'  # Fix the typo
        
        # Replace the old constructors
        new_content = old_pattern.sub(replacement.format(class_name=class_name), content)
        
        if new_content != content:
            with open(file_path, 'w', encoding='utf-8') as f:
                f.write(new_content)
            print(f"Fixed {file_path}")
        else:
            print(f"No changes needed for {file_path}")
    else:
        print(f"File not found: {file_path}")