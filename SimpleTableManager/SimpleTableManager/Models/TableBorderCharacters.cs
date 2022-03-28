using System.Collections.Generic;
using System.IO;
using System.Linq;

using Newtonsoft.Json;

namespace SimpleTableManager.Models
{

	public static class TableBorderCharacters
	{
		private static List<TableBorderCharacter> _CHARACTERS = new List<TableBorderCharacter>();

		public static void FromJson(string path)
		{
			_CHARACTERS = JsonConvert.DeserializeObject<List<TableBorderCharacter>>(File.ReadAllText(path));
		}

		public static char Get(TableBorderCharacterMode mode)
		{
			if (_CHARACTERS.SingleOrDefault(c => 
				c.Mode == (TableBorderCharacterMode)((int)mode & ~(int)TableBorderCharacterMode.None)) is var res && res is { })
			{
				return Settings.Current.ModernTableBorder ? res.Modern : res.Retro;
			}
			else
			{
				return 'X';
			}
		}
	}
}
