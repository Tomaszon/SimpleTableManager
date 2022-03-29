﻿using System;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;

using SimpleTableManager.Extensions;
using SimpleTableManager.Services;

namespace SimpleTableManager.Models
{
	public partial class Cell
	{
		public Size ContentSize => Content is { } && Content.Count > 0 ?
			new Size(Content.Max(e => e.ToString().Length + 2), Content.Count) : new Size(0, 0);

		public Size GivenSize { get; set; } = new Size(0, 0);

		public Size Size =>
			new Size(Math.Max(ContentSize.Width, GivenSize.Width), Math.Max(ContentSize.Height, GivenSize.Height));

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

		public ConsoleColor ForegroundColor { get; set; } = Settings.Current.DefaultCellForegroundColor;

		public ConsoleColor BackgroundColor { get; set; } = Settings.Current.DefaultCellBackgroundColor;

		public VertialAlignment VertialAlignment { get; set; } = VertialAlignment.Center;

		public HorizontalAlignment HorizontalAlignment { get; set; } = HorizontalAlignment.Center;

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
