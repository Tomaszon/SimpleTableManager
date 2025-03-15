using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

	[Test]
	[TestCase(true, true, true, false)]
	[TestCase(false, false, false, true)]
	[TestCase(false, true, true, false)]
	[TestCase(true, false, true, false)]
	public void CellVisibilityTest(bool isColumnHidden, bool isRowHidden, bool isHidden, bool isVisible)
	{
		var visibility = new CellVisibility()
		{
			IsColumnHidden = isColumnHidden,
			IsRowHidden = isRowHidden
		};

		CheckResults([visibility.IsHidden, visibility.IsVisible], [isHidden, isVisible]);
	}

	[Test]
	public void CellSelectionTest()
	{
		var selection = new CellSelection();

		CheckResult(selection.GetHighestSelectionLevel(), CellSelectionLevel.None);
		CheckResult(selection.IsPrimarySelected, false);
		CheckResult(selection.IsNotPrimarySelected, true);

		selection.SelectPrimary();

		CheckResult(selection.IsPrimarySelected, true);
		CheckResult(selection.IsNotPrimarySelected, false);

		selection.SelectSecondary();

		CheckResult(selection.IsSecondarySelected, true);
		CheckResult(selection.IsNotSecondarySelected, false);

		selection.SelectSecondary();
		selection.SelectTertiary();

		CheckResult(selection.GetHighestSelectionLevel(), CellSelectionLevel.Primary);
		CheckResult(selection.IsTertiarySelected, true);
		CheckResult(selection.IsNotTertiarySelected, false);

		selection.DeselectPrimary();

		CheckResult(selection.IsPrimarySelected, false);
		CheckResult(selection.GetHighestSelectionLevel(), CellSelectionLevel.Secondary);

		selection.DeselectSecondary();

		CheckResult(selection.IsSecondarySelected, true);

		selection.DeselectSecondary();

		CheckResult(selection.IsSecondarySelected, false);
		CheckResult(selection.GetHighestSelectionLevel(), CellSelectionLevel.Tertiary);

		selection.DeselectTertiary();

		CheckResult(selection.IsTertiarySelected, false);
		CheckResult(selection.GetHighestSelectionLevel(), CellSelectionLevel.None);
	}

	[Test]
	[TestCase(true, false, true, true, BorderType.None, BorderType.None, BorderType.None, BorderType.None, BorderType.None, BorderType.None, BorderType.None, BorderType.Horizontal)]
	[TestCase(true, true, true, true, BorderType.None, BorderType.None, BorderType.None, BorderType.None, BorderType.None, BorderType.None, BorderType.None, BorderType.None)]
	public void CellBorderTest1(bool trimTop, bool trimBottom, bool trimLeft, bool trimRight, params BorderType[] results)
	{
		var border = new CellBorder()
		{
			Bottom = BorderType.Horizontal,
			BottomLeft = BorderType.Up | BorderType.Right,
			BottomRight = BorderType.Up | BorderType.Left,
			Left = BorderType.Vertical,
			Right = BorderType.Vertical,
			Top = BorderType.Horizontal,
			TopLeft = BorderType.Down | BorderType.Right,
			TopRight = BorderType.Down | BorderType.Left
		};

		border = border.TrimSide(trimTop, trimBottom, trimLeft, trimRight);

		CheckResults([border.BottomLeft, border.BottomRight, border.Left, border.Right, border.Top, border.TopLeft, border.TopRight, border.Bottom], results);
	}

	[Test]
	[TestCase(true, false, true, true, BorderType.Horizontal, BorderType.None, BorderType.None, BorderType.Vertical, BorderType.Vertical, BorderType.Horizontal, BorderType.None, BorderType.Down | BorderType.Left)]
	[TestCase(true, true, true, true, BorderType.Horizontal, BorderType.None, BorderType.None, BorderType.Vertical, BorderType.Vertical, BorderType.Horizontal, BorderType.None, BorderType.None)]
	public void CellBorderTest2(bool trimTopLeft, bool trimTopRight, bool trimBottomLeft, bool trimBottomRight, params BorderType[] results)
	{
		var border = new CellBorder()
		{
			Bottom = BorderType.Horizontal,
			BottomLeft = BorderType.Up | BorderType.Right,
			BottomRight = BorderType.Up | BorderType.Left,
			Left = BorderType.Vertical,
			Right = BorderType.Vertical,
			Top = BorderType.Horizontal,
			TopLeft = BorderType.Down | BorderType.Right,
			TopRight = BorderType.Down | BorderType.Left
		};

		border = border.TrimCorner(trimTopLeft, trimTopRight, trimBottomLeft, trimBottomRight);

		CheckResults([border.Bottom, border.BottomLeft, border.BottomRight, border.Left, border.Right, border.Top, border.TopLeft, border.TopRight], results);
	}

	[Test]
	public void CommandReferenceTest()
	{
		_ = new CommandReference("SomeClassName", "SomeMethodName", true);
	}

	[Test]
	public void ConsoleColorSetTest()
	{
		var colors1 = new ConsoleColorSet(ConsoleColor.Red, ConsoleColor.Green);
		var colors2 = new ConsoleColorSet(ConsoleColor.Red, ConsoleColor.Green);
		var colors3 = new ConsoleColorSet(ConsoleColor.Green, ConsoleColor.Green);

		CheckResults([colors1 == colors2, colors1 == colors3], [true, false]);
		CheckResults([colors1 != colors2, colors1 != colors3], [false, true]);
		CheckResults([colors1.Equals(colors2), colors1.Equals(colors3), colors1.Equals(null)], [true, false, false]);

		CheckResults([colors1.ToString(), colors1.ToString()], [colors2.ToString(), $"F:{colors1.Foreground}, B:{colors1.Background}"]);
	}

	[Test]
	public void CompareToTest()
	{
		var timeSpanResult = ConvertibleTimeSpan.Parse("1.02:03:05", null).CompareTo(null);
		var timeResult = ConvertibleTimeOnly.Parse("02:03:05", null).CompareTo(null);
		var dateResult = ConvertibleDateOnly.Parse("1993.12.19", null).CompareTo(null);
		var boolResult = FormattableBoolean.Parse("true", null).CompareTo(null);

		CheckResults([timeResult, timeSpanResult, dateResult, boolResult], [1, 1, 1, 1]);
	}

	[TestCase("alma körte szilva", new[] { "alma" }, new[] { "körte" }, new[] { "szilva" })]
	[TestCase("\\\\alma körte szilva", new[] { "\\alma" }, new[] { "körte" }, new[] { "szilva" })]
	[TestCase("(alma körte) szilva", new[] { "alma körte" }, new[] { "szilva" })]
	[TestCase("{alma körte} szilva", new[] { "alma", "körte" }, new[] { "szilva" })]
	[TestCase("{alma (körte szilva)}", new[] { "alma", "körte szilva" })]
	[TestCase("{alma \\(körte szilva\\)}", new[] { "alma", "(körte", "szilva)" })]
	[TestCase("\\{alma \\(körte szilva\\)\\}", new[] { "{alma" }, new[] { "(körte" }, new[] { "szilva)}" })]
	public void StackMataTests(string input, params string[][] argumentGroups)
	{
		var results = StackMata.ProcessArguments(input);

		for (int i = 0; i < results.Count; i++)
		{
			CheckResults(results[i], argumentGroups[i]);
		}
	}

	[TestCase("(alma körte", "Invalid argument merging syntax")]
	[TestCase("(alma {körte})", "Invalid argument grouping syntax")]
	[TestCase("alma)", "Invalid argument merging syntax")]
	[TestCase("{alma körte", "Invalid argument grouping syntax")]
	[TestCase("alma körte}", "Invalid argument grouping syntax")]
	[TestCase("\\alma körte", "Invalid argument escaping syntax")]
	[TestCase("alma körte\\", "Invalid argument escaping syntax")]
	public void StackMataTests2(string input, string error)
	{
		Assert.Throws<ArgumentException>(() => StackMata.ProcessArguments(input), error);
	}

	[TestCase(typeof(string), true, "1", 1)]
	[TestCase(typeof(int), false, "1", 1)]
	public void ParsableStringConverterTests(Type type, bool expectedCanConvert, string value, int expected)
	{
		var intConverter = new ParsableStringConverter<int>();

		var canConvert = intConverter.CanConvertFrom(null, type);

		if (canConvert)
		{
			var result = intConverter.ConvertFrom(value);

			CheckResult(result!, expected);
		}

		CheckResult(canConvert, expectedCanConvert);
	}

	[TestCase(typeof(int), "Int")]
	[TestCase(typeof(double), "Fraction")]
	[TestCase(typeof(List<int>), "List(Int)")]
	[TestCase(typeof(Dictionary<double, string>), "Dictionary(Fraction,String)")]
	[TestCase(typeof(Dictionary<double, Dictionary<ConvertibleTimeOnly, ConvertibleDateOnly>>), "Dictionary(Fraction,Dictionary(Time,Date))")]
	public void TypeNameFormatTests(Type type, string expected)
	{
		CheckResult(Shared.FormatTypeName(type), expected);
	}
}