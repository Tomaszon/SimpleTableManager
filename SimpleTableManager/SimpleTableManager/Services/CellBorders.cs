﻿namespace SimpleTableManager.Services;

public static class CellBorders
{
	private static Dictionary<CellBorderType, CellBorder> _BORDERS = [];

	public static void FromJson(string path)
	{
		_BORDERS = JsonConvert.DeserializeObject<Dictionary<CellBorderType, CellBorder>>(File.ReadAllText(path))!;
	}


	public static CellBorder Get(CellBorderType name)
	{
		return _BORDERS[name];
	}
}