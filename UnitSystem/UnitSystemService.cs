using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using FoundryRulesAndUnits.Units;

namespace FoundryRulesAndUnits.Units
{
	public enum UnitSystemType
	{
		IPS,
		FPS,
		MKS,
		CGS,
		mmNs
	}


	public class UnitCategoryService
	{
		protected Dictionary<UnitFamilyName, UnitCategory> CategoryLookup { get; private set; } = new();

		public void Category(UnitCategory category)
		{
			CategoryLookup.Add(category.UnitFamily(), category);
		}

		public List<UnitCategory> Categories()
		{
			return CategoryLookup.Values.ToList();
		}
	}

	public interface IUnitSystem
	{
		List<UnitCategory> Categories();
		bool Apply(UnitSystemType type);
		void SetPixelsPerMeter(double pixelsPerMeter);
	}

	public class UnitSystem : IUnitSystem
	{
		public UnitCategory? length { get;  set;}
		public UnitCategory? angle { get;  set;}
		public UnitCategory? storage { get;  set;}
		public UnitCategory? worktime { get; set; }
		public UnitCategory? mass { get; set; }
		public UnitCategory? force { get; set; }
		public UnitCategory? temperature { get; set; }

		/// <summary>
		/// Track the currently active unit system for base unit accuracy
		/// </summary>
		public UnitSystemType ActiveSystem { get; private set; } = UnitSystemType.MKS;

		public UnitCategoryService UnitCategories { get; set; } = new();

		public List<UnitCategory> Categories()
		{
			return UnitCategories.Categories();
		}

		public double Square(double v) { return v * v; }

		public double Cube(double v) { return v * v * v; }



		public bool Apply(UnitSystemType type)
		{
			var success = type switch
			{
				UnitSystemType.IPS => IPS(),
				UnitSystemType.FPS => FPS(),
				UnitSystemType.MKS => MKS(),
				UnitSystemType.CGS => CGS(),
				UnitSystemType.mmNs => MMNs(),
				_ => throw new NotImplementedException(),
			};

			if (success)
			{
				ActiveSystem = type;
			}

			return success;
		}

		private bool MMNs()
		{
			return EstablishMMNsUnits();
		}

		private bool CGS()
		{
			return EstablishCGSUnits();
		}

		private bool MKS()
		{
			return EstablishMKSUnits(); // Renamed from EstablishCommonUnit
		}

		private bool FPS()
		{
			return EstablishFPSUnits();
		}

		private bool IPS()
		{
			return EstablishIPSUnits();
		}

		public void SetPixelsPerMeter(double pixelsPerMeter)
		{
			length?.Conversion(pixelsPerMeter, "px", 1.0, "m");
		}

		public bool EstablishMKSUnits()
		{

			//var PixelsPerInch = 40; // 70; pixels per in or SRS machine

			// Length: METERS as true base unit (MKS system)
			length = new UnitCategory("Length", new UnitSpec("m", "meters", UnitFamilyName.Length))
				.AddMetricLengthUnits("m")        // mm, cm, km with exact conversions
				.AddCrossSystemConversions()      // in, ft with high precision
				.Units("px", "pixels")
				.Conversion(5000, "px", 1, "m");
			

			UnitCategories.Category(length);
			Length.Category = () => length;

			// Mass: KILOGRAMS as true base unit (MKS system)
			mass = new UnitCategory("Mass", new UnitSpec("kg", "kilograms", UnitFamilyName.Mass))
				.AddMassUnits("kg");              // g, mg, lb, oz with conversions

			UnitCategories.Category(mass);

			// Force: NEWTONS as true base unit (MKS system)  
			force = new UnitCategory("Force", new UnitSpec("N", "newtons", UnitFamilyName.Force))
				.AddForceUnits("N");              // kN, dyne, lbf with conversions

			UnitCategories.Category(force);

			// Temperature: CELSIUS as base unit (MKS system)
			temperature = new UnitCategory("Temperature", new UnitSpec("C", "Celsius", UnitFamilyName.Temperature))
				.AddTemperatureConversions();     // F, K with exact formulas

			UnitCategories.Category(temperature);
			Temperature.Category = () => temperature;

			// System-independent categories
			EstablishSystemIndependentCategories();

			// Derived categories with proper base unit adaptation
			var area = new UnitCategory("Area", new UnitSpec("m2", "sq meters", UnitFamilyName.Area))
				.Units("cm2", "sq centimeters")
				.Conversion(Square(100), "cm2", 1, "m2")
				.Units("km2", "sq kilometers")  // Fixed: was "km2" for cubic
				.Conversion(1, "km2", Square(1000), "m2")
				.Units("mm2", "sq millimeters")
				.Conversion(Square(1000), "mm2", 1, "m2");

			UnitCategories.Category(area);
			Area.Category = () => area;

			var volume = new UnitCategory("Volume", new UnitSpec("m3", "cubic meters", UnitFamilyName.Volume))
				.Units("cm3", "cubic centimeters")
				.Conversion(Cube(100), "cm3", 1, "m3")
				.Units("km3", "cubic kilometers")  // Fixed: was "km2"
				.Conversion(1, "km3", Cube(1000), "m3")
				.Units("mm3", "cubic millimeters")
				.Conversion(Cube(1000), "mm3", 1, "m3");

			UnitCategories.Category(volume);
			Volume.Category = () => volume;

			return true;
		}

