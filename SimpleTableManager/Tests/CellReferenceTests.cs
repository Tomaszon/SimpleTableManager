using NUnit.Framework.Internal;
using SimpleTableManager.Models.CommandExecuters;

namespace SimpleTableManager.Tests;

[ExcludeFromCodeCoverage]
public class CellReferenceTests : TestBase
{
	[Test]
	[TestCase(1, 2, 1, 2, "T:Table0,X:1,Y:2", "Table0:1:2")]
	[TestCase(1, 2, 1, 2, "T:Table0,X:*1,Y:*2", "Table0:*1:*2", false, false, false, false)]
	[TestCase(1, 2, 3, 4, "T:Table0,X:1,Y:2-X:3,Y:4", "Table0:1:2-3:4")]
	[TestCase(1, 2, 3, 4, "T:Table0,X:*1,Y:*2-X:*3,Y:*4", "Table0:*1:*2-*3:*4", false, false, false, false)]
	[TestCase(1, 2, 3, 4, "T:Table0,X:*1,Y:*2-X:3,Y:4", "Table0:*1:*2-3:4", false, false)]
	public void ReferenceToString(int x1, int y1, int x2, int y2, string s1, string s2, bool x1Locked = true, bool y1Locked = true, bool x2Locked = true, bool y2Locked = true)
	{
		var id = InstanceMap.Instance.GetInstances<Table>().First()!.Id;

		var reference = new CellReference(id, new(x1, y1, x1Locked, y1Locked), new(x2, y2, x2Locked, y2Locked));

		var rs1 = reference.ToString();
		var rs2 = reference.ToShortString();

		CheckResults([rs1, rs2], [s1, s2]);
	}

	[Test]
	[TestCase(1, 2, 3, 4, 6, 8, 8, 10, 5, 6, false, false)]
	[TestCase(1, 2, 3, 4, 6, 8, 3, 4, 5, 6, true, true)]
	public void ShiftReferencedPosition(int x1, int y1, int x2, int y2, int rX1, int rY1, int rX2, int rY2, int shiftX, int shiftY, bool lockX2, bool lockY2)
	{
		var reference = new CellReference(Guid.Empty, new(x1, y1, false, false), new(x2, y2, lockX2, lockY2));

		reference.ShiftReferencedPositions(new(shiftX, shiftY));

		CheckResults([reference.PositionFrom.X, reference.PositionFrom.Y, reference.PositionTo.X, reference.PositionTo.Y], [rX1, rY1, rX2, rY2]);
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
	public void ParseReferenceArgument(string value, bool shouldParse, int x1 = 0, int y1 = 0, int? x2 = null, int? y2 = null, int? x3 = null, int? y3 = null, int? x4 = null, int? y4 = null)
	{
		if (shouldParse)
		{
			var reference = ReferenceFunctionArgument.Parse(value, null);

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

			CheckResults(reference.Reference.ReferencedPositions, expected);
		}
		else
		{
			Assert.Throws<FormatException>(() => Position.Parse(value, null));
		}
	}
}