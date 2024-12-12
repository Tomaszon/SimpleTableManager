namespace SimpleTableManager.Models;

public class Rectangle : Shape2Sized2dBase<Rectangle>, IParsable<Rectangle>
{
	public override decimal Area => Size1 * Size2;

	public override decimal Perimeter => 2 * (Size1 + Size2);

	public Rectangle() { }

	public Rectangle(decimal size1, decimal size2) : base(size1, size2) { }
}
