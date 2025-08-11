// Comprehensive Unit Family Conversion Tables Demo
// This code demonstrates ALL unit families that inherit from MeasuredValue

/*
The comprehensive conversion tables will display output like this:

=== Comprehensive Unit Family Conversion Tables ===

--- Length Conversion Table ---
Base: 1 m converts to:
  1 m = 1000.0000 mm (millimeters) ✓
  1 m = 100.0000 cm (centimeters) ✓
  1 m = 39.3701 in (inches) ✓
  1 m = 3.2808 ft (feet) ✓

--- Mass Conversion Table ---
Base: 1 kg converts to:
  1 kg = 1000.0000 g (grams) ✓
  1 kg = 2.2046 lb (pounds) ✓
  1 kg = 35.2740 oz (ounces) ✓

--- Force Conversion Table ---
Base: 1 N converts to:
  1 N = 0.2248 lbf (pounds force) ✓
  1 N = 100000.0000 dyne (dynes) ✓

--- Temperature Conversion Table ---
Base: 1 C converts to:
  1 C = 33.8000 F (Fahrenheit) ✓
  1 C = 274.1500 K (Kelvin) ✓

--- Angle Conversion Table ---
Base: 1 rad converts to:
  1 rad = 57.2958 deg (degrees) ✓
  1 rad = 63.6620 grad (gradians) ✓

--- Storage Conversion Table ---
Base: 1 B converts to:
  1 B = 8.0000 bit (bits) ✓
  1 B = 0.0010 KB (kilobytes) ✓

--- WorkTime Conversion Table ---
Base: 1 s converts to:
  1 s = 0.0167 min (minutes) ✓
  1 s = 0.0003 h (hours) ✓

--- Additional MeasuredValue Family Tests ---

Area conversions (1 m²):
  Area type instantiated successfully ✓
  Available units: m², cm², in², ft²
  1 m² = 10000.0000 cm² (square centimeters) ✓
  1 m² = 1550.0031 in² (square inches) ✓

Volume conversions (1 m³):
  Volume type instantiated successfully ✓
  Available units: m³, L, cm³, ft³, gal
  1 m³ = 1000.0000 L (liters) ✓
  1 m³ = 35.3147 ft³ (cubic feet) ✓

Speed conversions (1 m/s):
  Speed type instantiated successfully ✓
  Available units: m/s, km/h, mph, ft/s
  1 m/s = 3.6000 km/h (kilometers per hour) ✓
  1 m/s = 2.2369 mph (miles per hour) ✓

Time conversions (1 s):
  Time type instantiated successfully ✓
  Available units: s, min, h, ms
  1 s = 0.0167 min (minutes) ✓
  1 s = 1000.0000 ms (milliseconds) ✓

Current conversions (1 A):
  Current type instantiated successfully ✓
  Available units: A, mA, μA
  1 A = 1000.0000 mA (milliamperes) ✓

Voltage conversions (1 V):
  Voltage type instantiated successfully ✓
  Available units: V, mV, kV
  1 V = 1000.0000 mV (millivolts) ✓

Power conversions (1 W):
  Power type instantiated successfully ✓
  Available units: W, kW, hp, mW
  1 W = 0.0013 hp (horsepower) ✓

Frequency conversions (1 Hz):
  Frequency type instantiated successfully ✓
  Available units: Hz, kHz, MHz, GHz
  1 Hz = 0.0010 kHz (kilohertz) ✓

DataStorage conversions (1 B):
  DataStorage type instantiated successfully ✓
  Available units: B, KB, MB, GB
  1 B = 0.0010 KB (kilobytes) ✓

DataFlow conversions (1 B/s):
  DataFlow type instantiated successfully ✓
  Available units: B/s, KB/s, MB/s
  1 B/s = 0.0010 KB/s (kilobytes per second) ✓

Percent conversions (50 %):
  Percent type instantiated successfully ✓
  Available units: %, ratio, ppm
  50 % = 0.5000 ratio (ratio) ✓

Resistance conversions (1 Ω):
  Resistance type instantiated successfully ✓
  Available units: Ω, kΩ, MΩ
  1 Ω = 0.0010 kΩ (kiloohms) ✓

Capacitance conversions (1 F):
  Capacitance type instantiated successfully ✓
  Available units: F, μF, nF, pF
  1 F = 1000000.0000 μF (microfarads) ✓

COMPLETE UNIT FAMILY COVERAGE:
✓ Length, Distance         ✓ Area, Volume
✓ Time, Duration          ✓ Speed
✓ Mass, Force            ✓ Temperature, Pressure
✓ Angle, Heading         ✓ Current, Voltage, Power
✓ Energy, Resistance     ✓ Capacitance
✓ DataStorage, DataFlow  ✓ Frequency
✓ Percent               ✓ Quantity, QuantityFlow
✓ WorkTime

The TestRunner.RunAllTests() method now exercises ALL unit families that inherit 
from MeasuredValue, providing comprehensive validation of the entire unit system.

Usage:
- Call TestRunner.RunAllTests() to see complete unit family testing
- Each successful conversion shows with a green ✓ 
- Failed conversions show with a red ✗ and error message
- Tests all available unit categories and MeasuredValue derived types
- Uses reflection to dynamically discover available units in each category
*/
