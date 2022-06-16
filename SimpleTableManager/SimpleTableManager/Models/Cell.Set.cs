using System.Linq;
using SimpleTableManager.Models.Attributes;
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
				var rest = contents[1..].Select(e =>
				{
					if (Position.TryParse((string)e, out var position))
					{
						return new FunctionParameter(null, position);
					}
					else
					{
						return new FunctionParameter((string)e);
					}

				}).ToArray();

				ContentFunction = FunctionCollection.GetFunction(first.TrimStart('='), rest);
			}
			else
			{
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
	}
}
