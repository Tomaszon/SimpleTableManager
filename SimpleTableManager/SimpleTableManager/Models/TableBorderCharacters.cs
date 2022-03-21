using System.Collections.Generic;
using System.IO;
using System.Linq;

using Newtonsoft.Json;

namespace SimpleTableManager.Models
{
	public class BorderCharacter
	{
		public char Retro { get; set; }

		public char Modern { get; set; }

		public TableBorderCharacterMode Mode { get; set; }
	}

	public static class TableBorderCharacters
	{
		private static List<BorderCharacter> Characters = new List<BorderCharacter>();

		public static void FromJson(string path)
		{
			Characters = JsonConvert.DeserializeObject<List<BorderCharacter>>(File.ReadAllText(path));
		}

		public static char Get(TableBorderCharacterMode mode)
		{
			if (Characters.SingleOrDefault(c => c.Mode == mode) is var res && res is { })
			{
				return Settings.ModernTableBorder ? res.Modern : res.Retro;
			}
			else
			{
				return 'X';
			}
		}
	}
}
