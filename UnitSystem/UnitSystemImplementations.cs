using System;
using System.Collections.Generic;
using FoundryRulesAndUnits.Units;

namespace FoundryRulesAndUnits.Units
{
    /// <summary>
    /// Base class for all unit system implementations
    /// </summary>
    public abstract class BaseUnitSystem
    {
        public abstract UnitSystemType SystemType { get; }
        public abstract string SystemName { get; }
        public abstract void Configure(UnitSystem unitSystem);
    }

    /// <summary>
    /// MKS (Meter-Kilogram-Second) unit system implementation
    /// </summary>
    public class MKSUnitSystem : BaseUnitSystem
    {
        public override UnitSystemType SystemType => UnitSystemType.MKS;
        public override string SystemName => "Meter-Kilogram-Second";

        public override void Configure(UnitSystem unitSystem)
        {
            // Length: METERS as base unit
            unitSystem.length = new UnitCategory("Length", new UnitSpec("m", "meters", UnitFamilyName.Length))
                .AddAllLengthUnits();

            // Mass: KILOGRAMS as base unit  
            unitSystem.mass = new UnitCategory("Mass", new UnitSpec("kg", "kilograms", UnitFamilyName.Mass))
                .AddMassUnits("kg");

            // Force: NEWTONS as base unit
            unitSystem.force = new UnitCategory("Force", new UnitSpec("N", "newtons", UnitFamilyName.Force))
                .AddForceUnits("N");

            // Temperature: CELSIUS as base unit
            unitSystem.temperature = new UnitCategory("Temperature", new UnitSpec("C", "Celsius", UnitFamilyName.Temperature))
                .AddTemperatureConversions();

            // Register categories and set up class associations
            RegisterCategories(unitSystem);
        }

        private void RegisterCategories(UnitSystem unitSystem)
        {
            unitSystem.UnitCategories.Category(unitSystem.length);
            unitSystem.UnitCategories.Category(unitSystem.mass);
            unitSystem.UnitCategories.Category(unitSystem.force);
            unitSystem.UnitCategories.Category(unitSystem.temperature);

            Length.Category = () => unitSystem.length;
            Mass.Category = () => unitSystem.mass;
            Force.Category = () => unitSystem.force;
            Temperature.Category = () => unitSystem.temperature;
        }
    }

    /// <summary>
    /// CGS (Centimeter-Gram-Second) unit system implementation
    /// </summary>
    public class CGSUnitSystem : BaseUnitSystem
    {
        public override UnitSystemType SystemType => UnitSystemType.CGS;
        public override string SystemName => "Centimeter-Gram-Second";

        public override void Configure(UnitSystem unitSystem)
        {
            // Length: CENTIMETERS as base unit
            unitSystem.length = new UnitCategory("Length", new UnitSpec("cm", "centimeters", UnitFamilyName.Length))
                .AddAllLengthUnits();

            // Mass: GRAMS as base unit
            unitSystem.mass = new UnitCategory("Mass", new UnitSpec("g", "grams", UnitFamilyName.Mass))
                .AddMassUnits("g");

            // Force: DYNES as base unit
            unitSystem.force = new UnitCategory("Force", new UnitSpec("dyne", "dynes", UnitFamilyName.Force))
                .AddForceUnits("dyne");

            // Temperature: CELSIUS as base unit
            unitSystem.temperature = new UnitCategory("Temperature", new UnitSpec("C", "Celsius", UnitFamilyName.Temperature))
                .AddTemperatureConversions();

            RegisterCategories(unitSystem);
        }

        private void RegisterCategories(UnitSystem unitSystem)
        {
            unitSystem.UnitCategories.Category(unitSystem.length);
            unitSystem.UnitCategories.Category(unitSystem.mass);
            unitSystem.UnitCategories.Category(unitSystem.force);
            unitSystem.UnitCategories.Category(unitSystem.temperature);

            Length.Category = () => unitSystem.length;
            Mass.Category = () => unitSystem.mass;
            Force.Category = () => unitSystem.force;
            Temperature.Category = () => unitSystem.temperature;
        }
    }

    /// <summary>
    /// IPS (Inch-Pound-Second) unit system implementation
    /// </summary>
    public class IPSUnitSystem : BaseUnitSystem
    {
        public override UnitSystemType SystemType => UnitSystemType.IPS;
        public override string SystemName => "Inch-Pound-Second";

