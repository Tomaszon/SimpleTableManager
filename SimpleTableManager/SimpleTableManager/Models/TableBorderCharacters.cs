using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Newtonsoft.Json;

namespace SimpleTableManager.Models
{
	public class BorderCharacter
	{
		public char Retro { get; set; }

		public char Modern { get; set; }
	}

	public static class TableBorderCharacters
	{
		private static Dictionary<TableBorderCharacter, BorderCharacter> Characters = new Dictionary<TableBorderCharacter, BorderCharacter>();

		public static void FromJson(string path)
		{
			Characters = JsonConvert.DeserializeObject<Dictionary<TableBorderCharacter, BorderCharacter>>(File.ReadAllText(path));
		}

		public static char Get(TableBorderCharacter tableBorderCharacter)
		{
			if (Characters.TryGetValue(tableBorderCharacter, out var character))
			{
				return Settings.ModernTableBorder ? character.Modern : character.Retro;
			}
			else
			{
				return ' ';
			}
		}
	}
}
