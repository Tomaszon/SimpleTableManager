namespace SimpleTableManager.Models;

public abstract class ChartBase<TDataX, TDataY>(IEnumerable<TDataX> xs, IEnumerable<TDataY> ys) :
	IChart
	where TDataX : IParsable<TDataX>, IComparable, IConvertible
	where TDataY : IParsable<TDataY>, IComparable, IConvertible
{
	public TDataX[] Xs { get; set; } = [.. xs];

	public TDataY[] Ys { get; set; } = [.. ys];

	object[] IChart.Xs { get => [.. Xs.Cast<object>()]; set => Xs = [.. value.Select(e => (TDataX)e)]; }

	object[] IChart.Ys { get => [.. Ys.Cast<object>()]; set => Ys = [.. value.Select(e => (TDataY)e)]; }

	public abstract int CompareTo(object? obj);

    public abstract string ToString(string? format, IFormatProvider? formatProvider);

}