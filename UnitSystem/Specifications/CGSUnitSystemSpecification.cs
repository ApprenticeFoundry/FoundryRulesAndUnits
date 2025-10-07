using System;
using System.Collections.Generic;
using FoundryRulesAndUnits.Units;

namespace FoundryRulesAndUnits.Units.Specifications
{
    /// <summary>
    /// CGS (Centimeter-Gram-Second) Unit System Specification
    /// Base units: centimeters, grams, seconds, Celsius, degrees, dynes
    /// Uses base unit hub architecture - all conversions go through the base unit
    /// Appropriate for laboratory and scientific applications
    /// </summary>
    public class CGSUnitSystemSpecification : UnitSystemSpecificationBase
    {
        public override string SystemName => "CGS";

        public override string SystemDescription => "CGS (Centimeter-Gram-Second) Unit System with base units: centimeters, grams, seconds, Celsius, degrees, dynes";

        public override UnitSystemType SystemType => UnitSystemType.CGS;

        public override IReadOnlyList<UnitDefinition> UnitDefinitions { get; } = new List<UnitDefinition>
        {
            // Length units (centimeters as base) - Small-scale measurements
            UnitDefinition.BaseUnit("cm", "centimeters", UnitFamilyName.Length),
            UnitDefinition.LinearUnit("mm", "millimeters", UnitFamilyName.Length, 0.1),         // 1 mm = 0.1 cm
            UnitDefinition.LinearUnit("μm", "micrometers", UnitFamilyName.Length, 0.0001),      // 1 μm = 0.0001 cm
            UnitDefinition.LinearUnit("nm", "nanometers", UnitFamilyName.Length, 0.0000001),    // 1 nm = 0.0000001 cm
            UnitDefinition.LinearUnit("m", "meters", UnitFamilyName.Length, 100.0),             // 1 m = 100 cm
            UnitDefinition.LinearUnit("in", "inches", UnitFamilyName.Length, 2.54),             // 1 in = 2.54 cm
            UnitDefinition.LinearUnit("ft", "feet", UnitFamilyName.Length, 30.48),              // 1 ft = 30.48 cm
            UnitDefinition.LinearUnit("Å", "angstroms", UnitFamilyName.Length, 0.00000001),     // 1 Å = 1e-8 cm

            // Mass units (grams as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("g", "grams", UnitFamilyName.Mass),
            UnitDefinition.LinearUnit("mg", "milligrams", UnitFamilyName.Mass, 0.001),          // 1 mg = 0.001 g
            UnitDefinition.LinearUnit("μg", "micrograms", UnitFamilyName.Mass, 0.000001),       // 1 μg = 0.000001 g
            UnitDefinition.LinearUnit("ng", "nanograms", UnitFamilyName.Mass, 0.000000001),     // 1 ng = 1e-9 g
            UnitDefinition.LinearUnit("kg", "kilograms", UnitFamilyName.Mass, 1000.0),          // 1 kg = 1000 g
            UnitDefinition.LinearUnit("lb", "pounds", UnitFamilyName.Mass, 453.592),            // 1 lb = 453.592 g
            UnitDefinition.LinearUnit("oz", "ounces", UnitFamilyName.Mass, 28.3495),            // 1 oz = 28.3495 g
            UnitDefinition.LinearUnit("u", "atomic mass units", UnitFamilyName.Mass, 1.66054e-24), // 1 u in grams

            // Force units (dynes as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("dyne", "dynes", UnitFamilyName.Force),
            UnitDefinition.LinearUnit("μdyne", "microdyne", UnitFamilyName.Force, 0.000001),    // 1 μdyne = 1e-6 dyne
            UnitDefinition.LinearUnit("N", "newtons", UnitFamilyName.Force, 100000.0),          // 1 N = 100,000 dyne
            UnitDefinition.LinearUnit("kN", "kilonewtons", UnitFamilyName.Force, 100000000.0),  // 1 kN = 1e8 dyne
            UnitDefinition.LinearUnit("lbf", "pounds-force", UnitFamilyName.Force, 444822.0),   // 1 lbf = 444,822 dyne
            UnitDefinition.LinearUnit("gf", "grams-force", UnitFamilyName.Force, 980.665),      // 1 gf = 980.665 dyne
            UnitDefinition.LinearUnit("kgf", "kilograms-force", UnitFamilyName.Force, 980665.0), // 1 kgf = 980,665 dyne

            // Temperature units (Celsius as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("C", "Celsius", UnitFamilyName.Temperature),
            UnitDefinition.DerivedUnit("K", "Kelvin", UnitFamilyName.Temperature,
                k => k - 273.15,          // K to C: subtract 273.15
                c => c + 273.15),         // C to K: add 273.15
            UnitDefinition.DerivedUnit("F", "Fahrenheit", UnitFamilyName.Temperature,
                f => (f - 32.0) * 5.0/9.0,       // F to C: (F-32)*5/9
                c => c * 9.0/5.0 + 32.0),        // C to F: C*9/5 + 32

            // Angle units (radians as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("rad", "radians", UnitFamilyName.Angle),
            UnitDefinition.LinearUnit("deg", "degrees", UnitFamilyName.Angle, Math.PI / 180.0),  // 1 deg = π/180 rad
            UnitDefinition.LinearUnit("mrad", "milliradians", UnitFamilyName.Angle, 0.001),     // 1 mrad = 0.001 rad

            // Time units (seconds as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("s", "seconds", UnitFamilyName.Time),
            UnitDefinition.LinearUnit("ms", "milliseconds", UnitFamilyName.Time, 0.001),        // 1 ms = 0.001 s
            UnitDefinition.LinearUnit("μs", "microseconds", UnitFamilyName.Time, 0.000001),     // 1 μs = 0.000001 s
            UnitDefinition.LinearUnit("ns", "nanoseconds", UnitFamilyName.Time, 0.000000001),   // 1 ns = 1e-9 s
            UnitDefinition.LinearUnit("ps", "picoseconds", UnitFamilyName.Time, 0.000000000001), // 1 ps = 1e-12 s
            UnitDefinition.LinearUnit("min", "minutes", UnitFamilyName.Time, 60.0),             // 1 min = 60 s
            UnitDefinition.LinearUnit("hr", "hours", UnitFamilyName.Time, 3600.0),              // 1 hr = 3600 s

            // Area units (square centimeters as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("cm2", "square centimeters", UnitFamilyName.Area),
            UnitDefinition.LinearUnit("mm2", "square millimeters", UnitFamilyName.Area, 0.01),  // 1 mm² = 0.01 cm²
            UnitDefinition.LinearUnit("μm2", "square micrometers", UnitFamilyName.Area, 0.00000001), // 1 μm² = 1e-8 cm²
            UnitDefinition.LinearUnit("m2", "square meters", UnitFamilyName.Area, 10000.0),     // 1 m² = 10,000 cm²
            UnitDefinition.LinearUnit("in2", "square inches", UnitFamilyName.Area, 6.4516),     // 1 in² = 6.4516 cm²
            UnitDefinition.LinearUnit("ft2", "square feet", UnitFamilyName.Area, 929.0304),     // 1 ft² = 929.0304 cm²

            // Volume units (cubic centimeters as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("cm3", "cubic centimeters", UnitFamilyName.Volume),
            UnitDefinition.LinearUnit("mL", "milliliters", UnitFamilyName.Volume, 1.0),         // 1 mL = 1 cm³
            UnitDefinition.LinearUnit("mm3", "cubic millimeters", UnitFamilyName.Volume, 0.001), // 1 mm³ = 0.001 cm³
            UnitDefinition.LinearUnit("L", "liters", UnitFamilyName.Volume, 1000.0),            // 1 L = 1000 cm³
            UnitDefinition.LinearUnit("m3", "cubic meters", UnitFamilyName.Volume, 1000000.0),  // 1 m³ = 1,000,000 cm³
            UnitDefinition.LinearUnit("in3", "cubic inches", UnitFamilyName.Volume, 16.3871),   // 1 in³ = 16.3871 cm³
            UnitDefinition.LinearUnit("ft3", "cubic feet", UnitFamilyName.Volume, 28316.8),     // 1 ft³ = 28,316.8 cm³

            // Speed units (centimeters per second as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("cm/s", "centimeters per second", UnitFamilyName.Speed),
            UnitDefinition.LinearUnit("mm/s", "millimeters per second", UnitFamilyName.Speed, 0.1), // 1 mm/s = 0.1 cm/s
            UnitDefinition.LinearUnit("m/s", "meters per second", UnitFamilyName.Speed, 100.0), // 1 m/s = 100 cm/s
            UnitDefinition.LinearUnit("km/h", "kilometers per hour", UnitFamilyName.Speed, 27.7778), // 1 km/h = 27.78 cm/s
            UnitDefinition.LinearUnit("ft/s", "feet per second", UnitFamilyName.Speed, 30.48),  // 1 ft/s = 30.48 cm/s
            UnitDefinition.LinearUnit("mph", "miles per hour", UnitFamilyName.Speed, 44.704),   // 1 mph = 44.704 cm/s

            // Distance units (kilometers as base) - Large-scale measurements with dual family support
            UnitDefinition.BaseUnit("km", "kilometers", UnitFamilyName.Distance),
            // Dual family support: Accept all Length units but convert to km base
            UnitDefinition.LinearUnit("cm", "centimeters", UnitFamilyName.Distance, 0.00001),   // 1 cm = 0.00001 km
            UnitDefinition.LinearUnit("mm", "millimeters", UnitFamilyName.Distance, 0.000001),  // 1 mm = 0.000001 km
            UnitDefinition.LinearUnit("μm", "micrometers", UnitFamilyName.Distance, 0.000000001), // 1 μm = 1e-9 km
            UnitDefinition.LinearUnit("nm", "nanometers", UnitFamilyName.Distance, 0.000000000001), // 1 nm = 1e-12 km
            UnitDefinition.LinearUnit("m", "meters", UnitFamilyName.Distance, 0.001),            // 1 m = 0.001 km
            UnitDefinition.LinearUnit("in", "inches", UnitFamilyName.Distance, 0.0000254),       // 1 in = 0.0000254 km
            UnitDefinition.LinearUnit("ft", "feet", UnitFamilyName.Distance, 0.0003048),         // 1 ft = 0.0003048 km
            UnitDefinition.LinearUnit("Å", "angstroms", UnitFamilyName.Distance, 0.0000000000001), // 1 Å = 1e-13 km
            // Traditional distance units
            UnitDefinition.LinearUnit("mi", "miles", UnitFamilyName.Distance, 1.609344),        // 1 mi = 1.609344 km
            UnitDefinition.LinearUnit("nmi", "nautical miles", UnitFamilyName.Distance, 1.852), // 1 nmi = 1.852 km

            // Pressure units (barye as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("Ba", "barye", UnitFamilyName.Pressure),
            UnitDefinition.LinearUnit("μBa", "microbarye", UnitFamilyName.Pressure, 0.000001),  // 1 μBa = 1e-6 Ba
            UnitDefinition.LinearUnit("Pa", "pascals", UnitFamilyName.Pressure, 10.0),          // 1 Pa = 10 Ba
            UnitDefinition.LinearUnit("kPa", "kilopascals", UnitFamilyName.Pressure, 10000.0),  // 1 kPa = 10,000 Ba
            UnitDefinition.LinearUnit("bar", "bars", UnitFamilyName.Pressure, 1000000.0),       // 1 bar = 1,000,000 Ba
            UnitDefinition.LinearUnit("atm", "atmospheres", UnitFamilyName.Pressure, 1013250.0), // 1 atm = 1,013,250 Ba
            UnitDefinition.LinearUnit("torr", "torr", UnitFamilyName.Pressure, 1333.22),        // 1 torr = 1333.22 Ba
            UnitDefinition.LinearUnit("psi", "pounds per square inch", UnitFamilyName.Pressure, 68947.6), // 1 psi in Ba

            // Energy units (ergs as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("erg", "ergs", UnitFamilyName.Energy),
            UnitDefinition.LinearUnit("J", "joules", UnitFamilyName.Energy, 10000000.0),        // 1 J = 10,000,000 erg
            UnitDefinition.LinearUnit("kJ", "kilojoules", UnitFamilyName.Energy, 10000000000.0), // 1 kJ = 1e10 erg
            UnitDefinition.LinearUnit("cal", "calories", UnitFamilyName.Energy, 41840000.0),     // 1 cal = 41,840,000 erg
            UnitDefinition.LinearUnit("kcal", "kilocalories", UnitFamilyName.Energy, 41840000000.0), // 1 kcal = 4.184e10 erg
            UnitDefinition.LinearUnit("eV", "electron volts", UnitFamilyName.Energy, 1.602176634e-12), // 1 eV in erg
            UnitDefinition.LinearUnit("keV", "kiloelectron volts", UnitFamilyName.Energy, 1.602176634e-9), // 1 keV in erg

            // Power units (ergs per second as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("erg/s", "ergs per second", UnitFamilyName.Power),
            UnitDefinition.LinearUnit("W", "watts", UnitFamilyName.Power, 10000000.0),          // 1 W = 10,000,000 erg/s
            UnitDefinition.LinearUnit("mW", "milliwatts", UnitFamilyName.Power, 10000.0),       // 1 mW = 10,000 erg/s
            UnitDefinition.LinearUnit("kW", "kilowatts", UnitFamilyName.Power, 10000000000.0),  // 1 kW = 1e10 erg/s
            UnitDefinition.LinearUnit("hp", "horsepower", UnitFamilyName.Power, 7457000000.0),  // 1 hp = 7.457e9 erg/s

            // Frequency units (hertz as base) - same across all systems
            UnitDefinition.BaseUnit("Hz", "hertz", UnitFamilyName.Frequency),
            UnitDefinition.LinearUnit("kHz", "kilohertz", UnitFamilyName.Frequency, 1000.0),    // 1 kHz = 1000 Hz
            UnitDefinition.LinearUnit("MHz", "megahertz", UnitFamilyName.Frequency, 1000000.0), // 1 MHz = 1,000,000 Hz
            UnitDefinition.LinearUnit("GHz", "gigahertz", UnitFamilyName.Frequency, 1000000000.0), // 1 GHz = 1e9 Hz
            UnitDefinition.LinearUnit("THz", "terahertz", UnitFamilyName.Frequency, 1000000000000.0), // 1 THz = 1e12 Hz

            // Voltage units (volts as base) - same across all systems
            UnitDefinition.BaseUnit("V", "volts", UnitFamilyName.Voltage),
            UnitDefinition.LinearUnit("mV", "millivolts", UnitFamilyName.Voltage, 0.001),       // 1 mV = 0.001 V
            UnitDefinition.LinearUnit("μV", "microvolts", UnitFamilyName.Voltage, 0.000001),    // 1 μV = 0.000001 V
            UnitDefinition.LinearUnit("kV", "kilovolts", UnitFamilyName.Voltage, 1000.0),       // 1 kV = 1000 V

            // Current units (amperes as base) - same across all systems
            UnitDefinition.BaseUnit("A", "amperes", UnitFamilyName.Current),
            UnitDefinition.LinearUnit("mA", "milliamperes", UnitFamilyName.Current, 0.001),     // 1 mA = 0.001 A
            UnitDefinition.LinearUnit("μA", "microamperes", UnitFamilyName.Current, 0.000001),  // 1 μA = 0.000001 A
            UnitDefinition.LinearUnit("nA", "nanoamperes", UnitFamilyName.Current, 0.000000001), // 1 nA = 1e-9 A
            UnitDefinition.LinearUnit("pA", "picoamperes", UnitFamilyName.Current, 0.000000000001) // 1 pA = 1e-12 A
        };
    }
}