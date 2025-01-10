using System.Globalization;

namespace SimpleTableManager.Services;

/// <summary>
/// Global application settings
/// </summary>
public class Settings
{
	public string[] Logo { get; set; } = [];

	public string[] LoadingScreenLogo { get; set; } = [];

	public string[] LoadingScreenSplashes { get; set; } = [];

	public bool ShowLoadingScreen { get; set; } = true;

	public int LoadingScreenDelay { get; set; } = 1500;

	public ConsoleColorSet TextColor { get; set; } = new(ConsoleColor.Gray, ConsoleColor.Black);

	public ConsoleColorSet DefaultContentColor { get; set; } = new(ConsoleColor.Gray, ConsoleColor.Black);

	public ConsoleColorSet DefaultBackgroundColor { get; set; } = new(ConsoleColor.DarkGray, ConsoleColor.Black);

	public ConsoleColorSet DefaultBorderColor { get; set; } = new(ConsoleColor.Gray, ConsoleColor.Black);

	public ConsoleColorSet PrimarySelectionContentColor { get; set; } = new(ConsoleColor.Black, ConsoleColor.DarkGreen);

	public ConsoleColorSet PrimarySelectionBackgroundColor { get; set; } = new(ConsoleColor.Black, ConsoleColor.DarkGreen);

	public ConsoleColorSet PrimarySelectionBorderColor { get; set; } = new(ConsoleColor.DarkGreen, ConsoleColor.Black);

	public ConsoleColorSet SecondarySelectionContentColor { get; set; } = new(ConsoleColor.Black, ConsoleColor.DarkYellow);

	public ConsoleColorSet SecondarySelectionBackgroundColor { get; set; } = new(ConsoleColor.Black, ConsoleColor.DarkYellow);

	public ConsoleColorSet SecondarySelectionBorderColor { get; set; } = new(ConsoleColor.DarkYellow, ConsoleColor.Black);

	public ConsoleColorSet TertiarySelectionContentColor { get; set; } = new(ConsoleColor.Black, ConsoleColor.DarkGray);

	public ConsoleColorSet TertiarySelectionBackgroundColor { get; set; } = new(ConsoleColor.Black, ConsoleColor.DarkGray);

	public ConsoleColorSet TertiarySelectionBorderColor { get; set; } = new(ConsoleColor.DarkGray, ConsoleColor.Black);

	public ConsoleColorSet NotAvailableContentColor { get; set; } = new(ConsoleColor.DarkGray, ConsoleColor.Black);

	public ConsoleColorSet OkLabelColor { get; set; } = new(ConsoleColor.Green, ConsoleColor.Black);

	public ConsoleColorSet NotOkLabelColor { get; set; } = new(ConsoleColor.Red, ConsoleColor.Black);

	public bool ModernTableBorder { get; set; } = false;

	public char IndexCellLeftArrow { get; set; }

	public char IndexCellRightArrow { get; set; }

	public char IndexCellUpArrow { get; set; }

	public char IndexCellDownArrow { get; set; }

	public char IndexCellFiltered { get; set; }

	public char TmpBackgroundCharacter { get; set; }

	public char DefaultCellBackgroundCharacter { get; set; }

	public string DefaultWorkDirectory { get; set; } = "";

	public int CommandHintRowCount { get; set; } = 2;

	public uint CommandHistoryLength { get; set; }

	public uint EditHistoryLength { get; set; }

	public Size DefaultTableSize { get; set; } = new(10, 5);

	public bool Autosave { get; set; }

	public string Author { get; set; } = "";

	public bool Audio { get; set; }

	public Note[] StartupNotes { get; set; } = [];

	public Note[] ShutdownNotes { get; set; } = [];

	public Note[] OkNotes { get; set; } = [];

	public Note[] QuestionNotes { get; set; } = [];

	public Note[] ErrorNotes { get; set; } = [];

	public Note[] CriticalNotes { get; set; } = [];

	public byte Volume { get; set; } = 25;

	public string Culture { get; set; } = CultureInfo.CurrentUICulture.Name;

	public bool CheckAppVersionOnDocumentLoad { get; set; } = true;

	public static Settings Current { get; private set; } = new();

	public static void FromJson(string path)
	{
		Current = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(path))!;
	}
}