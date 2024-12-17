namespace SimpleTableManager.Models;

public class Rectangle : Shape2Sized2dBase<Rectangle>, IParsable<Rectangle>
{
	public override double Area => Size1 * Size2;

	public override double Perimeter => 2 * (Size1 + Size2);

	public Rectangle() { }

	public Rectangle(double size1, double size2) : base(size1, size2) { }
}
