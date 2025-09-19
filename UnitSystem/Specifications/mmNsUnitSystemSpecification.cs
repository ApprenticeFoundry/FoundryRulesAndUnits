using System;
using System.Collections.Generic;
using FoundryRulesAndUnits.Units;

namespace FoundryRulesAndUnits.Units.Specifications
{
    /// <summary>
    /// mmNs (Millimeter-Newton-Second) Unit System Specification
    /// Base units: millimeters, grams, seconds, Celsius, degrees, newtons
    /// Uses base unit hub architecture - all conversions go through the base unit
    /// Appropriate for precision engineering, CAD, and micro-manufacturing applications
    /// </summary>
    public class mmNsUnitSystemSpecification : IUnitSystemSpecification
    {
        public string SystemName => "mmNs";

        public string SystemDescription => "mmNs (Millimeter-Newton-Second) Unit System with base units: millimeters, grams, seconds, Celsius, degrees, newtons";

        private List<string>? _cachedUnitSymbols = null;
        private List<UnitDefinition>? _cachedBaseUnits = null;
        private Dictionary<UnitFamilyName, UnitDefinition>? _cachedBaseUnitsByFamily = null;
        private Dictionary<UnitFamilyName, List<UnitDefinition>>? _cachedUnitsByFamily = null;
        private Dictionary<string, UnitFamilyName>? _cachedSymbolToFamily = null;

        public List<string> GetAllUnitSymbols()
        {
            if (_cachedUnitSymbols == null)
            {
                _cachedUnitSymbols = new List<string>();
                foreach (var unit in UnitDefinitions)
                {
                    _cachedUnitSymbols.Add(unit.Symbol);
                }
            }
            return _cachedUnitSymbols;
        }

        public List<UnitDefinition> GetAllBaseUnits()
        {
            if (_cachedBaseUnits == null)
            {
                _cachedBaseUnits = new List<UnitDefinition>();
                foreach (var unit in UnitDefinitions)
                {
                    if (unit.IsBaseUnit)
                    {
                        _cachedBaseUnits.Add(unit);
                    }
                }
            }
            return _cachedBaseUnits;
        }

        public Dictionary<UnitFamilyName, UnitDefinition> GetBaseUnitsByFamily()
        {
            if (_cachedBaseUnitsByFamily == null)
            {
                _cachedBaseUnitsByFamily = new Dictionary<UnitFamilyName, UnitDefinition>();
                foreach (var unit in UnitDefinitions)
                {
                    if (unit.IsBaseUnit)
                    {
                        _cachedBaseUnitsByFamily[unit.Family] = unit;
                    }
                }
            }
            return _cachedBaseUnitsByFamily;
        }

        public Dictionary<UnitFamilyName, List<UnitDefinition>> GetAllUnitsByFamily()
        {
            if (_cachedUnitsByFamily == null)
            {
                _cachedUnitsByFamily = new Dictionary<UnitFamilyName, List<UnitDefinition>>();
                foreach (var unit in UnitDefinitions)
                {
                    if (!_cachedUnitsByFamily.ContainsKey(unit.Family))
                    {
                        _cachedUnitsByFamily[unit.Family] = new List<UnitDefinition>();
                    }
                    _cachedUnitsByFamily[unit.Family].Add(unit);
                }
            }
            return _cachedUnitsByFamily;
        }

        public Dictionary<string, UnitFamilyName> GetSymbolToFamilyMap()
        {
            if (_cachedSymbolToFamily == null)
            {
                _cachedSymbolToFamily = new Dictionary<string, UnitFamilyName>();
                foreach (var unit in UnitDefinitions)
                {
                    _cachedSymbolToFamily[unit.Symbol] = unit.Family;
                }
            }
            return _cachedSymbolToFamily;
        }

