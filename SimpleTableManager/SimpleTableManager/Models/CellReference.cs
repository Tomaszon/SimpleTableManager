namespace SimpleTableManager.Models;

[method: JsonConstructor]
public class CellReference(Guid referencedTableId, LockablePosition positionFrom, LockablePosition positionTo)
{
	public Guid ReferencedTableId { get; set; } = referencedTableId;

	public LockablePosition PositionFrom { get; set; } = positionFrom;

	public LockablePosition PositionTo { get; set; } = positionTo;

	public List<Position> ReferencedPositions => Position.Range(PositionFrom, PositionTo);

	public CellReference(Guid referencedTableId, LockablePosition position) :
		this(referencedTableId, position, position)
	{ }

	public void ShiftReferencedPositions(Size size, Position? referencePosition = null, bool ignoreLocking = false)
	{
		//TODO resolve shifting to negative regime on table resizing
		referencePosition ??= new(0, 0);

		if (PositionFrom.X >= referencePosition.X && PositionFrom.Y >= referencePosition.Y)
		{
			PositionFrom.Shift(size, ignoreLocking);
		}

		if (!ReferenceEquals(PositionFrom, PositionTo) &&
			PositionTo.X >= referencePosition.X && PositionTo.Y >= referencePosition.Y)
		{
			PositionTo.Shift(size, ignoreLocking);
		}
	}

	public override string ToString()
	{
		var doc = InstanceMap.Instance.GetInstance<Document>()!;

		var table = doc[ReferencedTableId].Name;

		return PositionFrom.Equals(PositionTo) ?
			$"T:{table},{PositionFrom}" :
			$"T:{table},{PositionFrom}-{PositionTo}";
	}

	public string ToShortString()
	{
		var doc = InstanceMap.Instance.GetInstance<Document>()!;

		var table = doc[ReferencedTableId].Name;

		var isCurrentTable = doc.GetActiveTable().Id == ReferencedTableId;

		return PositionFrom.Equals(PositionTo) ?
			$"{(isCurrentTable ? "" : $"{table}:")}{PositionFrom.ToShortString()}" :
			$"{(isCurrentTable ? "" : $"{table}:")}{PositionFrom.ToShortString()}-{PositionTo.ToShortString()}";
	}
}