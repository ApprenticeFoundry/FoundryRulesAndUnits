using FoundryRulesAndUnits.Units.Specifications;
using System;
using System.Collections.Generic;

namespace FoundryRulesAndUnits.Units
{
    /// <summary>
    /// Factory service for creating MeasuredValue instances with proper UnitGroup injection
    /// Eliminates global dependencies by providing a centralized factory pattern
    /// </summary>
    public class UnitFactory
    {
        private readonly Dictionary<UnitFamilyName, UnitGroup> _unitGroups;
        private readonly UnitSystemType _systemType;

        /// <summary>
        /// Create a factory for a specific unit system
        /// </summary>
        public UnitFactory(UnitSystemType systemType)
        {
            _systemType = systemType;
            
            // Get the appropriate unit system specification
            UnitSystemSpecificationBase specification = systemType switch
            {
                UnitSystemType.SI => new SIUnitSystemSpecification(),
                UnitSystemType.MKS => new MKSUnitSystemSpecification(),
                UnitSystemType.CGS => new CGSUnitSystemSpecification(),
                UnitSystemType.FPS => new FPSUnitSystemSpecification(),
                UnitSystemType.IPS => new IPSUnitSystemSpecification(),
                UnitSystemType.mmNs => new mmNsUnitSystemSpecification(),
                _ => new SIUnitSystemSpecification() // Default to SI
            };

            _unitGroups = specification.GetUnitGroups();
        }

        /// <summary>
        /// Create a factory from an existing unit system specification
        /// </summary>
        public UnitFactory(UnitSystemSpecificationBase specification)
        {
            _systemType = specification.SystemType;
            _unitGroups = specification.GetUnitGroups();
        }

        /// <summary>
        /// Get the unit system type this factory creates
        /// </summary>
        public UnitSystemType SystemType => _systemType;

        /// <summary>
        /// Get a specific UnitGroup from this factory
        /// </summary>
        public UnitGroup GetUnitGroup(UnitFamilyName family)
        {
            if (_unitGroups.TryGetValue(family, out var group))
                return group;
            
            throw new ArgumentException($"Unit family {family} not available in {_systemType} system");
        }

        // Factory methods for creating typed measurement instances

        /// <summary>
        /// Create a Length instance with injected UnitGroup
        /// </summary>
        public Length CreateLength(double value = 0, string? units = null)
        {
            var unitGroup = _unitGroups[UnitFamilyName.Length];
            var length = new Length(unitGroup);
            var defaultUnit = units ?? unitGroup.BaseUnit.Symbol;
            length.Init(value, defaultUnit);
            return length;
        }

        /// <summary>
        /// Create an Angle instance with injected UnitGroup
        /// </summary>
        public Angle CreateAngle(double value = 0, string? units = null)
        {
            var unitGroup = _unitGroups[UnitFamilyName.Angle];
            var angle = new Angle(unitGroup);
            var defaultUnit = units ?? unitGroup.BaseUnit.Symbol;
            angle.Init(value, defaultUnit);
            return angle;
        }

        /// <summary>
        /// Create a Temperature instance with injected UnitGroup
        /// </summary>
        public Temperature CreateTemperature(double value = 0, string? units = null)
        {
            var unitGroup = _unitGroups[UnitFamilyName.Temperature];
            var temperature = new Temperature(unitGroup);
            var defaultUnit = units ?? unitGroup.BaseUnit.Symbol;
            temperature.Init(value, defaultUnit);
            return temperature;
        }

        /// <summary>
        /// Create a Mass instance with injected UnitGroup
        /// </summary>
        public Mass CreateMass(double value = 0, string? units = null)
        {
            var unitGroup = _unitGroups[UnitFamilyName.Mass];
            var mass = new Mass(unitGroup);
            var defaultUnit = units ?? unitGroup.BaseUnit.Symbol;
            mass.Init(value, defaultUnit);
            return mass;
        }

        /// <summary>
        /// Create a Time instance with injected UnitGroup
        /// </summary>
        public Time CreateTime(double value = 0, string? units = null)
        {
            var unitGroup = _unitGroups[UnitFamilyName.Time];
            var time = new Time(unitGroup);
            var defaultUnit = units ?? unitGroup.BaseUnit.Symbol;
            time.Init(value, defaultUnit);
            return time;
        }

        /// <summary>
        /// Create a Speed instance with injected UnitGroup
        /// </summary>
        public Speed CreateSpeed(double value = 0, string? units = null)
        {
            var unitGroup = _unitGroups[UnitFamilyName.Speed];
            var speed = new Speed(unitGroup);
            var defaultUnit = units ?? unitGroup.BaseUnit.Symbol;
            speed.Init(value, defaultUnit);
            return speed;
        }

