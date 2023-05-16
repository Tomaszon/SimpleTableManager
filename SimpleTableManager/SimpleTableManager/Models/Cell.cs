using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using SimpleTableManager.Models.Attributes;
using SimpleTableManager.Services;
using SimpleTableManager.Services.Functions;

namespace SimpleTableManager.Models
{
	[CommandInformation("Cell related commands")]
	public partial class Cell : INotifyPropertyChanged
	{
		public Table Table { get; set; }

		/// <summary>
		/// Manually set size not including the borders
		/// </summary>
		public Size GivenSize { get; set; } = new Size(7, 1);

		/// <summary>
		/// Size not including the borders
		/// </summary>
		public Size GetContentSize()
		{
			var content = GetContents();

			if (content is { } && content.Count > 0)
			{
				return new Size(content.Max(e => e.ToString().Length), content.Count);
			}
			else
			{
				return new Size(1, 1);
			}
		}

		/// <summary>
		/// Max of <see cref="GetContentSize"/> and <see cref="GivenSize"/> plus border size
		/// </summary>
		public Size GetSize()
		{
			var contentSize = GetContentSize();

			return new Size
				(
					Shared.Max(contentSize.Width + 2 + ContentPadding.Left + ContentPadding.Right, GivenSize.Width + 2),
					Shared.Max(contentSize.Height + 2 + ContentPadding.Top + ContentPadding.Bottom, GivenSize.Height + 2)
				);
		}

		public Type ContentType { get; set; } = typeof(string);

		public List<object> GetContents()
		{
			var result = Table.ExecuteCellFunctionWithParameters(this, out var contentType);

			return result.Select(r => r.Value).Where(r => r is not null).Select(r =>
			{
				var b = ContentType.IsAssignableFrom(r.GetType());

				return b ? r : Shared.ParseStringValue(ContentType.Name, r.ToString());

			}).ToList();
		}

		public IFunction ContentFunction { get; set; }

		public IFunction2 ContentFunction2 { get; set; }

		public bool IsSelected { get; set; }

		public CellVisibility Visibility { get; set; } = new CellVisibility();

		public ContentPadding ContentPadding { get; set; } = new ContentPadding();

		public ContentAlignment ContentAlignment { get; set; } = (HorizontalAlignment.Center, VerticalAlignment.Center);

		public ConsoleColorSet ContentColor { get; set; } = new ConsoleColorSet(Settings.Current.DefaultContentColor);

		public ConsoleColorSet BorderColor { get; set; } = new ConsoleColorSet(Settings.Current.DefaultBorderColor);

		public int LayerIndex { get; set; } = 0;

		public bool IsContentColorDefault => ContentColor.Equals(Settings.Current.DefaultContentColor);

		public bool IsBorderColorDefault => BorderColor.Equals(Settings.Current.DefaultBorderColor);

		public event PropertyChangedEventHandler PropertyChanged;

		[JsonConstructor]
		private Cell()
		{

		}

		public Cell(Table table, params object[] contents)
		{
			Table = table;

			SetContent(contents);
		}

		public void ClearContent()
		{
			ContentFunction = ObjectFunction.Empty();
		}

		public void AddArgumentFirst(params object[] contents)
		{
			ContentFunction.InsertArguments(0, contents);
		}

		public void AddArgumentLast(params object[] contents)
		{
			ContentFunction.AddArguments(contents);
		}

		public void RemoveLastArgument()
		{
			ContentFunction.RemoveLastArgument();
		}

		private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
