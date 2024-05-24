using SimpleTableManager.Services;
using SimpleTableManager.Services.Functions;

namespace SimpleTableManager.Models;

public partial class Cell
{
	private void SetFunction<T>(Enum functionOperator, params string[] arguments)
		where T : IParsable<T>
	{
		var args = Shared.SeparateNamedArguments<T>(arguments);

		ContentFunction = FunctionCollection.GetFunction<T>(functionOperator.ToString(), args.Item1, args.Item2.Cast<object>());
	}

	[CommandReference]
	public void SetContent(params string[] contents)
	{
		ThrowIf(contents.Length == 0, "Argument count must be greater then 0!");

		var args = Shared.SeparateNamedArguments<string>(contents);

		args.Item1.TryGetValue(ArgumentName.Type, out var typeName);

		ContentFunction = FunctionCollection.GetFunction(typeName ?? "string", "const", null, args.Item2);
	}

	[CommandReference]
	public void SetAreaContentFunction(AreaFunctionOperator functionOperator, params string[] arguments)
	{
		SetFunction<Shape>(functionOperator, arguments);
	}

	[CommandReference]
	public void SetStringContentFunction(StringFunctionOperator functionOperator, params string[] arguments)
	{
		SetFunction<string>(functionOperator, arguments);
	}

	[CommandReference]
	public void SetIntegerContentFunction(NumericFunctionOperator functionOperator, params string[] arguments)
	{
		SetFunction<int>(functionOperator, arguments);
	}

	[CommandReference]
	public void SetDecimalContentFunction(NumericFunctionOperator functionOperator, params string[] arguments)
	{
		SetFunction<decimal>(functionOperator, arguments);
	}

	[CommandReference]
	public void SetBooleanContentFunction(BooleanFunctionOperator functionOperator, params string[] arguments)
	{
		SetFunction<bool>(functionOperator, arguments);
	}

	[CommandReference]
	public void SetCharacterContentFunction(CharacterFunctionOperator functionOperator, params string[] arguments)
	{
		SetFunction<char>(functionOperator, arguments);
	}

	[CommandReference]
	public void SetDateTimeContentFunction(DateTimeFunctionOperator functionOperator, params string[] arguments)
	{
		SetFunction<DateTime>(functionOperator, arguments);
	}

	[CommandReference]
	public void SetDateContentFunction(DateTimeFunctionOperator functionOperator, params string[] arguments)
	{
		SetFunction<DateOnly>(functionOperator, arguments);
	}

	[CommandReference]
	public void SetTimeContentFunction(DateTimeFunctionOperator functionOperator, params string[] arguments)
	{
		SetFunction<TimeOnly>(functionOperator, arguments);
	}

	[CommandReference]
	public void SetContentFunctionOperator(string @operator)
	{
		ThrowIf<InvalidOperationException>(ContentFunction is null, "Content function is null!");

		//TODO find a way to list possible values
		ContentFunction = FunctionCollection.GetFunction(ContentFunction.GetInType().Name, @operator, ContentFunction.NamedArguments, ContentFunction.Arguments);
	}

	[CommandReference]
	public void SetContentFunctionArguments(params string[] arguments)
	{
		ThrowIf<InvalidOperationException>(ContentFunction is null, "Content function is null!");

		(var namedArgs, var args) = Shared.SeparateNamedArguments<string>(arguments);

		ContentFunction = FunctionCollection.GetFunction(ContentFunction.GetInType().Name, ContentFunction.Operator.ToString(), namedArgs.Count > 0 ? namedArgs : ContentFunction.NamedArguments, args.Any() ? args.Cast<object>() : ContentFunction.Arguments);
	}

	[CommandReference]
	public void SetVerticalAlignment(VerticalAlignment alignment)
	{
		ContentAlignment.Vertical = alignment;
	}

	[CommandReference]
	public void SetHorizontalAlignment(HorizontalAlignment alignment)
	{
		ContentAlignment.Horizontal = alignment;
	}

	[CommandReference]
	public void SetPadding(int top, int bottom, int left, int right)
	{
		ThrowIf(top < 0 || bottom < 0 || left < 0 || right < 0, "Padding can not be less then 0!");

		ContentPadding = new(top, bottom, left, right);
	}

	[CommandReference]
	public void SetVerticalPadding(int top, int bottom)
	{
		ThrowIf(top < 0 || bottom < 0, "Padding can not be less then 0!");

		ContentPadding = new(top, bottom, ContentPadding.Left, ContentPadding.Right);
	}

	[CommandReference]
	public void SetHorizontalPadding(int left, int right)
	{
		ThrowIf(left < 0 || right < 0, "Padding can not be less then 0!");

		ContentPadding = new(ContentPadding.Top, ContentPadding.Bottom, left, right);
	}

	[CommandReference]
	public void SetBorderColor(ConsoleColor foreground, ConsoleColor? background = null)
	{
		if (LayerIndex == 0)
		{
			SetLayerIndexToMax();
		}

		BorderColor = (foreground, background);
	}

	[CommandReference]
	public void SetContentColor(ConsoleColor foreground, ConsoleColor? background = null)
	{
		ContentColor = (foreground, background);
	}

	[CommandReference]
	public void IncreaseLayerIndex()
	{
		LayerIndex = (int)Math.Min(int.MaxValue, (long)LayerIndex + 1);
	}

	[CommandReference]
	public void DecreaseLayerIndex()
	{
		LayerIndex = (int)Math.Max(int.MinValue, (long)LayerIndex - 1);
	}

	[CommandReference]
	public void SetLayerIndex(int value)
	{
		LayerIndex = value;
	}

	[CommandReference]
	public void SetLayerIndexToMax()
	{
		LayerIndex = Table.GetMaxCellLayerIndex();
		IncreaseLayerIndex();
	}

	[CommandReference]
	public void SetLayerIndexToMin()
	{
		LayerIndex = Table.GetMinCellLayerIndex();
		DecreaseLayerIndex();
	}

	[CommandReference(StateModifier = false)]
	public void SetComment(string comment)
	{
		Comment = comment;
	}
}