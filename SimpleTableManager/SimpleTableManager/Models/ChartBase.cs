namespace SimpleTableManager.Models;

public abstract class ChartBase<T, TData>(IEnumerable<TData> xs, IEnumerable<TData> ys) :
	IConvertibleBase,
	IFormattable,
	IChart
	where T : ChartBase<T, TData>
	where TData : IParsable<TData>, IComparable, IConvertible
{
	public TData[] Xs { get; set; } = [.. xs];

	public TData[] Ys { get; set; } = [.. ys];

	object[] IChart.Xs { get => [.. Xs.Cast<object>()]; set => Xs = [.. value.Select(e => (TData)e)]; }

	object[] IChart.Ys { get => [.. Ys.Cast<object>()]; set => Ys = [.. value.Select(e => (TData)e)]; }

	public abstract string ToString(string? format, IFormatProvider? formatProvider);

	public abstract int CompareTo(object? obj);

	int IComparable.CompareTo(object? obj)
	{
		throw new NotImplementedException();
	}
}