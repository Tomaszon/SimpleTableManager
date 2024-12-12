namespace SimpleTableManager.Models;

public class Ellipse : Shape2Sized2dBase<Ellipse>, IParsable<Ellipse>
{
	public override decimal Area => Size1 * Size2 * Math.PI.ToType<decimal>();

	public override decimal Perimeter =>
		(Math.PI * Math.Sqrt((2 * (Size1 * Size1 + Size2 * Size2)).ToType<double>())).ToType<decimal>();

	public Ellipse() { }

	public Ellipse(decimal size1, decimal size2) : base(size1, size2) { }
}
