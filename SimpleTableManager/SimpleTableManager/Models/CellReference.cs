using SimpleTableManager.Models.CommandExecuters;
using SimpleTableManager.Services;

namespace SimpleTableManager.Models;

[ParseFormat("TableName,$x,$y ($ for axis lock)", ".+,\\$?\\d,\\$?\\d")]
[ParseFormat("$x,$y ($ for axis lock)", "\\$?\\d,\\$?\\d")]
public class CellReference : ParsableBase<CellReference>, IParsable<CellReference>
{
	public Table Table { get; set; }

	public Position Position { get; set; }

	public bool HorizontallyLocked { get; set; }

	public bool VerticallyLocked { get; set; }

	public CellReference(Table table, Position position, bool horizontallyLocked = true, bool verticallyLocked = true)
	{
		Table = table;
		Position = position;
		HorizontallyLocked = horizontallyLocked;
		VerticallyLocked = verticallyLocked;
	}

	public static CellReference Parse(string value, IFormatProvider? _)
	{
		return ParseWrapper(value, (args) =>
		{
			var ns = args.Length == 3 ? args[0] : null;
			var xs = args.Length == 3 ? args[1] : args[0];
			var ys = args.Length == 3 ? args[2] : args[1];

			var n = ns;
			var x = int.Parse(xs.Trim(Shared.REF_CHAR).Trim());
			var y = int.Parse(ys.Trim(Shared.REF_CHAR).Trim());

			var doc = InstanceMap.Instance.GetInstance<Document>()!;

			var t = n is null ? 
				doc.GetActiveTable() :
				doc.Tables.Single(t => t.Name.Equals(n, StringComparison.InvariantCultureIgnoreCase));

			return new CellReference(t, new Position(x, y), xs.Contains(Shared.REF_CHAR), ys.Contains(Shared.REF_CHAR));
		});
	}

	public override string ToString()
	{
		return $"T:{Table.Name}, X:{(HorizontallyLocked ? Shared.REF_CHAR : "")}{Position.X}, Y:{(VerticallyLocked ? Shared.REF_CHAR : "")}{Position.Y}";
	}
}