        public override void Configure(UnitSystem unitSystem)
        {
            // Length: INCHES as base unit
            unitSystem.length = new UnitCategory("Length", new UnitSpec("in", "inches", UnitFamilyName.Length))
                .AddAllLengthUnits();

            // Mass: POUNDS as base unit
            unitSystem.mass = new UnitCategory("Mass", new UnitSpec("lb", "pounds", UnitFamilyName.Mass))
                .AddMassUnits("lb");

            // Force: POUND-FORCE as base unit
            unitSystem.force = new UnitCategory("Force", new UnitSpec("lbf", "pounds-force", UnitFamilyName.Force))
                .AddForceUnits("lbf");

            // Temperature: FAHRENHEIT as base unit
            unitSystem.temperature = new UnitCategory("Temperature", new UnitSpec("F", "Fahrenheit", UnitFamilyName.Temperature))
                .AddTemperatureConversions();

            RegisterCategories(unitSystem);
        }

        private void RegisterCategories(UnitSystem unitSystem)
        {
            unitSystem.UnitCategories.Category(unitSystem.length);
            unitSystem.UnitCategories.Category(unitSystem.mass);
            unitSystem.UnitCategories.Category(unitSystem.force);
            unitSystem.UnitCategories.Category(unitSystem.temperature);

            Length.Category = () => unitSystem.length;
            Mass.Category = () => unitSystem.mass;
            Force.Category = () => unitSystem.force;
            Temperature.Category = () => unitSystem.temperature;
        }
    }

    /// <summary>
    /// FPS (Foot-Pound-Second) unit system implementation
    /// </summary>
    public class FPSUnitSystem : BaseUnitSystem
    {
        public override UnitSystemType SystemType => UnitSystemType.FPS;
        public override string SystemName => "Foot-Pound-Second";

        public override void Configure(UnitSystem unitSystem)
        {
            // Length: FEET as base unit
            unitSystem.length = new UnitCategory("Length", new UnitSpec("ft", "feet", UnitFamilyName.Length))
                .AddAllLengthUnits();

            // Mass: POUNDS as base unit
            unitSystem.mass = new UnitCategory("Mass", new UnitSpec("lb", "pounds", UnitFamilyName.Mass))
                .AddMassUnits("lb");

            // Force: POUND-FORCE as base unit
            unitSystem.force = new UnitCategory("Force", new UnitSpec("lbf", "pounds-force", UnitFamilyName.Force))
                .AddForceUnits("lbf");

            // Temperature: FAHRENHEIT as base unit
            unitSystem.temperature = new UnitCategory("Temperature", new UnitSpec("F", "Fahrenheit", UnitFamilyName.Temperature))
                .AddTemperatureConversions();

            RegisterCategories(unitSystem);
        }

        private void RegisterCategories(UnitSystem unitSystem)
        {
            unitSystem.UnitCategories.Category(unitSystem.length);
            unitSystem.UnitCategories.Category(unitSystem.mass);
            unitSystem.UnitCategories.Category(unitSystem.force);
            unitSystem.UnitCategories.Category(unitSystem.temperature);

            Length.Category = () => unitSystem.length;
            Mass.Category = () => unitSystem.mass;
            Force.Category = () => unitSystem.force;
            Temperature.Category = () => unitSystem.temperature;
        }
    }

    /// <summary>
    /// mmNs (Millimeter-Newton-Second) unit system implementation
    /// </summary>
    public class MMNsUnitSystem : BaseUnitSystem
    {
        public override UnitSystemType SystemType => UnitSystemType.mmNs;
        public override string SystemName => "Millimeter-Newton-Second";

        public override void Configure(UnitSystem unitSystem)
        {
            // Length: MILLIMETERS as base unit
            unitSystem.length = new UnitCategory("Length", new UnitSpec("mm", "millimeters", UnitFamilyName.Length))
                .AddAllLengthUnits();

            // Mass: GRAMS as base unit
            unitSystem.mass = new UnitCategory("Mass", new UnitSpec("g", "grams", UnitFamilyName.Mass))
                .AddMassUnits("g");

            // Force: NEWTONS as base unit
            unitSystem.force = new UnitCategory("Force", new UnitSpec("N", "newtons", UnitFamilyName.Force))
                .AddForceUnits("N");

            // Temperature: CELSIUS as base unit
            unitSystem.temperature = new UnitCategory("Temperature", new UnitSpec("C", "Celsius", UnitFamilyName.Temperature))
                .AddTemperatureConversions();

            RegisterCategories(unitSystem);
        }

        private void RegisterCategories(UnitSystem unitSystem)
        {
            unitSystem.UnitCategories.Category(unitSystem.length);
            unitSystem.UnitCategories.Category(unitSystem.mass);
            unitSystem.UnitCategories.Category(unitSystem.force);
            unitSystem.UnitCategories.Category(unitSystem.temperature);

            Length.Category = () => unitSystem.length;
            Mass.Category = () => unitSystem.mass;
            Force.Category = () => unitSystem.force;
            Temperature.Category = () => unitSystem.temperature;
        }
    }
}