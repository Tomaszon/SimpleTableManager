using NUnit.Framework.Internal;

namespace SimpleTableManager.Tests;

public class MiscTests : TestBase
{
	[Test]
	[TestCase("4,1", true, 4, 1)]
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
		var range = new CellReference(Guid.Empty, new(1, 1), new(4, 1));

		CheckResults<Position>(range.ReferencedPositions, [new(1, 1), new(2, 1), new(3, 1), new(4, 1)]);
	}

	[Test]
	public void ReferenceRangeCompile2()
	{
		var range = new CellReference(Guid.Empty, new(4, 1), new(1, 1));

		CheckResults<Position>(range.ReferencedPositions, [new(1, 1), new(2, 1), new(3, 1), new(4, 1)]);
	}

	[Test]
	public void ReferenceRangeCompile3()
	{
		var range = new CellReference(Guid.Empty, new(1, 1), new(1, 4));

		CheckResults<Position>(range.ReferencedPositions, [new(1, 1), new(1, 2), new(1, 3), new(1, 4)]);
	}

	[Test]
	public void ReferenceRangeCompile4()
	{
		var range = new CellReference(Guid.Empty, new(1, 4), new(1, 1));

		CheckResults<Position>(range.ReferencedPositions, [new(1, 1), new(1, 2), new(1, 3), new(1, 4)]);
	}

	[Test]
	[TestCase("4,1", true, 4, 1)]
	[TestCase("2,1-1,1", true, 1, 1, 2, 1)]
	[TestCase("2,1-3,1", true, 2, 1, 3, 1)]
	[TestCase("1,1-1,3", true, 1, 1, 1, 2, 1, 3)]
	[TestCase("2,2-3,3", true, 2, 2, 3, 2, 2, 3, 3, 3)]
	[TestCase("3,3-2,2", true, 2, 2, 3, 2, 2, 3, 3, 3)]
	[TestCase("3,2-2,3", true, 2, 2, 3, 2, 2, 3, 3, 3)]
	[TestCase("2,3-3,2", true, 2, 2, 3, 2, 2, 3, 3, 3)]
	[TestCase("asd", false)]
	public void ParseCellReference(string value, bool shouldParse, int x1 = 0, int y1 = 0, int? x2 = null, int? y2 = null, int? x3 = null, int? y3 = null, int? x4 = null, int? y4 = null)
	{
		if (shouldParse)
		{
			var reference = CellReference.Parse(value, null);

			var expected = new List<Position> { new(x1, y1) };

			if (x2 is not null && y2 is not null)
			{
				expected.Add(new(x2.Value, y2.Value));
			}
			if (x3 is not null && y3 is not null)
			{
				expected.Add(new(x3.Value, y3.Value));
			}
			if (x4 is not null && y4 is not null)
			{
				expected.Add(new(x4.Value, y4.Value));
			}

			CheckResults(reference.ReferencedPositions, expected);
		}
		else
		{
			Assert.Throws<FormatException>(() => Position.Parse(value, null));
		}
	}
}