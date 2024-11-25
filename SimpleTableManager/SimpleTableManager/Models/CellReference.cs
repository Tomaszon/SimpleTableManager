using SimpleTableManager.Models.CommandExecuters;
using SimpleTableManager.Services;

namespace SimpleTableManager.Models;

[ParseFormat("TableName,x,y", ".+,$?\\d,$?\\d")]
public class CellReference : ParsableBase<CellReference>, IParsable<CellReference>
{
	public Table Table { get; set; }

	public Position Position { get; set; }

	public bool HorizontallyLocked { get; set; }

	public bool VerticallyLocked { get; set; }

	public CellReference(Table table, Position position, bool horizontallyLocked, bool verticallyLocked)
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
			var n = args[0];
			var x = int.Parse(args[1].Trim(Shared.REF_CHAR).Trim());
			var y = int.Parse(args[2].Trim(Shared.REF_CHAR).Trim());

			//EXPERIMENTAL do this better?
			var t = InstanceMap.Instance.GetInstance<Document>()!.Tables.Single(t => t.Name == n);

			return new CellReference(t, new Position(x, y), args[1].Contains(Shared.REF_CHAR), args[2].Contains(Shared.REF_CHAR));
		});
	}

	public override string ToString()
	{
		return $"{Table.Name}:{(HorizontallyLocked ? Shared.REF_CHAR : "")}X{Position.X}{(VerticallyLocked ? Shared.REF_CHAR : "")}Y{Position.Y}";
	}
}