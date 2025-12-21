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
}
