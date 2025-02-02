namespace SimpleTableManager.Models;

[ParseFormat("R[size1*size2] (fractions)", "^R\\[(?<s>\\d*(\\.|,)?\\d+)\\*(?<s2>\\d*(\\.|,)?\\d+)\\]$")]
[ParseFormat("R[size1xsize2] (fractions)", "^R\\[(?<s>\\d*(\\.|,)?\\d+)x(?<s2>\\d*(\\.|,)?\\d+)\\]$")]
[ParseFormat("{0}[size1*size2] (fractions)", "^Arg0\\[(?<s>\\d*(\\.|,)?\\d+)\\*(?<s2>\\d*(\\.|,)?\\d+)\\]$")]
[ParseFormat("{0}[size1xsize2] (fractions)", "^Arg0\\[(?<s>\\d*(\\.|,)?\\d+)x(?<s2>\\d*(\\.|,)?\\d+)\\]$", [nameof(Rectangle)])]
public class Rectangle(double size1, double size2) : Shape2Sized2dBase<Rectangle>(size1, size2), IParsable<Rectangle>
{
	public override double Area => Size1 * Size2;

	public override double Perimeter => 2 * (Size1 + Size2);
}
