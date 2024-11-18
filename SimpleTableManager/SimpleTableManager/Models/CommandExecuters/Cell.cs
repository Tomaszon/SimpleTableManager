using System.Runtime.Serialization;

using SimpleTableManager.Services;
using SimpleTableManager.Services.Functions;

namespace SimpleTableManager.Models.CommandExecuters;

[CommandInformation("Cell related commands")]
public partial class Cell : CommandExecuterBase
{
	[JsonIgnore]
	private IEnumerable<string> _cachedFormattedContent = Enumerable.Empty<string>();

	public Table Table { get; set; } = default!;

	/// <summary>
	/// Manually set size not including the borders
	/// </summary>
	public Size GivenSize { get; set; } = new(7, 1);

	public IFunction? ContentFunction { get; set; }

	public bool IsSelected { get; set; }

	public CellVisibility Visibility { get; set; } = new();

	//TODO not rendered properly
	public ContentPadding ContentPadding { get; set; } = new();

	public ContentAlignment ContentAlignment { get; set; } = (HorizontalAlignment.Center, VerticalAlignment.Center);

	public ContentStyle ContentStyle { get; set; } = ContentStyle.Normal;

	public ConsoleColorSet ContentColor { get; set; } = new(Settings.Current.DefaultContentColor);

	public ConsoleColorSet BackgroundColor { get; set; } = new(Settings.Current.DefaultBackgroundColor);

	public ConsoleColorSet BorderColor { get; set; } = new(Settings.Current.DefaultBorderColor);

	public char BackgroundCharacter { get; set; } = Settings.Current.DefaultCellBackgroundCharacter;

	public int LayerIndex { get; set; } = 0;

	public string? Comment { get; set; }

	[JsonIgnore]
	public bool IsContentColorDefault => ContentColor.Equals(Settings.Current.DefaultContentColor);

	[JsonIgnore]
	public bool IsBorderColorDefault => BorderColor.Equals(Settings.Current.DefaultBorderColor);

	/// <summary>
	/// Size not including the borders
	/// </summary>
	public Size GetContentSize()
	{
		var contents = GetFormattedContents();

		if (contents is { } && contents.Any())
		{
			return new Size(contents.Max(e => e?.Length ?? 0), contents.Count());
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

	public IEnumerable<string> GetFormattedContents()
	{
		if (_cachedFormattedContent.Any())
		{
			return _cachedFormattedContent;
		}
		else
		{
			try
			{
				return _cachedFormattedContent = ContentFunction?.ExecuteAndFormat() ?? Enumerable.Empty<string>();
			}
			catch
			{
				return _cachedFormattedContent = new[] { "Content function error" };
			}
		}
	}

	[JsonConstructor]
	public Cell() { }

	public Cell(Table table, params string[] contents)
	{
		Table = table;

		if (contents?.Length > 0)
		{
			SetContent(contents);
		}

		StateModifierCommandExecuted += OnStateModifierCommandExecuted;
	}

	[OnDeserialized]
	public void OnDeserialized(StreamingContext _)
	{
		StateModifierCommandExecuted += OnStateModifierCommandExecuted;
	}

	public override void OnStateModifierCommandExecuted()
	{
		_cachedFormattedContent = Enumerable.Empty<string>();
	}
}