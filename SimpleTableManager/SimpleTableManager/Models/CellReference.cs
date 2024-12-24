namespace SimpleTableManager.Models;

[ParseFormat("TableName,{0}x,{0}y ({0} for axis unlock)", "(?<t>.+),(?<x>{1}?\\d+),(?<y>{1}?\\d+)", [Shared.REF_CHAR, Shared.REGEX_REF_CHAR])]
[ParseFormat("{0}x,{0}y ({0} for axis unlock)", "(?<x>{1}?\\d+),(?<y>{1}?\\d+)", [Shared.REF_CHAR, Shared.REGEX_REF_CHAR])]
public class CellReference(Table table, Position position, bool horizontallyLocked = true, bool verticallyLocked = true) : ParsableBase<CellReference>, IParsable<CellReference>, IParseCore<CellReference>
{
	public Table Table { get; set; } = table;

	public Position Position { get; set; } = position;

	public bool HorizontallyLocked { get; set; } = horizontallyLocked;

	public bool VerticallyLocked { get; set; } = verticallyLocked;

	public override string ToString()
	{
		return $"T:{Table.Name}, X:{(HorizontallyLocked ? "" : Shared.REF_CHAR)}{Position.X}, Y:{(VerticallyLocked ? "" : Shared.REF_CHAR)}{Position.Y}";
	}

	public string ToShortString()
	{
		return $"{Table.Name}:{(HorizontallyLocked ? "" : Shared.REF_CHAR)}{Position.X}:{(VerticallyLocked ? "" : Shared.REF_CHAR)}{Position.Y}";
	}

	public static CellReference ParseCore(GroupCollection args)
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

		return new CellReference(t, new Position(x, y), !xs.Contains(Shared.REF_CHAR), !ys.Contains(Shared.REF_CHAR));
	}
}