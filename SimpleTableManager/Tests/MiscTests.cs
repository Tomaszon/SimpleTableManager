using NUnit.Framework.Internal;

namespace SimpleTableManager.Tests;

public class MiscTests : TestBase
{
	[Test]
	[TestCase("4,1", true, 4, 1)]
	[TestCase("2;1", true, 2, 1)]
	[TestCase("asd", false)]
	public void TryParsePosition(string value, bool shouldParse, int x = 0, int y = 0)
	{
		if (Position.TryParse(value, null, out var position))
		{
			CheckResults([true, position.X, position.Y], new object[] { shouldParse, x, y });
		}
		else
		{
			CheckResults(((object)false).Wrap(), shouldParse.Wrap());
		}
	}

	[Test]
	[TestCase("4,1", true, 4, 1)]
	[TestCase("2;1", true, 2, 1)]
	[TestCase("asd", false)]
	public void ParsePosition(string value, bool shouldParse, int x = 0, int y = 0)
	{
		if (shouldParse)
		{
			var position = Position.Parse(value, null);
			CheckResults([position.X, position.Y], new object[] { x, y });
		}
		else
		{
			Assert.Throws<FormatException>(() => Position.Parse(value, null));
		}
	}

	[Test]
	public void ReferenceRangeCompile1()
	{
		var range = new CellReferenceRange(Guid.Empty, new(1, 1), new(4, 1));

		var result = range.Compile().Select(r => r.ReferencedPosition);

		CheckResults<Position>(result, [new(1, 1), new(2, 1), new(3, 1), new(4, 1)]);
	}

	[Test]
	public void ReferenceRangeCompile2()
	{
		var range = new CellReferenceRange(Guid.Empty, new(4, 1), new(1, 1));

		var result = range.Compile().Select(r => r.ReferencedPosition);

		CheckResults<Position>(result, [new(1, 1), new(2, 1), new(3, 1), new(4, 1)]);
	}

	[Test]
	public void ReferenceRangeCompile3()
	{
		var range = new CellReferenceRange(Guid.Empty, new(1, 1), new(1, 4));

		var result = range.Compile().Select(r => r.ReferencedPosition);

		CheckResults<Position>(result, [new(1, 1), new(1, 2), new(1, 3), new(1, 4)]);
	}

	[Test]
	public void ReferenceRangeCompile4()
	{
		var range = new CellReferenceRange(Guid.Empty, new(1, 4), new(1, 1));

		var result = range.Compile().Select(r => r.ReferencedPosition);

		CheckResults<Position>(result, [new(1, 1), new(1, 2), new(1, 3), new(1, 4)]);
	}

	[Test]
	[TestCase("4,1", true, 4, 1)]
	[TestCase("2;1", true, 2, 1)]
	[TestCase("asd", false)]
	public void ParseCellReference(string value, bool shouldParse, int x = 0, int y = 0)
	{
		if (shouldParse)
		{
			var reference = CellReference.Parse(value, null);
			CheckResults([reference.ReferencedPosition.X, reference.ReferencedPosition.Y], new object[] { x, y });
		}
		else
		{
			Assert.Throws<FormatException>(() => Position.Parse(value, null));
		}
	}

	[Test]
	[TestCase("2,1-1,1", true, 1, 1, 2, 1)]
	[TestCase("2;1-3;1", true, 2, 1, 3, 1)]
	[TestCase("asd", false)]
	public void ParseCellReferenceRange(string value, bool shouldParse, int x1 = 0, int y1 = 0, int x2 = 0, int y2 = 0)
	{
		if (shouldParse)
		{
			var range = CellReferenceRange.Parse(value, null);

			var result = range.Compile().Select(r => r.ReferencedPosition);

			CheckResults<Position>(result, [new(x1, y1), new(x2, y2)]);
		}
		else
		{
			Assert.Throws<FormatException>(() => CellReferenceRange.Parse(value, null));
		}
	}
}