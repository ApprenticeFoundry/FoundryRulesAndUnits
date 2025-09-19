using System;
using System.Collections.Generic;
using FoundryRulesAndUnits.Units;

namespace FoundryRulesAndUnits.Units;

    /// <summary>
    /// MKS (Meter-Kilogram-Second) Unit System Specification
    /// Base units: meters, kilograms, seconds, Celsius, radians, newtons
    /// Uses base unit hub architecture - all conversions go through the base unit
    /// </summary>
public class MKSUnitSystemSpecification : IUnitSystemSpecification
{
    public string SystemName => "MKS";

    public string SystemDescription => "MKS (Meter-Kilogram-Second) Unit System with base units: meters, kilograms, seconds, Celsius, radians, newtons";

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
        // Length units (meters as base) - using enhanced approach with UnitFamilyName enum!
        UnitDefinition.BaseUnit("m", "meters", UnitFamilyName.Length),
        UnitDefinition.LinearUnit("cm", "centimeters", UnitFamilyName.Length, 0.01),         // 1 cm = 0.01 m
        UnitDefinition.LinearUnit("mm", "millimeters", UnitFamilyName.Length, 0.001),        // 1 mm = 0.001 m
        UnitDefinition.LinearUnit("km", "kilometers", UnitFamilyName.Length, 1000.0),        // 1 km = 1000 m
        UnitDefinition.LinearUnit("in", "inches", UnitFamilyName.Length, 0.0254),            // 1 in = 0.0254 m
        UnitDefinition.LinearUnit("ft", "feet", UnitFamilyName.Length, 0.3048),              // 1 ft = 0.3048 m
        UnitDefinition.LinearUnit("yd", "yards", UnitFamilyName.Length, 0.9144),             // 1 yd = 0.9144 m
        UnitDefinition.LinearUnit("mi", "miles", UnitFamilyName.Length, 1609.344),           // 1 mi = 1609.344 m
        UnitDefinition.LinearUnit("px", "pixels", UnitFamilyName.Length, 1.0 / 96.0 * 0.0254), // 96 DPI

        // Mass units (kilograms as base) - using enhanced approach with UnitFamilyName enum!
        UnitDefinition.BaseUnit("kg", "kilograms", UnitFamilyName.Mass),
        UnitDefinition.LinearUnit("g", "grams", UnitFamilyName.Mass, 0.001),                  // 1 g = 0.001 kg
        UnitDefinition.LinearUnit("mg", "milligrams", UnitFamilyName.Mass, 0.000001),        // 1 mg = 0.000001 kg
        UnitDefinition.LinearUnit("lb", "pounds", UnitFamilyName.Mass, 0.453592),            // 1 lb = 0.453592 kg
        UnitDefinition.LinearUnit("oz", "ounces", UnitFamilyName.Mass, 0.0283495),           // 1 oz = 0.0283495 kg

        // Force units (newtons as base) - using enhanced approach with UnitFamilyName enum!
        UnitDefinition.BaseUnit("N", "newtons", UnitFamilyName.Force),
        UnitDefinition.LinearUnit("kN", "kilonewtons", UnitFamilyName.Force, 1000.0),        // 1 kN = 1000 N
        UnitDefinition.LinearUnit("dyne", "dynes", UnitFamilyName.Force, 0.00001),         // 1 dyne = 0.00001 N
        UnitDefinition.LinearUnit("lbf", "pounds-force", UnitFamilyName.Force, 4.44822),    // 1 lbf = 4.44822 N

        // Temperature units (Kelvin as base for absolute temperature scale) - using enhanced approach with UnitFamilyName enum!
        UnitDefinition.BaseUnit("K", "Kelvin", UnitFamilyName.Temperature),
        UnitDefinition.DerivedUnit("C", "Celsius", UnitFamilyName.Temperature,
            c => c + 273.15,          // C to K: add 273.15
            k => k - 273.15),         // K to C: subtract 273.15
        UnitDefinition.DerivedUnit("F", "Fahrenheit", UnitFamilyName.Temperature,
            f => (f - 32.0) * 5.0 / 9.0 + 273.15,    // F to K: (F-32)*5/9 + 273.15
            k => (k - 273.15) * 9.0 / 5.0 + 32.0),   // K to F: (K-273.15)*9/5 + 32

        // Angle units (radians as base) - using enhanced approach with UnitFamilyName enum!
        UnitDefinition.BaseUnit("rad", "radians", UnitFamilyName.Angle),
        UnitDefinition.LinearUnit("deg", "degrees", UnitFamilyName.Angle, Math.PI / 180.0),  // 1 deg = π/180 rad
        UnitDefinition.LinearUnit("mrad", "milliradians", UnitFamilyName.Angle, 0.001),     // 1 mrad = 0.001 rad