		/// <summary>
		/// Establishes system-independent categories that are the same for all unit systems.
		/// </summary>
		private void EstablishSystemIndependentCategories()
		{
			// Angles - same for all systems (radians/degrees)
			angle = new UnitCategory("Angle", new UnitSpec("rad", "radians", UnitFamilyName.Angle))
				.Units("deg", "degrees")
				.Conversion("deg", "rad", v => Math.PI * v / 180.0)
				.Conversion("rad", "deg", v => 180.0 * v / Math.PI);

			UnitCategories.Category(angle);
			Angle.Category = () => angle;

			// Data Storage - same for all systems (bytes-based)
			storage = new UnitCategory("DataStorage", new UnitSpec("KB", "KiloBytes", UnitFamilyName.DataStorage))
				.Units("GB", "GigaBytes")
				.Conversion(1000, "KB", 1, "GB")
				.Units("TB", "TeraBytes")
				.Conversion(1000000, "KB", 1, "TB")
				.Units("Bytes", "Bytes")
				.Conversion(1000, "Bytes", 1, "KB");

			UnitCategories.Category(storage);

			var transfer = new UnitCategory("DataFlow", new UnitSpec("KB/sec", "KiloBytes per second", UnitFamilyName.DataFlow))
				.Units("Bytes/sec", "Bytes per second")
				.Conversion(1000, "Bytes/sec", 1, "KB/sec")
				.Units("GB/sec", "GigaBytes per second")
				.Conversion(1000, "KB/sec", 1, "GB/sec");

			UnitCategories.Category(transfer);

			// Work Time - same for all systems (hours-based)
			worktime = new UnitCategory("WorkTime", new UnitSpec("Hrs", "Hours", UnitFamilyName.WorkTime))
				.Units("Days", "Days")
				.Conversion(24, "Hrs", 1, "Days")
				.Units("Wdays", "WorkDays")
				.Conversion(5.0, "Days", 1.0, "Wdays")
				.Units("Wks", "Weeks")
				.Conversion(7.0, "Days", 1.0, "Wks")
				.Units("Mins", "Minutes")
				.Conversion(60, "Mins", 1, "Hrs");  // Fixed: was backwards

			UnitCategories.Category(worktime);

			// Quantity - same for all systems (each-based)
			var quantity = new UnitCategory("Quantity", new UnitSpec("ea", "each", UnitFamilyName.Quantity))  // Fixed spelling
				.Units("dz", "dozen")
				.Conversion(1, "dz", 12, "ea")
				.Units("gr", "gross")
				.Conversion(1, "gr", 144, "ea");

			UnitCategories.Category(quantity);
			Quantity.Category = () => quantity;

			var quantityFlow = new UnitCategory("QuantityFlow", new UnitSpec("ea/s", "each per sec", UnitFamilyName.QuantityFlow))  // Fixed spelling
				.Units("dz/s", "dozen per sec")
				.Conversion(1, "dz/s", 12, "ea/s")  // Fixed: was "dz/hr"
				.Units("ea/m", "each per min")
				.Conversion(1, "ea/m", 60, "ea/s")
				.Units("ea/day", "each per day")
				.Conversion(1, "ea/day", 60 * 60 * 24, "ea/s")
				.Units("ea/hr", "each per hour")
				.Conversion(1, "ea/hr", 60 * 60, "ea/s");

			UnitCategories.Category(quantityFlow);
			QuantityFlow.Category = () => quantityFlow;
		}

		/// <summary>
		/// Establishes IPS (Inch-Pound-Second) unit system with inches, pounds, and Fahrenheit as base units.
		/// </summary>
		/// <returns>True if successful</returns>
		private bool EstablishIPSUnits()
		{
			// Length: INCHES as true base unit (native storage)
			length = new UnitCategory("Length", new UnitSpec("in", "inches", UnitFamilyName.Length))
				.AddImperialLengthUnits("in")     // ft, yd, mi with exact conversions
				.AddCrossSystemConversions()      // mm, cm, m with exact definitions
				.Units("px", "pixels")
				.Conversion(96, "px", 1, "in");   // Standard 96 DPI

			UnitCategories.Category(length);
			Length.Category = () => length;

			// Mass: POUNDS as true base unit (native storage)
			mass = new UnitCategory("Mass", new UnitSpec("lb", "pounds", UnitFamilyName.Mass))
				.AddMassUnits("lb");              // oz, ton, kg, g with conversions

			UnitCategories.Category(mass);

			// Force: POUND-FORCE as true base unit (IPS system)
			force = new UnitCategory("Force", new UnitSpec("lbf", "pounds-force", UnitFamilyName.Force))
				.AddForceUnits("lbf");            // N, dyne with conversions

			UnitCategories.Category(force);

			// Temperature: FAHRENHEIT as base unit (native storage)
			temperature = new UnitCategory("Temperature", new UnitSpec("F", "Fahrenheit", UnitFamilyName.Temperature))
				.AddTemperatureConversions();     // C, K with exact formulas

			UnitCategories.Category(temperature);
			Temperature.Category = () => temperature;

			EstablishSystemIndependentCategories();
			return true;
		}

