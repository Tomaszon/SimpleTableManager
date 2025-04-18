﻿using System.Runtime.Serialization;

namespace SimpleTableManager.Models.CommandExecuters;

[CommandInformation("Cell selection and table related commands")]
[JsonObject(IsReference = true)]
public partial class Table : CommandExecuterBase
{
	public Guid Id { get; set; } = Guid.NewGuid();

	public Document Document { get; set; } = default!;

	public event Action<int?, int?, int?, int?>? ViewChanged;

	public string Name { get; set; } = default!;

	public Size Size { get; set; } = new(0, 0);

	public bool IsActive { get; set; }

	public bool IsHeadLess { get; set; }

	public ViewOptions ViewOptions { get; set; } = new(0, 0, 0, 0);

	public List<Cell> Content { get; set; } = [];

	public List<IndexCell> Header { get; set; } = [];

	public List<IndexCell> Sider { get; set; } = [];

	public Cell CornerCell { get; set; } = default!;

	public HashSet<int> HiddenRows { get; set; } = [];

	public HashSet<int> HiddenColumns { get; set; } = [];

	public Dictionary<int, string> RowFilters { get; set; } = [];

	public Dictionary<int, string> ColumnFilters { get; set; } = [];

	public Dictionary<int, List<Cell>> Columns =>
		Shared.IndexArray(Size.Width).ToDictionary(x => x, ColumnAt);

	public Dictionary<int, List<Cell>> Rows =>
		Shared.IndexArray(Size.Height).ToDictionary(y => y, RowAt);

	public bool TryGetCellAt(Position position, [NotNullWhen(true)] out Cell? cell)
	{
		cell = position.IsBetween(new(0, 0), new(Size.Width - 1, Size.Height - 1)) is var isBetween && isBetween ? this[position] : null;

		return isBetween;
	}

	public Cell this[Position position] => this[position.X, position.Y];

	public Cell this[int x, int y]
	{
		get
		{
			ThrowIf<InvalidPositionException>(y < 0 || y >= Size.Height || x < 0 || x >= Size.Width, "Position is out of table");

			return Content[y * Size.Width + x];
		}
	}

	public Position this[Cell cell]
	{
		get
		{
			var index = Content.IndexOf(cell);

			return new Position(index % Size.Width, index / Size.Width);
		}
	}

	public List<Cell> this[int x1, int y1, int x2, int y2] =>
		[.. Shared.IndexArray(y2 - y1 + 1, y1).SelectMany(y =>
			Shared.IndexArray(x2 - x1 + 1, x1).Select(x => this[x, y]))];

	public List<Cell> this[Position position1, Position position2] => this[position1.X, position1.Y, position2.X, position2.Y];

	[JsonConstructor]
	private Table() { }

	public Table(Document document, string name, int columnCount, int rowCount)
	{
		Document = document;

		Name = name;

		CornerCell = new(this, @"y \ x");

		AddColumnAtCore(0, columnCount);
		AddRowAtCore(0, rowCount);

		ResetViewOptionsCore(false);

		ViewOptions.ViewChanged += OnViewChanged;
	}

	private void ResetViewOptionsCore(bool triggerEvent = true)
	{
		ViewOptions.Set(0, 0, Math.Max(Size.Width - 1, 0), Math.Max(Size.Height - 1, 0), triggerEvent);
	}

	public override void OnStateModifierCommandExecuted(IStateModifierCommandExecuter sender, StateModifierCommandExecutedEventArgs args)
	{
		if (args.IsPropagable)
		{
			InvokeStateModifierCommandExecutedEvent(args);
		}
	}

	public void OnViewChanged()
	{
		var fhi = GetFirstVisibleHeaderInView()?.Index;
		var lhi = GetLastVisibleHeaderInView()?.Index;
		var fsi = GetFirstVisibleSiderInView()?.Index;
		var lsi = GetLastVisibleSiderInView()?.Index;

		ViewChanged?.Invoke(fhi, lhi, fsi, lsi);
	}

	[OnDeserialized]
	public void OnDeserialized(StreamingContext _)
	{
		Content.ForEach(c => c.StateModifierCommandExecutedEvent += OnStateModifierCommandExecuted);

		ViewOptions.ViewChanged += OnViewChanged;
	}

