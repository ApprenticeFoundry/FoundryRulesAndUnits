namespace FoundryRulesAndUnits.Units;

public enum UnitSystemType
{
    IPS,
    FPS,
    MKS,
    CGS,
    mmNs,
    SI
}
public enum UnitFamilyName
{
    None,
    Length,
    Distance,
    Time,
    Duration,
    Mass,
    Angle,
    Bearing,
    Quantity,
    QuantityFlow,
    Area,
    Volume,
    Speed,
    Temperature,
    Pressure,
    Force,
    Stiffness,   // N/m — spring constants (Force ÷ Length)
    Damping,     // Ns/m — damping coefficients (Force ÷ Speed)
    DataStorage,
    DataFlow,
    WorkTime,
    Voltage,
    Current,
    Power,
    Energy,
    Momentum,
    Resistance,
    Capacitance,
    Percent,
    Frequency,

    // Additional unit families required by unit system specifications
    AbsorbedDose,
    AmountOfSubstance,
    Conductance,
    ElectricCharge,
    EquivalentDose,
    Illuminance,
    Inductance,
    LuminousFlux,
    LuminousIntensity,
    MagneticFlux,
    MagneticFluxDensity,
    Radioactivity,

    // Currency and cost-per-unit families (Phase 1 - System-Independent)
    Currency,
    CostPerQuantity,
    CostPerTime,
    CostPerMass,
    CostPerLength,
    CostPerArea,
    CostPerVolume,
    CostPerEnergy,
    
    // Mechanical engineering families (Phase 4 - Unicode Support)
    Torque,
    Inertia,
    AngularVelocity,
    AngularAcceleration,
    Acceleration,

    // Material properties — added 2026-08-30 (FoundryFrameworkLab, the unit-honest hydraulic
    // cylinder: steel at 7.85 g/cm³, part volume × density → mass). Coherent: M·L⁻³.
    Density,

    // Three-valued logic - system-independent verdict scale (-1 False, 0 Undecided, 1 True)
    Verdict,

    // Utility-scale power plants — added 2026-10-20 (FoundrySolar). Both are NOMINAL-ONLY (absent from FamilySignatures):
    // kVA is not kW, and that distinction is purely semantic (like Bq vs Hz), so the two never add or auto-promote.
    // Appended at the end so no existing enum value moves.
    ApparentPower,   // VA — inverter and transformer ratings
    Irradiation,     // J/m² — solar energy per area over a period (plane-of-array kWh/m²)

    // Solar flux — added 2026-10-20 (FoundrySolar: the 1,000 W/m² STC irradiance was being written as `/ 1000 W` and `/ 1 m2`).
    // Unlike the two above this one HAS a dimensional signature (power per area), so area × irradiance composes into power.
    Irradiance,      // W/m² — solar power per area

    // A rate per kelvin — added 2026-10-21 (FoundrySolar: a module's %/K temperature coefficients multiplied a temperature difference
    // that the model had to divide by `1 K` first). A signature of temperature⁻¹, so a coefficient times a temperature difference closes.
    InverseTemperature,   // 1/K — a temperature coefficient

    // Money per unit of power — added 2026-10-21 (FoundrySolar: installed cost per megawatt was `secCost / (acNameplate / 1 MW)`, a unit
    // conversion typed into a law). Signature: currency per power, so cost ÷ power closes to it without a literal.
    CostPerPower,         // USD/W, USD/kW, USD/MW — installed cost

    // Quantities per unit of power — added 2026-10-21 (FoundrySolar: land, row length and piles per megawatt were `x / (arrayDc / 1 MW)`).
    // Each has a signature that is the numerator over power, so the quotient closes without a literal.
    AreaPerPower,         // m2/W, acre/MW — land per capacity
    LengthPerPower,       // m/W, m/MW — row length per capacity
    QuantityPerPower,     // ea/W, ea/MW — a count per capacity (piles, modules)
}
