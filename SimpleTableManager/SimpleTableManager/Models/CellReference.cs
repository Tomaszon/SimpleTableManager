namespace SimpleTableManager.Models;

[ParseFormat("TableName;{0}x;{0}y ({0} for axis unlock)", "^(?<t>.+);(?<x>{1}?\\d+);(?<y>{1}?\\d+)$", [Shared.REF_CHAR, Shared.REGEX_REF_CHAR])]
[ParseFormat("TableName,{0}x,{0}y ({0} for axis unlock)", "^(?<t>.+),(?<x>{1}?\\d+),(?<y>{1}?\\d+)$", [Shared.REF_CHAR, Shared.REGEX_REF_CHAR])]
[ParseFormat("{0}x;{0}y ({0} for axis unlock)", "^(?<x>{1}?\\d+);(?<y>{1}?\\d+)$", [Shared.REF_CHAR, Shared.REGEX_REF_CHAR])]
[ParseFormat("{0}x,{0}y ({0} for axis unlock)", "^(?<x>{1}?\\d+),(?<y>{1}?\\d+)$", [Shared.REF_CHAR, Shared.REGEX_REF_CHAR])]
[method: JsonConstructor]
public class CellReference(Guid referencedTableId, Position referencedPosition, bool horizontallyLocked = true, bool verticallyLocked = true) : ParsableBase<CellReference>, IParsable<CellReference>, IParseCore<CellReference>
{
	public Guid ReferencedTableId { get; set; } = referencedTableId;

	public Position ReferencedPosition { get; set; } = referencedPosition;

	public bool HorizontallyLocked { get; set; } = horizontallyLocked;

	public bool VerticallyLocked { get; set; } = verticallyLocked;

	public void ShiftReferencedPosition(Size size)
	{
		if (!HorizontallyLocked)
		{
			ReferencedPosition.X += size.Width;
		}

		if (!VerticallyLocked)
		{
			ReferencedPosition.Y += size.Height;
		}
	}

	public override string ToString()
	{
		var doc = InstanceMap.Instance.GetInstance<Document>()!;

		return $"T:{doc[ReferencedTableId].Name},X:{(HorizontallyLocked ? "" : Shared.REF_CHAR)}{ReferencedPosition.X},Y:{(VerticallyLocked ? "" : Shared.REF_CHAR)}{ReferencedPosition.Y}";
	}

	public string ToShortString()
	{
		var doc = InstanceMap.Instance.GetInstance<Document>()!;

		return $"{doc[ReferencedTableId].Name}:{(HorizontallyLocked ? "" : Shared.REF_CHAR)}{ReferencedPosition.X}:{(VerticallyLocked ? "" : Shared.REF_CHAR)}{ReferencedPosition.Y}";
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

		return new CellReference(t.Id, new Position(x, y), !xs.Contains(Shared.REF_CHAR), !ys.Contains(Shared.REF_CHAR));
	}
}