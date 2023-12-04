namespace SimpleTableManager.Models;

public partial class Table
{
	[CommandReference]
	public void ResetViewOptions()
	{
		ViewOptions.Set(0, 0, Math.Max(Size.Width - 1, 0), Math.Max(Size.Height - 1, 0));
	}

	[CommandReference]
	public void HideColumnAt(int x)
	{
		Header[x].Visibility.IsColumnHidden = true;
		Columns[x].ForEach(c => c.Visibility.IsColumnHidden = true);

		ViewOptions.InvokeViewChangedEvent();
	}

	[CommandReference]
	public void HideRowAt(int y)
	{
		Sider[y].Visibility.IsRowHidden = true;
		Rows[y].ForEach(c => c.Visibility.IsRowHidden = true);

		ViewOptions.InvokeViewChangedEvent();
	}

	[CommandReference]
	public void ShowColumnAt(int x)
	{
		Header[x].Visibility.IsColumnHidden = false;
		Columns[x].ForEach(c => c.Visibility.IsColumnHidden = false);

		ViewOptions.InvokeViewChangedEvent();
	}

	[CommandReference]
	public void ShowRowAt(int y)
	{
		Sider[y].Visibility.IsRowHidden = false;
		Rows[y].ForEach(c => c.Visibility.IsRowHidden = false);

		ViewOptions.InvokeViewChangedEvent();
	}

	[CommandReference]
	public void ShowAllRows()
	{
		Sider.ForEach(s => s.Visibility.IsRowHidden = false);
		Rows.ForEach(r => r.Value.ForEach(c => c.Visibility.IsRowHidden = false));

		ViewOptions.InvokeViewChangedEvent();
	}

	[CommandReference]
	public void ShowAllColumns()
	{
		Header.ForEach(s => s.Visibility.IsColumnHidden = false);
		Columns.ForEach(r => r.Value.ForEach(c => c.Visibility.IsColumnHidden = false));

		ViewOptions.InvokeViewChangedEvent();
	}

	[CommandReference]
	public void ShowAllCells()
	{
		ShowAllColumns();
		ShowAllRows();
	}

	[CommandReference(StateModifier = false)]
	public object ShowCellDetails(Position position)
	{
		return this[position].ShowDetails();
	}

	[CommandReference(StateModifier = false)]
	public object ShowCellContentFunction(Position position)
	{
		return this[position].ShowContentFunction();
	}
}