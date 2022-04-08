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
		/// Manually set size not including the borders
		/// </summary>
		public Size GivenSize { get; set; } = new Size(1, 1);

		/// <summary>
		/// Size not including the borders
		/// </summary>
		public Size ContentSize => Content is { } && Content.Count > 0 ?
			new Size(Content.Max(e => e.ToString().Length), Content.Count) : new Size(1, 1);

		/// <summary>
		/// Max of <see cref="ContentSize"/> and <see cref="GivenSize"/> plus border size
		/// </summary>
		public Size Size => new Size
			(
				Shared.Max(ContentSize.Width + 2 + Padding.Left + Padding.Right, GivenSize.Width + 2),
				Shared.Max(ContentSize.Height + 2 + Padding.Top + Padding.Bottom, GivenSize.Height + 2)
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

		public ContentAlignment ContentAlignment { get; set; } = (HorizontalAlignment.Center, VerticalAlignment.Center);

		public ConsoleColorSet ContentColor { get; set; } = new ConsoleColorSet(Settings.Current.DefaultContentColor);

		public ConsoleColorSet BorderColor { get; set; } = new ConsoleColorSet(Settings.Current.DefaultBorderColor);

		public bool IsContentColorDefault => ContentColor.Equals(Settings.Current.DefaultContentColor);

		public bool IsBorderColorDefault => BorderColor.Equals(Settings.Current.DefaultBorderColor);

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
