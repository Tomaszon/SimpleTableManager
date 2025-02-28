namespace SimpleTableManager.Models;

public class RawChart<TData>(IEnumerable<TData> xs, IEnumerable<TData> ys) :
	ChartBase<RawChart<TData>, TData>(xs, ys)
	where TData: IParsable<TData>, IConvertible, IComparable
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