using System.Linq;

using SimpleTableManager.Extensions;
using SimpleTableManager.Models.Attributes;
using SimpleTableManager.Services;

namespace SimpleTableManager.Models
{
	public partial class Table
	{
		[CommandReference]
		public void RemoveRowAt(int index)
		{
			Shared.Validate(() => index >= 0 && index <= Size.Height - 1, $"Index is not in the needed range: [0, {Size.Height - 1}]");

			Content.RemoveRange(index * Size.Width, Size.Width);

			if (ViewOptions.EndPosition.Y == Size.Height - 1)
			{
				ViewOptions.DecreaseHeight();
			}

			Size.Height--;
		}

		[CommandReference]
		public void RemoveFirstRow()
		{
			RemoveRowAt(0);
		}

		[CommandReference]
		public void RemoveLastRow()
		{
			RemoveRowAt(Size.Height - 1);
		}

		[CommandReference]
		public void RemoveColumnAt(int index)
		{
			Shared.Validate(() => index >= 0 && index <= Size.Width - 1, $"Index is not in the needed range: [0, {Size.Width - 1}]");

			for (int y = 0; y < Size.Height; y++)
			{
				Content.RemoveAt(Size.Width * y - y + index);
			}

			if (ViewOptions.EndPosition.X == Size.Width - 1)
			{
				ViewOptions.DecreaseWidth();
			}

			Size.Width--;
		}

		[CommandReference]
		public void RemoveFirstColumn()
		{
			RemoveColumnAt(0);
		}

		[CommandReference]
		public void RemoveLastColumn()
		{
			RemoveColumnAt(Size.Width - 1);
		}

		[CommandReference]
		public void RemoveCellContent()
		{
			Content.Where(c => c.IsSelected).ForEach(c => c.RemoveContent());
		}
	}
}
