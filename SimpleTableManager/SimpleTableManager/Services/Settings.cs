using SimpleTableManager.Models;

namespace SimpleTableManager.Services;

public class Settings
{
	public string[] Logo { get; set; } = Array.Empty<string>();

	public ConsoleColorSet TextColor { get; set; } = (ConsoleColor.Gray, ConsoleColor.Black);

	public ConsoleColorSet DefaultContentColor { get; set; } = (ConsoleColor.Gray, ConsoleColor.Black);

	public ConsoleColorSet DefaultBackgroundColor { get; set; } = (ConsoleColor.DarkGray, ConsoleColor.Black);

	public ConsoleColorSet DefaultBorderColor { get; set; } = (ConsoleColor.Gray, ConsoleColor.Black);

	public ConsoleColorSet SelectedContentColor { get; set; } = (ConsoleColor.Yellow, ConsoleColor.Black);

	public ConsoleColorSet SelectedBackgroundColor { get; set; } = (ConsoleColor.Yellow, ConsoleColor.Black);

	public ConsoleColorSet SelectedBorderColor { get; set; } = (ConsoleColor.Yellow, ConsoleColor.Black);

	public ConsoleColorSet NotAvailableContentColor { get; set; } = (ConsoleColor.DarkGray, ConsoleColor.Black);

	public ConsoleColorSet OkLabelColor { get; set; } = (ConsoleColor.Green, ConsoleColor.Black);

	public ConsoleColorSet NotOkLabelColor { get; set; } = (ConsoleColor.Red, ConsoleColor.Black);

	public bool ModernTableBorder { get; set; } = false;

	public char IndexCellLeftArrow { get; set; }

	public char IndexCellRightArrow { get; set; }

	public char IndexCellUpArrow { get; set; }

	public char IndexCellDownArrow { get; set; }

	public char IndexCellFiltered { get; set; }

	public char TmpBackgroundCharacter { get; set; }

	public char DefaultCellBackgroundCharacter { get; set; }

	public string DefaultWorkDirectory { get; set; } = "";

	public uint CommandHistoryLength { get; set; }

	public uint EditHistoryLength { get; set; }

	public Size DefaultTableSize { get; set; } = new(10, 5);

	public bool Autosave { get; set; }

	public string Author { get; set; } = "";

	public bool Audio { get; set; }

	public static Settings Current { get; private set; } = new();

	public static void FromJson(string path)
	{
		Current = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(path))!;
	}
}