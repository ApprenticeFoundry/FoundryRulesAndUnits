using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

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

		public UnitCategoryService UnitCategories { get; set; } = new();

		public List<UnitCategory> Categories()
		{
			return UnitCategories.Categories();
		}

		public double Square(double v) { return v * v; }

		public double Cube(double v) { return v * v * v; }



		public bool Apply(UnitSystemType type)
		{
			return type switch
			{
				UnitSystemType.IPS => IPS(),
				UnitSystemType.FPS => FPS(),
				UnitSystemType.MKS => MKS(),
				UnitSystemType.CGS => CGS(),
				UnitSystemType.mmNs => MMNs(),
				_ => throw new NotImplementedException(),
			};

		}

		private bool MMNs()
		{
			return EstablishCommonUnit();
		}

		private bool CGS()
		{
			return EstablishCommonUnit();
		}

		private bool MKS()
		{
			return EstablishCommonUnit();
		}

		private bool FPS()
		{
			return EstablishCommonUnit();
		}

		private bool IPS()
		{
			return EstablishCommonUnit();
		}

		public void SetPixelsPerMeter(double pixelsPerMeter)
		{
			length?.Conversion(pixelsPerMeter, "px", 1.0, "m");
		}

		public bool EstablishCommonUnit()
		{

			//var PixelsPerInch = 40; // 70; pixels per in or SRS machine

			length = new UnitCategory("Length", new UnitSpec("m", "meters", UnitFamilyName.Length))
					.Units("cm", "centimeters")
					.Conversion(100, "cm", 1, "m")
					.Units("km", "kilometers")
					.Conversion(1, "km", 1000, "m")
					.Units("mm", "millimeters")
					.Conversion(1000, "mm", 1, "m")
					.Units("in", "inches")
					.Conversion(39.3701, "in", 1, "m")
					.Units("px", "pixels")
					.Conversion(5000, "px", 1, "m");
			

			UnitCategories.Category(length);
			Length.Category = () => length;

			angle = new UnitCategory("Angle", new UnitSpec("rad", "radians", UnitFamilyName.Angle))
				.Units("deg", "degrees")
				.Conversion("deg", "rad", v => Math.PI * v / 180.0 )
				.Conversion("rad", "deg", v => 180.0 * v / Math.PI );


			UnitCategories.Category(angle);
			Angle.Category = () => angle;

			var area = new UnitCategory("Area", new UnitSpec("m2", "sq meters", UnitFamilyName.Area))
				.Units("cm2", "sq centimeters")
				.Conversion(Square(100), "cm2", 1, "m2")
				.Units("km2", "sq kilometers")
				.Conversion(1, "km2", Square(1000), "m2")
				.Units("mm2", "sq millimeters")
				.Conversion(Square(1000), "mm2", 1, "m2");

			UnitCategories.Category(area);
			Area.Category = () => area;

			var volume = new UnitCategory("Volume", new UnitSpec("m3", "cubic meters", UnitFamilyName.Volume))
				.Units("cm3", "cubic centimeters")
				.Conversion(Cube(100), "cm3", 1, "m3")
				.Units("km2", "cubic kilometers")
				.Conversion(1, "km3", Cube(1000), "m3")
				.Units("mm3", "cubic millimeters")
				.Conversion(Cube(1000), "mm3", 1, "m3");

			UnitCategories.Category(volume);
			Volume.Category = () => volume;

			var quanity = new UnitCategory("Quantity", new UnitSpec("ea", "each", UnitFamilyName.Quantity))
				.Units("dz", "dozen")
				.Conversion(1, "dz", 12, "ea")
				.Units("gr", "gross")
				.Conversion(1, "gr", 144, "ea");

			UnitCategories.Category(quanity);
			Quantity.Category = () => quanity;

			//(32°F − 32) × 5/9 = 0°C
			//(32°F − 32) × 5/9 + 273.15 = 273.15K
			//(32°C × 9/5) + 32 = 89.6°F

			var temp = new UnitCategory("Temperature", new UnitSpec("C", "Celsius", UnitFamilyName.Temperature))
				.Units("F", "Fahrenheit")
				.Conversion("C", "F", v => v * 9 / 5 + 32.0)
				.Units("K", "Kelvin")
				.Conversion("C", "K", v => v + 273.15)  //v => (v - 32) * 5 / 9
				.Conversion("F", "C", v => (v - 32.0) * 5 / 9);

			UnitCategories.Category(temp);
			Temperature.Category = () => temp;



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

			worktime = new UnitCategory("WorkTime", new UnitSpec("Hrs", "Hours", UnitFamilyName.WorkTime))
				.Units("Days", "Days")
				.Conversion(24, "Hrs", 1, "Days")
				.Units("Wdays", "WorkDays")
				.Conversion(5.0, "Days", 1.0, "Wdays")
				.Units("Wks", "Weeks")
				.Conversion(7.0, "Days", 1.0, "Wks")
				.Units("Mins", "Minutes")
				.Conversion(60.0, "Hrs", 1.0, "Mins");

			UnitCategories.Category(worktime);
			return true;
		}
	}
}
