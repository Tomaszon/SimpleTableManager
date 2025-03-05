using System.Runtime.Serialization;

using SimpleTableManager.Services.Functions;

namespace SimpleTableManager.Models.CommandExecuters;

[CommandInformation("Cell related commands")]
[JsonObject(IsReference = true)]
public partial class Cell : CommandExecuterBase
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

	public ContentAlignment ContentAlignment { get; set; } = new(HorizontalAlignment.Center, VerticalAlignment.Center);

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

		return contents is { } && contents.Any() ?
			new Size(contents.Max(e => e?.Length ?? 0), contents.Count()) :
			new Size(1, 1);
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

	[JsonConstructor]
	private Cell() { }

	public Cell(Table table, params string[] contents)
	{
		Table = table;

		if (contents?.Length > 0)
		{
			SetStringContent(contents);
		}

		table.ViewOptions.ViewChanged += OnTableViewChanged;
		StateModifierCommandExecutedEvent += OnStateModifierCommandExecuted;
	}

	public void Select()
	{
		Selection.SelectPrimary();

		var doc = InstanceMap.Instance.GetInstance<Document>()!;

		ContentFunction?.UnnamedReferenceArguments.Select(a => a.Reference).ForEach(r =>
		{
			var table = doc[r.ReferencedTableId];

			r.ReferencedPositions.ForEach(p =>
			{
				if (table.TryGetCellAt(p, out var c) && c != this)
				{
					c.Selection.SelectSecondary();

					TertiarySelectionRecursive(c, false, this);
				}
			});
		});
	}

	public void Deselect()
	{
		Selection.DeselectPrimary();

		var doc = InstanceMap.Instance.GetInstance<Document>()!;

		ContentFunction?.UnnamedReferenceArguments.Select(a => a.Reference).ForEach(r =>
		{
			var table = doc[r.ReferencedTableId];

			r.ReferencedPositions.ForEach(p =>
			{
				if (table.TryGetCellAt(p, out var c) && c != this)
				{
					c.Selection.DeselectSecondary();

					TertiaryDeselectionRecursive(c, false, this);
				}
			});
		});
	}

	private static void TertiarySelectionRecursive(Cell cell, bool selectSelf, Cell root)
	{
		var doc = InstanceMap.Instance.GetInstance<Document>()!;

		if (cell.ContentFunction is not null)
		{
			cell.ContentFunction.UnnamedReferenceArguments.Select(a => a.Reference).ForEach(r =>
			{
				var table = doc[r.ReferencedTableId];

				r.ReferencedPositions.ForEach(p =>
				{
					if (table.TryGetCellAt(p, out var c) && c != cell && c != root)
					{
						TertiarySelectionRecursive(c, false, root);
					}
				});
			});

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

	private static void TertiaryDeselectionRecursive(Cell cell, bool selectSelf, Cell root)
	{
		var doc = InstanceMap.Instance.GetInstance<Document>()!;

		if (cell.ContentFunction is not null)
		{
			cell.ContentFunction.UnnamedReferenceArguments.Select(a => a.Reference).ForEach(r =>
			{
				var table = doc[r.ReferencedTableId];

				r.ReferencedPositions.ForEach(p =>
				{
					if (table.TryGetCellAt(p, out var c) && c != cell && c != root)
					{
						TertiaryDeselectionRecursive(c, false, root);
					}
				});
			});

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
		Table.ViewOptions.ViewChanged += OnTableViewChanged;
		StateModifierCommandExecutedEvent += OnStateModifierCommandExecuted;

		UpdateReferenceSubscription();
	}

	public override void OnStateModifierCommandExecuted(IStateModifierCommandExecuter sender, StateModifierCommandExecutedEventArgs arg)
	{
		_cachedFormattedContent = [];

		ContentFunction?.ClearError();

		if (sender == this)
		{
			return;
		}

		if (arg.Root != this)
		{
			InvokeStateModifierCommandExecutedEvent(arg);
		}
		else
		{
			ContentFunction?.SetError("!CIRCULAR");
		}
	}

	public void OnTableViewChanged()
	{
		UpdateReferenceSubscription();

		InvokeStateModifierCommandExecutedEvent(new(this));
	}

	private void UpdateReferenceSubscription()
	{
		UpdateReferenceSubscription(ContentFunction, ContentFunction);
	}

	private void UpdateReferenceSubscription(IFunction? oldFunction, IFunction? newFunction)
	{
		oldFunction?.ReferenceArguments.ForEach(a =>
		{
			if (a.TryGetReferencedCells(out var referencedCells))
			{
				referencedCells.ForEach(c =>
					c.StateModifierCommandExecutedEvent -= OnStateModifierCommandExecuted);
			}
		});

		newFunction?.ReferenceArguments.ForEach(a =>
		{
			if (a.TryGetReferencedCells(out var referencedCells))
			{
				referencedCells.ForEach(c =>
					c.StateModifierCommandExecutedEvent += OnStateModifierCommandExecuted);
			}
		});
	}
}