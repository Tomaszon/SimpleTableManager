namespace SimpleTableManager.Models;

[ParseFormat("TableName,{0}x1,{0}y1-{0}x2,{0}y2 ({0} for axis unlock)",
	"^(?<t>.+),(?<x1>Arg1?\\d+),(?<y1>Arg1?\\d+)-(?<x2>Arg1?\\d+),(?<y2>Arg1?\\d+)$",
	[Shared.REF_CHAR, Shared.REGEX_REF_CHAR])]
[ParseFormat("{0}x1,{0}y1-{0}x2,{0}y2 ({0} for axis unlock)",
	"^(?<x1>Arg1?\\d+),(?<y1>Arg1?\\d+)-(?<x2>Arg1?\\d+),(?<y2>Arg1?\\d+)$",
	[Shared.REF_CHAR, Shared.REGEX_REF_CHAR])]
[ParseFormat("ArgName{0}TableName,{1}x,{1}y ({1} for axis unlock, ArgName{0} for argument naming, TableName for referring to other tables)",
	"^((?<n>.+)Arg0)?((?<t>.+),)?(?<x>Arg2?\\d+),(?<y>Arg2?\\d+)$",
	[Shared.NAMED_ARG_SEPARATOR, Shared.REF_CHAR, Shared.REGEX_REF_CHAR])]
public class ReferenceFunctionArgument(CellReference reference, ArgumentName? name = null, int groupingId = 0) :
	ParsableBase<ReferenceFunctionArgument>,
	IParsable<ReferenceFunctionArgument>,
	IParsableCore<ReferenceFunctionArgument>,
	IFunctionArgument
{
	public ArgumentName? Name { get; set; } = name;

	public CellReference Reference { get; set; } = reference;

	public int GroupingId { get; set; } = groupingId;

	public IEnumerable<IConvertible> Resolve()
	{
		var doc = InstanceMap.Instance.GetInstance<Document>()!;

		var table = doc[Reference.ReferencedTableId];

		return Reference.ReferencedPositions.SelectMany(r => table[r].ContentFunction?.Execute() is var result && result is not null ? result : throw new NullReferenceException());
	}

	public bool TryResolve(out IEnumerable<object>? result, [NotNullWhen(false)] out string? error)
	{
		try
		{
			result = Resolve().ToList();

			error = null;

			return true;
		}
		catch (Exception ex)
		{
			result = null;

			error = ex.Message;

			return false;
		}
	}

	public static ReferenceFunctionArgument ParseCore(GroupCollection args, IFormatProvider? formatProvider)
	{
		var narg = args["n"];
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

		ArgumentName? name = null;

		if (narg.Success)
		{
			name = Enum.TryParse<ArgumentName>(narg.Value, true, out var n) ?
				(ArgumentName?)n : throw new FormatException($"Argument name '{narg.Value}' not found");
		}

		if (xarg.Success && yarg.Success)
		{
			var x = int.Parse(xarg.Value.Trim(Shared.REF_CHAR));
			var y = int.Parse(yarg.Value.Trim(Shared.REF_CHAR));

			var hl = !xarg.Value.Contains(Shared.REF_CHAR);
			var vl = !yarg.Value.Contains(Shared.REF_CHAR);

			return new ReferenceFunctionArgument(new(t.Id, new(x, y, hl, vl)), name);
		}
		else
		{
			var x1 = int.Parse(x1s.Trim(Shared.REF_CHAR));
			var y1 = int.Parse(y1s.Trim(Shared.REF_CHAR));
			var x2 = int.Parse(x2s.Trim(Shared.REF_CHAR));
			var y2 = int.Parse(y2s.Trim(Shared.REF_CHAR));

			var hl1 = !x1s.Contains(Shared.REF_CHAR);
			var vl1 = !y1s.Contains(Shared.REF_CHAR);
			var hl2 = !x2s.Contains(Shared.REF_CHAR);
			var vl2 = !y2s.Contains(Shared.REF_CHAR);

			return new ReferenceFunctionArgument(new(t.Id, new(x1, y1, hl1, vl1), new(x2, y2, vl2, vl2)), name);
		}
	}

	public override string ToString()
	{
		return $"{(Name is not null ? $"{Name}:" : "")}{Reference}";
	}
}