        // Time units (seconds as base) - using enhanced approach with UnitFamilyName enum!
        UnitDefinition.BaseUnit("s", "seconds", UnitFamilyName.Time),
        UnitDefinition.LinearUnit("ms", "milliseconds", UnitFamilyName.Time, 0.001),          // 1 ms = 0.001 s
        UnitDefinition.LinearUnit("min", "minutes", UnitFamilyName.Time, 60.0),              // 1 min = 60 s
        UnitDefinition.LinearUnit("hr", "hours", UnitFamilyName.Time, 3600.0),                // 1 hr = 3600 s
        UnitDefinition.LinearUnit("day", "days", UnitFamilyName.Time, 86400.0),              // 1 day = 86400 s

        // Area units (square meters as base) - using enhanced approach with UnitFamilyName enum!
        UnitDefinition.BaseUnit("m2", "square meters", UnitFamilyName.Area),
        UnitDefinition.LinearUnit("cm2", "square centimeters", UnitFamilyName.Area, 0.0001), // 1 cm² = 0.0001 m²
        UnitDefinition.LinearUnit("mm2", "square millimeters", UnitFamilyName.Area, 0.000001), // 1 mm² = 0.000001 m²
        UnitDefinition.LinearUnit("km2", "square kilometers", UnitFamilyName.Area, 1000000.0), // 1 km² = 1,000,000 m²

        // Volume units (cubic meters as base) - using enhanced approach with UnitFamilyName enum!
        UnitDefinition.BaseUnit("m3", "cubic meters", UnitFamilyName.Volume),
        UnitDefinition.LinearUnit("cm3", "cubic centimeters", UnitFamilyName.Volume, 0.000001), // 1 cm³ = 0.000001 m³
        UnitDefinition.LinearUnit("mm3", "cubic millimeters", UnitFamilyName.Volume, 0.000000001), // 1 mm³ = 1e-9 m³
        UnitDefinition.LinearUnit("km3", "cubic kilometers", UnitFamilyName.Volume, 1000000000.0), // 1 km³ = 1e9 m³

        // Speed units (meters per second as base) - using enhanced approach with UnitFamilyName enum!
        UnitDefinition.BaseUnit("m/s", "meters per second", UnitFamilyName.Speed),
        UnitDefinition.LinearUnit("km/h", "kilometers per hour", UnitFamilyName.Speed, 1000.0 / 3600.0), // 1 km/h = 1000/3600 m/s
        UnitDefinition.LinearUnit("mph", "miles per hour", UnitFamilyName.Speed, 0.44704),              // 1 mph = 0.44704 m/s
        UnitDefinition.LinearUnit("ft/s", "feet per second", UnitFamilyName.Speed, 0.3048),           // 1 ft/s = 0.3048 m/s
        UnitDefinition.LinearUnit("knot", "knots", UnitFamilyName.Speed, 0.514444),                    // 1 knot = 0.514444 m/s

        // Distance units (kilometers as base) - using enhanced approach with UnitFamilyName enum!
        UnitDefinition.BaseUnit("km", "kilometers", UnitFamilyName.Distance),
        UnitDefinition.LinearUnit("m", "meters", UnitFamilyName.Distance, 0.001),              // 1 m = 0.001 km
        UnitDefinition.LinearUnit("cm", "centimeters", UnitFamilyName.Distance, 0.00001),      // 1 cm = 0.00001 km
        UnitDefinition.LinearUnit("mm", "millimeters", UnitFamilyName.Distance, 0.000001),     // 1 mm = 0.000001 km
        UnitDefinition.LinearUnit("ft", "feet", UnitFamilyName.Distance, 0.0003048),           // 1 ft = 0.0003048 km
        UnitDefinition.LinearUnit("mi", "miles", UnitFamilyName.Distance, 1.609344),           // 1 mi = 1.609344 km
        UnitDefinition.LinearUnit("nmi", "nautical miles", UnitFamilyName.Distance, 1.852),    // 1 nmi = 1.852 km

