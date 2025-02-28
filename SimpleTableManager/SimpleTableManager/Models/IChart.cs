namespace SimpleTableManager.Models;

public interface IChart :
	IConvertible, IComparable
{
	public object[] Xs { get; set; }

	public object[] Ys { get; set; }
}