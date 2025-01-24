namespace SimpleTableManager.Models;

public interface IShape2d : IConvertible
{
	double Area { get; }

	double Perimeter { get; }
}