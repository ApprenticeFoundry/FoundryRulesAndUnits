

namespace FoundryRulesAndUnits.Models
{

	/// <summary>
	/// RandomName class, used to generate a random name.
	/// </summary>
	public class MockDataGenerator
	{
		class NameList
		{
			public string[] first { get; set; }
			public string[] last { get; set; }
			public string[] symbols { get; set; }
			public string[] colors { get; set; }

	

			public NameList()
			{
				first = new string[] {
					"Thomas", "Casen", "Eric","Steve","Greg", "Sherman","Jason","Nathan","Ward","Ron","JB"
				};
				last = new string[] {
					"North", "South", "East","West","Earth","Wind","Fire","Water"
				};
				symbols = new string[] {
					"SFG-UCI----", "SFGPUH2----", "SFGPEWRH--MT", "SFG-UCI----D", "SHG---------", "SJGPUCI-----", "SFGPUCIO----", "SFG--------K",  "SFGP-------H"
				};
				colors = new string[] { "Red", "Orange", "Yellow", "Green", "Blue", "Purple", "Pink", "Brown", "Grey", "Black", "White", "Crimson", "Indigo", "Violet", "Magenta", "Turquoise", "Teal", "SlateGray", "DarkSlateGray", "SaddleBrown", "Sienna", "DarkKhaki", "Goldenrod", "DarkGoldenrod", "FireBrick", "DarkRed", "RosyBrown", "DarkMagenta", "DarkOrchid", "DeepSkyBlue" };
			}
		}

		readonly Random rand;
		readonly List<string> firstNames;
		readonly List<string> lastnames;
		readonly List<string> symbols;
		readonly List<string> words;
		readonly List<string> colors;

			// Dark colors array
		readonly List<string> darkColors = new List<string>() {
			"#000000", // Black
			"#342D7E", // DarkBlue  
			"#483D8B", // DarkSlateBlue
			"#2F4F4F", // DarkSlateGray
			"#006400", // DarkGreen
			"#8B0000", // DarkRed
			"#4B0082", // Indigo
			"#556B2F", // DarkOliveGreen 
			"#000000", // Black
			"#696969", // DimGray
			"#A9A9A9"  // DarkGray
		};

		// Light colors array
		readonly List<string> lightColors = new List<string>() {
			"#FFFFFF", // White
			"#DCDCDC", // Gainsboro
			"#F5F5F5", // WhiteSmoke
			"#D3D3D3", // LightGrey
			"#90EE90", // LightGreen
			"#FFB6C1", // LightPink
			"#FFA07A", // LightSalmon
			"#20B2AA", // LightSeaGreen
			"#87CEFA", // LightSkyBlue
			"#778899", // LightSlateGray 
			"#B0C4DE"  // LightSteelBlue
		};

		public MockDataGenerator()
		{
			this.rand = new Random();
			var list = new NameList();

			firstNames = new List<string>(list.first);
			lastnames = new List<string>(list.last);
			symbols = new List<string>(list.symbols);
			colors = new List<string>(list.colors);

			var data = "tortor risus dapibus augue vel accumsan tellus nisi eu orci mauris lacinia sapien quis libero nullam sit amet turpis elementum ligula vehicula consequat morbi a ipsum integer a nibh in quis justo maecenas rhoncus aliquam lacus morbi quis tortor id nulla ultrices aliquet maecenas leo odio condimentum id luctus nec molestie sed justo pellentesque viverra pede ac diam cras pellentesque volutpat dui maecenas tristique est et tempus semper est quam pharetra magna ac consequat metus sapien ut nunc vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae mauris viverra diam vitae quam suspendisse potenti nullam".Split(" ");
			words = new List<string>(data);
		}

		public string GenerateName()
		{
			string first = firstNames[rand.Next(firstNames.Count)];
			string last = lastnames[rand.Next(lastnames.Count)];

			return $"{first}_{last}";
		}

		public string GenerateText()
		{
			var list = new List<string>();
			for (int i = 0; i < GenerateInt(5, 12); i++)
			{
				string word = words[rand.Next(words.Count)];
				list.Add(word);
			}

			return string.Join(" ", list);
		}

		public string GenerateWord()
		{
			string word = words[rand.Next(words.Count)];
			return word;
		}

		public string GenerateSymbol()
		{
			string symbol = symbols[rand.Next(symbols.Count)];
			return symbol;
		}

		public double GenerateDouble(double min = 0.0, double max = 1.0)
		{
			return min + (max - min) * rand.NextDouble();
		}

		public int GenerateInt(int min = 0, int max = 1)
		{
			return rand.Next(min, max);
		}

		public string GenerateColor()
		{
			var color = colors[rand.Next(colors.Count)];
			return color;
		}
		public string GenerateLightColor()
		{
			var color = lightColors[rand.Next(lightColors.Count)];
			return color;
		}
		public string GenerateDarkColor()
		{
			var color = darkColors[rand.Next(darkColors.Count)];
			return color;
		}
	}
}
