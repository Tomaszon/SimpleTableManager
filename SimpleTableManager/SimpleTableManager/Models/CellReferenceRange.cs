
namespace SimpleTableManager.Models;

[ParseFormat("TableName;{0}x1;{0}y1-{0}x2;{0}y2 ({0} for axis unlock)", "^(?<t>.+);(?<x1>{1}?\\d+);(?<y1>{1}?\\d+)-(?<x2>{1}?\\d+);(?<y2>{1}?\\d+)$", [Shared.REF_CHAR, Shared.REGEX_REF_CHAR])]
[ParseFormat("TableName,{0}x1,{0}y1-{0}x2,{0}y2 ({0} for axis unlock)", "^(?<t>.+),(?<x1>{1}?\\d+),(?<y1>{1}?\\d+)-(?<x2>{1}?\\d+),(?<y2>{1}?\\d+)$", [Shared.REF_CHAR, Shared.REGEX_REF_CHAR])]
[ParseFormat("{0}x1;{0}y1-{0}x2;{0}y2 ({0} for axis unlock)", "^(?<x1>{1}?\\d+);(?<y1>{1}?\\d+)-(?<x2>{1}?\\d+);(?<y2>{1}?\\d+)$", [Shared.REF_CHAR, Shared.REGEX_REF_CHAR])]
[ParseFormat("{0}x1,{0}y1-{0}x2,{0}y2 ({0} for axis unlock)", "^(?<x1>{1}?\\d+),(?<y1>{1}?\\d+)-(?<x2>{1}?\\d+),(?<y2>{1}?\\d+)$", [Shared.REF_CHAR, Shared.REGEX_REF_CHAR])]
public class CellReferenceRange : ParsableBase<CellReferenceRange>, IParsable<CellReferenceRange>, IParseCore<CellReferenceRange>
{
	public Guid ReferencedTableId { get; set; }

	public CellReference From { get; set; }

	public CellReference To { get; set; }

	public bool HorizontallyLocked { get; set; }

	public bool VerticallyLocked { get; set; }

	[JsonConstructor]
	public CellReferenceRange(Guid referencedTableId, Position fromPosition, Position toPosition, bool horizontallyLocked1 = true, bool verticallyLocked1 = true, bool horizontallyLocked2 = true, bool verticallyLocked2 = true)
	{
		From = new CellReference(referencedTableId, fromPosition, horizontallyLocked1, verticallyLocked1);

		To = new CellReference(referencedTableId, toPosition, horizontallyLocked2, verticallyLocked2);

		HorizontallyLocked = horizontallyLocked1;

		VerticallyLocked = verticallyLocked1;

		//TODO throw exception on table ref, lock differences or if they are not in line
	}

	public List<CellReference> Compile()
	{
		var results = new List<CellReference>();

		var position = new Position(Math.Min(From.ReferencedPosition.X, To.ReferencedPosition.X), Math.Min(From.ReferencedPosition.Y, To.ReferencedPosition.Y));
		var endPosition = new Position(Math.Max(From.ReferencedPosition.X, To.ReferencedPosition.X), Math.Max(From.ReferencedPosition.Y, To.ReferencedPosition.Y));

		do
		{
			results.Add(new CellReference(ReferencedTableId, position, HorizontallyLocked, VerticallyLocked));

			position += new Size(position.X != endPosition.X ? 1 : 0, position.Y != endPosition.Y ? 1 : 0);
		}
		while (!position.Equals(endPosition));

		return results;
	}

	public static CellReferenceRange ParseCore(GroupCollection args, IFormatProvider? formatProvider = null)
	{
		var tns = args["t"].Value;
		var x1s = args["x1"].Value;
		var y1s = args["y1"].Value;
		var x2s = args["x2"].Value;
		var y2s = args["y2"].Value;

		var tn = tns;
		var x1 = int.Parse(x1s.Trim(Shared.REF_CHAR));
		var y1 = int.Parse(y1s.Trim(Shared.REF_CHAR));
		var x2 = int.Parse(x2s.Trim(Shared.REF_CHAR));
		var y2 = int.Parse(y2s.Trim(Shared.REF_CHAR));

		var doc = InstanceMap.Instance.GetInstance<Document>()!;

		var t = string.IsNullOrEmpty(tn) ?
			doc.GetActiveTable() :
			doc.Tables.Single(t => t.Name.Equals(tn, StringComparison.InvariantCultureIgnoreCase));

		return new CellReferenceRange(t.Id, new(x1, y1), new(x2, y2), !x1s.Contains(Shared.REF_CHAR), !y1s.Contains(Shared.REF_CHAR), !x2s.Contains(Shared.REF_CHAR), !y2s.Contains(Shared.REF_CHAR));
	}
}
