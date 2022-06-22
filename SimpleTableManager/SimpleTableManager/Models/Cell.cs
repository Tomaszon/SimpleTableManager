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
		/// <summary>
		/// Manually set size not including the borders
		/// </summary>
		public Size GivenSize { get; set; } = new Size(7, 1);

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
				Shared.Max(ContentSize.Width + 2 + ContentPadding.Left + ContentPadding.Right, GivenSize.Width + 2),
				Shared.Max(ContentSize.Height + 2 + ContentPadding.Top + ContentPadding.Bottom, GivenSize.Height + 2)
			);

		private Type _contentType = typeof(string);
		public Type ContentType
		{
			get => _contentType;
			set
			{
				_contentType = value;

				Content = _content.Select(c => (object)c).ToList();
			}
		}

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

				NotifyPropertyChanged();
			}
		}

		private IFunction _contentFunction;
		public IFunction ContentFunction
		{
			get => _contentFunction;
			set
			{
				_contentFunction = value;

				NotifyPropertyChanged(nameof(ContentFunction));
			}
		}

		public bool IsSelected { get; set; }

		public bool IsHidden { get; set; }

		public ContentPadding ContentPadding { get; set; } = new ContentPadding();

		public ContentAlignment ContentAlignment { get; set; } = (HorizontalAlignment.Center, VerticalAlignment.Center);

		public ConsoleColorSet ContentColor { get; set; } = new ConsoleColorSet(Settings.Current.DefaultContentColor);

		public ConsoleColorSet BorderColor { get; set; } = new ConsoleColorSet(Settings.Current.DefaultBorderColor);

		public bool IsContentColorDefault => ContentColor.Equals(Settings.Current.DefaultContentColor);

		public bool IsBorderColorDefault => BorderColor.Equals(Settings.Current.DefaultBorderColor);

		public event PropertyChangedEventHandler PropertyChanged;

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

		public void AddContentsFirst(params object[] contents)
		{
			Content.InsertRange(0, contents);
		}

		public void AddContentsLast(params object[] contents)
		{
			Content.AddRange(contents);
		}

		public void RemoveContent()
		{
			Content.RemoveAt(Content.Count - 1);
		}

		[CommandReference]
		public object ShowDetails()
		{
			var funcDic = ContentFunction is not null ? 
				ContentFunction.GetType().GetProperties().ToDictionary(k => k.Name, v => v.GetValue(ContentFunction)) : null;

			return new
			{
				Size = Size.ToString(),
				ContentType = ContentType.Name,
				ContentFunction = funcDic is not null ? 
					$"{funcDic[nameof(Function<Enum, object>.TypeName)]}:{funcDic[nameof(Function<Enum, object>.Operator)]}" : null,
				Padding = ContentPadding.ToString(),
				Alignment = ContentAlignment.ToString()
			};
		}

		[CommandReference]
		public object ShowContentFunction()
		{
			return ContentFunction;
		}

		private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
