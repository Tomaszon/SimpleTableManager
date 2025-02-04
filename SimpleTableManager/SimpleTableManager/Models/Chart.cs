namespace SimpleTableManager.Models;

[ParseFormat("{x1;x2;...xn}{y1;y2...yn}", "{(?<x>(.*;)*.*)}{(?<y>(.*;)*.*)}")]
public class Chart(IEnumerable<IConvertible> xs, IEnumerable<IConvertible> ys) : ConvertibleBase<Chart>, IParsable<Chart>, IParsableCore<Chart>, IFormattable
{
	public IConvertible[] Xs { get; set; } = [.. xs];

	public IConvertible[] Ys { get; set; } = [.. ys];

	public static Chart ParseCore(GroupCollection args, IFormatProvider? formatProvider = null)
	{
		var xs = args["x"].Value.Split(';');
		var ys = args["y"].Value.Split(';');

		return new Chart(xs, ys);
	}

	public string ToString(string? format, IFormatProvider? formatProvider)
	{
		//TODO IFormattable handling
		return $"{{{string.Join(';', Xs.Select(v => v.ToString()))}}}->{{{string.Join(';', Ys.Select(v => v.ToString()))}}}";
	}

	public override string ToString()
	{
		return $"{{{string.Join(';', Xs.Select(v => v.ToString()))}}}->{{{string.Join(';', Ys.Select(v => v.ToString()))}}}";
	}
}