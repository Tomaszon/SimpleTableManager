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
			if (contents.FirstOrDefault() is string first && first is not null && first.StartsWith('='))
			{
				_content = new();

				var rest = contents.Skip(1).Select(e =>
					Position.TryParse(e.ToString(), out var position) ?
						new FunctionParameter(null, position) : new FunctionParameter(e));

				ContentFunction = FunctionCollection.GetFunction(first.TrimStart('='), rest);
			}
			else
			{
				_contentFunction = null;

				Content = contents.ToList();
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
			Padding = new ContentPadding(top, bottom, left, right);
		}

		[CommandReference]
		public void SetVerticalPadding(int top, int bottom)
		{
			Padding = new ContentPadding(top, bottom, Padding.Left, Padding.Right);
		}

		[CommandReference]
		public void SetHorizontalPadding(int left, int right)
		{
			Padding = new ContentPadding(Padding.Top, Padding.Bottom, left, right);
		}

		[CommandReference]
		public void SetType(string typeName)
		{
			var type = Shared.GetTypeByName(typeName);

			ContentType = type;
		}
	}
}