		/// <summary>
		/// Establishes FPS (Foot-Pound-Second) unit system with feet, pounds, and Fahrenheit as base units.
		/// </summary>
		/// <returns>True if successful</returns>
		private bool EstablishFPSUnits()
		{
			// Length: FEET as true base unit (native storage)
			length = new UnitCategory("Length", new UnitSpec("ft", "feet", UnitFamilyName.Length))
				.AddImperialLengthUnits("ft")     // in, yd, mi with exact conversions
				.AddCrossSystemConversions()      // m, cm with exact definitions
				.Units("px", "pixels")
				.Conversion(1152, "px", 1, "ft"); // 96 DPI * 12 inches/foot

			UnitCategories.Category(length);
			Length.Category = () => length;

			// Mass: POUNDS as true base unit (same as IPS)
			mass = new UnitCategory("Mass", new UnitSpec("lb", "pounds", UnitFamilyName.Mass))
				.AddMassUnits("lb");

			UnitCategories.Category(mass);

			// Force: POUND-FORCE as true base unit (same as IPS)
			force = new UnitCategory("Force", new UnitSpec("lbf", "pounds-force", UnitFamilyName.Force))
				.AddForceUnits("lbf");

			UnitCategories.Category(force);

			// Temperature: FAHRENHEIT as base unit (same as IPS)
			temperature = new UnitCategory("Temperature", new UnitSpec("F", "Fahrenheit", UnitFamilyName.Temperature))
				.AddTemperatureConversions();

			UnitCategories.Category(temperature);
			Temperature.Category = () => temperature;

			EstablishSystemIndependentCategories();
			return true;
		}

		/// <summary>
		/// Establishes CGS (Centimeter-Gram-Second) unit system with centimeters, grams, and dynes as base units.
		/// </summary>
		/// <returns>True if successful</returns>
		private bool EstablishCGSUnits()
		{
			// Length: CENTIMETERS as true base unit (native storage)
			length = new UnitCategory("Length", new UnitSpec("cm", "centimeters", UnitFamilyName.Length))
				.AddMetricLengthUnits("cm")       // mm, m, km with exact conversions
				.AddCrossSystemConversions();     // in, ft with high precision

			UnitCategories.Category(length);
			Length.Category = () => length;

			// Mass: GRAMS as true base unit (native storage)
			mass = new UnitCategory("Mass", new UnitSpec("g", "grams", UnitFamilyName.Mass))
				.AddMassUnits("g");               // mg, kg, lb, oz with conversions

			UnitCategories.Category(mass);

			// Force: DYNES as true base unit (CGS specific)
			force = new UnitCategory("Force", new UnitSpec("dyne", "dynes", UnitFamilyName.Force))
				.AddForceUnits("dyne");           // N, lbf with conversions

			UnitCategories.Category(force);

			// Temperature: CELSIUS as base unit
			temperature = new UnitCategory("Temperature", new UnitSpec("C", "Celsius", UnitFamilyName.Temperature))
				.AddTemperatureConversions();

			UnitCategories.Category(temperature);
			Temperature.Category = () => temperature;

			EstablishSystemIndependentCategories();
			return true;
		}

		/// <summary>
		/// Establishes mmNs (Millimeter-Newton-Second) unit system with millimeters, newtons, and grams as base units.
		/// </summary>
		/// <returns>True if successful</returns>
		private bool EstablishMMNsUnits()
		{
			// Length: MILLIMETERS as true base unit (native storage)
			length = new UnitCategory("Length", new UnitSpec("mm", "millimeters", UnitFamilyName.Length))
				.AddMetricLengthUnits("mm")       // cm, m, km with exact conversions
				.AddCrossSystemConversions()      // in, ft with high precision
				.Units("μm", "micrometers")
				.Conversion(1000, "μm", 1, "mm"); // Exact: 1000 μm = 1 mm

			UnitCategories.Category(length);
			Length.Category = () => length;

			// Force: NEWTONS as true base unit (same as MKS)
			force = new UnitCategory("Force", new UnitSpec("N", "newtons", UnitFamilyName.Force))
				.AddForceUnits("N");              // kN, dyne, lbf with conversions

			UnitCategories.Category(force);

			// Mass: GRAMS as base unit (derived from force via F=ma)
			mass = new UnitCategory("Mass", new UnitSpec("g", "grams", UnitFamilyName.Mass))
				.AddMassUnits("g");

			UnitCategories.Category(mass);

			// Temperature: CELSIUS as base unit
			temperature = new UnitCategory("Temperature", new UnitSpec("C", "Celsius", UnitFamilyName.Temperature))
				.AddTemperatureConversions();

			UnitCategories.Category(temperature);
			Temperature.Category = () => temperature;

			EstablishSystemIndependentCategories();
			return true;
		}
	}
}
