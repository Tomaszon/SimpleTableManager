namespace SimpleTableManager.Models.CommandExecuters;

public class IndexCell : Cell
{
	public IndexCellType IndexCellType { get; set; }

	public char LowerArrow { get; set; }

	public char HigherArrow { get; set; }

	public int Index { get; set; }

	public IndexCell(Table table, IndexCellType indexCellType, int index, char lowerArrow, char higherArrow) : base(table, index.ToString())
	{
		IndexCellType = indexCellType;
		Index = index;
		LowerArrow = lowerArrow;
		HigherArrow = higherArrow;

		table.ViewChanged += OnViewChanged;
	}

	public void OnViewChanged(int? firstHeaderIndex, int? lastHeaderIndex, int? firstSiderIndex, int? lastSiderIndex)
	{
		if (IndexCellType == IndexCellType.Header)
		{
			OnViewChangedCore(firstHeaderIndex, lastHeaderIndex, Table.Size.Width - 1, Table.ColumnFilters.ContainsKey(Index));
		}
		else
		{
			OnViewChangedCore(firstSiderIndex, lastSiderIndex, Table.Size.Height - 1, Table.RowFilters.ContainsKey(Index));
		}

		InvokeStateModifierCommandExecutedEvent(new(this));
	}

	private void OnViewChangedCore(int? first, int? last, int maxIndex, bool filtered)
	{
		RemoveEllipses();

		if(filtered)
		{
			AppendFilteredMark();
		}

		if (first is not null && Index == first && Index > 0)
		{
			AppendLowerEllipsis();
		}
		if (last is not null && Index == last && Index < maxIndex)
		{
			AppendHigherEllipsis(maxIndex);
		}
	}

	public void AppendHigherEllipsis(int lastIndex)
	{
		var content = ContentFunction!.ExecuteAndFormat().First();

		SetStringContent($"{content} {HigherArrow} {lastIndex}");
	}

	public void AppendFilteredMark()
	{
		var content = ContentFunction!.ExecuteAndFormat().First();

		SetStringContent($"{content} {Settings.Current.IndexCellFiltered}");
	}

	public void AppendLowerEllipsis()
	{
		var content = ContentFunction!.ExecuteAndFormat().First();

		SetStringContent($"0 {LowerArrow} {content}");
	}

	public void RemoveEllipses()
	{
		SetStringContent(Index.ToString());
	}
}