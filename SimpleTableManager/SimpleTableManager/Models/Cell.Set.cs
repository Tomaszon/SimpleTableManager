using System;
using System.Collections.Generic;
using System.Linq;

using SimpleTableManager.Models.Attributes;
using SimpleTableManager.Services;
using SimpleTableManager.Services.Functions;

namespace SimpleTableManager.Models
{
	public partial class Cell
	{
		[CommandReference]
		public void SetContent(params object[] contents)
		{
			var funcName = contents.FirstOrDefault()?.ToString();

			var explicitFunc = FunctionCollection.HasFunction(funcName, out _);

			if (contents.Length > 1)
			{
				ContentFunction = explicitFunc ?
					FunctionCollection.GetFunction(funcName, contents.Skip(1).Select(p =>
						p is IFunction f && f is not null ? f : new ObjectFunction(p))) :
					FunctionCollection.GetFunctionCore(typeof(ObjectFunction), ObjectFunctionOperator.Const, contents.Select(p =>
						p is IFunction f && f is not null ? f : new ObjectFunction(p)));
			}
			else if (contents.Length == 1)
			{
				ContentFunction = contents.First() is IFunction f && f is not null ? f :
					new ObjectFunction(contents.First());
			}
			else
			{
				ContentFunction = ObjectFunction.Empty();
			}
		}

		[CommandReference]
		public void SetContent2(params object[] contents)
		{
			ContentFunction2 = FunctionCollection2.GetFunction(ContentType.Name, "const", null, contents);
		}

		[CommandReference]
		public void SetStringContentFunction(StringFunctionOperator functionOperator, params string[] arguments)
		{
			var args = Shared.SeparateNamedArguments<string>(arguments);

			ContentFunction2 = new StringFunction2(functionOperator, args.Item1, args.Item2);
		}

		[CommandReference]
		public void SetDecimalContentFunction(NumericFunctionOperator functionOperator, params string[] arguments)
		{
			var args = Shared.SeparateNamedArguments<decimal>(arguments);

			ContentFunction2 = new DecimalNumericFunction2(functionOperator, args.Item1, args.Item2);
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
			var type = Shared.GetTypeByName(typeName);

			ContentType = type;
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
