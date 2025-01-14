namespace SimpleTableManager.Models;

//TODO move these formats to ReferenceFunctionArgument
[ParseFormat("TableName,{0}x1,{0}y1-{0}x2,{0}y2 ({0} for axis unlock)",
	"^(?<t>.+),(?<x1>{1}?\\d+),(?<y1>{1}?\\d+)-(?<x2>{1}?\\d+),(?<y2>{1}?\\d+)$",
	[Shared.REF_CHAR, Shared.REGEX_REF_CHAR])]
[ParseFormat("{0}x1,{0}y1-{0}x2,{0}y2 ({0} for axis unlock)",
	"^(?<x1>{1}?\\d+),(?<y1>{1}?\\d+)-(?<x2>{1}?\\d+),(?<y2>{1}?\\d+)$",
	[Shared.REF_CHAR, Shared.REGEX_REF_CHAR])]
[ParseFormat("TableName,{0}x,{0}y ({0} for axis unlock)",
	"^(?<t>.+),(?<x>{1}?\\d+),(?<y>{1}?\\d+)$",
	[Shared.REF_CHAR, Shared.REGEX_REF_CHAR])]
[ParseFormat("{0}x,{0}y ({0} for axis unlock)",
	"^(?<x>{1}?\\d+),(?<y>{1}?\\d+)$",
	[Shared.REF_CHAR, Shared.REGEX_REF_CHAR])]
[method: JsonConstructor]
public class CellReference(Guid referencedTableId, LockablePosition positionFrom, LockablePosition positionTo) : ParsableBase<CellReference>, IParsable<CellReference>, IParseCore<CellReference>
{
	public Guid ReferencedTableId { get; set; } = referencedTableId;

	public LockablePosition PositionFrom { get; set; } = positionFrom;

	public LockablePosition PositionTo { get; set; } = positionTo;

	public List<Position> ReferencedPositions => Position.Range(PositionFrom, PositionTo);

	public CellReference(Guid referencedTableId, LockablePosition position) :
		this(referencedTableId, position, position)
	{ }

	public void ShiftReferencedPositions(Size size)
	{
		PositionFrom.Shift(size);
		PositionTo.Shift(size);
	}

	public override string ToString()
	{
		var doc = InstanceMap.Instance.GetInstance<Document>()!;

		var table = doc[ReferencedTableId].Name;

		if (PositionFrom.Equals(PositionTo))
		{
			return $"T:{table},{PositionFrom}";
		}
		else
		{
			return $"T:{table},{PositionFrom}-{PositionTo}";
		}
	}

	public string ToShortString()
	{
		var doc = InstanceMap.Instance.GetInstance<Document>()!;

		var table = doc[ReferencedTableId].Name;

		if (PositionFrom.Equals(PositionTo))
		{
			return $"{table}:{PositionFrom.ToShortString()}";
		}
		else
		{
			return $"{table}:{PositionFrom.ToShortString()}-{PositionTo.ToShortString()}";
		}
	}

	public static CellReference ParseCore(GroupCollection args, IFormatProvider? formatProvider)
	{
		var targ = args["t"];
		var xarg = args["x"];
		var yarg = args["y"];
		var x1s = args["x1"].Value;
		var y1s = args["y1"].Value;
		var x2s = args["x2"].Value;
		var y2s = args["y2"].Value;

		var doc = InstanceMap.Instance.GetInstance<Document>()!;

		var t = !targ.Success ?
			doc.GetActiveTable() :
			doc.Tables.Single(t => t.Name.Equals(targ.Value, StringComparison.InvariantCultureIgnoreCase));

		if (xarg.Success && yarg.Success)
		{
			var x = int.Parse(xarg.Value.Trim(Shared.REF_CHAR));
			var y = int.Parse(yarg.Value.Trim(Shared.REF_CHAR));

			return new CellReference(t.Id, new LockablePosition(x, y, !xarg.Value.Contains(Shared.REF_CHAR), !yarg.Value.Contains(Shared.REF_CHAR)));
		}
		else
		{
			var x1 = int.Parse(x1s.Trim(Shared.REF_CHAR));
			var y1 = int.Parse(y1s.Trim(Shared.REF_CHAR));
			var x2 = int.Parse(x2s.Trim(Shared.REF_CHAR));
			var y2 = int.Parse(y2s.Trim(Shared.REF_CHAR));

			return new CellReference(t.Id, new LockablePosition(x1, y1, !x1s.Contains(Shared.REF_CHAR), !y1s.Contains(Shared.REF_CHAR)), new LockablePosition(x2, y2, !x2s.Contains(Shared.REF_CHAR), !y2s.Contains(Shared.REF_CHAR)));
		}
	}
}