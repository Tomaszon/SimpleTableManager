namespace SimpleTableManager.Models;

[ParseFormat("TableName,{0}x,{0}y ({0} for axis unlock)", "(?<t>.+),(?<x>{1}?\\d+),(?<y>{1}?\\d+)", [Shared.REF_CHAR, Shared.REGEX_REF_CHAR])]
[ParseFormat("{0}x,{0}y ({0} for axis unlock)", "(?<x>{1}?\\d+),(?<y>{1}?\\d+)", [Shared.REF_CHAR, Shared.REGEX_REF_CHAR])]
public class CellReference : ParsableBase<CellReference>, IParsable<CellReference>, IParseCore<CellReference>
{
	public Table Table { get; set; }

	public Cell Cell { get; set; }

	[JsonProperty]
	private readonly Size? _offset;

	[JsonProperty]
	private readonly Position? _position;

	public Position ReferencedPosition
	{
		get
		{
			var ownerCellPosition = Table[Cell];

			return new(HorizontallyLocked ? _position.X : ownerCellPosition.X + _offset.Width, VerticallyLocked ? _position.Y : ownerCellPosition.Y + _offset.Height);
		}
	}

	[MemberNotNullWhen(true, nameof(_position))]
	[MemberNotNullWhen(false, nameof(_offset))]
	public bool HorizontallyLocked { get; set; }

	[MemberNotNullWhen(true, nameof(_position))]
	[MemberNotNullWhen(false, nameof(_offset))]
	public bool VerticallyLocked { get; set; }

	[JsonConstructor]
	private CellReference() { Table = null!; Cell = null!; }

	public CellReference(Table table, Cell cell, Position position, bool horizontallyLocked = true, bool verticallyLocked = true)
	{
		Table = table;
		Cell = cell;
		HorizontallyLocked = horizontallyLocked;
		VerticallyLocked = verticallyLocked;

		var cellPosition = table[cell];

		if (horizontallyLocked)
		{
			_position = new Position(position.X, 0);
		}
		else
		{
			_offset = new Size(position.X - cellPosition.X, 0);
		}

		if (verticallyLocked)
		{
			_position ??= new Position(0, 0);
			_position.Y = position.Y;
		}
		else
		{
			_offset ??= new Size(0, 0);
			_offset.Height = position.Y - cellPosition.Y;
		}
	}

	public override string ToString()
	{
		return $"T:{Table.Name}, X:{(HorizontallyLocked ? "" : Shared.REF_CHAR)}{ReferencedPosition.X}, Y:{(VerticallyLocked ? "" : Shared.REF_CHAR)}{ReferencedPosition.Y}";
	}

	public string ToShortString()
	{
		return $"{Table.Name}:{(HorizontallyLocked ? "" : Shared.REF_CHAR)}{ReferencedPosition.X}:{(VerticallyLocked ? "" : Shared.REF_CHAR)}{ReferencedPosition.Y}";
	}

	public static CellReference ParseCore(GroupCollection args, IFormatProvider? formatProvider)
	{
		var tns = args["t"].Value;
		var xs = args["x"].Value;
		var ys = args["y"].Value;

		var tn = tns;
		var x = int.Parse(xs.Trim(Shared.REF_CHAR));
		var y = int.Parse(ys.Trim(Shared.REF_CHAR));

		var doc = InstanceMap.Instance.GetInstance<Document>()!;

		var t = string.IsNullOrEmpty(tn) ?
			doc.GetActiveTable() :
			doc.Tables.Single(t => t.Name.Equals(tn, StringComparison.InvariantCultureIgnoreCase));

		return new CellReference(t, (Cell)formatProvider!, new Position(x, y), !xs.Contains(Shared.REF_CHAR), !ys.Contains(Shared.REF_CHAR));
	}
}