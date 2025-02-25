// namespace SimpleTableManager.Models;

// [ParseFormat("E[size1*size2] (fractions)", "^E\\[(?<s>\\d*(\\.|,)?\\d+)\\*(?<s2>\\d*(\\.|,)?\\d+)\\]$")]
// [ParseFormat("E[size1xsize2] (fractions)", "^E\\[(?<s>\\d*(\\.|,)?\\d+)x(?<s2>\\d*(\\.|,)?\\d+)\\]$")]
// [ParseFormat("{0}[size1*size2] (fractions)", "^Arg0\\[(?<s>\\d*(\\.|,)?\\d+)\\*(?<s2>\\d*(\\.|,)?\\d+)\\]$")]
// [ParseFormat("{0}[size1xsize2] (fractions)", "^Arg0\\[(?<s>\\d*(\\.|,)?\\d+)x(?<s2>\\d*(\\.|,)?\\d+)\\]$", [nameof(Ellipse)])]
// public class Ellipse(double size1 = 0, double size2 = 0) : Shape2Sized2dBase<Ellipse>(size1, size2), IParsable<Ellipse>
// {
// 	public override double Area => Size1 * Size2 * Math.PI;

// 	public override double Perimeter => Math.PI * Math.Sqrt(2 * (Size1 * Size1 + Size2 * Size2));
// }
