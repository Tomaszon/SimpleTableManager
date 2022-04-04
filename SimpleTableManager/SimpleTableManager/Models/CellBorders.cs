using System.Collections.Generic;
using System.IO;

using Newtonsoft.Json;

namespace SimpleTableManager.Models
{
	public class CellBorders
	{
		private static Dictionary<CellBorderName, CellBorder> _BORDERS = new Dictionary<CellBorderName, CellBorder>();

		public static void FromJson(string path)
		{
			_BORDERS = JsonConvert.DeserializeObject<Dictionary<CellBorderName, CellBorder>>(File.ReadAllText(path));
		}


		public static CellBorder Get(CellBorderName name)
		{
			return _BORDERS[name];
		}
	}
}
