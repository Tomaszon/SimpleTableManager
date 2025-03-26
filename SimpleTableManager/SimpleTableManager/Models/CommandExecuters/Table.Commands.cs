namespace SimpleTableManager.Models.CommandExecuters;

public partial class Table
{
	[CommandFunction]
	public void ResetViewOptions()
	{
		ResetViewOptionsCore();
	}

	[CommandFunction]
	public Dictionary<int, string> ShowRowFilters()
	{
		return RowFilters;
	}

	[CommandFunction]
	public Dictionary<int, string> ShowColumnFilters()
	{
		return ColumnFilters;
	}

	[CommandFunction]
	public void HideColumnsAt(params IEnumerable<int> xs)
	{
		ThrowIf(xs.Any(e => e < 0 || e >= Size.Width), $"Index is not in the needed range: [0, {Size.Width}]");

		xs.ForEach(x => HiddenColumns.Add(x));

		HideAndFilterCells();
	}

	[CommandFunction]
	public void HideRowsAt(params IEnumerable<int> ys)
	{
		ThrowIf(ys.Any(e => e < 0 || e >= Size.Height), $"Index is not in the needed range: [0, {Size.Height}]");

		ys.ForEach(y => HiddenRows.Add(y));

		HideAndFilterCells();
	}

	[CommandFunction]
	public void ShowColumnsAt(params IEnumerable<int> xs)
	{
		ThrowIf(xs.Any(e => e < 0 || e >= Size.Width), $"Index is not in the needed range: [0, {Size.Width}]");

		xs.ForEach(x => HiddenColumns.Remove(x));

		HideAndFilterCells();
	}

	[CommandFunction]
	public void ShowRowsAt(params IEnumerable<int> ys)
	{
		ThrowIf(ys.Any(e => e < 0 || e >= Size.Height), $"Index is not in the needed range: [0, {Size.Height}]");

		ys.ForEach(y => HiddenRows.Remove(y));

		HideAndFilterCells();
	}

	[CommandFunction]
	public void ShowAllRows()
	{
		HiddenRows.Clear();

		HideAndFilterCells();
	}

	[CommandFunction]
	public void ShowAllColumns()
	{
		HiddenColumns.Clear();

		HideAndFilterCells();
	}

	[CommandFunction]
	public void ShowAllCells()
	{
		HiddenRows.Clear();
		HiddenColumns.Clear();

		HideAndFilterCells();
	}

	[CommandFunction(StateModifier = false)]
	public object ShowCellDetails(Position position)
	{
		return this[position].ShowDetails();
	}

	[CommandFunction(StateModifier = false)]
	public object ShowCellContentFunction(Position position)
	{
		return this[position].ShowContentFunction();
	}

	[CommandFunction(StateModifier = false)]
	[CommandShortcut("exportTable"), CommandInformation("Exports table as a CSV file")]
	public void Export(bool overwrite = false)
	{
		var fileName = Shared.GetWorkFilePath(Name, "csv");

		Export(fileName, overwrite);
	}

	public void Export(string fileName, bool overwrite)
	{
		ThrowIf<InvalidOperationException>(File.Exists(fileName) && !overwrite, $"File '{fileName}' already exists, set {nameof(overwrite)} to 'true' to force file save");

		try
		{
			using var f = File.Create(fileName);
			using var sw = new StreamWriter(f);

			Rows.ForEach(r =>
			{
				var contents = r.Value.Select(c =>
				{
					var contents = c.GetFormattedContents().ToList();
					var formatted = string.Join("\n", contents).Trim();

					return contents.Count > 1 || formatted.Contains(',') ? $"\"{formatted}\"" : formatted;
				}).ToList();

				sw.WriteLine(string.Join(",", contents));
			});
		}
		catch (Exception ex)
		{
			if (File.Exists(fileName))
			{
				File.Delete(fileName);
			}

			throw new OperationCanceledException("Can not save document", ex);
		}
	}

	[CommandFunction(ClearsCache = true)]
	public void SwapCells(Position position1, Position position2)
	{
		var cell1 = this[position1];
		var cell2 = this[position2];

		var index1 = Content.IndexOf(cell1);
		var index2 = Content.IndexOf(cell2);

		Content.RemoveAt(index1);
		Content.Insert(index1, cell2);

		Content.RemoveAt(index2);
		Content.Insert(index2, cell1);
	}

	[CommandFunction]
	public Dictionary<Position, List<string>> ShowCellComments()
	{
		return Shared.IndexArray(Size.Width).SelectMany(x =>
			Shared.IndexArray(Size.Height).Select(y =>
				(Position: new Position(x, y), this[x, y].Comments)))
					.Where(p => p.Comments.Count > 0).ToDictionary(k => k.Position, v => v.Comments);
	}
}