        // Duration units (seconds as base) - using enhanced approach with UnitFamilyName enum!
        UnitDefinition.BaseUnit("s", "seconds", UnitFamilyName.Duration),
        UnitDefinition.LinearUnit("min", "minutes", UnitFamilyName.Duration, 60.0),            // 1 min = 60 s
        UnitDefinition.LinearUnit("hr", "hours", UnitFamilyName.Duration, 3600.0),             // 1 hr = 3600 s
        UnitDefinition.LinearUnit("day", "days", UnitFamilyName.Duration, 86400.0),            // 1 day = 86400 s
        UnitDefinition.LinearUnit("week", "weeks", UnitFamilyName.Duration, 604800.0),         // 1 week = 604800 s
        UnitDefinition.LinearUnit("month", "months", UnitFamilyName.Duration, 2629746.0),      // 1 month ≈ 30.44 days
        UnitDefinition.LinearUnit("year", "years", UnitFamilyName.Duration, 31556952.0),       // 1 year = 365.2425 days

        // Heading units (degrees as base) - using enhanced approach with UnitFamilyName enum!
        UnitDefinition.BaseUnit("deg", "degrees", UnitFamilyName.Heading),
        UnitDefinition.LinearUnit("rad", "radians", UnitFamilyName.Heading, 180.0 / Math.PI),  // 1 rad = 180/π deg
        UnitDefinition.LinearUnit("grad", "gradians", UnitFamilyName.Heading, 0.9),            // 1 grad = 0.9 deg
        UnitDefinition.LinearUnit("mil", "mils", UnitFamilyName.Heading, 0.05625),             // 1 mil = 0.05625 deg

        // Quantity units (each as base) - using enhanced approach with UnitFamilyName enum!
        UnitDefinition.BaseUnit("ea", "each", UnitFamilyName.Quantity),
        UnitDefinition.LinearUnit("dz", "dozen", UnitFamilyName.Quantity, 12.0),               // 1 dz = 12 ea
        UnitDefinition.LinearUnit("gr", "gross", UnitFamilyName.Quantity, 144.0),              // 1 gr = 144 ea
        UnitDefinition.LinearUnit("pair", "pairs", UnitFamilyName.Quantity, 2.0),              // 1 pair = 2 ea
        UnitDefinition.LinearUnit("score", "scores", UnitFamilyName.Quantity, 20.0),           // 1 score = 20 ea

        // QuantityFlow units (each per second as base) - using enhanced approach with UnitFamilyName enum!
        UnitDefinition.BaseUnit("ea/s", "each per second", UnitFamilyName.QuantityFlow),
        UnitDefinition.LinearUnit("ea/min", "each per minute", UnitFamilyName.QuantityFlow, 1.0 / 60.0),     // 1 ea/min = 1/60 ea/s
        UnitDefinition.LinearUnit("ea/hr", "each per hour", UnitFamilyName.QuantityFlow, 1.0 / 3600.0),      // 1 ea/hr = 1/3600 ea/s
        UnitDefinition.LinearUnit("ea/day", "each per day", UnitFamilyName.QuantityFlow, 1.0 / 86400.0),     // 1 ea/day = 1/86400 ea/s

        // Pressure units (pascals as base) - using enhanced approach with UnitFamilyName enum!
        UnitDefinition.BaseUnit("Pa", "pascals", UnitFamilyName.Pressure),
        UnitDefinition.LinearUnit("kPa", "kilopascals", UnitFamilyName.Pressure, 1000.0),     // 1 kPa = 1000 Pa
        UnitDefinition.LinearUnit("MPa", "megapascals", UnitFamilyName.Pressure, 1000000.0),  // 1 MPa = 1,000,000 Pa
        UnitDefinition.LinearUnit("bar", "bars", UnitFamilyName.Pressure, 100000.0),          // 1 bar = 100,000 Pa
        UnitDefinition.LinearUnit("atm", "atmospheres", UnitFamilyName.Pressure, 101325.0),   // 1 atm = 101,325 Pa
        UnitDefinition.LinearUnit("psi", "pounds per square inch", UnitFamilyName.Pressure, 6894.76), // 1 psi = 6894.76 Pa
        UnitDefinition.LinearUnit("torr", "torr", UnitFamilyName.Pressure, 133.322),          // 1 torr = 133.322 Pa
        UnitDefinition.LinearUnit("mmHg", "millimeters of mercury", UnitFamilyName.Pressure, 133.322), // 1 mmHg = 133.322 Pa

