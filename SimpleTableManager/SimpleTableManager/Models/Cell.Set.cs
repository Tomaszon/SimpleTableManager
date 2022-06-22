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

			var explicitFunc = FunctionCollection.HasFunction(funcName, out var t); 

			if (explicitFunc)
			{
				var args = contents.Skip(1).Select(e =>
					Position.TryParse(e.ToString(), out var position) ?
						new FunctionParameter(null, position) : new FunctionParameter(e));

				ContentFunction = FunctionCollection.GetFunction(funcName, args);
			}
			else
			{
				var args = contents.Select(e => new FunctionParameter(e));

				ContentFunction = FunctionCollection.GetFunction(ObjectFunctionOperator.Const, args);
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
	}
}
