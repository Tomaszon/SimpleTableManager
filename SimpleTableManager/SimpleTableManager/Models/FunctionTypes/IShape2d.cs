namespace SimpleTableManager.Models.FunctionTypes;

public interface IShape2d : IConvertible, IComparable, IFormattable
{
	double Area { get; }

	double Perimeter { get; }
}