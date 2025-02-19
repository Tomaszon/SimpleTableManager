using NUnit.Framework.Internal;

namespace SimpleTableManager.Tests;

[ExcludeFromCodeCoverage]
public class MiscTests : TestBase
{
	[Test]
	[TestCase("4,1", true, 4, 1)]
	[TestCase("asd", false)]
	[TestCase(null, false)]
	public void TryParsePosition(string? value, bool shouldParse, int x = 0, int y = 0)
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
	public void PositionTest()
	{
		var p = new Position(new(1, 2));

		var (x, y) = p;

		var p2 = p! + new Size(1, 2);
		var s = p! - new Position(1, 2);

		var p3 = new Position(0, 0);
		var p4 = new Position(5, 5);

		var p5 = new Position(3, 3);
		var p6 = new Position(6, 4);

		CheckResult(p.ToString(), "X:1,Y:2");
		CheckResult(p.Equals(null), false);
		CheckResults(results: [x, y], [1, 2]);
		CheckResults([p2.X, p2.Y, s.Width, s.Height], [2, 4, 0, 0]);
		CheckResults([p5.IsBetween(p3, p4), p6.IsBetween(p3, p4), p5.IsNotBetween(p3, p4), p6.IsNotBetween(p3, p4)], [true, false, false, true]);
	}

	[Test]
	public void TimeOnlyTest()
	{
		var to = new TimeOnly(1, 2, 3, 4);

		ConvertibleTimeOnly cto = to;

		var to2 = (TimeOnly)cto;

		var dt = cto.ToDateTime(null);

		CheckResults([cto.Hour, cto.Minute, cto.Second, cto.Millisecond], [to.Hour, to.Minute, to.Second, to.Millisecond]);
		CheckResults([to2.Hour, to2.Minute, to2.Second, to2.Millisecond], [to.Hour, to.Minute, to.Second, to.Millisecond]);
		CheckResults([cto.Equals(to), cto.Equals(null)], [true, false]);
		CheckResults<object>([dt.Date, dt.TimeOfDay], [DateTime.MinValue, new TimeSpan(0, 1, 2, 3, 4)]);
		CheckResult(cto!.ToString(), to.ToString());
	}

	[Test]
	public void DateOnlyTest()
	{
		var doy = new DateOnly(1993, 12, 19);

		ConvertibleDateOnly cdo = doy;

		var do2 = (DateOnly)cdo;

		var dt = cdo.ToDateTime(null);

		CheckResults([cdo.Year, cdo.Month, cdo.Day], [doy.Year, doy.Month, doy.Day]);
		CheckResults([do2.Year, do2.Month, do2.Day], [doy.Year, doy.Month, doy.Day]);

		CheckResults([cdo.Equals(doy), cdo.Equals(null)], [true, false]);
		CheckResults<object>([dt.Date, dt.TimeOfDay], [new DateTime(1993, 12, 19), TimeSpan.FromTicks(0)]);
		CheckResult(cdo!.ToString(), doy.ToString());
	}

	[Test]
	public void BooleanTest()
	{
		var fb = FormattableBoolean.Parse("yes", null);

		CheckResults([fb.Equals(true), fb.Equals(null)], [true, false]);
		CheckResults<object>([fb!.ToBoolean(null), fb!.ToInt32(null), fb!.ToInt64(null), fb!.ToDouble(null)], [true, 1, 1L, 1d]);
		CheckResults([fb.ToString(), fb.ToString("yn", null), fb.ToString("yesno", null), fb.ToString("10", null)], ["True", "Y", "Yes", "1"]);
	}

	[Test]
	public void MetadataTest()
	{
		_ = new Metadata()
		{
			AppVersion = new Version(1, 0, 0),
			CreateTime = DateTime.Now,
			Path = "SomeFolderName",
			Size = 123456789
		};
	}
}