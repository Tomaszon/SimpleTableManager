namespace SimpleTableManager.Models;

public interface IChart :
	IConvertibleBase,
	IComparable,
	IFormattable
{
	public object[] Xs { get; set; }

	public object[] Ys { get; set; }
}