        public IReadOnlyList<UnitDefinition> UnitDefinitions { get; } = new List<UnitDefinition>
        {
            // Length units (millimeters as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("mm", "millimeters", UnitFamilyName.Length),
            UnitDefinition.LinearUnit("μm", "micrometers", UnitFamilyName.Length, 0.001),       // 1 μm = 0.001 mm
            UnitDefinition.LinearUnit("nm", "nanometers", UnitFamilyName.Length, 0.000001),     // 1 nm = 0.000001 mm
            UnitDefinition.LinearUnit("cm", "centimeters", UnitFamilyName.Length, 10.0),        // 1 cm = 10 mm
            UnitDefinition.LinearUnit("m", "meters", UnitFamilyName.Length, 1000.0),            // 1 m = 1000 mm
            UnitDefinition.LinearUnit("km", "kilometers", UnitFamilyName.Length, 1000000.0),    // 1 km = 1,000,000 mm
            UnitDefinition.LinearUnit("in", "inches", UnitFamilyName.Length, 25.4),             // 1 in = 25.4 mm
            UnitDefinition.LinearUnit("ft", "feet", UnitFamilyName.Length, 304.8),              // 1 ft = 304.8 mm
            UnitDefinition.LinearUnit("mil", "mils", UnitFamilyName.Length, 0.0254),            // 1 mil = 0.0254 mm

            // Mass units (grams as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("g", "grams", UnitFamilyName.Mass),
            UnitDefinition.LinearUnit("mg", "milligrams", UnitFamilyName.Mass, 0.001),          // 1 mg = 0.001 g
            UnitDefinition.LinearUnit("μg", "micrograms", UnitFamilyName.Mass, 0.000001),       // 1 μg = 0.000001 g
            UnitDefinition.LinearUnit("kg", "kilograms", UnitFamilyName.Mass, 1000.0),          // 1 kg = 1000 g
            UnitDefinition.LinearUnit("lb", "pounds", UnitFamilyName.Mass, 453.592),            // 1 lb = 453.592 g
            UnitDefinition.LinearUnit("oz", "ounces", UnitFamilyName.Mass, 28.3495),            // 1 oz = 28.3495 g

            // Force units (newtons as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("N", "newtons", UnitFamilyName.Force),
            UnitDefinition.LinearUnit("mN", "millinewtons", UnitFamilyName.Force, 0.001),       // 1 mN = 0.001 N
            UnitDefinition.LinearUnit("μN", "micronewtons", UnitFamilyName.Force, 0.000001),    // 1 μN = 0.000001 N
            UnitDefinition.LinearUnit("kN", "kilonewtons", UnitFamilyName.Force, 1000.0),       // 1 kN = 1000 N
            UnitDefinition.LinearUnit("dyne", "dynes", UnitFamilyName.Force, 0.00001),          // 1 dyne = 0.00001 N
            UnitDefinition.LinearUnit("lbf", "pounds-force", UnitFamilyName.Force, 4.44822),    // 1 lbf = 4.44822 N
            UnitDefinition.LinearUnit("gf", "grams-force", UnitFamilyName.Force, 0.00980665),   // 1 gf = 0.009807 N

            // Temperature units (Celsius as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("C", "Celsius", UnitFamilyName.Temperature),
            UnitDefinition.DerivedUnit("K", "Kelvin", UnitFamilyName.Temperature,
                k => k - 273.15,          // K to C: subtract 273.15
                c => c + 273.15),         // C to K: add 273.15
            UnitDefinition.DerivedUnit("F", "Fahrenheit", UnitFamilyName.Temperature,
                f => (f - 32.0) * 5.0/9.0,       // F to C: (F-32)*5/9
                c => c * 9.0/5.0 + 32.0),        // C to F: C*9/5 + 32

            // Angle units (degrees as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("deg", "degrees", UnitFamilyName.Angle),
            UnitDefinition.LinearUnit("rad", "radians", UnitFamilyName.Angle, 180.0 / Math.PI), // 1 rad = 180/π deg
            UnitDefinition.LinearUnit("mrad", "milliradians", UnitFamilyName.Angle, 180.0 / Math.PI * 0.001), // 1 mrad
            UnitDefinition.LinearUnit("rev", "revolutions", UnitFamilyName.Angle, 360.0),       // 1 rev = 360 deg

            // Time units (seconds as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("s", "seconds", UnitFamilyName.Time),
            UnitDefinition.LinearUnit("ms", "milliseconds", UnitFamilyName.Time, 0.001),        // 1 ms = 0.001 s
            UnitDefinition.LinearUnit("μs", "microseconds", UnitFamilyName.Time, 0.000001),     // 1 μs = 0.000001 s
            UnitDefinition.LinearUnit("ns", "nanoseconds", UnitFamilyName.Time, 0.000000001),   // 1 ns = 0.000000001 s
            UnitDefinition.LinearUnit("min", "minutes", UnitFamilyName.Time, 60.0),             // 1 min = 60 s
            UnitDefinition.LinearUnit("hr", "hours", UnitFamilyName.Time, 3600.0),              // 1 hr = 3600 s

            // Area units (square millimeters as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("mm2", "square millimeters", UnitFamilyName.Area),
            UnitDefinition.LinearUnit("μm2", "square micrometers", UnitFamilyName.Area, 0.000001), // 1 μm² = 0.000001 mm²
            UnitDefinition.LinearUnit("cm2", "square centimeters", UnitFamilyName.Area, 100.0), // 1 cm² = 100 mm²
            UnitDefinition.LinearUnit("m2", "square meters", UnitFamilyName.Area, 1000000.0),   // 1 m² = 1,000,000 mm²
            UnitDefinition.LinearUnit("in2", "square inches", UnitFamilyName.Area, 645.16),     // 1 in² = 645.16 mm²

            // Volume units (cubic millimeters as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("mm3", "cubic millimeters", UnitFamilyName.Volume),
            UnitDefinition.LinearUnit("μm3", "cubic micrometers", UnitFamilyName.Volume, 0.000000001), // 1 μm³ = 1e-9 mm³
            UnitDefinition.LinearUnit("cm3", "cubic centimeters", UnitFamilyName.Volume, 1000.0), // 1 cm³ = 1000 mm³
            UnitDefinition.LinearUnit("mL", "milliliters", UnitFamilyName.Volume, 1000.0),      // 1 mL = 1000 mm³
            UnitDefinition.LinearUnit("L", "liters", UnitFamilyName.Volume, 1000000.0),         // 1 L = 1,000,000 mm³
            UnitDefinition.LinearUnit("m3", "cubic meters", UnitFamilyName.Volume, 1000000000.0), // 1 m³ = 1e9 mm³
            UnitDefinition.LinearUnit("in3", "cubic inches", UnitFamilyName.Volume, 16387.1),   // 1 in³ = 16387.1 mm³

            // Speed units (millimeters per second as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("mm/s", "millimeters per second", UnitFamilyName.Speed),
            UnitDefinition.LinearUnit("μm/s", "micrometers per second", UnitFamilyName.Speed, 0.001), // 1 μm/s = 0.001 mm/s
            UnitDefinition.LinearUnit("cm/s", "centimeters per second", UnitFamilyName.Speed, 10.0), // 1 cm/s = 10 mm/s
            UnitDefinition.LinearUnit("m/s", "meters per second", UnitFamilyName.Speed, 1000.0),    // 1 m/s = 1000 mm/s
            UnitDefinition.LinearUnit("mm/min", "millimeters per minute", UnitFamilyName.Speed, 1.0/60.0), // 1 mm/min = 1/60 mm/s
            UnitDefinition.LinearUnit("in/s", "inches per second", UnitFamilyName.Speed, 25.4),    // 1 in/s = 25.4 mm/s
            UnitDefinition.LinearUnit("ft/s", "feet per second", UnitFamilyName.Speed, 304.8),     // 1 ft/s = 304.8 mm/s

            // Distance units (kilometers as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("km", "kilometers", UnitFamilyName.Distance),
            UnitDefinition.LinearUnit("m", "meters", UnitFamilyName.Distance, 0.001),           // 1 m = 0.001 km
            UnitDefinition.LinearUnit("cm", "centimeters", UnitFamilyName.Distance, 0.00001),   // 1 cm = 0.00001 km
            UnitDefinition.LinearUnit("mm", "millimeters", UnitFamilyName.Distance, 0.000001),  // 1 mm = 0.000001 km
            UnitDefinition.LinearUnit("mi", "miles", UnitFamilyName.Distance, 1.609344),        // 1 mi = 1.609344 km
            UnitDefinition.LinearUnit("nmi", "nautical miles", UnitFamilyName.Distance, 1.852), // 1 nmi = 1.852 km

            // Pressure units (kilopascals as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("kPa", "kilopascals", UnitFamilyName.Pressure),
            UnitDefinition.LinearUnit("Pa", "pascals", UnitFamilyName.Pressure, 0.001),         // 1 Pa = 0.001 kPa
            UnitDefinition.LinearUnit("MPa", "megapascals", UnitFamilyName.Pressure, 1000.0),   // 1 MPa = 1000 kPa
            UnitDefinition.LinearUnit("bar", "bars", UnitFamilyName.Pressure, 100.0),           // 1 bar = 100 kPa
            UnitDefinition.LinearUnit("atm", "atmospheres", UnitFamilyName.Pressure, 101.325),  // 1 atm = 101.325 kPa
            UnitDefinition.LinearUnit("psi", "pounds per square inch", UnitFamilyName.Pressure, 6.89476), // 1 psi = 6.895 kPa
            UnitDefinition.LinearUnit("torr", "torr", UnitFamilyName.Pressure, 0.133322),       // 1 torr = 0.1333 kPa

            // Energy units (millijoules as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("mJ", "millijoules", UnitFamilyName.Energy),
            UnitDefinition.LinearUnit("μJ", "microjoules", UnitFamilyName.Energy, 0.001),       // 1 μJ = 0.001 mJ
            UnitDefinition.LinearUnit("nJ", "nanojoules", UnitFamilyName.Energy, 0.000001),     // 1 nJ = 0.000001 mJ
            UnitDefinition.LinearUnit("J", "joules", UnitFamilyName.Energy, 1000.0),            // 1 J = 1000 mJ
            UnitDefinition.LinearUnit("kJ", "kilojoules", UnitFamilyName.Energy, 1000000.0),    // 1 kJ = 1,000,000 mJ
            UnitDefinition.LinearUnit("cal", "calories", UnitFamilyName.Energy, 4184.0),        // 1 cal = 4184 mJ
            UnitDefinition.LinearUnit("eV", "electron volts", UnitFamilyName.Energy, 1.602176634e-16), // 1 eV in mJ

            // Power units (milliwatts as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("mW", "milliwatts", UnitFamilyName.Power),
            UnitDefinition.LinearUnit("μW", "microwatts", UnitFamilyName.Power, 0.001),         // 1 μW = 0.001 mW
            UnitDefinition.LinearUnit("nW", "nanowatts", UnitFamilyName.Power, 0.000001),       // 1 nW = 0.000001 mW
            UnitDefinition.LinearUnit("W", "watts", UnitFamilyName.Power, 1000.0),              // 1 W = 1000 mW
            UnitDefinition.LinearUnit("kW", "kilowatts", UnitFamilyName.Power, 1000000.0),      // 1 kW = 1,000,000 mW
            UnitDefinition.LinearUnit("hp", "horsepower", UnitFamilyName.Power, 745700.0),      // 1 hp = 745,700 mW

            // Frequency units (hertz as base) - same across all systems
            UnitDefinition.BaseUnit("Hz", "hertz", UnitFamilyName.Frequency),
            UnitDefinition.LinearUnit("kHz", "kilohertz", UnitFamilyName.Frequency, 1000.0),    // 1 kHz = 1000 Hz
            UnitDefinition.LinearUnit("MHz", "megahertz", UnitFamilyName.Frequency, 1000000.0), // 1 MHz = 1,000,000 Hz
            UnitDefinition.LinearUnit("GHz", "gigahertz", UnitFamilyName.Frequency, 1000000000.0), // 1 GHz = 1,000,000,000 Hz
            UnitDefinition.LinearUnit("rpm", "revolutions per minute", UnitFamilyName.Frequency, 1.0/60.0), // 1 rpm = 1/60 Hz

            // Voltage units (volts as base) - same across all systems
            UnitDefinition.BaseUnit("V", "volts", UnitFamilyName.Voltage),
            UnitDefinition.LinearUnit("mV", "millivolts", UnitFamilyName.Voltage, 0.001),       // 1 mV = 0.001 V
            UnitDefinition.LinearUnit("μV", "microvolts", UnitFamilyName.Voltage, 0.000001),    // 1 μV = 0.000001 V
            UnitDefinition.LinearUnit("kV", "kilovolts", UnitFamilyName.Voltage, 1000.0),       // 1 kV = 1000 V

            // Current units (amperes as base) - same across all systems
            UnitDefinition.BaseUnit("A", "amperes", UnitFamilyName.Current),
            UnitDefinition.LinearUnit("mA", "milliamperes", UnitFamilyName.Current, 0.001),     // 1 mA = 0.001 A
            UnitDefinition.LinearUnit("μA", "microamperes", UnitFamilyName.Current, 0.000001),  // 1 μA = 0.000001 A
            UnitDefinition.LinearUnit("nA", "nanoamperes", UnitFamilyName.Current, 0.000000001), // 1 nA = 0.000000001 A
            UnitDefinition.LinearUnit("kA", "kiloamperes", UnitFamilyName.Current, 1000.0)      // 1 kA = 1000 A
        };
    }
}