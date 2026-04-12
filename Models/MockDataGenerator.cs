
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
			"#8B008B", // DarkMagenta
			"#FF8C00", // DarkOrange
			"#9932CC", // DarkOrchid
			"#8B0000", // DarkRed
			"#E9967A", // DarkSalmon
			"#8FBC8F", // DarkSeaGreen
			"#483D8B", // DarkSlateBlue
			"#2F4F4F", // DarkSlateGray
			"#00CED1", // DarkTurquoise
			"#9400D3", // DarkViolet
			"#FF1493", // DeepPink
			"#00BFFF", // DeepSkyBlue
			"#696969", // DimGray
			"#1E90FF", // DodgerBlue
			"#B22222", // FireBrick
			"#228B22", // ForestGreen
			"#FF00FF", // Fuchsia
			"#FFD700", // Gold
			"#DAA520", // Goldenrod
			"#808080", // Gray
			"#008000", // Green
			"#ADFF2F", // GreenYellow
			"#FF69B4"  // HotPink
		};

		public MockDataGenerator()
		{
			rand = new Random();
			firstNames = new List<string>();
			lastnames = new List<string>();
			symbols = new List<string>();
			words = new List<string>();
			colors = new List<string>();

			var names = new NameList();
			firstNames.AddRange(names.first);
			lastnames.AddRange(names.last);
			symbols.AddRange(names.symbols);
			colors.AddRange(names.colors);

			words.AddRange(firstNames);
			words.AddRange(lastnames);
			words.AddRange(colors);
		}

		public string RandomFirstName() => firstNames[rand.Next(firstNames.Count)];
		public string RandomLastName() => lastnames[rand.Next(lastnames.Count)];
		public string RandomFullName() => $"{RandomFirstName()} {RandomLastName()}";
		public string RandomSymbol() => symbols[rand.Next(symbols.Count)];
		public string RandomColor() => colors[rand.Next(colors.Count)];
		public string RandomDarkColor() => darkColors[rand.Next(darkColors.Count)];
		public string RandomWord() => words[rand.Next(words.Count)];

		public string RandomSentence(int wordCount = 5)
		{
			var sentence = new List<string>();
			for (int i = 0; i < wordCount; i++)
			{
				sentence.Add(RandomWord());
			}
			return string.Join(" ", sentence);
		}

		public int RandomInt(int min = 0, int max = 100) => rand.Next(min, max + 1);
		public double RandomDouble(double min = 0.0, double max = 100.0) => rand.NextDouble() * (max - min) + min;
		
		public string RandomGuid() => Guid.NewGuid().ToString();
		
		public DateTime RandomDateTime(DateTime? start = null, DateTime? end = null)
		{
			var startDate = start ?? DateTime.Now.AddYears(-1);
			var endDate = end ?? DateTime.Now;
			var range = endDate - startDate;
			var randomTime = new TimeSpan((long)(rand.NextDouble() * range.Ticks));
			return startDate + randomTime;
		}
	}
}