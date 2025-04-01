using SimpleTableManager.Services.Functions;

namespace SimpleTableManager.Models.CommandExecuters;

public partial class Cell
{
	[CommandFunction, CommandShortcut("cutCellContent")]
	public void CutContent()
	{
		Table.Document.GlobalStorage.Add(GlobalStorageKey.CellContent, (Table[this], ContentFunction));

		ResetContent();
	}


	[CommandFunction]
	//TODO check what happens in case of IShape
	public void AddContentFunctionArguments([MinLength(1), ValueTypes<long, double, char, FormattableBoolean, ConvertibleTimeOnly, ConvertibleDateOnly, DateTime, ConvertibleTimeSpan, Rectangle, Ellipse, RightTriangle, string>] params IEnumerable<IFunctionArgument> arguments)
	{
		ThrowIf<InvalidOperationException>(validator: ContentFunction is null, "Content function is null!");

		ContentFunction.Arguments.AddRange(arguments);
	}

	[CommandFunction]
	public void RemoveContentFunctionArguments([MinLength(1)] params IEnumerable<int> positions)
	{
		ThrowIf<InvalidOperationException>(validator: ContentFunction is null, "Content function is null!");

		positions.OrderDescending().ForEach(p => ContentFunction.Arguments.RemoveAt(p));
	}
}