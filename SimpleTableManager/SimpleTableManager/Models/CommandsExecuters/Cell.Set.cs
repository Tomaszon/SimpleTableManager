﻿using SimpleTableManager.Services;
using SimpleTableManager.Services.Functions;

namespace SimpleTableManager.Models.CommandExecuters;

public partial class Cell
{
	private void SetFunction<T>(Enum functionOperator, params string[] arguments)
		where T : IParsable<T>
	{
		var args = Shared.SeparateNamedArguments<T>(arguments);

		ContentFunction = FunctionCollection.GetFunction<T>(functionOperator.ToString(), args.Item1, args.Item2.Cast<object>());
	}

	[CommandFunction]
	public void SetContent(params string[] contents)
	{
		ThrowIf(contents.Length == 0, "Argument count must be greater then 0!");

		var args = Shared.SeparateNamedArguments<string>(contents);

		args.Item1.TryGetValue(ArgumentName.Type, out var typeName);

		ContentFunction = FunctionCollection.GetFunction(typeName ?? "string", "const", args.Item1, args.Item2);
	}

	[CommandFunction]
	public void SetRectangleContentFunction(Shape2dOperator functionOperator, params string[] arguments)
	{
		SetFunction<Rectangle>(functionOperator, arguments);
	}

	[CommandFunction]
	public void SetEllipseContentFunction(Shape2dOperator functionOperator, params string[] arguments)
	{
		SetFunction<Ellipse>(functionOperator, arguments);
	}

	[CommandFunction]
	public void SetRightTriangleContentFunction(Shape2dOperator functionOperator, params string[] arguments)
	{
		SetFunction<RightTriangle>(functionOperator, arguments);
	}

	[CommandFunction]
	public void SetStringContentFunction(StringFunctionOperator functionOperator, params string[] arguments)
	{
		SetFunction<string>(functionOperator, arguments);
	}

	[CommandFunction]
	public void SetIntegerContentFunction(NumericFunctionOperator functionOperator, params string[] arguments)
	{
		SetFunction<int>(functionOperator, arguments);
	}

	[CommandFunction]
	public void SetDecimalContentFunction(NumericFunctionOperator functionOperator, params string[] arguments)
	{
		SetFunction<decimal>(functionOperator, arguments);
	}

	[CommandFunction]
	public void SetBooleanContentFunction(BooleanFunctionOperator functionOperator, params string[] arguments)
	{
		SetFunction<bool>(functionOperator, arguments);
	}

	[CommandFunction]
	public void SetCharacterContentFunction(CharacterFunctionOperator functionOperator, params string[] arguments)
	{
		SetFunction<char>(functionOperator, arguments);
	}

	[CommandFunction]
	public void SetDateTimeContentFunction(DateTimeFunctionOperator functionOperator, params string[] arguments)
	{
		SetFunction<DateTime>(functionOperator, arguments);
	}

	[CommandFunction]
	public void SetDateContentFunction(DateTimeFunctionOperator functionOperator, params string[] arguments)
	{
		SetFunction<DateOnly>(functionOperator, arguments);
	}

	[CommandFunction]
	public void SetTimeContentFunction(DateTimeFunctionOperator functionOperator, params string[] arguments)
	{
		SetFunction<TimeOnly>(functionOperator, arguments);
	}

	[CommandFunction]
	public void SetContentFunctionOperator(string @operator)
	{
		ThrowIf<InvalidOperationException>(ContentFunction is null, "Content function is null!");

		//TODO find a way to list possible values
		ContentFunction.Operator = (Enum)ContentParser.ParseStringValue(ContentFunction.Operator.GetType(), @operator);
	}

	[CommandFunction]
	public void SetContentFunctionArguments(params string[] arguments)
	{
		ThrowIf<InvalidOperationException>(ContentFunction is null, "Content function is null!");

		(var namedArgs, var args) = Shared.SeparateNamedArguments<string>(arguments);

		ContentFunction.NamedArguments = namedArgs;

		var argType = ContentFunction.Arguments.GetType().GenericTypeArguments.First();

		var targetArray = FunctionCollection.ParseArgumentList(args, argType);

		ContentFunction.Arguments = targetArray.Cast<object>();
	}

	[CommandFunction]
	public void SetVerticalAlignment(VerticalAlignment alignment)
	{
		ContentAlignment.Vertical = alignment;
	}

	[CommandFunction]
	public void SetHorizontalAlignment(HorizontalAlignment alignment)
	{
		ContentAlignment.Horizontal = alignment;
	}

	[CommandFunction]
	public void SetPadding(int top, int bottom, int left, int right)
	{
		ThrowIf(top < 0 || bottom < 0 || left < 0 || right < 0, "Padding can not be less then 0!");

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

		BorderColor = (foreground, background);
	}

	[CommandFunction]
	public void SetBackgroundColor(ConsoleColor foreground, ConsoleColor? background = null)
	{
		BackgroundColor = (foreground, background);
	}

	[CommandFunction]
	public void SetContentColor(ConsoleColor foreground, ConsoleColor? background = null)
	{
		ContentColor = (foreground, background);
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
	public void SetComment(string comment)
	{
		Comment = comment;
	}

	[CommandFunction(IgnoreReferencedObject = true)]
	public void SetReferenceCell(Position position)
	{
		ReferencedObject = Table[position.X, position.Y];
	}
}