using SimpleTableManager.Services;
using SimpleTableManager.Services.Functions;

namespace SimpleTableManager.Models
{
	public partial class Cell
	{
		[CommandReference]
		public void SetContent(params string[] contents)
		{
			var args = Shared.SeparateNamedArguments<string>(contents);

			args.Item1.TryGetValue(ArgumentName.Type, out var typeName);

			ContentFunction = contents?.Length > 0 ?
				ContentFunction = FunctionCollection.GetFunction(typeName ?? "string", "const", null, args.Item2) :
				null;
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
			ContentPadding = new ContentPadding(top, bottom, left, right);
		}

		[CommandReference]
		public void SetVerticalPadding(int top, int bottom)
		{
			ContentPadding = new ContentPadding(top, bottom, ContentPadding.Left, ContentPadding.Right);
		}

		[CommandReference]
		public void SetHorizontalPadding(int left, int right)
		{
			ContentPadding = new ContentPadding(ContentPadding.Top, ContentPadding.Bottom, left, right);
		}

		[CommandReference]
		public void SetType(string typeName)
		{
			// TODO make it work with functions, but how?
			//fix asd
			// var type = Shared.GetTypeByName(typeName);
			// SetContent(GetContents().Select(c => Shared.ParseStringValue(type, c.ToString())));
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
	}
}
