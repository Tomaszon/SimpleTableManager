using System;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;

using SimpleTableManager.Extensions;
using SimpleTableManager.Services;

namespace SimpleTableManager.Models
{
	public partial class Cell
	{
		/// <summary>
		/// Manually set size including the borders
		/// </summary>
		public Size GivenSize { get; set; } = new Size(0, 0);

		/// <summary>
		/// Size not including the borders
		/// </summary>
		public Size ContentSize => Content is { } && Content.Count > 0 ?
			new Size(Content.Max(e => e.ToString().Length), Content.Count) : new Size(1, 1);

		/// <summary>
		/// Max of <see cref="ContentSize"/> and <see cref="GivenSize"/>
		/// </summary>
		public Size Size => new Size
			(
				Shared.Max(ContentSize.Width, GivenSize.Width),
				Shared.Max(ContentSize.Height, GivenSize.Height)
			);

		public Type ContentType { get; set; } = typeof(string);

		private List<object> _content = new();
		public List<object> Content
		{
			get => _content;
			set
			{
				_content = value.Select(e =>
				{
					var b = ContentType.IsAssignableFrom(e.GetType());

					return b ? e : Shared.ParseStringValue(ContentType.Name, e.ToString());
				}).ToList();
			}
		}

		public bool IsSelected { get; set; }

		public bool IsHidden { get; set; }

		public ContentPadding Padding { get; set; } = new ContentPadding();

		public HorizontalAlignment HorizontalAlignment { get; set; } = HorizontalAlignment.Center;

		public VertialAlignment VertialAlignment { get; set; } = VertialAlignment.Center;

		public ConsoleColor ForegroundColor { get; set; } = Settings.Current.DefaultCellForegroundColor;

		public ConsoleColor BackgroundColor { get; set; } = Settings.Current.DefaultCellBackgroundColor;

		public ConsoleColor BorderForegroundColor { get; set; } = Settings.Current.DefaultBorderForegroundColor;

		public ConsoleColor BorderBackgroundColor { get; set; } = Settings.Current.DefaultBorderBackgroundColor;

		[JsonConstructor]
		private Cell()
		{

		}

		public Cell(params object[] contents)
		{
			SetContent(contents);
		}

		public void ClearContent()
		{
			Content.Clear();
		}

		public void AddContents(params object[] contents)
		{
			contents.ForEach(c => Content.Add(c));
		}

		public void RemoveContent()
		{
			Content.RemoveAt(Content.Count - 1);
		}
	}
}