        /// <summary>
        /// Create an Area instance with injected UnitGroup
        /// </summary>
        public Area CreateArea(double value = 0, string? units = null)
        {
            var unitGroup = _unitGroups[UnitFamilyName.Area];
            var area = new Area(unitGroup);
            var defaultUnit = units ?? unitGroup.BaseUnit.Symbol;
            area.Init(value, defaultUnit);
            return area;
        }

        /// <summary>
        /// Create a Volume instance with injected UnitGroup
        /// </summary>
        public Volume CreateVolume(double value = 0, string? units = null)
        {
            var unitGroup = _unitGroups[UnitFamilyName.Volume];
            var volume = new Volume(unitGroup);
            var defaultUnit = units ?? unitGroup.BaseUnit.Symbol;
            volume.Init(value, defaultUnit);
            return volume;
        }

        /// <summary>
        /// Create a Force instance with injected UnitGroup
        /// </summary>
        public Force CreateForce(double value = 0, string? units = null)
        {
            var unitGroup = _unitGroups[UnitFamilyName.Force];
            var force = new Force(unitGroup);
            var defaultUnit = units ?? unitGroup.BaseUnit.Symbol;
            force.Init(value, defaultUnit);
            return force;
        }

        /// <summary>
        /// Create a Current instance with injected UnitGroup
        /// </summary>
        public Current CreateCurrent(double value = 0, string? units = null)
        {
            var unitGroup = _unitGroups[UnitFamilyName.Current];
            var current = new Current(unitGroup);
            var defaultUnit = units ?? unitGroup.BaseUnit.Symbol;
            current.Init(value, defaultUnit);
            return current;
        }

        /// <summary>
        /// Create a DataFlow instance with injected UnitGroup
        /// </summary>
        public DataFlow CreateDataFlow(double value = 0, string? units = null)
        {
            var unitGroup = _unitGroups[UnitFamilyName.DataFlow];
            var dataFlow = new DataFlow(unitGroup);
            var defaultUnit = units ?? unitGroup.BaseUnit.Symbol;
            dataFlow.Init(value, defaultUnit);
            return dataFlow;
        }

        /// <summary>
        /// Create a DataStorage instance with injected UnitGroup
        /// </summary>
        public DataStorage CreateDataStorage(double value = 0, string? units = null)
        {
            var unitGroup = _unitGroups[UnitFamilyName.DataStorage];
            var dataStorage = new DataStorage(unitGroup);
            var defaultUnit = units ?? unitGroup.BaseUnit.Symbol;
            dataStorage.Init(value, defaultUnit);
            return dataStorage;
        }

        /// <summary>
        /// Create a Frequency instance with injected UnitGroup
        /// </summary>
        public Frequency CreateFrequency(double value = 0, string? units = null)
        {
            var unitGroup = _unitGroups[UnitFamilyName.Frequency];
            var frequency = new Frequency(unitGroup);
            var defaultUnit = units ?? unitGroup.BaseUnit.Symbol;
            frequency.Init(value, defaultUnit);
            return frequency;
        }

        /// <summary>
        /// Create a Percent instance with injected UnitGroup
        /// </summary>
        public Percent CreatePercent(double value = 0, string? units = null)
        {
            var unitGroup = _unitGroups[UnitFamilyName.Percent];
            var percent = new Percent(unitGroup);
            var defaultUnit = units ?? unitGroup.BaseUnit.Symbol;
            percent.Init(value, defaultUnit);
            return percent;
        }

        /// <summary>
        /// Create a Distance instance with injected UnitGroup (uses Length family)
        /// </summary>
        public Distance CreateDistance(double value = 0, string? units = null)
        {
            var unitGroup = _unitGroups[UnitFamilyName.Length];
            var distance = new Distance(unitGroup);
            var defaultUnit = units ?? unitGroup.BaseUnit.Symbol;
            distance.Init(value, defaultUnit);
            return distance;
        }

        /// <summary>
        /// Create a Power instance with injected UnitGroup
        /// </summary>
        public Power CreatePower(double value = 0, string? units = null)
        {
            var unitGroup = _unitGroups[UnitFamilyName.Power];
            var power = new Power(unitGroup);
            var defaultUnit = units ?? unitGroup.BaseUnit.Symbol;
            power.Init(value, defaultUnit);
            return power;
        }

        /// <summary>
        /// Create a Voltage instance with injected UnitGroup
        /// </summary>
        public Voltage CreateVoltage(double value = 0, string? units = null)
        {
            var unitGroup = _unitGroups[UnitFamilyName.Voltage];
            var voltage = new Voltage(unitGroup);
            var defaultUnit = units ?? unitGroup.BaseUnit.Symbol;
            voltage.Init(value, defaultUnit);
            return voltage;
        }