	private void RemoveDeadCellReferences()
	{
		Content.Where(c =>
			c.ReferencedObject is not null &&
			!Content.Contains((Cell)c.ReferencedObject)).ForEach(c => c.ResetReferenceCell());
	}

	public List<Cell> ColumnAt(int x)
	{
		return this[x, 0, x, Size.Height - 1];
	}

	public List<Cell> RowAt(int y)
	{
		return this[0, y, Size.Width - 1, y];
	}

	private void AddNewContentCell(int index)
	{
		var cell = new Cell(this)
		{
			ContentColor = new(Settings.Current.DefaultContentColor)
		};

		Content.Insert(index, cell);

		cell.StateModifierCommandExecutedEvent += OnStateModifierCommandExecuted;
	}

	public int GetRowHeight(int index)
	{
		return Shared.Max(RowAt(index).Where(c =>
			c.Visibility.IsVisible).Max(c => c.GetSize().Height), Sider[index].GetSize().Height);
	}

	public int GetColumnWidth(int index)
	{
		return Shared.Max(ColumnAt(index).Where(c =>
			c.Visibility.IsVisible).Max(c => c.GetSize().Width), Header[index].GetSize().Width);
	}

	public int GetSiderWidth()
	{
		return IsHeadLess ? 0 : Shared.Max(Sider.Max(c => c.GetSize().Width), CornerCell.GetSize().Width);
	}

	public int GetHeaderHeight()
	{
		return IsHeadLess ? 0 : Shared.Max(Header.Max(c => c.GetSize().Height), CornerCell.GetSize().Height);
	}

	public Size GetTableSize()
	{
		var sumWidth = Shared.IndexArray(Size.Width).Where(x =>
			!IsColumnHidden(x) && IsColumnInView(x)).Sum(x => GetColumnWidth(x) - 1);
		var sumHeight = Shared.IndexArray(Size.Height).Where(y =>
			!IsRowHidden(y) && IsRowInView(y)).Sum(y => GetRowHeight(y) - 1);

		return new Size((IsHeadLess ? 0 : GetSiderWidth()) + sumWidth, (IsHeadLess ? 0 : GetHeaderHeight()) + sumHeight);
	}

	public Size GetContentCellSize(int x, int y)
	{
		return new Size(GetColumnWidth(x), GetRowHeight(y));
	}

	public Size GetHeaderCellSize(int x)
	{
		return IsHeadLess ? new(0, 0) : new(GetColumnWidth(x), GetHeaderHeight());
	}

	public Size GetSiderCellSize(int y)
	{
		return IsHeadLess ? new(0, 0) : new(GetSiderWidth(), GetRowHeight(y));
	}

	public Size GetCornerCellSize()
	{
		return IsHeadLess ? new(0, 0) : new(GetSiderWidth(), GetHeaderHeight());
	}

	public Position GetHeaderCellPosition(Size tableOffset, int x)
	{
		var sumWidth = x > 0 ? Shared.IndexArray(x).Where(x =>
			!IsColumnHidden(x) && IsColumnInView(x)).Sum(x => GetColumnWidth(x) - 1) : 0;

		return new Position(tableOffset.Width + GetSiderWidth() + sumWidth - 1, tableOffset.Height);
	}

	public Position GetSiderCellPosition(Size tableOffset, int y)
	{
		var sumHeight = y > 0 ? Shared.IndexArray(y).Where(y =>
			!IsRowHidden(y) && IsRowInView(y)).Sum(y => GetRowHeight(y) - 1) : 0;

		return new Position(tableOffset.Width, tableOffset.Height + GetHeaderHeight() + sumHeight - 1);
	}

	public Position GetContentCellPosition(Size tableOffset, int x, int y)
	{
		var sumWidth = x > 0 ? Shared.IndexArray(x).Where(x =>
			!IsColumnHidden(x) && IsColumnInView(x)).Sum(x => GetColumnWidth(x) - 1) : 0;

		var sumHeight = y > 0 ? Shared.IndexArray(y).Where(y =>
			!IsRowHidden(y) && IsRowInView(y)).Sum(y => GetRowHeight(y) - 1) : 0;

		return new Position(tableOffset.Width + (IsHeadLess ? 0 : GetSiderWidth() - 1) + sumWidth, tableOffset.Height + (IsHeadLess ? 0 : GetHeaderHeight() - 1) + sumHeight);
	}

	public bool IsColumnHidden(int x)
	{
		return ColumnAt(x).All(c => c.Visibility.IsColumnHidden);
	}

