namespace SimpleTableManager.Models;

public class RawChart(IEnumerable<object> xs, IEnumerable<object> ys) :
	ChartBase<RawChart>(xs, ys)
{
	public override string ToString(string? format, IFormatProvider? formatProvider)
	{
		return $"{{{string.Join(';', Xs.Select(v => v.ToString()))}}}->{{{string.Join(';', Ys.Select(v => v.ToString()))}}}";
	}

	public override int CompareTo(object? obj)
	{
		throw new NotImplementedException();
	}

	public override string ToString()
	{
		return $"{{{string.Join(';', Xs.Select(v => v.ToString()))}}}->{{{string.Join(';', Ys.Select(v => v.ToString()))}}}";
	}
}