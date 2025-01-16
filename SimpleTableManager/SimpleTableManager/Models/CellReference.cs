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
}