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
            var length = new Length(_unitGroups[UnitFamilyName.Length]);
            if (value != 0 || units != null)
                length.Init(value, units);
            return length;
        }

        /// <summary>
        /// Create an Angle instance with injected UnitGroup
        /// </summary>
        public Angle CreateAngle(double value = 0, string? units = null)
        {
            var angle = new Angle(_unitGroups[UnitFamilyName.Angle]);
            if (value != 0 || units != null)
                angle.Init(value, units);
            return angle;
        }

        /// <summary>
        /// Create a Temperature instance with injected UnitGroup
        /// </summary>
        public Temperature CreateTemperature(double value = 0, string? units = null)
        {
            var temperature = new Temperature(_unitGroups[UnitFamilyName.Temperature]);
            if (value != 0 || units != null)
                temperature.Init(value, units);
            return temperature;
        }

        /// <summary>
        /// Create a Mass instance with injected UnitGroup
        /// </summary>
        public Mass CreateMass(double value = 0, string? units = null)
        {
            var mass = new Mass(_unitGroups[UnitFamilyName.Mass]);
            if (value != 0 || units != null)
                mass.Init(value, units);
            return mass;
        }

        /// <summary>
        /// Create a Time instance with injected UnitGroup
        /// </summary>
        public Time CreateTime(double value = 0, string? units = null)
        {
            var time = new Time(_unitGroups[UnitFamilyName.Time]);
            if (value != 0 || units != null)
                time.Init(value, units);
            return time;
        }

        /// <summary>
        /// Create a Speed instance with injected UnitGroup
        /// </summary>
        public Speed CreateSpeed(double value = 0, string? units = null)
        {
            var speed = new Speed(_unitGroups[UnitFamilyName.Speed]);
            if (value != 0 || units != null)
                speed.Init(value, units);
            return speed;
        }

        /// <summary>
        /// Create an Area instance with injected UnitGroup
        /// </summary>
        public Area CreateArea(double value = 0, string? units = null)
        {
            var area = new Area(_unitGroups[UnitFamilyName.Area]);
            if (value != 0 || units != null)
                area.Init(value, units);
            return area;
        }

        /// <summary>
        /// Create a Volume instance with injected UnitGroup
        /// </summary>
        public Volume CreateVolume(double value = 0, string? units = null)
        {
            var volume = new Volume(_unitGroups[UnitFamilyName.Volume]);
            if (value != 0 || units != null)
                volume.Init(value, units);
            return volume;
        }

        /// <summary>
        /// Create a Force instance with injected UnitGroup
        /// </summary>
        public Force CreateForce(double value = 0, string? units = null)
        {
            var force = new Force(_unitGroups[UnitFamilyName.Force]);
            if (value != 0 || units != null)
                force.Init(value, units);
            return force;
        }

        /// <summary>
        /// Create a Current instance with injected UnitGroup
        /// </summary>
        public Current CreateCurrent(double value = 0, string? units = null)
        {
            var current = new Current(_unitGroups[UnitFamilyName.Current]);
            if (value != 0 || units != null)
                current.Init(value, units);
            return current;
        }

        /// <summary>
        /// Create a DataFlow instance with injected UnitGroup
        /// </summary>
        public DataFlow CreateDataFlow(double value = 0, string? units = null)
        {
            var dataFlow = new DataFlow(_unitGroups[UnitFamilyName.DataFlow]);
            if (value != 0 || units != null)
                dataFlow.Init(value, units);
            return dataFlow;
        }

        /// <summary>
        /// Create a DataStorage instance with injected UnitGroup
        /// </summary>
        public DataStorage CreateDataStorage(double value = 0, string? units = null)
        {
            var dataStorage = new DataStorage(_unitGroups[UnitFamilyName.DataStorage]);
            if (value != 0 || units != null)
                dataStorage.Init(value, units);
            return dataStorage;
        }

        /// <summary>
        /// Create a Frequency instance with injected UnitGroup
        /// </summary>
        public Frequency CreateFrequency(double value = 0, string? units = null)
        {
            var frequency = new Frequency(_unitGroups[UnitFamilyName.Frequency]);
            if (value != 0 || units != null)
                frequency.Init(value, units);
            return frequency;
        }

        /// <summary>
        /// Create a Percent instance with injected UnitGroup
        /// </summary>
        public Percent CreatePercent(double value = 0, string? units = null)
        {
            var percent = new Percent(_unitGroups[UnitFamilyName.Percent]);
            if (value != 0 || units != null)
                percent.Init(value, units);
            return percent;
        }

        /// <summary>
        /// Create a Distance instance with injected UnitGroup (uses Length family)
        /// </summary>
        public Distance CreateDistance(double value = 0, string? units = null)
        {
            var distance = new Distance(_unitGroups[UnitFamilyName.Length]);
            if (value != 0 || units != null)
                distance.Init(value, units);
            return distance;
        }

        /// <summary>
        /// Create a Power instance with injected UnitGroup
        /// </summary>
        public Power CreatePower(double value = 0, string? units = null)
        {
            var power = new Power(_unitGroups[UnitFamilyName.Power]);
            if (value != 0 || units != null)
                power.Init(value, units);
            return power;
        }

        /// <summary>
        /// Create a Voltage instance with injected UnitGroup
        /// </summary>
        public Voltage CreateVoltage(double value = 0, string? units = null)
        {
            var voltage = new Voltage(_unitGroups[UnitFamilyName.Voltage]);
            if (value != 0 || units != null)
                voltage.Init(value, units);
            return voltage;
        }

        /// <summary>
        /// Create a Resistance instance with injected UnitGroup
        /// </summary>
        public Resistance CreateResistance(double value = 0, string? units = null)
        {
            var resistance = new Resistance(_unitGroups[UnitFamilyName.Resistance]);
            if (value != 0 || units != null)
                resistance.Init(value, units);
            return resistance;
        }

        /// <summary>
        /// Create a Capacitance instance with injected UnitGroup
        /// </summary>
        public Capacitance CreateCapacitance(double value = 0, string? units = null)
        {
            var capacitance = new Capacitance(_unitGroups[UnitFamilyName.Capacitance]);
            if (value != 0 || units != null)
                capacitance.Init(value, units);
            return capacitance;
        }

        /// <summary>
        /// Create a Duration instance with injected UnitGroup
        /// </summary>
        public Duration CreateDuration(double value = 0, string? units = null)
        {
            var duration = new Duration(_unitGroups[UnitFamilyName.Duration]);
            if (value != 0 || units != null)
                duration.Init(value, units);
            return duration;
        }

        /// <summary>
        /// Create a Dimensionless instance with injected UnitGroup
        /// </summary>
        public Dimensionless CreateDimensionless(double value = 0, string? units = null)
        {
            var dimensionless = new Dimensionless(_unitGroups[UnitFamilyName.None]);
            if (value != 0 || units != null)
                dimensionless.Init(value, units);
            return dimensionless;
        }

        /// <summary>
        /// Create a Heading instance with injected UnitGroup
        /// </summary>
        public Heading CreateHeading(double value = 0, string? units = null)
        {
            var heading = new Heading(_unitGroups[UnitFamilyName.Heading]);
            if (value != 0 || units != null)
                heading.Init(value, units);
            return heading;
        }

        /// <summary>
        /// Create a Quantity instance with injected UnitGroup
        /// </summary>
        public Quantity CreateQuantity(double value = 0, string? units = null)
        {
            var quantity = new Quantity(_unitGroups[UnitFamilyName.Quantity]);
            if (value != 0 || units != null)
                quantity.Init(value, units);
            return quantity;
        }

        /// <summary>
        /// Create a QuantityFlow instance with injected UnitGroup
        /// </summary>
        public QuantityFlow CreateQuantityFlow(double value = 0, string? units = null)
        {
            var quantityFlow = new QuantityFlow(_unitGroups[UnitFamilyName.QuantityFlow]);
            if (value != 0 || units != null)
                quantityFlow.Init(value, units);
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
            if (value != 0 || units != null)
                measuredValue.Init(value, units);
            return measuredValue;
        }

        /// <summary>
        /// Create a strongly-typed measurement instance using C# generics
        /// </summary>
        /// <typeparam name="T">The measurement type to create (must inherit from MeasuredValue)</typeparam>
        /// <param name="value">Initial value</param>
        /// <param name="units">Initial units</param>
        /// <returns>Strongly-typed measurement instance</returns>
        public T Create<T>(double value = 0, string? units = null) where T : MeasuredValue
        {
            // Determine the unit family based on the type
            var family = GetUnitFamilyForType<T>();
            
            if (!_unitGroups.TryGetValue(family, out var unitGroup))
                throw new ArgumentException($"Unit family {family} not available in {_systemType} system");

            // Create instance using the UnitGroup constructor
            var instance = (T)Activator.CreateInstance(typeof(T), unitGroup)!;
            
            if (value != 0 || units != null)
                instance.Init(value, units);
                
            return instance;
        }

        /// <summary>
        /// Helper method to determine UnitFamilyName from measurement type using static property
        /// </summary>
        private static UnitFamilyName GetUnitFamilyForType<T>() where T : MeasuredValue
        {
            // Try to get the StaticUnitFamily property from the type
            var staticProperty = typeof(T).GetProperty("StaticUnitFamily", 
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                
            if (staticProperty != null && staticProperty.PropertyType == typeof(UnitFamilyName))
            {
                return (UnitFamilyName)staticProperty.GetValue(null)!;
            }
            
            // Fallback to the switch statement for types that haven't been updated yet
            return typeof(T).Name switch
            {
                nameof(Length) => UnitFamilyName.Length,
                nameof(Mass) => UnitFamilyName.Mass,
                nameof(Time) => UnitFamilyName.Time,
                nameof(Temperature) => UnitFamilyName.Temperature,
                nameof(Angle) => UnitFamilyName.Angle,
                nameof(Area) => UnitFamilyName.Area,
                nameof(Volume) => UnitFamilyName.Volume,
                nameof(Speed) => UnitFamilyName.Speed,
                nameof(Force) => UnitFamilyName.Force,
                nameof(Power) => UnitFamilyName.Power,
                nameof(Voltage) => UnitFamilyName.Voltage,
                nameof(Current) => UnitFamilyName.Current,
                nameof(Resistance) => UnitFamilyName.Resistance,
                nameof(Capacitance) => UnitFamilyName.Capacitance,
                nameof(Frequency) => UnitFamilyName.Frequency,
                nameof(DataFlow) => UnitFamilyName.DataFlow,
                nameof(DataStorage) => UnitFamilyName.DataStorage,
                nameof(Percent) => UnitFamilyName.Percent,
                nameof(Distance) => UnitFamilyName.Length, // Distance uses Length family
                nameof(Duration) => UnitFamilyName.Duration,
                nameof(Dimensionless) => UnitFamilyName.None,
                nameof(Heading) => UnitFamilyName.Heading,
                nameof(Quantity) => UnitFamilyName.Quantity,
                nameof(QuantityFlow) => UnitFamilyName.QuantityFlow,
                _ => throw new ArgumentException($"Unknown measurement type: {typeof(T).Name}")
            };
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