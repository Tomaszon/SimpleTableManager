namespace SimpleTableManager.Models.FunctionTypes;

public class RawChart<TDataX, TDataY>(IEnumerable<TDataX> xs, IEnumerable<TDataY> ys) :
	ChartBase<TDataX, TDataY>(xs, ys)
	where TDataX: IParsable<TDataX>, IConvertible, IComparable
	where TDataY : IParsable<TDataY>, IComparable, IConvertible
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