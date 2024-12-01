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
			catch (OperationCanceledException ex)
			{
				return _cachedFormattedContent = ex.Message.Wrap();
			}
			catch
			{
				return _cachedFormattedContent = ContentFunction!.GetError().Wrap();
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
			SetPrimitiveContent(contents);
		}

		StateModifierCommandExecuted += OnStateModifierCommandExecuted;
	}

	public static bool TrySeparateArgumentsAs<TType>(string[] arguments, [NotNullWhen(true)] out (Dictionary<ArgumentName, IFunctionArgument>, IEnumerable<IFunctionArgument>)? result, [NotNullWhen(true)] out Type? resultType)
	where TType : IParsable<TType>
	{
		try
		{
			result = SeparateArgumentsAs<TType>(arguments);

			resultType = typeof(TType);

			return true;
		}
		catch
		{
			result = null;

			resultType = null;

			return false;
		}
	}

	public static (Dictionary<ArgumentName, IFunctionArgument>, IEnumerable<IFunctionArgument>) SeparateArgumentsAs<TType>(string[] arguments)
	where TType : IParsable<TType>
	{
		var namedArgs = arguments.Where(a => a.Contains(Shared.NAMED_ARG_SEPARATOR) == true);

		var regularArgs = ContentParser.ParseFunctionArguments<TType>(arguments.Where(a => !namedArgs.Contains(a)));

		var namedArgsDic = namedArgs.ToDictionary(
			k => Enum.Parse<ArgumentName>(k.Split(Shared.NAMED_ARG_SEPARATOR)[0], true),
			v => ContentParser.ParseFunctionArgument<string>(v.Split(Shared.NAMED_ARG_SEPARATOR)[1]));

		return (namedArgsDic, regularArgs);
	}

	[OnDeserialized]
	public void OnDeserialized(StreamingContext _)
	{
		StateModifierCommandExecuted += OnStateModifierCommandExecuted;
	}

	public override void OnStateModifierCommandExecuted(IStateModifierCommandExecuter sender, IStateModifierCommandExecuter root)
	{
		_cachedFormattedContent = Enumerable.Empty<string>();

		ContentFunction?.ClearError();
	}
}