        // DataStorage units (bytes as base) - using enhanced approach with UnitFamilyName enum!
        UnitDefinition.BaseUnit("B", "bytes", UnitFamilyName.DataStorage),
        UnitDefinition.LinearUnit("KB", "kilobytes", UnitFamilyName.DataStorage, 1024.0),     // 1 KB = 1024 B
        UnitDefinition.LinearUnit("MB", "megabytes", UnitFamilyName.DataStorage, 1048576.0),  // 1 MB = 1024² B
        UnitDefinition.LinearUnit("GB", "gigabytes", UnitFamilyName.DataStorage, 1073741824.0), // 1 GB = 1024³ B
        UnitDefinition.LinearUnit("TB", "terabytes", UnitFamilyName.DataStorage, 1099511627776.0), // 1 TB = 1024⁴ B
        UnitDefinition.LinearUnit("bit", "bits", UnitFamilyName.DataStorage, 0.125),           // 1 bit = 0.125 B

        // DataFlow units (bytes per second as base) - using enhanced approach with UnitFamilyName enum!
        UnitDefinition.BaseUnit("B/s", "bytes per second", UnitFamilyName.DataFlow),
        UnitDefinition.LinearUnit("KB/s", "kilobytes per second", UnitFamilyName.DataFlow, 1024.0),    // 1 KB/s = 1024 B/s
        UnitDefinition.LinearUnit("MB/s", "megabytes per second", UnitFamilyName.DataFlow, 1048576.0), // 1 MB/s = 1024² B/s
        UnitDefinition.LinearUnit("GB/s", "gigabytes per second", UnitFamilyName.DataFlow, 1073741824.0), // 1 GB/s = 1024³ B/s
        UnitDefinition.LinearUnit("bit/s", "bits per second", UnitFamilyName.DataFlow, 0.125),         // 1 bit/s = 0.125 B/s
        UnitDefinition.LinearUnit("Mbps", "megabits per second", UnitFamilyName.DataFlow, 125000.0),   // 1 Mbps = 125,000 B/s
        UnitDefinition.LinearUnit("Gbps", "gigabits per second", UnitFamilyName.DataFlow, 125000000.0), // 1 Gbps = 125,000,000 B/s

        // WorkTime units (hours as base) - using enhanced approach with UnitFamilyName enum!
        UnitDefinition.BaseUnit("hr", "hours", UnitFamilyName.WorkTime),
        UnitDefinition.LinearUnit("min", "minutes", UnitFamilyName.WorkTime, 1.0 / 60.0),       // 1 min = 1/60 hr
        UnitDefinition.LinearUnit("day", "work days", UnitFamilyName.WorkTime, 8.0),          // 1 work day = 8 hr
        UnitDefinition.LinearUnit("week", "work weeks", UnitFamilyName.WorkTime, 40.0),       // 1 work week = 40 hr
        UnitDefinition.LinearUnit("month", "work months", UnitFamilyName.WorkTime, 173.33),   // 1 work month ≈ 173.33 hr
        UnitDefinition.LinearUnit("year", "work years", UnitFamilyName.WorkTime, 2080.0),     // 1 work year = 2080 hr

        // Voltage units (volts as base) - using enhanced approach with UnitFamilyName enum!
        UnitDefinition.BaseUnit("V", "volts", UnitFamilyName.Voltage),
        UnitDefinition.LinearUnit("mV", "millivolts", UnitFamilyName.Voltage, 0.001),         // 1 mV = 0.001 V
        UnitDefinition.LinearUnit("kV", "kilovolts", UnitFamilyName.Voltage, 1000.0),         // 1 kV = 1000 V
        UnitDefinition.LinearUnit("MV", "megavolts", UnitFamilyName.Voltage, 1000000.0),      // 1 MV = 1,000,000 V

        // Current units (amperes as base) - using enhanced approach with UnitFamilyName enum!
        UnitDefinition.BaseUnit("A", "amperes", UnitFamilyName.Current),
        UnitDefinition.LinearUnit("mA", "milliamperes", UnitFamilyName.Current, 0.001),       // 1 mA = 0.001 A
        UnitDefinition.LinearUnit("μA", "microamperes", UnitFamilyName.Current, 0.000001),    // 1 μA = 0.000001 A
        UnitDefinition.LinearUnit("kA", "kiloamperes", UnitFamilyName.Current, 1000.0),       // 1 kA = 1000 A

        // Power units (watts as base) - using enhanced approach with UnitFamilyName enum!
        UnitDefinition.BaseUnit("W", "watts", UnitFamilyName.Power),
        UnitDefinition.LinearUnit("mW", "milliwatts", UnitFamilyName.Power, 0.001),           // 1 mW = 0.001 W
        UnitDefinition.LinearUnit("kW", "kilowatts", UnitFamilyName.Power, 1000.0),           // 1 kW = 1000 W
        UnitDefinition.LinearUnit("MW", "megawatts", UnitFamilyName.Power, 1000000.0),        // 1 MW = 1,000,000 W
        UnitDefinition.LinearUnit("hp", "horsepower", UnitFamilyName.Power, 745.7),           // 1 hp = 745.7 W

