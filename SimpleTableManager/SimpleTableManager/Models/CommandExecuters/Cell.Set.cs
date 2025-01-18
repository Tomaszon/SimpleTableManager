using SimpleTableManager.Services.Functions;

namespace SimpleTableManager.Models.CommandExecuters;

public partial class Cell
{
	public void SetStringContent(params IEnumerable<string> contents)
	{
		ContentFunction = new StringFunction()
		{
			Operator = StringFunctionOperator.Const,
			Arguments = contents.Select(c => new ConstFunctionArgument<string>(c)).Cast<IFunctionArgument>().ToList()
		};
	}

	private void SetContent(IFunction? newFunction, Size? positionShift = default, bool selectDeselect = true)
	{
		if (positionShift is not null)
		{
			newFunction?.ShiftReferenceArgumentPositions(positionShift);
		}

		if (selectDeselect)
		{
			Deselect();
		}

		ContentFunction = newFunction;

		if (selectDeselect)
		{
			Select();
		}
	}

	private void SetFunction<T>(Enum functionOperator, params IEnumerable<IFunctionArgument> arguments)
		where T : IParsable<T>
	{
		var newFunction = FunctionCollection.GetFunction<T>(functionOperator, arguments);

		SetContent(newFunction);
	}

	private void SetFunction(Type valueType, string functionOperator, params IEnumerable<IFunctionArgument> arguments)
	{
		var newFunction = FunctionCollection.GetFunction(valueType, functionOperator, arguments);

		SetContent(newFunction);
	}

	[CommandFunction(WithSelector = true)]
	[CommandInformation("Sets the content function based on the type of the given arguments")]
	public void SetContent(Type type, [MinLength(1), ValueTypes<long, double, char, bool, TimeOnly, DateOnly, DateTime, string>] params IFunctionArgument[] contents)
	{
		SetFunction(type, "const", contents);
	}

	[CommandFunction]
	public void SetRectangleContentFunction(Shape2dOperator functionOperator, [ValueTypes<Rectangle>] params IFunctionArgument[] arguments)
	{
		SetFunction<Rectangle>(functionOperator, arguments);
	}

	[CommandFunction]
	public void SetEllipseContentFunction(Shape2dOperator functionOperator, [ValueTypes<Ellipse>] params IFunctionArgument[] arguments)
	{
		SetFunction<Ellipse>(functionOperator, arguments);
	}

	[CommandFunction]
	public void SetRightTriangleContentFunction(Shape2dOperator functionOperator, [ValueTypes<RightTriangle>] params IFunctionArgument[] arguments)
	{
		SetFunction<RightTriangle>(functionOperator, arguments);
	}

	[CommandFunction(WithSelector = true)]
	public void SetStringContentFunction(StringFunctionOperator functionOperator, [ValueTypes<string>] params IFunctionArgument[] arguments)
	{
		SetFunction<string>(functionOperator, arguments);
	}

	[CommandFunction(WithSelector = true)]
	public void SetIntegerContentFunction(NumericFunctionOperator functionOperator, [ValueTypes<long>] params IFunctionArgument[] arguments)
	{
		SetFunction<long>(functionOperator, arguments);
	}

	[CommandFunction]
	public void SetFractionContentFunction(NumericFunctionOperator functionOperator, [ValueTypes<double>] params IFunctionArgument[] arguments)
	{
		SetFunction<double>(functionOperator, arguments);
	}

	[CommandFunction]
	public void SetBooleanContentFunction(BooleanFunctionOperator functionOperator, [ValueTypes<bool>] params IFunctionArgument[] arguments)
	{
		SetFunction<bool>(functionOperator, arguments);
	}

	[CommandFunction]
	public void SetCharacterContentFunction(CharacterFunctionOperator functionOperator, [ValueTypes<char>] params IFunctionArgument[] arguments)
	{
		SetFunction<char>(functionOperator, arguments);
	}

	[CommandFunction]
	public void SetDateTimeContentFunction(DateTimeFunctionOperator functionOperator, [ValueTypes<DateTime>] params IFunctionArgument[] arguments)
	{
		SetFunction<DateTime>(functionOperator, arguments);
	}

	[CommandFunction]
	public void SetDateContentFunction(DateTimeFunctionOperator functionOperator, [ValueTypes<DateOnly>] params IFunctionArgument[] arguments)
	{
		SetFunction<DateOnly>(functionOperator, arguments);
	}

