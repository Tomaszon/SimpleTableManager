namespace SimpleTableManager.Tests;

[ExcludeFromCodeCoverage]
public class SharedTests : TestBase
{
	[TestCase(new[] { 5, 4, 8, 5, 9 }, 9)]
	public void MaxTest(int[] array, int max)
	{
		CheckResult(Shared.Max(array), max);
	}

	[TestCase(new[] { 5, 4, 8, 5, 9 }, 4)]
	public void MinTest(int[] array, int min)
	{
		CheckResult(Shared.Min(array), min);
	}

	[Test]
	public void IsHandledTest()
	{
		CheckResults([new IndexOutOfRangeException().IsHandled(), new FormatException().IsHandled()], [false, true]);
	}

	[Test]
	public void SerializeCloneTest()
	{
		var position = new Position(3, 4);

		var position2 = Shared.SerializeClone(position);

		CheckResult(position, position2);
	}

	[TestCase("alma:körte:szilva", new[] { "alma", "körte" }, new[] { "szilva" })]
	public void JoinTest(string expected, params string[][] args)
	{
		var result = Shared.Join(':', args);

		CheckResult(expected, result);
	}

	[TestCase(50, "50 B")]
	[TestCase(500, "500 B")]
	[TestCase(5_000, "4.88 KB")]
	[TestCase(50_000, "48.83 KB")]
	[TestCase(5_000_000, "4.77 MB")]
	public void ConvertBytesToOtherSizesTest(long value, string expected)
	{
		CheckResult(Shared.ConvertBytesToOtherSizes(value), expected);
	}
}