// UNIFIED LENGTH CATEGORY PATCH
// Replace the fragmented AddMetricLengthUnits + AddCrossSystemConversions approach
// with a single AddAllLengthUnits that contains ALL length units regardless of base unit

// BEFORE (MKS System):
// length = new UnitCategory("Length", new UnitSpec("m", "meters", UnitFamilyName.Length))
//     .AddMetricLengthUnits("m")        // mm, cm, km with exact conversions
//     .AddCrossSystemConversions()      // in, ft with high precision

// AFTER (MKS System):
// length = new UnitCategory("Length", new UnitSpec("m", "meters", UnitFamilyName.Length))
//     .AddAllLengthUnits()              // ALL length units (metric + imperial) with exact conversions

// BEFORE (IPS System):
// length = new UnitCategory("Length", new UnitSpec("in", "inches", UnitFamilyName.Length))
//     .AddImperialLengthUnits("in")     // ft, yd, mi with exact conversions
//     .AddCrossSystemConversions()      // mm, cm, m with exact definitions

// AFTER (IPS System):
// length = new UnitCategory("Length", new UnitSpec("in", "inches", UnitFamilyName.Length))
//     .AddAllLengthUnits()              // ALL length units (metric + imperial) with exact conversions

// BEFORE (FPS System):
// length = new UnitCategory("Length", new UnitSpec("ft", "feet", UnitFamilyName.Length))
//     .AddImperialLengthUnits("ft")     // in, yd, mi with exact conversions  
//     .AddCrossSystemConversions()      // m, cm with exact definitions

// AFTER (FPS System):
// length = new UnitCategory("Length", new UnitSpec("ft", "feet", UnitFamilyName.Length))
//     .AddAllLengthUnits()              // ALL length units (metric + imperial) with exact conversions

// This eliminates:
// 1. Category overwriting problem (each system creating new Length category)
// 2. Missing conversion errors (m|ft, m|in not found)
// 3. Confusing "cross system" concept - no crosses within a unit family!
// 4. Fragmented unit coverage based on initialization order

// Result: ONE unified Length category with complete unit coverage regardless of base unit choice
