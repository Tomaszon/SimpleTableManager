using SimpleTableManager.Services.Functions;

namespace SimpleTableManager.Models.CommandExecuters;

public partial class Cell
{
	public void SetStringContent(params IEnumerable<string> contents)
	{
		ContentFunction = new StringFunction()
		{
			Operator = StringFunctionOperator.Const,
			Arguments = [.. contents.Select(c => new ConstFunctionArgument<string>(c)).Cast<IFunctionArgument>()]
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

		ContentFunction?.ReferenceArguments.ForEach(a =>
			a.GetReferencedCells().ForEach(c =>
				c.StateModifierCommandExecutedEvent -= OnStateModifierCommandExecuted));

		newFunction?.ReferenceArguments.ForEach(a =>
			a.GetReferencedCells().ForEach(c =>
				c.StateModifierCommandExecutedEvent += OnStateModifierCommandExecuted));

		ContentFunction = newFunction;

		if (selectDeselect)
		{
			Select();
		}
	}

	private void SetFunction<T, T2>(T2 functionOperator, IEnumerable<IFunctionArgument> arguments1, IEnumerable<IFunctionArgument>? arguments2 = null)
		where T : IFunction, new()
		where T2 : Enum
	{
		var arguments = arguments2 is not null ? arguments1.Union(arguments2) : arguments1;

		SetContent(new T() { Arguments = [.. arguments], Operator = functionOperator });
	}

	private void SetFunction(Type valueType, string functionOperator, params IEnumerable<IFunctionArgument> arguments)
	{
		var newFunction = FunctionCollection.GetFunction(valueType, functionOperator, arguments);

		SetContent(newFunction);
	}

	[CommandFunction(WithSelector = true)]
	[CommandInformation("Sets the content function based on the type of the given arguments")]
	public void SetContent(Type type, [MinLength(1), ValueTypes<long, double, char, FormattableBoolean, ConvertibleTimeOnly, ConvertibleDateOnly, DateTime, Rectangle, Ellipse, RightTriangle, string>] params IFunctionArgument[] contents)
	{
		SetFunction(type, functionOperator: "const", contents);
	}

	[CommandFunction(WithSelector = true)]
	public void SetRectangleContentFunction(Shape2dOperator functionOperator, [MinLength(1), ValueTypes<Rectangle>] params IFunctionArgument[] arguments)
	{
		SetFunction<Shape2dFunction, Shape2dOperator>(functionOperator, arguments);
	}

	[CommandFunction(WithSelector = true)]
	public void SetEllipseContentFunction(Shape2dOperator functionOperator, [MinLength(1), ValueTypes<Ellipse>] params IFunctionArgument[] arguments)
	{
		SetFunction<Shape2dFunction, Shape2dOperator>(functionOperator, arguments);
	}

	[CommandFunction(WithSelector = true)]
	public void SetRightTriangleContentFunction(Shape2dOperator functionOperator, [MinLength(1), ValueTypes<RightTriangle>] params IFunctionArgument[] arguments)
	{
		SetFunction<Shape2dFunction, Shape2dOperator>(functionOperator, arguments);
	}

	[CommandFunction(WithSelector = true)]
	public void SetStringContentFunction(StringFunctionOperator functionOperator, [MinLength(1), ValueTypes<string>] params IFunctionArgument[] arguments)
	{
		SetFunction<StringFunction, StringFunctionOperator>(functionOperator, arguments);
	}

	[CommandFunction(WithSelector = true)]
	public void SetIntegerContentFunction(NumericFunctionOperator functionOperator, [MinLength(1), ValueTypes<long>] params IFunctionArgument[] arguments)
	{
		SetFunction<IntegerNumericFunction, NumericFunctionOperator>(functionOperator, arguments);
	}

	[CommandFunction(WithSelector = true)]
	public void SetFractionContentFunction(NumericFunctionOperator functionOperator, [MinLength(1), ValueTypes<double>] params IFunctionArgument[] arguments)
	{
		SetFunction<FractionNumericFunction, NumericFunctionOperator>(functionOperator, arguments);
	}

	[CommandFunction(WithSelector = true)]
	public void SetBooleanContentFunction(BooleanFunctionOperator functionOperator, [MinLength(1), ValueTypes<FormattableBoolean>] params IFunctionArgument[] arguments)
	{
		SetFunction<BooleanFunction, BooleanFunctionOperator>(functionOperator, arguments);
	}

	[CommandFunction(WithSelector = true)]
	public void SetCharacterContentFunction(CharacterFunctionOperator functionOperator, [MinLength(1), ValueTypes<char>] params IFunctionArgument[] arguments)
	{
		SetFunction<CharacterFunction, CharacterFunctionOperator>(functionOperator, arguments);
	}

	[CommandFunction(WithSelector = true)]
	public void SetDateTimeContentFunction(DateTimeFunctionOperator functionOperator, [ValueTypes<DateTime>] params IFunctionArgument[] arguments)
	{
		var args = functionOperator == DateTimeFunctionOperator.Now ?
			arguments.Where(a => a.IsNamed).Append(new ConstFunctionArgument<DateTime>(DateTime.Now)) :
			arguments;

		SetFunction<DateTimeFunction, DateTimeFunctionOperator>(functionOperator, args);
	}

	[CommandFunction(WithSelector = true)]
	public void SetDateContentFunction(DateTimeFunctionOperator functionOperator, [ValueTypes<ConvertibleDateOnly>] params IFunctionArgument[] arguments)
	{
		var args = functionOperator == DateTimeFunctionOperator.Now ?
			arguments.Where(a => a.IsNamed).Append(new ConstFunctionArgument<ConvertibleDateOnly>(DateOnly.FromDateTime(DateTime.Now))) :
			arguments;

		SetFunction<DateFunction, DateTimeFunctionOperator>(functionOperator, args);
	}

	[CommandFunction(WithSelector = true)]
	public void SetTimeContentFunction(DateTimeFunctionOperator functionOperator, [ValueTypes<ConvertibleTimeOnly>] params IFunctionArgument[] arguments)
	{
		var args = functionOperator == DateTimeFunctionOperator.Now ?
			arguments.Where(a => a.IsNamed).Append(new ConstFunctionArgument<ConvertibleTimeOnly>(TimeOnly.FromDateTime(DateTime.Now))) :
			arguments;

		SetFunction<TimeFunction, DateTimeFunctionOperator>(functionOperator, args);
	}

	[CommandFunction(WithSelector = true)]
	public void SetChartContentFunction(ChartFunctionOperator functionOperator, [ValueTypes<int, string>, GroupingId('X')] IFunctionArgument[] x, [ValueTypes<int, string>, GroupingId('Y')] IFunctionArgument[]? y = null)
	{
		SetFunction<ChartFunction, ChartFunctionOperator>(functionOperator, x, y);
	}

	[CommandFunction]
	public void SetContentFunctionOperator([MinLength(1)] string @operator)
	{
		ThrowIf<InvalidOperationException>(ContentFunction is null, "Content function is null!");

		//TODO find a way to list possible values
		ContentFunction.Operator = (Enum)ContentParser.ParseStringValue(ContentFunction.Operator.GetType(), @operator, null)!;
	}

	[CommandFunction(WithSelector = true)]
	//TODO check what happens in case of IShape
	public void SetContentFunctionArguments(Type type, [ValueTypes<long, double, char, FormattableBoolean, ConvertibleTimeOnly, ConvertibleDateOnly, DateTime, Rectangle, Ellipse, RightTriangle, string>] params IEnumerable<IFunctionArgument> arguments)
	{
		ThrowIf<InvalidOperationException>(validator: ContentFunction is null, "Content function is null!");

		var argType = ContentFunction.GetInType();

		ThrowIfNot(type.IsAssignableTo(argType), $"Arguments of type '{argType.GetFriendlyName()}' expected");

		ContentFunction.Arguments = [.. arguments];
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
	public void SetVerticalPadding([MinValue(0)] int top, [MinValue(0)] int bottom)
	{
		ContentPadding = new(top, bottom, ContentPadding.Left, ContentPadding.Right);
	}

	[CommandFunction]
	public void SetHorizontalPadding([MinValue(0)] int left, [MinValue(0)] int right)
	{
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