	public bool IsRowHidden(int y)
	{
		return RowAt(y).All(c => c.Visibility.IsRowHidden);
	}

	public bool IsColumnSelected(int index)
	{
		return index >= 0 && index < Size.Width && ColumnAt(index).Any(c => c.Selection.IsPrimarySelected);
	}

	public bool IsRowSelected(int index)
	{
		return index >= 0 && index < Size.Height && RowAt(index).Any(c => c.Selection.IsPrimarySelected);
	}

	public bool IsCellSelected(int x, int y)
	{
		return x > 0 && x < Size.Width && y > 0 && y < Size.Height && this[x, y].Selection.IsPrimarySelected;
	}

	public IndexCell? GetFirstVisibleHeaderInView()
	{
		return Header.FirstOrDefault(h => IsColumnInView(h.Index) && !IsColumnHidden(h.Index));
	}

	public IndexCell? GetLastVisibleHeaderInView()
	{
		return Header.LastOrDefault(h => IsColumnInView(h.Index) && !IsColumnHidden(h.Index));
	}

	public IndexCell? GetFirstVisibleSiderInView()
	{
		return Sider.FirstOrDefault(s => IsRowInView(s.Index) && !IsRowHidden(s.Index));
	}

	public IndexCell? GetLastVisibleSiderInView()
	{
		return Sider.LastOrDefault(s => IsRowInView(s.Index) && !IsRowHidden(s.Index));
	}

	public IEnumerable<Cell> GetPrimarySelectedCells()
	{
		return Content.Where(c => c.Selection.IsPrimarySelected);
	}

	public bool IsCellInView(int x = -1, int y = -1)
	{
		x = x == -1 ? ViewOptions.StartPosition.X : x;
		y = y == -1 ? ViewOptions.StartPosition.Y : y;

		return IsColumnInView(x) && IsRowInView(y);
	}

	public Position PositionInView(int x = -1, int y = -1)
	{
		return new Position(x - ViewOptions.StartPosition.X, y - ViewOptions.StartPosition.Y);
	}

	public bool IsColumnInView(int x)
	{
		return x >= ViewOptions.StartPosition.X && x <= ViewOptions.EndPosition.X;
	}

	public bool IsRowInView(int y)
	{
		return y >= ViewOptions.StartPosition.Y && y <= ViewOptions.EndPosition.Y;
	}

	public int GetMaxCellLayerIndex()
	{
		return Content.Max(c => c.LayerIndex);
	}

	public int GetMinCellLayerIndex()
	{
		return Content.Min(c => c.LayerIndex);
	}

	private void HideColumnAtCore(int x)
	{
		Header[x].Visibility.IsColumnHidden = true;
		ColumnAt(x).ForEach(c => c.Visibility.IsColumnHidden = true);
	}

	private void HideRowAtCore(int y)
	{
		Sider[y].Visibility.IsRowHidden = true;
		RowAt(y).ForEach(c => c.Visibility.IsRowHidden = true);
	}

	private void ShowColumnAtCore(int x)
	{
		Header[x].Visibility.IsColumnHidden = false;
		ColumnAt(x).ForEach(c => c.Visibility.IsColumnHidden = false);
	}

	private void ShowRowAtCore(int y)
	{
		Sider[y].Visibility.IsRowHidden = false;
		RowAt(y).ForEach(c => c.Visibility.IsRowHidden = false);
	}

	private void HideAndFilterCells()
	{
		Shared.IndexArray(Size.Height).ForEach(ShowRowAtCore);
		Shared.IndexArray(Size.Width).ForEach(ShowColumnAtCore);

		ApplyFilterCore(ColumnFilters, Columns, HideRowAtCore);
		ApplyFilterCore(RowFilters, Rows, HideColumnAtCore);

		HiddenColumns.ForEach(i => HideColumnAtCore(i));
		HiddenRows.ForEach(i => HideRowAtCore(i));

		ViewOptions.InvokeViewChangedEvent();
	}

