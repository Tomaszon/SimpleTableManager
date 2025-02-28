namespace SimpleTableManager.Models;

public abstract class ChartBase<T>(IEnumerable<object> xs, IEnumerable<object> ys) :
	IConvertibleBase,
	IFormattable,
	IChart
	where T : ChartBase<T>
{
	public object[] Xs { get; set; } = [.. xs];

	public object[] Ys { get; set; } = [.. ys];

	public abstract string ToString(string? format, IFormatProvider? formatProvider);

	public abstract int CompareTo(object? obj);
}