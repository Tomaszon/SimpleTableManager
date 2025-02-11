namespace SimpleTableManager.Models;

public abstract class ChartBase<T>(IEnumerable<IConvertible> xs, IEnumerable<IConvertible> ys) :
	IConvertibleBase,
	IFormattable,
	IChart
	where T : ChartBase<T>
{
	public IConvertible[] Xs { get; set; } = [.. xs];

	public IConvertible[] Ys { get; set; } = [.. ys];

	public abstract string ToString(string? format, IFormatProvider? formatProvider);
}