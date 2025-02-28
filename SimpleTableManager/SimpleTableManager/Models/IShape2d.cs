namespace SimpleTableManager.Models;

public interface IShape2d : IConvertible, IComparable, IFormattable
{
	double Area { get; }

	double Perimeter { get; }
}