        // Energy units (joules as base) - using enhanced approach with UnitFamilyName enum!
        UnitDefinition.BaseUnit("J", "joules", UnitFamilyName.Energy),
        UnitDefinition.LinearUnit("kJ", "kilojoules", UnitFamilyName.Energy, 1000.0),         // 1 kJ = 1000 J
        UnitDefinition.LinearUnit("MJ", "megajoules", UnitFamilyName.Energy, 1000000.0),      // 1 MJ = 1,000,000 J
        UnitDefinition.LinearUnit("kWh", "kilowatt-hours", UnitFamilyName.Energy, 3600000.0), // 1 kWh = 3,600,000 J
        UnitDefinition.LinearUnit("cal", "calories", UnitFamilyName.Energy, 4.184),           // 1 cal = 4.184 J
        UnitDefinition.LinearUnit("kcal", "kilocalories", UnitFamilyName.Energy, 4184.0),     // 1 kcal = 4184 J
        UnitDefinition.LinearUnit("BTU", "British thermal units", UnitFamilyName.Energy, 1055.06), // 1 BTU = 1055.06 J

        // Resistance units (ohms as base) - using enhanced approach with UnitFamilyName enum!
        UnitDefinition.BaseUnit("Ω", "ohms", UnitFamilyName.Resistance),
        UnitDefinition.LinearUnit("mΩ", "milliohms", UnitFamilyName.Resistance, 0.001),       // 1 mΩ = 0.001 Ω
        UnitDefinition.LinearUnit("kΩ", "kilohms", UnitFamilyName.Resistance, 1000.0),        // 1 kΩ = 1000 Ω
        UnitDefinition.LinearUnit("MΩ", "megohms", UnitFamilyName.Resistance, 1000000.0),     // 1 MΩ = 1,000,000 Ω

        // Capacitance units (farads as base) - using enhanced approach with UnitFamilyName enum!
        UnitDefinition.BaseUnit("F", "farads", UnitFamilyName.Capacitance),
        UnitDefinition.LinearUnit("μF", "microfarads", UnitFamilyName.Capacitance, 0.000001), // 1 μF = 0.000001 F
        UnitDefinition.LinearUnit("nF", "nanofarads", UnitFamilyName.Capacitance, 0.000000001), // 1 nF = 0.000000001 F
        UnitDefinition.LinearUnit("pF", "picofarads", UnitFamilyName.Capacitance, 0.000000000001), // 1 pF = 0.000000000001 F
        UnitDefinition.LinearUnit("mF", "millifarads", UnitFamilyName.Capacitance, 0.001),    // 1 mF = 0.001 F

        // Percent units (percent as base) - using enhanced approach with UnitFamilyName enum!
        UnitDefinition.BaseUnit("%", "percent", UnitFamilyName.Percent),
        UnitDefinition.LinearUnit("ratio", "ratio", UnitFamilyName.Percent, 0.01),            // 1 ratio = 0.01 %
        UnitDefinition.LinearUnit("ppm", "parts per million", UnitFamilyName.Percent, 0.0001), // 1 ppm = 0.0001 %
        UnitDefinition.LinearUnit("ppb", "parts per billion", UnitFamilyName.Percent, 0.0000001), // 1 ppb = 0.0000001 %

        // Frequency units (hertz as base) - using enhanced approach with UnitFamilyName enum!
        UnitDefinition.BaseUnit("Hz", "hertz", UnitFamilyName.Frequency),
        UnitDefinition.LinearUnit("kHz", "kilohertz", UnitFamilyName.Frequency, 1000.0),      // 1 kHz = 1000 Hz
        UnitDefinition.LinearUnit("MHz", "megahertz", UnitFamilyName.Frequency, 1000000.0),   // 1 MHz = 1,000,000 Hz
        UnitDefinition.LinearUnit("GHz", "gigahertz", UnitFamilyName.Frequency, 1000000000.0), // 1 GHz = 1,000,000,000 Hz
        UnitDefinition.LinearUnit("rpm", "revolutions per minute", UnitFamilyName.Frequency, 1.0 / 60.0), // 1 rpm = 1/60 Hz
        UnitDefinition.LinearUnit("rps", "revolutions per second", UnitFamilyName.Frequency, 1.0)  // 1 rps = 1 Hz
    };

}