	private static void ApplyFilterCore(Dictionary<int, string> filters, Dictionary<int, List<Cell>> cells, Action<int> action)
	{
		foreach (var filter in filters)
		{
			for (int i = 0; i < cells[filter.Key].Count; i++)
			{
				if (cells[filter.Key][i].ContentFunction is null ||
					cells[filter.Key][i].GetFormattedContents().All(v =>
						!Regex.IsMatch(v, filter.Value, RegexOptions.IgnoreCase)) &&
					cells[filter.Key][i].ContentFunction?.Execute().All(v =>
						!Regex.IsMatch(v?.ToString() ?? "", filter.Value, RegexOptions.IgnoreCase)) == true)
				{
					action(i);
				}
			}
		}
	}

	private void AddColumnAtCore(int index, int count)
	{
		var selectedCells = Content.Where(c => c.Selection.IsPrimarySelected).ToList();

		DeselectCellsCore(selectedCells);

		for (int i = 0; i < count; i++)
		{
			Header.Insert(index, new IndexCell(this, IndexCellType.Header, index, Settings.Current.IndexCellLeftArrow, Settings.Current.IndexCellRightArrow));

			for (int y = 0; y < Size.Height; y++)
			{
				AddNewContentCell(Size.Width * y + y + index);
			}

			Size.Width++;

			if (ViewOptions.EndPosition.X == Size.Width - 2)
			{
				ViewOptions.EndPosition.X++;
			}
		}

		Shared.IndexArray(Header.Count).ForEach(i => Header[i].Index = i);

		Content.ForEach(c =>
			c.ContentFunction?.ShiftReferenceArgumentPositions(Id, new(count, 0), new(index, 0), true));

		SelectCells(selectedCells);
	}

	private void AddRowAtCore(int index, int count)
	{
		var selectedCells = Content.Where(c => c.Selection.IsPrimarySelected).ToList();

		DeselectCellsCore(selectedCells);

		for (int i = 0; i < count; i++)
		{
			Sider.Insert(index, new IndexCell(this, IndexCellType.Sider, index, Settings.Current.IndexCellUpArrow, Settings.Current.IndexCellDownArrow));

			for (int x = 0; x < Size.Width; x++)
			{
				AddNewContentCell(index * Size.Width + x);
			}

			Size.Height++;

			if (ViewOptions.EndPosition.Y == Size.Height - 2)
			{
				ViewOptions.EndPosition.Y++;
			}
		}

		Shared.IndexArray(Sider.Count).ForEach(i => Sider[i].Index = i);

		Content.ForEach(c =>
			c.ContentFunction?.ShiftReferenceArgumentPositions(Id, new(0, count), new(0, index), true));

		SelectCells(selectedCells);
	}

	private void RemoveColumnAtCore(int index, int count)
	{
		var selectedCells = Content.Where(c => c.Selection.IsPrimarySelected).ToList();

		DeselectCellsCore(selectedCells);

		for (int i = 0; i < count; i++)
		{
			Header.RemoveAt(index);

			for (int y = 0; y < Size.Height; y++)
			{
				Content.RemoveAt(Size.Width * y - y + index);
			}

			Size.Width--;

			if (ViewOptions.EndPosition.X == Size.Width)
			{
				ViewOptions.EndPosition.X--;
			}
		}

		Shared.IndexArray(Header.Count).ForEach(i => Header[i].Index = i);

		Content.ForEach(c =>
			c.ContentFunction?.ShiftReferenceArgumentPositions(Id, new(-count, 0), new(index, 0), true));

		RemoveDeadCellReferences();

		SelectCells(selectedCells);
	}

	private void RemoveRowAtCore(int index, int count)
	{
		var selectedCells = Content.Where(c => c.Selection.IsPrimarySelected).ToList();

		DeselectCellsCore(selectedCells);

		for (int i = 0; i < count; i++)
		{
			Sider.RemoveAt(index);

			Content.RemoveRange(index * Size.Width, Size.Width);

			Size.Height--;

			if (ViewOptions.EndPosition.Y == Size.Height)
			{
				ViewOptions.EndPosition.Y--;
			}
		}

		Shared.IndexArray(Sider.Count).ForEach(i => Sider[i].Index = i);

		Content.ForEach(c =>
			c.ContentFunction?.ShiftReferenceArgumentPositions(Id, new(0, -count), new(0, index), true));

		RemoveDeadCellReferences();

		SelectCells(selectedCells);
	}

	private void DeselectAllCore()
	{
		Content.ForEach(c => c.Deselect());
	}

	private static void DeselectCellsCore(IEnumerable<Cell> cells)
	{
		cells.ForEach(c => c.Deselect());
	}
}