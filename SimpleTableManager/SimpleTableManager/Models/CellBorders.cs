using System.Collections.Generic;
using System.IO;

using Newtonsoft.Json;

namespace SimpleTableManager.Models
{
	public class CellBorders
	{
		private static Dictionary<CellBorderType, CellBorder> _BORDERS = new Dictionary<CellBorderType, CellBorder>();

		public static void FromJson(string path)
		{
			_BORDERS = JsonConvert.DeserializeObject<Dictionary<CellBorderType, CellBorder>>(File.ReadAllText(path));
		}


		public static CellBorder Get(CellBorderType name)
		{
			return _BORDERS[name];
		}
	}
}