	[CommandFunction]
	public void SetTimeContentFunction(DateTimeFunctionOperator functionOperator, [ValueTypes<TimeOnly>] params IFunctionArgument[] arguments)
	{
		SetFunction<TimeOnly>(functionOperator, arguments);
	}

	[CommandFunction]
	public void SetContentFunctionOperator(string @operator)
	{
		//REWORK
		// ThrowIf<InvalidOperationException>(ContentFunction is null, "Content function is null!");

		// //TODO find a way to list possible values
		// ContentFunction.Operator = (Enum)ContentParser.ParseStringValue(ContentFunction.Operator.GetType(), @operator, null);
	}

	[CommandFunction]
	public void SetContentFunctionArguments(params string[] arguments)
	{
		//REWORK
		// ThrowIf<InvalidOperationException>(ContentFunction is null, "Content function is null!");

		// (var namedArgs, var args) = Shared.SeparateNamedArguments<string>(arguments);

		// ContentFunction.NamedArguments = namedArgs;

		// var argType = ContentFunction.GetInType();

		// var targetArray = FunctionCollection.ParseArgumentList(args, argType);

		// ContentFunction.Arguments = targetArray.Cast<object>();
	}

	[CommandFunction]
	public void SetVerticalAlignment(VerticalAlignment alignment)
	{
		ContentAlignment = new(ContentAlignment.Horizontal, alignment);
	}

	[CommandFunction]
	public void SetHorizontalAlignment(HorizontalAlignment alignment)
	{
		ContentAlignment = new(alignment, ContentAlignment.Vertical);
	}

	[CommandFunction]
	public void SetPadding([MinValue(0)] int top, [MinValue(0)] int bottom, [MinValue(0)] int left, [MinValue(0)] int right)
	{
		ContentPadding = new(top, bottom, left, right);
	}

	[CommandFunction]
	public void SetVerticalPadding(int top, int bottom)
	{
		ThrowIf(top < 0 || bottom < 0, "Padding can not be less then 0!");

		ContentPadding = new(top, bottom, ContentPadding.Left, ContentPadding.Right);
	}

	[CommandFunction]
	public void SetHorizontalPadding(int left, int right)
	{
		ThrowIf(left < 0 || right < 0, "Padding can not be less then 0!");

		ContentPadding = new(ContentPadding.Top, ContentPadding.Bottom, left, right);
	}

	[CommandFunction]
	public void SetBorderColor(ConsoleColor foreground, ConsoleColor? background = null)
	{
		if (LayerIndex == 0)
		{
			SetLayerIndexToMax();
		}

		BorderColor = new(foreground, background);
	}

	[CommandFunction]
	public void SetBackgroundColor(ConsoleColor foreground, ConsoleColor? background = null)
	{
		BackgroundColor = new(foreground, background);
	}

	[CommandFunction]
	public void SetContentColor(ConsoleColor foreground, ConsoleColor? background = null)
	{
		ContentColor = new(foreground, background);
	}

	[CommandFunction]
	public void SetBackgroundCharacter(char character = ' ')
	{
		BackgroundCharacter = character;
	}

	[CommandFunction]
	public void IncreaseLayerIndex()
	{
		LayerIndex = (int)Math.Min(int.MaxValue, (long)LayerIndex + 1);
	}

	[CommandFunction]
	public void DecreaseLayerIndex()
	{
		LayerIndex = (int)Math.Max(int.MinValue, (long)LayerIndex - 1);
	}

	[CommandFunction]
	public void SetLayerIndex(int value)
	{
		LayerIndex = value;
	}

	[CommandFunction]
	public void SetLayerIndexToMax()
	{
		LayerIndex = Table.GetMaxCellLayerIndex();
		IncreaseLayerIndex();
	}

	[CommandFunction]
	public void SetLayerIndexToMin()
	{
		LayerIndex = Table.GetMinCellLayerIndex();
		DecreaseLayerIndex();
	}

	[CommandFunction(StateModifier = false)]
	public void SetComment(string[] comments)
	{
		Comments = [.. comments];
	}

	[CommandFunction(IgnoreReferencedObject = true)]
	public void SetReferenceCell(Position position)
	{
		ReferencedObject = Table[position.X, position.Y];
	}

	[CommandFunction]
	public void SetContentStyle(ContentStyle style)
	{
		ContentStyle = style;
	}
}