        /// <summary>
        /// Create a Resistance instance with injected UnitGroup
        /// </summary>
        public Resistance CreateResistance(double value = 0, string? units = null)
        {
            var unitGroup = _unitGroups[UnitFamilyName.Resistance];
            var resistance = new Resistance(unitGroup);
            var defaultUnit = units ?? unitGroup.BaseUnit.Symbol;
            resistance.Init(value, defaultUnit);
            return resistance;
        }

        /// <summary>
        /// Create a Capacitance instance with injected UnitGroup
        /// </summary>
        public Capacitance CreateCapacitance(double value = 0, string? units = null)
        {
            var unitGroup = _unitGroups[UnitFamilyName.Capacitance];
            var capacitance = new Capacitance(unitGroup);
            var defaultUnit = units ?? unitGroup.BaseUnit.Symbol;
            capacitance.Init(value, defaultUnit);
            return capacitance;
        }

        /// <summary>
        /// Create a Duration instance with injected UnitGroup
        /// </summary>
        public Duration CreateDuration(double value = 0, string? units = null)
        {
            var unitGroup = _unitGroups[UnitFamilyName.Duration];
            var duration = new Duration(unitGroup);
            var defaultUnit = units ?? unitGroup.BaseUnit.Symbol;
            duration.Init(value, defaultUnit);
            return duration;
        }

        /// <summary>
        /// Create a Dimensionless instance with injected UnitGroup
        /// </summary>
        public Dimensionless CreateDimensionless(double value = 0, string? units = null)
        {
            var unitGroup = _unitGroups[UnitFamilyName.None];
            var dimensionless = new Dimensionless(unitGroup);
            var defaultUnit = units ?? unitGroup.BaseUnit.Symbol;
            dimensionless.Init(value, defaultUnit);
            return dimensionless;
        }

        /// <summary>
        /// Create a Heading instance with injected UnitGroup
        /// </summary>
        public Heading CreateHeading(double value = 0, string? units = null)
        {
            var unitGroup = _unitGroups[UnitFamilyName.Heading];
            var heading = new Heading(unitGroup);
            var defaultUnit = units ?? unitGroup.BaseUnit.Symbol;
            heading.Init(value, defaultUnit);
            return heading;
        }

        /// <summary>
        /// Create a Quantity instance with injected UnitGroup
        /// </summary>
        public Quantity CreateQuantity(double value = 0, string? units = null)
        {
            var unitGroup = _unitGroups[UnitFamilyName.Quantity];
            var quantity = new Quantity(unitGroup);
            var defaultUnit = units ?? unitGroup.BaseUnit.Symbol;
            quantity.Init(value, defaultUnit);
            return quantity;
        }

        /// <summary>
        /// Create a QuantityFlow instance with injected UnitGroup
        /// </summary>
        public QuantityFlow CreateQuantityFlow(double value = 0, string? units = null)
        {
            var unitGroup = _unitGroups[UnitFamilyName.QuantityFlow];
            var quantityFlow = new QuantityFlow(unitGroup);
            var defaultUnit = units ?? unitGroup.BaseUnit.Symbol;
            quantityFlow.Init(value, defaultUnit);
            return quantityFlow;
        }

        /// <summary>
        /// Create a generic MeasuredValue for any unit family
        /// </summary>
        public MeasuredValue CreateMeasuredValue(UnitFamilyName family, double value = 0, string? units = null)
        {
            if (!_unitGroups.TryGetValue(family, out var unitGroup))
                throw new ArgumentException($"Unit family {family} not available in {_systemType} system");

            var measuredValue = new MeasuredValue(unitGroup);
            var defaultUnit = units ?? unitGroup.BaseUnit.Symbol;
            measuredValue.Init(value, defaultUnit);
            return measuredValue;
        }




        /// <summary>
        /// Static factory method for quick SI system creation
        /// </summary>
        public static UnitFactory SI() => new UnitFactory(UnitSystemType.SI);

        /// <summary>
        /// Static factory method for quick FPS system creation
        /// </summary>
        public static UnitFactory FPS() => new UnitFactory(UnitSystemType.FPS);

        /// <summary>
        /// Static factory method for quick IPS system creation
        /// </summary>
        public static UnitFactory IPS() => new UnitFactory(UnitSystemType.IPS);

        /// <summary>
        /// Static factory method for quick MKS system creation
        /// </summary>
        public static UnitFactory MKS() => new UnitFactory(UnitSystemType.MKS);

        /// <summary>
        /// Static factory method for quick CGS system creation
        /// </summary>
        public static UnitFactory CGS() => new UnitFactory(UnitSystemType.CGS);
    }
}