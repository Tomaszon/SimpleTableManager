﻿using System.Runtime.Serialization;

using SimpleTableManager.Services.Functions;

namespace SimpleTableManager.Models.CommandExecuters;

[CommandInformation("Cell related commands")]
[JsonObject(IsReference = true)]
public partial class Cell : CommandExecuterBase, IFormatProvider
{
	public Guid Id { get; set; } = Guid.NewGuid();

	[JsonIgnore]
	private IEnumerable<string> _cachedFormattedContent = [];

	public Table Table { get; set; } = default!;

	/// <summary>
	/// Manually set size not including the borders
	/// </summary>
	public Size GivenSize { get; set; } = new(7, 1);

	public IFunction? ContentFunction { get; set; }

	public CellSelection Selection { get; set; } = new();

	public CellVisibility Visibility { get; set; } = new();

	public ContentPadding ContentPadding { get; set; } = new();

	public ContentAlignment ContentAlignment { get; set; } = (HorizontalAlignment.Center, VerticalAlignment.Center);

	public ContentStyle ContentStyle { get; set; } = ContentStyle.Normal;

	public ConsoleColorSet ContentColor { get; set; } = new(Settings.Current.DefaultContentColor);

	public ConsoleColorSet BackgroundColor { get; set; } = new(Settings.Current.DefaultBackgroundColor);

	public ConsoleColorSet BorderColor { get; set; } = new(Settings.Current.DefaultBorderColor);

	public char BackgroundCharacter { get; set; } = Settings.Current.DefaultCellBackgroundCharacter;

	public int LayerIndex { get; set; } = 0;

	public List<string> Comments { get; set; } = [];

	public bool IsContentColorDefault => ContentColor == Settings.Current.DefaultContentColor;

	public bool IsBorderColorDefault => BorderColor == Settings.Current.DefaultBorderColor;

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
				return _cachedFormattedContent = ContentFunction?.ExecuteAndFormat() ?? [];
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
	private Cell() { }

	public Cell(Table table, params string[] contents)
	{
		Table = table;

		if (contents?.Length > 0)
		{
			SetStringContent(contents);
		}

		StateModifierCommandExecutedEvent += OnStateModifierCommandExecuted;
	}

	public bool TrySeparateArgumentsAs<TType>(string[] arguments, [NotNullWhen(true)] out (Dictionary<ArgumentName, IFunctionArgument>, IEnumerable<IFunctionArgument>)? result, [NotNullWhen(true)] out Type? resultType)
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

	public (Dictionary<ArgumentName, IFunctionArgument>, IEnumerable<IFunctionArgument>) SeparateArgumentsAs<TType>(string[] arguments)
	where TType : IParsable<TType>
	{
		var namedArgs = arguments.Where(a => a.Contains(Shared.NAMED_ARG_SEPARATOR) == true);

		var regularArgs = ContentParser.ParseFunctionArguments<TType>(this, arguments.Where(a => !namedArgs.Contains(a)));

		var namedArgsDic = namedArgs.ToDictionary(
			k => Enum.Parse<ArgumentName>(k.Split(Shared.NAMED_ARG_SEPARATOR)[0], true),
			v => ContentParser.ParseFunctionArgument<string>(this, v.Split(Shared.NAMED_ARG_SEPARATOR)[1]));

		return (namedArgsDic, regularArgs);
	}

	public void Select()
	{
		Selection.SelectPrimary();

		var doc = InstanceMap.Instance.GetInstance<Document>()!;

		ContentFunction?.ReferenceArguments.Select(a => a.Reference).ForEach(r =>
		{
			var table = doc[r.ReferencedTableId];

			var c = table[r.ReferencedPosition];

			c.Selection.SelectSecondary();

			TertiarySelectionRecursive(c, false);
		});
	}

	public void Deselect()
	{
		Selection.DeselectPrimary();

		var doc = InstanceMap.Instance.GetInstance<Document>()!;

		ContentFunction?.ReferenceArguments.Select(a => a.Reference).ForEach(r =>
		{
			var table = doc[r.ReferencedTableId];

			var c = table[r.ReferencedPosition];

			c.Selection.DeselectSecondary();

			TertiaryDeselectionRecursive(c, false);
		});
	}

	private static void TertiarySelectionRecursive(Cell cell, bool selectSelf)
	{
		var doc = InstanceMap.Instance.GetInstance<Document>()!;

		if (cell.ContentFunction is not null)
		{
			cell.ContentFunction.ReferenceArguments.Select(a => a.Reference).ForEach(r => TertiarySelectionRecursive(doc[r.ReferencedTableId][r.ReferencedPosition], false));

			if (selectSelf)
			{
				cell.Selection.SelectTertiary();
			}
		}
		else if (selectSelf)
		{
			cell.Selection.SelectTertiary();
		}
	}

	private static void TertiaryDeselectionRecursive(Cell cell, bool selectSelf)
	{
		var doc = InstanceMap.Instance.GetInstance<Document>()!;

		if (cell.ContentFunction is not null)
		{
			cell.ContentFunction.ReferenceArguments.Select(a => a.Reference).ForEach(r => TertiaryDeselectionRecursive(doc[r.ReferencedTableId][r.ReferencedPosition], false));

			if (selectSelf)
			{
				cell.Selection.DeselectTertiary();
			}
		}
		else if (selectSelf)
		{
			cell.Selection.DeselectTertiary();
		}
	}

	[OnDeserialized]
	public void OnDeserialized(StreamingContext _)
	{
		StateModifierCommandExecutedEvent += OnStateModifierCommandExecuted;
	}

	public override void OnStateModifierCommandExecuted(IStateModifierCommandExecuter sender, StateModifierCommandExecutedEventArgs arg)
	{
		_cachedFormattedContent = [];

		ContentFunction?.ClearError();
	}

	public object GetFormat(Type? formatType)
	{
		return this;
	}
}