using System;
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
			BorderColorIndex = Table.GetMaxCellBorderColorIndex() + 1;

			BorderColor = (foreground, background);
		}
	}
}
