using System.Text.Json.Serialization;
using System.Text.Json;

namespace FoundryRulesAndUnits.Units
{

    public class FoundryNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name) => name;
    }

	public class UnitSpec
	{
		protected string name { get; set; }
		protected string title { get; set; }
		protected UnitFamilyName family { get; set; }

		public UnitSpec(string units)
		{
			this.name = units;
			this.family = UnitFamilyName.None;
			this.title = units;
		}

		public UnitSpec(string units, string title, UnitFamilyName unitFamily)
		{
			this.name = units;
			this.family = unitFamily;
			this.title = title;
		}

		public string Title() { return title; }
		public string Name() { return name; }
		public UnitFamilyName UnitFamily() { return family; }

//https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/customize-properties
		public static JsonSerializerOptions JsonHydrateOptions(bool includeFields = false)
		{
			var options = new JsonSerializerOptions()
			{
				IncludeFields = includeFields,
				IgnoreReadOnlyFields = true,
				AllowTrailingCommas = true,
				PropertyNameCaseInsensitive = true,
				//PropertyNamingPolicy = new FoundryNamingPolicy(),
				DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
				WriteIndented = true,
				// Converters =
				// {
				// 	new MeasuredValueJsonConverter(),
				// 	new LengthJsonConverter(),
				// 	new AngleJsonConverter()
				// }
			};
			return options;
		}
	}
}

















