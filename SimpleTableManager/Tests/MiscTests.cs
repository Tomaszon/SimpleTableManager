﻿using NUnit.Framework.Internal;

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
			CheckResults(new object[] { true, position.X, position.Y }, new object[] { shouldParse, x, y });
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
			CheckResults(new object[] { position.X, position.Y }, new object[] { x, y });
		}
		else
		{
			Assert.Throws<FormatException>(() => Position.Parse(value, null));
		}
	}
}