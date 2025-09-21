using FoundryRulesAndUnits.Extensions;
using System;
using System.Collections.Generic;

namespace FoundryRulesAndUnits.Units
{
	[System.Serializable]
	public class Length : MeasuredValue
	{
		/// <summary>
		/// Gets the UnitFamily for Length measurements
		/// </summary>
		public override UnitFamilyName UnitFamily => UnitFamilyName.Length;


		/// <summary>
		/// Constructor with UnitGroup injection - preferred for factory pattern
		/// </summary>
		public Length(UnitGroup unitGroup) : base(unitGroup)
		{
			if (unitGroup.Family != UnitFamilyName.Length)
				throw new ArgumentException($"UnitGroup family must be {UnitFamilyName.Length}", nameof(unitGroup));
		}

		public Length Assign(double value, string? units)
		{
			Init(value, units); // Base class handles everything!
			return this;
		}

		public Length Assign(Length source)
		{
			Init(source.Value(), source.U); // Base class handles everything!
			return this;
		}

		public Length Copy()
		{
			var copy = new Length(_unitGroup);
			copy.Init(Value(), Internal());
			return copy;
		}

		// Static factory methods removed - use UnitFactory.CreateLength() instead
		// Example: factory.CreateLength(1000, "m") for kilometers

		// As() method inherited from MeasuredValue - no override needed!

		// Legacy compatibility methods
		public int AsPixels() 
		{
			// Handle pixel conversion with manual fallback when UnitGroup injection unavailable
			try 
			{
				return (int)Math.Round(As("px"));
			}
			catch (InvalidOperationException)
			{
				// Manual conversion fallback for common units to pixels (96 DPI standard)
				// This handles cases where UnitGroup injection is not available
				var valueInMeters = I switch
				{
					"m" => V,
					"cm" => V * 0.01,
					"mm" => V * 0.001,
					"in" => V * 0.0254,
					"ft" => V * 0.3048,
					"px" => V / (96.0 / 0.0254), // Convert px back to meters first, then to px (identity)
					_ => throw new InvalidOperationException($"Cannot convert {I} to pixels without UnitGroup injection")
				};
				
				// Convert meters to pixels (96 DPI: 96 pixels per inch, 0.0254 meters per inch)
				var pixelsPerMeter = 96.0 / 0.0254;
				return (int)Math.Round(valueInMeters * pixelsPerMeter);
			}
		}
		
		public static bool operator <(Length left, Length right) => left.Value() < right.Value();
		public static bool operator >(Length left, Length right) => left.Value() > right.Value();

		public static Length operator +(Length left, Length right)
		{
			var result = new Length(left._unitGroup);
			result.Init(left.Value() + right.Value(), left.Internal());
			return result;
		}
		
		public static Length operator -(Length left, Length right)
		{
			var result = new Length(left._unitGroup);
			result.Init(left.Value() - right.Value(), left.Internal());
			return result;
		}
		
		public static Length operator *(double left, Length right)
		{
			var result = new Length(right._unitGroup);
			result.Init(left * right.Value(), right.Internal());
			return result;
		}
		
		public static Length operator /(Length left, double right)
		{
			var result = new Length(left._unitGroup);
			result.Init(left.Value() / right, left.Internal());
			return result;
		}

		public static double operator /(Length left, Length right) => left.Value() / right.Value();

		// Cross-unit operations removed - these require UnitFactory to create proper instances
		// Use UnitFactory.CreateArea() and UnitFactory.CreateVolume() for cross-unit calculations
	}



}
