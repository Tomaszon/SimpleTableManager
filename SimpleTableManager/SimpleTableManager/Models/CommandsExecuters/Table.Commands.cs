using System.Text;
using SimpleTableManager.Services;

namespace SimpleTableManager.Models.CommandExecuters;

public partial class Table
{
	[CommandFunction]
	public void ResetViewOptions()
	{
		ViewOptions.Set(0, 0, Math.Max(Size.Width - 1, 0), Math.Max(Size.Height - 1, 0));
	}

	[CommandFunction]
	public void HideColumnAt(int x)
	{
		Header[x].Visibility.IsColumnHidden = true;
		Columns[x].ForEach(c => c.Visibility.IsColumnHidden = true);

		ViewOptions.InvokeViewChangedEvent();
	}

	[CommandFunction]
	public void HideRowAt(int y)
	{
		Sider[y].Visibility.IsRowHidden = true;
		Rows[y].ForEach(c => c.Visibility.IsRowHidden = true);

		ViewOptions.InvokeViewChangedEvent();
	}

	[CommandFunction]
	public void ShowColumnAt(int x)
	{
		Header[x].Visibility.IsColumnHidden = false;
		Columns[x].ForEach(c => c.Visibility.IsColumnHidden = false);

		ViewOptions.InvokeViewChangedEvent();
	}

	[CommandFunction]
	public void ShowRowAt(int y)
	{
		Sider[y].Visibility.IsRowHidden = false;
		Rows[y].ForEach(c => c.Visibility.IsRowHidden = false);

		ViewOptions.InvokeViewChangedEvent();
	}

	[CommandFunction]
	public void ShowAllRows()
	{
		Sider.ForEach(s => s.Visibility.IsRowHidden = false);
		Rows.ForEach(r => r.Value.ForEach(c => c.Visibility.IsRowHidden = false));

		ViewOptions.InvokeViewChangedEvent();
	}

	[CommandFunction]
	public void ShowAllColumns()
	{
		Header.ForEach(s => s.Visibility.IsColumnHidden = false);
		Columns.ForEach(r => r.Value.ForEach(c => c.Visibility.IsColumnHidden = false));

		ViewOptions.InvokeViewChangedEvent();
	}

	[CommandFunction]
	public void ShowAllCells()
	{
		ShowAllColumns();
		ShowAllRows();
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