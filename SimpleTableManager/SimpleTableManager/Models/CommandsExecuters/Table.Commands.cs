using System.Text;
using System.Text.RegularExpressions;
using SimpleTableManager.Services;

namespace SimpleTableManager.Models.CommandExecuters;

public partial class Table
{
	[CommandFunction]
	public void ResetViewOptions()
	{
		ViewOptions.Set(0, 0, Math.Max(Size.Width - 1, 0), Math.Max(Size.Height - 1, 0));
	}

	private void ApplyFilters()
	{
		if (ColumnFilters.Count > 0)
		{
			Shared.IndexArray(Rows.Count).ForEach(HideRowAtCore);
		}
		else
		{
			ShowAllRowsCore();
		}

		if (RowFilters.Count > 0)
		{
			Shared.IndexArray(Columns.Count).ForEach(HideColumnAtCore);
		}
		else
		{
			ShowAllColumnsCore();
		}

		// UNDONE only show cell if multiple column filters allow it
		foreach (var filter in ColumnFilters)
		{
			Shared.IndexArray(Rows.Count).ForEach(p =>
			{
				if (Rows[p][filter.Key].GetFormattedContents().Any(c => Regex.IsMatch(c, filter.Value)))
				{
					// UNDONE only show cell if column filter AND row filter allows it
					// if (RowFilters.TryGetValue(p, out var rowFilter))
					// {
					// 	if (Columns[filter.Key][p].GetFormattedContents().Any(c => Regex.IsMatch(c, filter.Value)))
					// 	{
					// 		ShowRowAtCore(p);
					// 	}
					// }
					// else
					// {
						ShowRowAtCore(p);
					// }
				}
			});
		}

		// UNDONE only show cell if multiple row filters allow it
		foreach (var filter in RowFilters)
		{
			Shared.IndexArray(Columns.Count).ForEach(p =>
			{
				if (Columns[p][filter.Key].GetFormattedContents().Any(c => Regex.IsMatch(c, filter.Value)))
				{
					// UNDONE only show cell if column filter AND row filter allows it
					// if (ColumnFilters.TryGetValue(p, out var columnFilter))
					// {
						// if (Rows[filter.Key][p].GetFormattedContents().Any(c => Regex.IsMatch(c, filter.Value)))
						// {
						// 	ShowColumnAtCore(p);
						// }
					// }
					// else
					// {
						ShowColumnAtCore(p);
					// }
				}
			});
		}

		ViewOptions.InvokeViewChangedEvent();
	}

	[CommandFunction]
	public void AddRowFilter(int y, string filterExpression)
	{
		RowFilters.Replace(y, filterExpression);

		ApplyFilters();
	}

	[CommandFunction]
	public void AddColumnFilter(int x, string filterExpression)
	{
		ColumnFilters.Replace(x, filterExpression);

		ApplyFilters();
	}

	[CommandFunction]
	public void RemoveRowFilter(int y)
	{
		RowFilters.Remove(y);

		ApplyFilters();
	}

	[CommandFunction]
	public void RemoveColumnFilter(int x)
	{
		ColumnFilters.Remove(x);

		ApplyFilters();
	}

	private void HideColumnAtCore(int x)
	{
		Header[x].Visibility.IsColumnHidden = true;
		Columns[x].ForEach(c => c.Visibility.IsColumnHidden = true);
	}

	private void HideRowAtCore(int y)
	{
		Sider[y].Visibility.IsRowHidden = true;
		Rows[y].ForEach(c => c.Visibility.IsRowHidden = true);
	}

	private void ShowColumnAtCore(int x)
	{
		Header[x].Visibility.IsColumnHidden = false;
		Columns[x].ForEach(c => c.Visibility.IsColumnHidden = false);
	}

	private void ShowRowAtCore(int y)
	{
		Sider[y].Visibility.IsRowHidden = false;
		Rows[y].ForEach(c => c.Visibility.IsRowHidden = false);
	}

	private void ShowAllRowsCore()
	{
		Shared.IndexArray(Rows.Count).ForEach(ShowRowAtCore);
	}

	private void ShowAllColumnsCore()
	{
		Shared.IndexArray(Columns.Count).ForEach(ShowColumnAtCore);
	}

	[CommandFunction]
	public void HideColumnAt(int x)
	{
		HideColumnAtCore(x);

		ColumnFilters.Remove(x);

		ApplyFilters();
	}

	[CommandFunction]
	public void HideRowAt(int y)
	{
		HideRowAtCore(y);

		RowFilters.Remove(y);

		ApplyFilters();
	}

	[CommandFunction]
	public void ShowColumnAt(int x)
	{
		ShowColumnAtCore(x);

		ColumnFilters.Remove(x);

		ApplyFilters();
	}

	[CommandFunction]
	public void ShowRowAt(int y)
	{
		ShowRowAtCore(y);

		RowFilters.Remove(y);

		ApplyFilters();
	}

	[CommandFunction]
	public void ShowAllRows()
	{
		ShowAllRowsCore();

		ApplyFilters();
	}

	[CommandFunction]
	public void ShowAllColumns()
	{
		ShowAllColumnsCore();

		ApplyFilters();
	}

	[CommandFunction]
	public void ShowAllCells()
	{
		ShowAllColumnsCore();
		ShowAllRowsCore();

		ApplyFilters();
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
					var formatted = string.Join("\r\n", contents).Trim();

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
}