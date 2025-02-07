namespace SimpleTableManager.Models;

public interface IChart : IConvertible
{
	public IConvertible[] Xs { get; set; }

	public IConvertible[] Ys { get; set; }
}