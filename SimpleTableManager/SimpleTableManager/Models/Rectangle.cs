namespace SimpleTableManager.Models;

public class Rectangle(double size1, double size2) : Shape2Sized2dBase<Rectangle>(size1, size2), IParsable<Rectangle>
{
	public override double Area => Size1 * Size2;

	public override double Perimeter => 2 * (Size1 + Size2);
}
