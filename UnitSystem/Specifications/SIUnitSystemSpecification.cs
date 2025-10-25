using System;
using System.Collections.Generic;
using FoundryRulesAndUnits.Units;

namespace FoundryRulesAndUnits.Units.Specifications
{
    /// <summary>
    /// SI (International System of Units) Unit System Specification
    /// Base units: meters, kilograms, seconds, Kelvin, amperes, moles, candela
    /// Uses base unit hub architecture - all conversions go through the base unit
    /// Complete SI base unit implementation including luminosity and amount of substance
    /// </summary>
    public class SIUnitSystemSpecification : UnitSystemSpecificationBase
    {
        public override string SystemName => "SI";

        public override string SystemDescription => "SI (International System of Units) with all seven base units: meters, kilograms, seconds, Kelvin, amperes, moles, candela";

        public override UnitSystemType SystemType => UnitSystemType.SI;

        public override IReadOnlyList<UnitDefinition> UnitDefinitions { get; } = new List<UnitDefinition>
        {
            // Length units (meters as base) - Small-scale measurements
            UnitDefinition.BaseUnit("m", "meters", UnitFamilyName.Length),
            UnitDefinition.LinearUnit("mm", "millimeters", UnitFamilyName.Length, 0.001),       // 1 mm = 0.001 m
            UnitDefinition.LinearUnit("cm", "centimeters", UnitFamilyName.Length, 0.01),        // 1 cm = 0.01 m
            UnitDefinition.LinearUnit("μm", "micrometers", UnitFamilyName.Length, 0.000001),    // 1 μm = 0.000001 m
            UnitDefinition.LinearUnit("um", "micrometers", UnitFamilyName.Length, 0.000001),    // 1 um = 0.000001 m (ASCII)
            UnitDefinition.LinearUnit("nm", "nanometers", UnitFamilyName.Length, 0.000000001),  // 1 nm = 1e-9 m
            UnitDefinition.LinearUnit("pm", "picometers", UnitFamilyName.Length, 0.000000000001), // 1 pm = 1e-12 m
            UnitDefinition.LinearUnit("Å", "angstroms", UnitFamilyName.Length, 0.0000000001),   // 1 Å = 1e-10 m
            UnitDefinition.LinearUnit("in", "inches", UnitFamilyName.Length, 0.0254),           // 1 in = 0.0254 m
            UnitDefinition.LinearUnit("ft", "feet", UnitFamilyName.Length, 0.3048),             // 1 ft = 0.3048 m
            UnitDefinition.LinearUnit("yd", "yards", UnitFamilyName.Length, 0.9144),            // 1 yd = 0.9144 m
            UnitDefinition.LinearUnit("km", "kilometers", UnitFamilyName.Length, 1000.0),       // 1 km = 1000 m
            UnitDefinition.LinearUnit("mi", "miles", UnitFamilyName.Length, 1609.344),          // 1 mi = 1609.344 m

            // Mass units (kilograms as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("kg", "kilograms", UnitFamilyName.Mass),
            UnitDefinition.LinearUnit("g", "grams", UnitFamilyName.Mass, 0.001),                // 1 g = 0.001 kg
            UnitDefinition.LinearUnit("mg", "milligrams", UnitFamilyName.Mass, 0.000001),       // 1 mg = 0.000001 kg
            UnitDefinition.LinearUnit("μg", "micrograms", UnitFamilyName.Mass, 0.000000001),    // 1 μg = 1e-9 kg
            UnitDefinition.LinearUnit("ug", "micrograms", UnitFamilyName.Mass, 0.000000001),    // 1 ug = 1e-9 kg (ASCII)
            UnitDefinition.LinearUnit("ng", "nanograms", UnitFamilyName.Mass, 0.000000000001),  // 1 ng = 1e-12 kg
            UnitDefinition.LinearUnit("t", "metric tons", UnitFamilyName.Mass, 1000.0),         // 1 t = 1000 kg
            UnitDefinition.LinearUnit("lb", "pounds", UnitFamilyName.Mass, 0.453592),           // 1 lb = 0.453592 kg
            UnitDefinition.LinearUnit("oz", "ounces", UnitFamilyName.Mass, 0.0283495),          // 1 oz = 0.0283495 kg
            UnitDefinition.LinearUnit("u", "atomic mass units", UnitFamilyName.Mass, 1.66054e-27), // 1 u in kg

            // Force units (newtons as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("N", "newtons", UnitFamilyName.Force),
            UnitDefinition.LinearUnit("mN", "millinewtons", UnitFamilyName.Force, 0.001),       // 1 mN = 0.001 N
            UnitDefinition.LinearUnit("μN", "micronewtons", UnitFamilyName.Force, 0.000001),    // 1 μN = 0.000001 N
            UnitDefinition.LinearUnit("uN", "micronewtons", UnitFamilyName.Force, 0.000001),    // 1 uN = 0.000001 N (ASCII)
            UnitDefinition.LinearUnit("nN", "nanonewtons", UnitFamilyName.Force, 0.000000001),  // 1 nN = 1e-9 N
            UnitDefinition.LinearUnit("kN", "kilonewtons", UnitFamilyName.Force, 1000.0),       // 1 kN = 1000 N
            UnitDefinition.LinearUnit("MN", "meganewtons", UnitFamilyName.Force, 1000000.0),    // 1 MN = 1,000,000 N
            UnitDefinition.LinearUnit("dyne", "dynes", UnitFamilyName.Force, 0.00001),          // 1 dyne = 0.00001 N
            UnitDefinition.LinearUnit("lbf", "pounds-force", UnitFamilyName.Force, 4.448222),   // 1 lbf = 4.448222 N
            UnitDefinition.LinearUnit("kgf", "kilograms-force", UnitFamilyName.Force, 9.80665), // 1 kgf = 9.80665 N

            // Temperature units (Kelvin as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("K", "Kelvin", UnitFamilyName.Temperature),
            UnitDefinition.DerivedUnit("C", "Celsius", UnitFamilyName.Temperature,
                c => c + 273.15,          // C to K: add 273.15
                k => k - 273.15),         // K to C: subtract 273.15
            UnitDefinition.DerivedUnit("F", "Fahrenheit", UnitFamilyName.Temperature,
                f => (f - 32.0) * 5.0/9.0 + 273.15,    // F to K: (F-32)*5/9 + 273.15
                k => (k - 273.15) * 9.0/5.0 + 32.0),   // K to F: (K-273.15)*9/5 + 32
            UnitDefinition.DerivedUnit("R", "Rankine", UnitFamilyName.Temperature,
                r => r * 5.0/9.0,        // R to K: multiply by 5/9
                k => k * 9.0/5.0),       // K to R: multiply by 9/5

            // Angle units (radians as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("rad", "radians", UnitFamilyName.Angle),
            UnitDefinition.LinearUnit("mrad", "milliradians", UnitFamilyName.Angle, 0.001),     // 1 mrad = 0.001 rad
            UnitDefinition.LinearUnit("μrad", "microradians", UnitFamilyName.Angle, 0.000001),  // 1 μrad = 0.000001 rad
            UnitDefinition.LinearUnit("deg", "degrees", UnitFamilyName.Angle, Math.PI / 180.0), // 1 deg = π/180 rad
            UnitDefinition.LinearUnit("grad", "gradians", UnitFamilyName.Angle, Math.PI / 200.0), // 1 grad = π/200 rad
            UnitDefinition.LinearUnit("sr", "steradians", UnitFamilyName.Angle, 1.0),           // 1 sr = 1 rad (solid angle)

            // Time units (seconds as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("s", "seconds", UnitFamilyName.Time),
            UnitDefinition.LinearUnit("ms", "milliseconds", UnitFamilyName.Time, 0.001),        // 1 ms = 0.001 s
            UnitDefinition.LinearUnit("μs", "microseconds", UnitFamilyName.Time, 0.000001),     // 1 μs = 0.000001 s
            UnitDefinition.LinearUnit("ns", "nanoseconds", UnitFamilyName.Time, 0.000000001),   // 1 ns = 1e-9 s
            UnitDefinition.LinearUnit("ps", "picoseconds", UnitFamilyName.Time, 0.000000000001), // 1 ps = 1e-12 s
            UnitDefinition.LinearUnit("fs", "femtoseconds", UnitFamilyName.Time, 0.000000000000001), // 1 fs = 1e-15 s
            UnitDefinition.LinearUnit("min", "minutes", UnitFamilyName.Time, 60.0),             // 1 min = 60 s
            UnitDefinition.LinearUnit("h", "hours", UnitFamilyName.Time, 3600.0),               // 1 h = 3600 s
            UnitDefinition.LinearUnit("d", "days", UnitFamilyName.Time, 86400.0),               // 1 d = 86400 s

            // Area units (square meters as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("m2", "square meters", UnitFamilyName.Area),
            UnitDefinition.LinearUnit("mm2", "square millimeters", UnitFamilyName.Area, 0.000001), // 1 mm² = 1e-6 m²
            UnitDefinition.LinearUnit("cm2", "square centimeters", UnitFamilyName.Area, 0.0001), // 1 cm² = 0.0001 m²
            UnitDefinition.LinearUnit("km2", "square kilometers", UnitFamilyName.Area, 1000000.0), // 1 km² = 1,000,000 m²
            UnitDefinition.LinearUnit("ha", "hectares", UnitFamilyName.Area, 10000.0),          // 1 ha = 10,000 m²
            UnitDefinition.LinearUnit("in2", "square inches", UnitFamilyName.Area, 0.00064516), // 1 in² = 0.00064516 m²
            UnitDefinition.LinearUnit("ft2", "square feet", UnitFamilyName.Area, 0.092903),     // 1 ft² = 0.092903 m²
            UnitDefinition.LinearUnit("acre", "acres", UnitFamilyName.Area, 4046.86),           // 1 acre = 4046.86 m²

            // Volume units (cubic meters as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("m3", "cubic meters", UnitFamilyName.Volume),
            UnitDefinition.LinearUnit("mm3", "cubic millimeters", UnitFamilyName.Volume, 0.000000001), // 1 mm³ = 1e-9 m³
            UnitDefinition.LinearUnit("cm3", "cubic centimeters", UnitFamilyName.Volume, 0.000001), // 1 cm³ = 1e-6 m³
            UnitDefinition.LinearUnit("L", "liters", UnitFamilyName.Volume, 0.001),             // 1 L = 0.001 m³
            UnitDefinition.LinearUnit("mL", "milliliters", UnitFamilyName.Volume, 0.000001),    // 1 mL = 1e-6 m³
            UnitDefinition.LinearUnit("μL", "microliters", UnitFamilyName.Volume, 0.000000001), // 1 μL = 1e-9 m³
            UnitDefinition.LinearUnit("uL", "microliters", UnitFamilyName.Volume, 0.000000001), // 1 uL = 1e-9 m³ (ASCII)
            UnitDefinition.LinearUnit("in3", "cubic inches", UnitFamilyName.Volume, 0.000016387), // 1 in³ = 1.6387e-5 m³
            UnitDefinition.LinearUnit("ft3", "cubic feet", UnitFamilyName.Volume, 0.0283168),   // 1 ft³ = 0.0283168 m³
            UnitDefinition.LinearUnit("gal", "US gallons", UnitFamilyName.Volume, 0.003785411784), // 1 gal = 0.003785 m³

            // Speed units (meters per second as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("m/s", "meters per second", UnitFamilyName.Speed),
            UnitDefinition.LinearUnit("mm/s", "millimeters per second", UnitFamilyName.Speed, 0.001), // 1 mm/s = 0.001 m/s
            UnitDefinition.LinearUnit("cm/s", "centimeters per second", UnitFamilyName.Speed, 0.01), // 1 cm/s = 0.01 m/s
            UnitDefinition.LinearUnit("km/h", "kilometers per hour", UnitFamilyName.Speed, 0.277778), // 1 km/h = 0.278 m/s
            UnitDefinition.LinearUnit("kph", "kilometers per hour", UnitFamilyName.Speed, 0.277778), // 1 kph = 0.278 m/s (ASCII alternative)
            UnitDefinition.LinearUnit("km/s", "kilometers per second", UnitFamilyName.Speed, 1000.0), // 1 km/s = 1000 m/s
            UnitDefinition.LinearUnit("ft/s", "feet per second", UnitFamilyName.Speed, 0.3048),  // 1 ft/s = 0.3048 m/s
            UnitDefinition.LinearUnit("mph", "miles per hour", UnitFamilyName.Speed, 0.44704),   // 1 mph = 0.44704 m/s

            // Distance units (kilometers as base) - Large-scale measurements with dual family support
            UnitDefinition.BaseUnit("km", "kilometers", UnitFamilyName.Distance),
            // Dual family support: Accept all Length units but convert to km base
            UnitDefinition.LinearUnit("m", "meters", UnitFamilyName.Distance, 0.001),           // 1 m = 0.001 km
            UnitDefinition.LinearUnit("mm", "millimeters", UnitFamilyName.Distance, 0.000001),  // 1 mm = 0.000001 km
            UnitDefinition.LinearUnit("cm", "centimeters", UnitFamilyName.Distance, 0.00001),   // 1 cm = 0.00001 km
            UnitDefinition.LinearUnit("μm", "micrometers", UnitFamilyName.Distance, 0.000000001), // 1 μm = 1e-9 km
            UnitDefinition.LinearUnit("nm", "nanometers", UnitFamilyName.Distance, 0.000000000001), // 1 nm = 1e-12 km
            UnitDefinition.LinearUnit("pm", "picometers", UnitFamilyName.Distance, 0.000000000000001), // 1 pm = 1e-15 km
            UnitDefinition.LinearUnit("Å", "angstroms", UnitFamilyName.Distance, 0.0000000000001), // 1 Å = 1e-13 km
            UnitDefinition.LinearUnit("in", "inches", UnitFamilyName.Distance, 0.0000254),       // 1 in = 0.0000254 km
            UnitDefinition.LinearUnit("ft", "feet", UnitFamilyName.Distance, 0.0003048),         // 1 ft = 0.0003048 km
            UnitDefinition.LinearUnit("yd", "yards", UnitFamilyName.Distance, 0.0009144),        // 1 yd = 0.0009144 km
            // Traditional distance units
            UnitDefinition.LinearUnit("mi", "miles", UnitFamilyName.Distance, 1.609344),        // 1 mi = 1.609344 km
            UnitDefinition.LinearUnit("nmi", "nautical miles", UnitFamilyName.Distance, 1.852), // 1 nmi = 1.852 km

            // Pressure units (pascals as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("Pa", "pascals", UnitFamilyName.Pressure),
            UnitDefinition.LinearUnit("mPa", "millipascals", UnitFamilyName.Pressure, 0.001),   // 1 mPa = 0.001 Pa
            UnitDefinition.LinearUnit("kPa", "kilopascals", UnitFamilyName.Pressure, 1000.0),   // 1 kPa = 1000 Pa
            UnitDefinition.LinearUnit("MPa", "megapascals", UnitFamilyName.Pressure, 1000000.0), // 1 MPa = 1,000,000 Pa
            UnitDefinition.LinearUnit("GPa", "gigapascals", UnitFamilyName.Pressure, 1000000000.0), // 1 GPa = 1e9 Pa
            UnitDefinition.LinearUnit("bar", "bars", UnitFamilyName.Pressure, 100000.0),        // 1 bar = 100,000 Pa
            UnitDefinition.LinearUnit("mbar", "millibars", UnitFamilyName.Pressure, 100.0),     // 1 mbar = 100 Pa
            UnitDefinition.LinearUnit("atm", "atmospheres", UnitFamilyName.Pressure, 101325.0), // 1 atm = 101,325 Pa
            UnitDefinition.LinearUnit("torr", "torr", UnitFamilyName.Pressure, 133.322),        // 1 torr = 133.322 Pa
            UnitDefinition.LinearUnit("psi", "pounds per square inch", UnitFamilyName.Pressure, 6894.76), // 1 psi = 6894.76 Pa

            // Energy units (joules as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("J", "joules", UnitFamilyName.Energy),
            UnitDefinition.LinearUnit("mJ", "millijoules", UnitFamilyName.Energy, 0.001),       // 1 mJ = 0.001 J
            UnitDefinition.LinearUnit("μJ", "microjoules", UnitFamilyName.Energy, 0.000001),    // 1 μJ = 0.000001 J
            UnitDefinition.LinearUnit("nJ", "nanojoules", UnitFamilyName.Energy, 0.000000001),  // 1 nJ = 1e-9 J
            UnitDefinition.LinearUnit("kJ", "kilojoules", UnitFamilyName.Energy, 1000.0),       // 1 kJ = 1000 J
            UnitDefinition.LinearUnit("MJ", "megajoules", UnitFamilyName.Energy, 1000000.0),    // 1 MJ = 1,000,000 J
            UnitDefinition.LinearUnit("GJ", "gigajoules", UnitFamilyName.Energy, 1000000000.0), // 1 GJ = 1e9 J
            UnitDefinition.LinearUnit("cal", "calories", UnitFamilyName.Energy, 4.184),         // 1 cal = 4.184 J
            UnitDefinition.LinearUnit("kcal", "kilocalories", UnitFamilyName.Energy, 4184.0),   // 1 kcal = 4184 J
            UnitDefinition.LinearUnit("eV", "electron volts", UnitFamilyName.Energy, 1.602176634e-19), // 1 eV in J
            UnitDefinition.LinearUnit("keV", "kiloelectron volts", UnitFamilyName.Energy, 1.602176634e-16), // 1 keV in J
            UnitDefinition.LinearUnit("MeV", "megaelectron volts", UnitFamilyName.Energy, 1.602176634e-13), // 1 MeV in J
            UnitDefinition.LinearUnit("GeV", "gigaelectron volts", UnitFamilyName.Energy, 1.602176634e-10), // 1 GeV in J
            UnitDefinition.LinearUnit("Wh", "watt-hours", UnitFamilyName.Energy, 3600.0),       // 1 Wh = 3600 J
            UnitDefinition.LinearUnit("kWh", "kilowatt-hours", UnitFamilyName.Energy, 3600000.0), // 1 kWh = 3,600,000 J

            // Power units (watts as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("W", "watts", UnitFamilyName.Power),
            UnitDefinition.LinearUnit("mW", "milliwatts", UnitFamilyName.Power, 0.001),         // 1 mW = 0.001 W
            UnitDefinition.LinearUnit("μW", "microwatts", UnitFamilyName.Power, 0.000001),      // 1 μW = 0.000001 W
            UnitDefinition.LinearUnit("nW", "nanowatts", UnitFamilyName.Power, 0.000000001),    // 1 nW = 1e-9 W
            UnitDefinition.LinearUnit("pW", "picowatts", UnitFamilyName.Power, 0.000000000001), // 1 pW = 1e-12 W
            UnitDefinition.LinearUnit("kW", "kilowatts", UnitFamilyName.Power, 1000.0),         // 1 kW = 1000 W
            UnitDefinition.LinearUnit("MW", "megawatts", UnitFamilyName.Power, 1000000.0),      // 1 MW = 1,000,000 W
            UnitDefinition.LinearUnit("GW", "gigawatts", UnitFamilyName.Power, 1000000000.0),   // 1 GW = 1e9 W
            UnitDefinition.LinearUnit("hp", "horsepower", UnitFamilyName.Power, 745.7),         // 1 hp = 745.7 W

            // Frequency units (hertz as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("Hz", "hertz", UnitFamilyName.Frequency),
            UnitDefinition.LinearUnit("mHz", "millihertz", UnitFamilyName.Frequency, 0.001),    // 1 mHz = 0.001 Hz
            UnitDefinition.LinearUnit("kHz", "kilohertz", UnitFamilyName.Frequency, 1000.0),    // 1 kHz = 1000 Hz
            UnitDefinition.LinearUnit("MHz", "megahertz", UnitFamilyName.Frequency, 1000000.0), // 1 MHz = 1,000,000 Hz
            UnitDefinition.LinearUnit("GHz", "gigahertz", UnitFamilyName.Frequency, 1000000000.0), // 1 GHz = 1e9 Hz
            UnitDefinition.LinearUnit("THz", "terahertz", UnitFamilyName.Frequency, 1000000000000.0), // 1 THz = 1e12 Hz

            // Voltage units (volts as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("V", "volts", UnitFamilyName.Voltage),
            UnitDefinition.LinearUnit("mV", "millivolts", UnitFamilyName.Voltage, 0.001),       // 1 mV = 0.001 V
            UnitDefinition.LinearUnit("μV", "microvolts", UnitFamilyName.Voltage, 0.000001),    // 1 μV = 0.000001 V
            UnitDefinition.LinearUnit("nV", "nanovolts", UnitFamilyName.Voltage, 0.000000001),  // 1 nV = 1e-9 V
            UnitDefinition.LinearUnit("kV", "kilovolts", UnitFamilyName.Voltage, 1000.0),       // 1 kV = 1000 V
            UnitDefinition.LinearUnit("MV", "megavolts", UnitFamilyName.Voltage, 1000000.0),    // 1 MV = 1,000,000 V

            // Current units (amperes as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("A", "amperes", UnitFamilyName.Current),
            UnitDefinition.LinearUnit("mA", "milliamperes", UnitFamilyName.Current, 0.001),     // 1 mA = 0.001 A
            UnitDefinition.LinearUnit("μA", "microamperes", UnitFamilyName.Current, 0.000001),  // 1 μA = 0.000001 A
            UnitDefinition.LinearUnit("nA", "nanoamperes", UnitFamilyName.Current, 0.000000001), // 1 nA = 1e-9 A
            UnitDefinition.LinearUnit("pA", "picoamperes", UnitFamilyName.Current, 0.000000000001), // 1 pA = 1e-12 A
            UnitDefinition.LinearUnit("kA", "kiloamperes", UnitFamilyName.Current, 1000.0),     // 1 kA = 1000 A

            // Amount of substance units (moles as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("mol", "moles", UnitFamilyName.AmountOfSubstance),
            UnitDefinition.LinearUnit("mmol", "millimoles", UnitFamilyName.AmountOfSubstance, 0.001), // 1 mmol = 0.001 mol
            UnitDefinition.LinearUnit("μmol", "micromoles", UnitFamilyName.AmountOfSubstance, 0.000001), // 1 μmol = 1e-6 mol
            UnitDefinition.LinearUnit("nmol", "nanomoles", UnitFamilyName.AmountOfSubstance, 0.000000001), // 1 nmol = 1e-9 mol
            UnitDefinition.LinearUnit("pmol", "picomoles", UnitFamilyName.AmountOfSubstance, 0.000000000001), // 1 pmol = 1e-12 mol
            UnitDefinition.LinearUnit("kmol", "kilomoles", UnitFamilyName.AmountOfSubstance, 1000.0), // 1 kmol = 1000 mol

            // Luminous intensity units (candela as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("cd", "candela", UnitFamilyName.LuminousIntensity),
            UnitDefinition.LinearUnit("mcd", "millicandela", UnitFamilyName.LuminousIntensity, 0.001), // 1 mcd = 0.001 cd
            UnitDefinition.LinearUnit("kcd", "kilocandela", UnitFamilyName.LuminousIntensity, 1000.0), // 1 kcd = 1000 cd

            // Luminous flux units (lumens as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("lm", "lumens", UnitFamilyName.LuminousFlux),
            UnitDefinition.LinearUnit("mlm", "millilumens", UnitFamilyName.LuminousFlux, 0.001), // 1 mlm = 0.001 lm
            UnitDefinition.LinearUnit("klm", "kilolumens", UnitFamilyName.LuminousFlux, 1000.0), // 1 klm = 1000 lm

            // Illuminance units (lux as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("lx", "lux", UnitFamilyName.Illuminance),
            UnitDefinition.LinearUnit("mlx", "millilux", UnitFamilyName.Illuminance, 0.001),    // 1 mlx = 0.001 lx
            UnitDefinition.LinearUnit("klx", "kilolux", UnitFamilyName.Illuminance, 1000.0),    // 1 klx = 1000 lx
            UnitDefinition.LinearUnit("fc", "foot-candles", UnitFamilyName.Illuminance, 10.764), // 1 fc = 10.764 lx

            // Capacitance units (farads as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("F", "farads", UnitFamilyName.Capacitance),
            UnitDefinition.LinearUnit("mF", "millifarads", UnitFamilyName.Capacitance, 0.001),  // 1 mF = 0.001 F
            UnitDefinition.LinearUnit("μF", "microfarads", UnitFamilyName.Capacitance, 0.000001), // 1 μF = 1e-6 F
            UnitDefinition.LinearUnit("nF", "nanofarads", UnitFamilyName.Capacitance, 0.000000001), // 1 nF = 1e-9 F
            UnitDefinition.LinearUnit("pF", "picofarads", UnitFamilyName.Capacitance, 0.000000000001), // 1 pF = 1e-12 F

            // Resistance units (ohms as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("Ω", "ohms", UnitFamilyName.Resistance),
            UnitDefinition.LinearUnit("mΩ", "milliohms", UnitFamilyName.Resistance, 0.001),     // 1 mΩ = 0.001 Ω
            UnitDefinition.LinearUnit("μΩ", "microhms", UnitFamilyName.Resistance, 0.000001),   // 1 μΩ = 1e-6 Ω
            UnitDefinition.LinearUnit("kΩ", "kilohms", UnitFamilyName.Resistance, 1000.0),      // 1 kΩ = 1000 Ω
            UnitDefinition.LinearUnit("MΩ", "megohms", UnitFamilyName.Resistance, 1000000.0),   // 1 MΩ = 1e6 Ω
            UnitDefinition.LinearUnit("GΩ", "gigohms", UnitFamilyName.Resistance, 1000000000.0), // 1 GΩ = 1e9 Ω

            // Conductance units (siemens as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("S", "siemens", UnitFamilyName.Conductance),
            UnitDefinition.LinearUnit("mS", "millisiemens", UnitFamilyName.Conductance, 0.001), // 1 mS = 0.001 S
            UnitDefinition.LinearUnit("μS", "microsiemens", UnitFamilyName.Conductance, 0.000001), // 1 μS = 1e-6 S
            UnitDefinition.LinearUnit("nS", "nanosiemens", UnitFamilyName.Conductance, 0.000000001), // 1 nS = 1e-9 S

            // Magnetic flux units (webers as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("Wb", "webers", UnitFamilyName.MagneticFlux),
            UnitDefinition.LinearUnit("mWb", "milliwebers", UnitFamilyName.MagneticFlux, 0.001), // 1 mWb = 0.001 Wb
            UnitDefinition.LinearUnit("μWb", "microwebers", UnitFamilyName.MagneticFlux, 0.000001), // 1 μWb = 1e-6 Wb
            UnitDefinition.LinearUnit("nWb", "nanowebers", UnitFamilyName.MagneticFlux, 0.000000001), // 1 nWb = 1e-9 Wb

            // Magnetic flux density units (tesla as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("T", "tesla", UnitFamilyName.MagneticFluxDensity),
            UnitDefinition.LinearUnit("mT", "millitesla", UnitFamilyName.MagneticFluxDensity, 0.001), // 1 mT = 0.001 T
            UnitDefinition.LinearUnit("μT", "microtesla", UnitFamilyName.MagneticFluxDensity, 0.000001), // 1 μT = 1e-6 T
            UnitDefinition.LinearUnit("nT", "nanotesla", UnitFamilyName.MagneticFluxDensity, 0.000000001), // 1 nT = 1e-9 T
            UnitDefinition.LinearUnit("G", "gauss", UnitFamilyName.MagneticFluxDensity, 0.0001), // 1 G = 0.0001 T

            // Inductance units (henries as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("H", "henries", UnitFamilyName.Inductance),
            UnitDefinition.LinearUnit("mH", "millihenries", UnitFamilyName.Inductance, 0.001),  // 1 mH = 0.001 H
            UnitDefinition.LinearUnit("μH", "microhenries", UnitFamilyName.Inductance, 0.000001), // 1 μH = 1e-6 H
            UnitDefinition.LinearUnit("nH", "nanohenries", UnitFamilyName.Inductance, 0.000000001), // 1 nH = 1e-9 H

            // Charge units (coulombs as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("C", "coulombs", UnitFamilyName.ElectricCharge),
            UnitDefinition.LinearUnit("mC", "millicoulombs", UnitFamilyName.ElectricCharge, 0.001), // 1 mC = 0.001 C
            UnitDefinition.LinearUnit("μC", "microcoulombs", UnitFamilyName.ElectricCharge, 0.000001), // 1 μC = 1e-6 C
            UnitDefinition.LinearUnit("nC", "nanocoulombs", UnitFamilyName.ElectricCharge, 0.000000001), // 1 nC = 1e-9 C
            UnitDefinition.LinearUnit("pC", "picocoulombs", UnitFamilyName.ElectricCharge, 0.000000000001), // 1 pC = 1e-12 C
            UnitDefinition.LinearUnit("e", "elementary charges", UnitFamilyName.ElectricCharge, 1.602176634e-19), // 1 e in C

            // Radioactivity units (becquerels as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("Bq", "becquerels", UnitFamilyName.Radioactivity),
            UnitDefinition.LinearUnit("kBq", "kilobecquerels", UnitFamilyName.Radioactivity, 1000.0), // 1 kBq = 1000 Bq
            UnitDefinition.LinearUnit("MBq", "megabecquerels", UnitFamilyName.Radioactivity, 1000000.0), // 1 MBq = 1e6 Bq
            UnitDefinition.LinearUnit("GBq", "gigabecquerels", UnitFamilyName.Radioactivity, 1000000000.0), // 1 GBq = 1e9 Bq
            UnitDefinition.LinearUnit("TBq", "terabecquerels", UnitFamilyName.Radioactivity, 1000000000000.0), // 1 TBq = 1e12 Bq
            UnitDefinition.LinearUnit("Ci", "curies", UnitFamilyName.Radioactivity, 37000000000.0), // 1 Ci = 3.7e10 Bq

            // Absorbed dose units (grays as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("Gy", "grays", UnitFamilyName.AbsorbedDose),
            UnitDefinition.LinearUnit("mGy", "milligrays", UnitFamilyName.AbsorbedDose, 0.001), // 1 mGy = 0.001 Gy
            UnitDefinition.LinearUnit("μGy", "micrograys", UnitFamilyName.AbsorbedDose, 0.000001), // 1 μGy = 1e-6 Gy
            UnitDefinition.LinearUnit("nGy", "nanograys", UnitFamilyName.AbsorbedDose, 0.000000001), // 1 nGy = 1e-9 Gy
            UnitDefinition.LinearUnit("rad", "rads", UnitFamilyName.AbsorbedDose, 0.01),        // 1 rad = 0.01 Gy

            // Equivalent dose units (sieverts as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("Sv", "sieverts", UnitFamilyName.EquivalentDose),
            UnitDefinition.LinearUnit("mSv", "millisieverts", UnitFamilyName.EquivalentDose, 0.001), // 1 mSv = 0.001 Sv
            UnitDefinition.LinearUnit("μSv", "microsieverts", UnitFamilyName.EquivalentDose, 0.000001), // 1 μSv = 1e-6 Sv
            UnitDefinition.LinearUnit("nSv", "nanosieverts", UnitFamilyName.EquivalentDose, 0.000000001), // 1 nSv = 1e-9 Sv
            UnitDefinition.LinearUnit("rem", "roentgen equivalent man", UnitFamilyName.EquivalentDose, 0.01), // 1 rem = 0.01 Sv

            // Momentum units (kg⋅m/s as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("kg⋅m/s", "kilogram-meters per second", UnitFamilyName.Momentum),
            UnitDefinition.LinearUnit("kg*m/s", "kilogram-meters per second", UnitFamilyName.Momentum, 1.0), // ASCII alternative
            UnitDefinition.LinearUnit("g⋅m/s", "gram-meters per second", UnitFamilyName.Momentum, 0.001),     // 1 g⋅m/s = 0.001 kg⋅m/s
            UnitDefinition.LinearUnit("g*m/s", "gram-meters per second", UnitFamilyName.Momentum, 0.001)      // ASCII alternative
        };
    }
}