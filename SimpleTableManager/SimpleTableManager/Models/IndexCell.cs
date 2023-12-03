namespace SimpleTableManager.Models;

public class IndexCell : Cell
{
	public char LowerArrow { get; set; }

	public char HigherArrow { get; set; }

	public int Index { get; set; }

	public IndexCell(Table table, int index, char lowerArrow, char higherArrow) : base(table, index.ToString())
	{
		Index = index;
		LowerArrow = lowerArrow;
		HigherArrow = higherArrow;
	}

	public void AppendHigherEllipsis(int width)
	{
		SetContent($"{GetFormattedContents().ElementAt(0)} {HigherArrow} {width - 1}");
	}

	public void AppendLowerEllipsis()
	{
		SetContent($"0 {LowerArrow} {GetFormattedContents().ElementAt(0)}");
	}

	public void RemoveEllipses()
	{
		if (ContentFunction?.Execute().First().ToString() != Index.ToString())
		{
			SetContent(Index.ToString());
		}
	}
}