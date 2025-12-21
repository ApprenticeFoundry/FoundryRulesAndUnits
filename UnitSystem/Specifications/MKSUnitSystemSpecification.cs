using System;
using System.Collections.Generic;
using FoundryRulesAndUnits.Units;
using FoundryRulesAndUnits.Units.Specifications;

namespace FoundryRulesAndUnits.Units;

    /// <summary>
    /// MKS (Meter-Kilogram-Second) Unit System Specification
    /// Base units: meters, kilograms, seconds, Celsius, radians, newtons
    /// Uses base unit hub architecture - all conversions go through the base unit
    /// </summary>
public class MKSUnitSystemSpecification : UnitSystemSpecificationBase
{
    public override string SystemName => "MKS";

    public override string SystemDescription => "MKS (Meter-Kilogram-Second) Unit System with base units: meters, kilograms, seconds, Celsius, radians, newtons";

    public override UnitSystemType SystemType => UnitSystemType.MKS;

    public override IReadOnlyList<UnitDefinition> UnitDefinitions { get; } = new List<UnitDefinition>
    {
        // Length units (meters as base) - All-scale measurements
        UnitDefinition.BaseUnit("m", "meters", UnitFamilyName.Length),
        UnitDefinition.LinearUnit("km", "kilometers", UnitFamilyName.Length, 1000.0),        // 1 km = 1000 m
        UnitDefinition.LinearUnit("dm", "decimeters", UnitFamilyName.Length, 0.1),           // 1 dm = 0.1 m  
        UnitDefinition.LinearUnit("cm", "centimeters", UnitFamilyName.Length, 0.01),         // 1 cm = 0.01 m
        UnitDefinition.LinearUnit("mm", "millimeters", UnitFamilyName.Length, 0.001),        // 1 mm = 0.001 m
        UnitDefinition.LinearUnit("μm", "micrometers", UnitFamilyName.Length, 0.000001),     // 1 μm = 1e-6 m
        UnitDefinition.LinearUnit("um", "micrometers", UnitFamilyName.Length, 0.000001),     // 1 um = 1e-6 m (ASCII alternative)
        UnitDefinition.LinearUnit("nm", "nanometers", UnitFamilyName.Length, 0.000000001),   // 1 nm = 1e-9 m
        UnitDefinition.LinearUnit("Å", "angstroms", UnitFamilyName.Length, 0.0000000001),    // 1 Å = 1e-10 m
        UnitDefinition.LinearUnit("in", "inches", UnitFamilyName.Length, 0.0254),            // 1 in = 0.0254 m
        UnitDefinition.LinearUnit("ft", "feet", UnitFamilyName.Length, 0.3048),              // 1 ft = 0.3048 m
        UnitDefinition.LinearUnit("yd", "yards", UnitFamilyName.Length, 0.9144),             // 1 yd = 0.9144 m
        UnitDefinition.LinearUnit("px", "pixels", UnitFamilyName.Length, 1.0 / 96.0 * 0.0254), // 96 DPI

        // Mass units (kilograms as base) - All-scale measurements
        UnitDefinition.BaseUnit("kg", "kilograms", UnitFamilyName.Mass),
        UnitDefinition.LinearUnit("t", "metric tons", UnitFamilyName.Mass, 1000.0),          // 1 t = 1000 kg
        UnitDefinition.LinearUnit("g", "grams", UnitFamilyName.Mass, 0.001),                  // 1 g = 0.001 kg
        UnitDefinition.LinearUnit("mg", "milligrams", UnitFamilyName.Mass, 0.000001),        // 1 mg = 0.000001 kg
        UnitDefinition.LinearUnit("lb", "pounds", UnitFamilyName.Mass, 0.453592),            // 1 lb = 0.453592 kg
        UnitDefinition.LinearUnit("oz", "ounces", UnitFamilyName.Mass, 0.0283495),           // 1 oz = 0.0283495 kg

        // Force units (newtons as base) - All-scale measurements
        UnitDefinition.BaseUnit("N", "newtons", UnitFamilyName.Force),
        UnitDefinition.LinearUnit("kN", "kilonewtons", UnitFamilyName.Force, 1000.0),        // 1 kN = 1000 N
        UnitDefinition.LinearUnit("MN", "meganewtons", UnitFamilyName.Force, 1000000.0),     // 1 MN = 1,000,000 N
        UnitDefinition.LinearUnit("mN", "millinewtons", UnitFamilyName.Force, 0.001),        // 1 mN = 0.001 N
        UnitDefinition.LinearUnit("μN", "micronewtons", UnitFamilyName.Force, 0.000001),     // 1 μN = 0.000001 N
        UnitDefinition.LinearUnit("uN", "micronewtons", UnitFamilyName.Force, 0.000001),     // 1 uN = 0.000001 N (ASCII alternative)
        UnitDefinition.LinearUnit("dyne", "dynes", UnitFamilyName.Force, 0.00001),           // 1 dyne = 0.00001 N
        UnitDefinition.LinearUnit("lbf", "pounds-force", UnitFamilyName.Force, 4.44822),     // 1 lbf = 4.44822 N
        UnitDefinition.LinearUnit("lbs", "pounds-force", UnitFamilyName.Force, 4.44822),     // 1 lbs = 1 lbf
        UnitDefinition.LinearUnit("ozf", "ounces-force", UnitFamilyName.Force, 0.278014),    // 1 ozf = 0.278014 N
        UnitDefinition.LinearUnit("kgf", "kilograms-force", UnitFamilyName.Force, 9.80665),  // 1 kgf = 9.80665 N

        // Temperature units (Kelvin as base for absolute temperature scale) - using enhanced approach with UnitFamilyName enum!
        UnitDefinition.BaseUnit("K", "Kelvin", UnitFamilyName.Temperature),
        UnitDefinition.DerivedUnit("C", "Celsius", UnitFamilyName.Temperature,
            c => c + 273.15,          // C to K: add 273.15
            k => k - 273.15),         // K to C: subtract 273.15
        UnitDefinition.DerivedUnit("F", "Fahrenheit", UnitFamilyName.Temperature,
            f => (f - 32.0) * 5.0 / 9.0 + 273.15,    // F to K: (F-32)*5/9 + 273.15
            k => (k - 273.15) * 9.0 / 5.0 + 32.0),   // K to F: (K-273.15)*9/5 + 32

        // Angle units (radians as base) - All-scale measurements
        UnitDefinition.BaseUnit("rad", "radians", UnitFamilyName.Angle),
        UnitDefinition.LinearUnit("deg", "degrees", UnitFamilyName.Angle, Math.PI / 180.0),  // 1 deg = π/180 rad
        UnitDefinition.LinearUnit("mrad", "milliradians", UnitFamilyName.Angle, 0.001),     // 1 mrad = 0.001 rad
        UnitDefinition.LinearUnit("grad", "gradians", UnitFamilyName.Angle, Math.PI / 200.0), // 1 grad = π/200 rad
        UnitDefinition.LinearUnit("turn", "turns", UnitFamilyName.Angle, 2.0 * Math.PI),    // 1 turn = 2π rad
        UnitDefinition.LinearUnit("°", "degrees", UnitFamilyName.Angle, Math.PI / 180.0),   // 1° = π/180 rad

        // Time units (seconds as base) - All-scale measurements
        UnitDefinition.BaseUnit("s", "seconds", UnitFamilyName.Time),
        UnitDefinition.LinearUnit("ms", "milliseconds", UnitFamilyName.Time, 0.001),          // 1 ms = 0.001 s
        UnitDefinition.LinearUnit("μs", "microseconds", UnitFamilyName.Time, 0.000001),       // 1 μs = 1e-6 s
        UnitDefinition.LinearUnit("us", "microseconds", UnitFamilyName.Time, 0.000001),       // 1 us = 1e-6 s (ASCII alternative)
        UnitDefinition.LinearUnit("min", "minutes", UnitFamilyName.Time, 60.0),              // 1 min = 60 s
        UnitDefinition.LinearUnit("hr", "hours", UnitFamilyName.Time, 3600.0),                // 1 hr = 3600 s
        UnitDefinition.LinearUnit("day", "days", UnitFamilyName.Time, 86400.0),              // 1 day = 86400 s

        // Area units (square meters as base) - All-scale measurements
        UnitDefinition.BaseUnit("m2", "square meters", UnitFamilyName.Area),
        UnitDefinition.LinearUnit("cm2", "square centimeters", UnitFamilyName.Area, 0.0001), // 1 cm² = 0.0001 m²
        UnitDefinition.LinearUnit("mm2", "square millimeters", UnitFamilyName.Area, 0.000001), // 1 mm² = 0.000001 m²
        UnitDefinition.LinearUnit("km2", "square kilometers", UnitFamilyName.Area, 1000000.0), // 1 km² = 1,000,000 m²
        UnitDefinition.LinearUnit("ft2", "square feet", UnitFamilyName.Area, 0.092903),       // 1 ft² = 0.092903 m²
        UnitDefinition.LinearUnit("in2", "square inches", UnitFamilyName.Area, 0.00064516),   // 1 in² = 0.00064516 m²
        UnitDefinition.LinearUnit("yd2", "square yards", UnitFamilyName.Area, 0.836127),      // 1 yd² = 0.836127 m²
        UnitDefinition.LinearUnit("acre", "acres", UnitFamilyName.Area, 4046.86),             // 1 acre = 4046.86 m²
        UnitDefinition.LinearUnit("ha", "hectares", UnitFamilyName.Area, 10000.0),            // 1 ha = 10,000 m²
        UnitDefinition.LinearUnit("sqft", "square feet", UnitFamilyName.Area, 0.092903),      // 1 sqft = 1 ft²

        // Volume units (cubic meters as base) - All-scale measurements
        UnitDefinition.BaseUnit("m3", "cubic meters", UnitFamilyName.Volume),
        UnitDefinition.LinearUnit("cm3", "cubic centimeters", UnitFamilyName.Volume, 0.000001), // 1 cm³ = 0.000001 m³
        UnitDefinition.LinearUnit("mm3", "cubic millimeters", UnitFamilyName.Volume, 0.000000001), // 1 mm³ = 1e-9 m³
        UnitDefinition.LinearUnit("km3", "cubic kilometers", UnitFamilyName.Volume, 1000000000.0), // 1 km³ = 1e9 m³
        UnitDefinition.LinearUnit("L", "liters", UnitFamilyName.Volume, 0.001),                 // 1 L = 0.001 m³
        UnitDefinition.LinearUnit("mL", "milliliters", UnitFamilyName.Volume, 0.000001),        // 1 mL = 1e-6 m³
        UnitDefinition.LinearUnit("cc", "cubic centimeters", UnitFamilyName.Volume, 0.000001),  // 1 cc = 1 cm³
        UnitDefinition.LinearUnit("ft3", "cubic feet", UnitFamilyName.Volume, 0.0283168),       // 1 ft³ = 0.0283168 m³
        UnitDefinition.LinearUnit("in3", "cubic inches", UnitFamilyName.Volume, 0.000016387),   // 1 in³ = 1.6387e-5 m³
        UnitDefinition.LinearUnit("gal", "gallons", UnitFamilyName.Volume, 0.00378541),         // 1 gal (US) = 0.00378541 m³
        UnitDefinition.LinearUnit("qt", "quarts", UnitFamilyName.Volume, 0.000946353),          // 1 qt = 0.000946353 m³
        UnitDefinition.LinearUnit("pt", "pints", UnitFamilyName.Volume, 0.000473176),           // 1 pt = 0.000473176 m³
        UnitDefinition.LinearUnit("cup", "cups", UnitFamilyName.Volume, 0.000236588),           // 1 cup = 0.000236588 m³
        UnitDefinition.LinearUnit("fl oz", "fluid ounces", UnitFamilyName.Volume, 0.0000295735), // 1 fl oz = 2.95735e-5 m³

        // Speed units (meters per second as base) - using enhanced approach with UnitFamilyName enum!
        UnitDefinition.BaseUnit("m/s", "meters per second", UnitFamilyName.Speed),
        UnitDefinition.LinearUnit("km/h", "kilometers per hour", UnitFamilyName.Speed, 1000.0 / 3600.0), // 1 km/h = 1000/3600 m/s
        UnitDefinition.LinearUnit("kph", "kilometers per hour", UnitFamilyName.Speed, 1000.0 / 3600.0), // 1 kph = 1000/3600 m/s (ASCII alternative)
        UnitDefinition.LinearUnit("mph", "miles per hour", UnitFamilyName.Speed, 0.44704),              // 1 mph = 0.44704 m/s
        UnitDefinition.LinearUnit("ft/s", "feet per second", UnitFamilyName.Speed, 0.3048),           // 1 ft/s = 0.3048 m/s
        UnitDefinition.LinearUnit("knot", "knots", UnitFamilyName.Speed, 0.514444),                    // 1 knot = 0.514444 m/s

        // Distance units (kilometers as base) - Large-scale measurements with dual family support
        UnitDefinition.BaseUnit("km", "kilometers", UnitFamilyName.Distance),
        // Dual family support: Accept all Length units but convert to km base
        UnitDefinition.LinearUnit("m", "meters", UnitFamilyName.Distance, 0.001),               // 1 m = 0.001 km
        UnitDefinition.LinearUnit("dm", "decimeters", UnitFamilyName.Distance, 0.0001),         // 1 dm = 0.0001 km
        UnitDefinition.LinearUnit("cm", "centimeters", UnitFamilyName.Distance, 0.00001),       // 1 cm = 0.00001 km
        UnitDefinition.LinearUnit("mm", "millimeters", UnitFamilyName.Distance, 0.000001),      // 1 mm = 0.000001 km
        UnitDefinition.LinearUnit("μm", "micrometers", UnitFamilyName.Distance, 0.000000001),   // 1 μm = 1e-9 km
        UnitDefinition.LinearUnit("um", "micrometers", UnitFamilyName.Distance, 0.000000001),   // 1 um = 1e-9 km (ASCII alternative)
        UnitDefinition.LinearUnit("nm", "nanometers", UnitFamilyName.Distance, 0.000000000001), // 1 nm = 1e-12 km
        UnitDefinition.LinearUnit("Å", "angstroms", UnitFamilyName.Distance, 0.0000000000001),  // 1 Å = 1e-13 km
        UnitDefinition.LinearUnit("in", "inches", UnitFamilyName.Distance, 0.0000254),          // 1 in = 0.0000254 km
        UnitDefinition.LinearUnit("ft", "feet", UnitFamilyName.Distance, 0.0003048),            // 1 ft = 0.0003048 km
        UnitDefinition.LinearUnit("yd", "yards", UnitFamilyName.Distance, 0.0009144),           // 1 yd = 0.0009144 km
        UnitDefinition.LinearUnit("px", "pixels", UnitFamilyName.Distance, 1.0 / 96.0 * 0.0254 / 1000.0), // pixels to km
        // Traditional distance units
        UnitDefinition.LinearUnit("mi", "miles", UnitFamilyName.Distance, 1.609344),             // 1 mi = 1.609344 km
        UnitDefinition.LinearUnit("nmi", "nautical miles", UnitFamilyName.Distance, 1.852),      // 1 nmi = 1.852 km
        UnitDefinition.LinearUnit("ly", "light years", UnitFamilyName.Distance, 9.461e12),       // 1 ly = 9.461e12 km
        UnitDefinition.LinearUnit("au", "astronomical units", UnitFamilyName.Distance, 149597870.7), // 1 au = 149,597,870.7 km
        UnitDefinition.LinearUnit("pc", "parsecs", UnitFamilyName.Distance, 3.086e13),           // 1 pc = 3.086e13 km

        // Duration units (seconds as base) - Dual family support with Time
        UnitDefinition.BaseUnit("s", "seconds", UnitFamilyName.Duration),
        // Dual family support: Accept all Time units plus extended duration units
        UnitDefinition.LinearUnit("ms", "milliseconds", UnitFamilyName.Duration, 0.001),        // 1 ms = 0.001 s
        UnitDefinition.LinearUnit("μs", "microseconds", UnitFamilyName.Duration, 0.000001),     // 1 μs = 1e-6 s
        UnitDefinition.LinearUnit("us", "microseconds", UnitFamilyName.Duration, 0.000001),     // 1 us = 1e-6 s (ASCII alternative)
        UnitDefinition.LinearUnit("min", "minutes", UnitFamilyName.Duration, 60.0),            // 1 min = 60 s
        UnitDefinition.LinearUnit("hr", "hours", UnitFamilyName.Duration, 3600.0),             // 1 hr = 3600 s
        UnitDefinition.LinearUnit("day", "days", UnitFamilyName.Duration, 86400.0),            // 1 day = 86400 s
        UnitDefinition.LinearUnit("week", "weeks", UnitFamilyName.Duration, 604800.0),         // 1 week = 604800 s
        UnitDefinition.LinearUnit("month", "months", UnitFamilyName.Duration, 2629746.0),      // 1 month ≈ 30.44 days
        UnitDefinition.LinearUnit("year", "years", UnitFamilyName.Duration, 31556952.0),       // 1 year = 365.2425 days

        // Bearing units (degrees as base) - Dual family support with Angle
        UnitDefinition.BaseUnit("deg", "degrees", UnitFamilyName.Bearing),
        // Dual family support: Accept all Angle units but convert to deg base
        UnitDefinition.LinearUnit("rad", "radians", UnitFamilyName.Bearing, 180.0 / Math.PI),  // 1 rad = 180/π deg
        UnitDefinition.LinearUnit("mrad", "milliradians", UnitFamilyName.Bearing, 180.0 / Math.PI * 0.001), // 1 mrad to deg
        UnitDefinition.LinearUnit("grad", "gradians", UnitFamilyName.Bearing, 0.9),            // 1 grad = 0.9 deg
        UnitDefinition.LinearUnit("turn", "turns", UnitFamilyName.Bearing, 360.0),             // 1 turn = 360 deg
        UnitDefinition.LinearUnit("°", "degrees", UnitFamilyName.Bearing, 1.0),                // 1° = 1 deg
        UnitDefinition.LinearUnit("mil", "mils", UnitFamilyName.Bearing, 0.05625),             // 1 mil = 0.05625 deg

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
        UnitDefinition.LinearUnit("uA", "microamperes", UnitFamilyName.Current, 0.000001),    // 1 uA = 0.000001 A (ASCII alternative)
        UnitDefinition.LinearUnit("kA", "kiloamperes", UnitFamilyName.Current, 1000.0),       // 1 kA = 1000 A

        // Currency units (USD as base) - System-independent, identical in all unit systems
        // Exchange rates as of December 2025 (approximate, should be updated periodically)
        UnitDefinition.BaseUnit("USD", "US Dollars", UnitFamilyName.Currency),
        UnitDefinition.LinearUnit("EUR", "Euros", UnitFamilyName.Currency, 0.92),           // 1 EUR = 0.92 USD
        UnitDefinition.LinearUnit("GBP", "British Pounds", UnitFamilyName.Currency, 0.79),  // 1 GBP = 0.79 USD
        UnitDefinition.LinearUnit("JPY", "Japanese Yen", UnitFamilyName.Currency, 145.0),   // 1 JPY = 145 USD
        UnitDefinition.LinearUnit("CNY", "Chinese Yuan", UnitFamilyName.Currency, 7.2),     // 1 CNY = 7.2 USD
        UnitDefinition.LinearUnit("CAD", "Canadian Dollars", UnitFamilyName.Currency, 1.36), // 1 CAD = 1.36 USD
        UnitDefinition.LinearUnit("AUD", "Australian Dollars", UnitFamilyName.Currency, 1.55), // 1 AUD = 1.55 USD
        UnitDefinition.LinearUnit("CHF", "Swiss Francs", UnitFamilyName.Currency, 0.88),    // 1 CHF = 0.88 USD
        UnitDefinition.LinearUnit("INR", "Indian Rupees", UnitFamilyName.Currency, 83.0),   // 1 INR = 83 USD
        UnitDefinition.LinearUnit("MXN", "Mexican Pesos", UnitFamilyName.Currency, 17.0),   // 1 MXN = 17 USD
        UnitDefinition.LinearUnit("BRL", "Brazilian Reais", UnitFamilyName.Currency, 5.0),  // 1 BRL = 5 USD
        UnitDefinition.LinearUnit("KRW", "South Korean Won", UnitFamilyName.Currency, 1320.0), // 1 KRW = 1320 USD
        UnitDefinition.LinearUnit("SGD", "Singapore Dollars", UnitFamilyName.Currency, 1.34), // 1 SGD = 1.34 USD
        UnitDefinition.LinearUnit("HKD", "Hong Kong Dollars", UnitFamilyName.Currency, 7.8), // 1 HKD = 7.8 USD
        UnitDefinition.LinearUnit("cent", "cents", UnitFamilyName.Currency, 0.01),          // 1 cent = 0.01 USD

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
        UnitDefinition.LinearUnit("uF", "microfarads", UnitFamilyName.Capacitance, 0.000001), // 1 uF = 0.000001 F (ASCII alternative)
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
        UnitDefinition.LinearUnit("rps", "revolutions per second", UnitFamilyName.Frequency, 1.0),  // 1 rps = 1 Hz

        // Momentum units (kg⋅m/s as base) - using enhanced approach with UnitFamilyName enum!
        UnitDefinition.BaseUnit("kg⋅m/s", "kilogram-meters per second", UnitFamilyName.Momentum),
        UnitDefinition.LinearUnit("kg*m/s", "kilogram-meters per second", UnitFamilyName.Momentum, 1.0), // ASCII alternative
        UnitDefinition.LinearUnit("g⋅m/s", "gram-meters per second", UnitFamilyName.Momentum, 0.001),     // 1 g⋅m/s = 0.001 kg⋅m/s
        UnitDefinition.LinearUnit("g*m/s", "gram-meters per second", UnitFamilyName.Momentum, 0.001),     // ASCII alternative
        UnitDefinition.LinearUnit("lb⋅ft/s", "pound-feet per second", UnitFamilyName.Momentum, 0.138255), // 1 lb⋅ft/s ≈ 0.138255 kg⋅m/s
        UnitDefinition.LinearUnit("slug⋅ft/s", "slug-feet per second", UnitFamilyName.Momentum, 4.44822),  // 1 slug⋅ft/s ≈ 4.44822 kg⋅m/s

        // Cost per quantity units (USD/unit as base) - System-independent pricing
        UnitDefinition.BaseUnit("USD/unit", "US Dollars per unit", UnitFamilyName.CostPerQuantity),
        UnitDefinition.LinearUnit("USD/ea", "US Dollars per each", UnitFamilyName.CostPerQuantity, 1.0),      // Alias for USD/unit
        UnitDefinition.LinearUnit("USD/piece", "US Dollars per piece", UnitFamilyName.CostPerQuantity, 1.0),  // Alias for USD/unit
        UnitDefinition.LinearUnit("USD/item", "US Dollars per item", UnitFamilyName.CostPerQuantity, 1.0),    // Alias for USD/unit
        UnitDefinition.LinearUnit("USD/dozen", "US Dollars per dozen", UnitFamilyName.CostPerQuantity, 1.0/12.0), // 1 USD/dozen = USD/unit ÷ 12
        UnitDefinition.LinearUnit("USD/hundred", "US Dollars per hundred", UnitFamilyName.CostPerQuantity, 0.01), // 1 USD/hundred = USD/unit ÷ 100
        UnitDefinition.LinearUnit("USD/thousand", "US Dollars per thousand", UnitFamilyName.CostPerQuantity, 0.001), // 1 USD/thousand = USD/unit ÷ 1000
        UnitDefinition.LinearUnit("EUR/unit", "Euros per unit", UnitFamilyName.CostPerQuantity, 0.92),        // 1 EUR/unit = 0.92 USD/unit
        UnitDefinition.LinearUnit("GBP/unit", "British Pounds per unit", UnitFamilyName.CostPerQuantity, 0.79), // 1 GBP/unit = 0.79 USD/unit
        UnitDefinition.LinearUnit("JPY/unit", "Japanese Yen per unit", UnitFamilyName.CostPerQuantity, 145.0), // 1 JPY/unit = 145 USD/unit
        UnitDefinition.LinearUnit("CAD/unit", "Canadian Dollars per unit", UnitFamilyName.CostPerQuantity, 1.36), // 1 CAD/unit = 1.36 USD/unit
        UnitDefinition.LinearUnit("cent/unit", "cents per unit", UnitFamilyName.CostPerQuantity, 0.01),        // 1 cent/unit = 0.01 USD/unit

        // Cost per time units (USD/hr as base) - System-independent labor/service rates
        UnitDefinition.BaseUnit("USD/hr", "US Dollars per hour", UnitFamilyName.CostPerTime),
        UnitDefinition.LinearUnit("USD/h", "US Dollars per hour", UnitFamilyName.CostPerTime, 1.0),          // Alias for USD/hr
        UnitDefinition.LinearUnit("USD/s", "US Dollars per second", UnitFamilyName.CostPerTime, 3600.0),     // 1 USD/s = 3600 USD/hr
        UnitDefinition.LinearUnit("USD/min", "US Dollars per minute", UnitFamilyName.CostPerTime, 60.0),     // 1 USD/min = 60 USD/hr
        UnitDefinition.LinearUnit("USD/day", "US Dollars per day", UnitFamilyName.CostPerTime, 1.0/24.0),    // 1 USD/day = USD/hr ÷ 24
        UnitDefinition.LinearUnit("USD/wk", "US Dollars per week", UnitFamilyName.CostPerTime, 1.0/168.0),   // 1 USD/wk = USD/hr ÷ 168
        UnitDefinition.LinearUnit("USD/mo", "US Dollars per month", UnitFamilyName.CostPerTime, 1.0/730.0),  // 1 USD/mo ≈ USD/hr ÷ 730 (30.4 days avg)
        UnitDefinition.LinearUnit("USD/yr", "US Dollars per year", UnitFamilyName.CostPerTime, 1.0/8760.0),  // 1 USD/yr = USD/hr ÷ 8760
        UnitDefinition.LinearUnit("EUR/hr", "Euros per hour", UnitFamilyName.CostPerTime, 0.92),            // 1 EUR/hr = 0.92 USD/hr
        UnitDefinition.LinearUnit("GBP/hr", "British Pounds per hour", UnitFamilyName.CostPerTime, 0.79),   // 1 GBP/hr = 0.79 USD/hr
        UnitDefinition.LinearUnit("JPY/hr", "Japanese Yen per hour", UnitFamilyName.CostPerTime, 145.0),    // 1 JPY/hr = 145 USD/hr
        UnitDefinition.LinearUnit("CAD/hr", "Canadian Dollars per hour", UnitFamilyName.CostPerTime, 1.36), // 1 CAD/hr = 1.36 USD/hr
        UnitDefinition.LinearUnit("cent/hr", "cents per hour", UnitFamilyName.CostPerTime, 0.01)           // 1 cent/hr = 0.01 USD/hr
    };

}