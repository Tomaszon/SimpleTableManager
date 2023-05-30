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

			if (content is { } && content.Count() > 0)
			{
				return new Size(content.Max(e => e.ToString().Length), content.Count());
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

		public Type ContentType => ContentFunction?.GetReturnType() ?? typeof(string);

		public IEnumerable<object> GetContents()
		{
			try
			{
				return ContentFunction?.Execute() ?? Enumerable.Empty<object>();//Table.ExecuteCellFunctionWithParameters
			}
			catch
			{
				return new object[] { "Content function error" };
			}
		}

		public IFunction ContentFunction { get; set; }

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

		public Cell(Table table, params string[] contents)
		{
			Table = table;

			SetContent(contents);
		}

		// public void ClearContent()
		// {
		// 	ContentFunction2 = ObjectFunction.Empty();
		// }

		// public void AddArgumentFirst(params object[] contents)
		// {
		// 	ContentFunction.InsertArguments(0, contents);
		// }

		// public void AddArgumentLast(params object[] contents)
		// {
		// 	ContentFunction.AddArguments(contents);
		// }

		// public void RemoveLastArgument()
		// {
		// 	ContentFunction.RemoveLastArgument();
		// }

		private void SetFunction<T>(Enum functionOperator, params string[] arguments)
			where T : IParsable<T>
		{
			var args = Shared.SeparateNamedArguments<T>(arguments);

			ContentFunction = FunctionCollection.GetFunction(typeof(T).Name, functionOperator.ToString(), args.Item1, args.Item2.Cast<object>());
		}

		private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
