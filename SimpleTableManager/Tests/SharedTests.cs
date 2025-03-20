using SimpleTableManager.Models.CommandExecuters;

namespace SimpleTableManager.Tests;

[ExcludeFromCodeCoverage]
public class SharedTests : TestBase
{
	[TestCase(new[] { 5, 4, 8, 5, 9 }, 9)]
	public void MaxTest(int[] array, int max)
	{
		CheckResult(Shared.Max(array), max);
	}

	[TestCase(new[] { 5, 4, 8, 5, 9 }, 4)]
	public void MinTest(int[] array, int min)
	{
		CheckResult(Shared.Min(array), min);
	}

	[Test]
	public void IsHandledTest()
	{
		CheckResults([new IndexOutOfRangeException().IsHandled(), new FormatException().IsHandled()], [false, true]);
	}

	[Test]
	public void SerializeCloneTest()
	{
		var position = new Position(3, 4);

		var position2 = Shared.SerializeClone(position);

		CheckResult(position, position2);
	}

	[TestCase("alma:körte:szilva", new[] { "alma", "körte" }, new[] { "szilva" })]
	public void JoinTest(string expected, params string[][] args)
	{
		var result = Shared.Join(':', args);

		CheckResult(expected, result);
	}

	[TestCase(50, "50 B")]
	[TestCase(500, "500 B")]
	[TestCase(5_000, "4.88 KB")]
	[TestCase(50_000, "48.83 KB")]
	[TestCase(5_000_000, "4.77 MB")]
	public void ConvertBytesToOtherSizesTest(long value, string expected)
	{
		CheckResult(Shared.ConvertBytesToOtherSizes(value), expected);
	}

	[TestCase("C:\\alma.txt", "", "C:\\alma.txt")]
	[TestCase("alma", "txt", "{0}\\alma.txt")]
	public void GetWorkFilePathTest(string fileName, string extension, string expected)
	{
		expected = expected.Replace("{0}", Settings.Current.DefaultWorkDirectory);

		CheckResult(Shared.GetWorkFilePath(fileName, extension), expected);
	}

	[TestCase(typeof(Table), new string[] { "deselectAllCells", "selectAllCells", "moveSelectionLeft", "moveSelectionRight", "moveSelectionUp", "moveSelectionDown", "exportTable" }, new string[] { nameof(Table.DeselectAll), nameof(Table.SelectAll), nameof(Table.MoveSelectionLeft), nameof(Table.MoveSelectionRight), nameof(Table.MoveSelectionUp), nameof(Table.MoveSelectionDown), nameof(Table.Export) })]
	public void GetMethodsTest(Type type, string[] shortcuts, string[] methodNames)
	{
		var methods = Shared.GetMethods<CommandShortcutAttribute>(type, k => k.Key.Key);

		CheckResults([.. methods.Keys.Select(e => e.ToLower()).Order()], shortcuts.Select(e => e.ToLower()).Order());

		CheckResults(methods.Values.Select(m => m.Name.ToLower()).Order(), methodNames.Select(e => e.ToLower()).Order());
	}

	[Test]
	public void PopulateDocumentTest()
	{
		var document = new Document(new(10, 5));

		var (w, h) = document.GetActiveTable().Size;
		CheckResults([w, h], [10, 5]);

		var state = Shared.SerializeObject(document);

		using var ms = new MemoryStream();
		using var sw = new StreamWriter(ms);
		using var sr = new StreamReader(ms);

		sw.Write(state);

		sw.Flush();

		ms.Position = 0;

		Shared.PopulateDocument(sr, document);

		sr.Close();
		sw.Close();
		ms.Close();

		document.GetActiveTable().SetSize(new(2, 3));

		(w, h) = document.GetActiveTable().Size;

		CheckResults([w, h], [2, 3]);
	}
}