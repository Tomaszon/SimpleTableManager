namespace SimpleTableManager.Tests;

[SuppressMessage("Usage", "CA1861")]
[ExcludeFromCodeCoverage]
public class FunctionTests : TestBase
{
	[Test]
	[TestCase(BooleanFunctionOperator.And, new[] { true, false }, false)]
	[TestCase(BooleanFunctionOperator.Or, new[] { false, true }, true)]
	[TestCase(BooleanFunctionOperator.Not, new[] { false, true }, new[] { true, false })]
	public void BoolTest(BooleanFunctionOperator operation, bool[] values, params bool[] results)
	{
		var fn = CreateFunction(operation, values.Select(e => (FormattableBoolean)e));

		CheckResults(fn.Execute(), results.Select(e => (FormattableBoolean)e));
	}

	[Test]
	[TestCase(DateTimeFunctionOperator.Const, new[] { "0002-02-02 10:30", "0002-02-02 02:20" }, "0002-02-02 10:30", "0002-02-02 02:20")]
	[TestCase(DateTimeFunctionOperator.Sum, new[] { "0002-02-02 10:30", "0002-02-02 02:20" }, "0004-04-04 12:50")]
	public void DateTimeTest(DateTimeFunctionOperator operation, string[] values, params string[] results)
	{
		var fn = CreateFunction(operation, values.Select(s => DateTime.Parse(s)));

		CheckResults(fn.Execute(), results.Select(s => DateTime.Parse(s)));
	}

	[Test]
	[TestCase(DateTimeFunctionOperator.Const, new[] { "0001-01-01", "0001-02-03" }, "0001-01-01", "0001-02-03")]
	[TestCase(DateTimeFunctionOperator.Sum, new[] { "0001-01-01", "0001-02-03" }, "0002-03-04")]
	public void DateOnlyTest(DateTimeFunctionOperator operation, string[] values, params string[] results)
	{
		var fn = CreateFunction(operation, values.Select(s => ConvertibleDateOnly.Parse(s, null)));

		CheckResults(fn.Execute(), results.Select(s => ConvertibleDateOnly.Parse(s, null)));
	}

	[Test]
	[TestCase(DateTimeFunctionOperator.Const, new[] { "01:02:03.001", "01:02:03.1", "01:02:03", "01:02" }, "01:02:03.001", "01:02:03.1", "01:02:03", "01:02")]
	[TestCase(DateTimeFunctionOperator.Sum, new[] { "01:02:03.001", "01:02:03.1", "01:02:03", "01:02" }, "04:08:09.101")]
	public void TimeOnlyTest(DateTimeFunctionOperator operation, string[] values, params string[] results)
	{
		var fn = CreateFunction(operation, values.Select(s => ConvertibleTimeOnly.Parse(s, null)));

		CheckResults(fn.Execute(), results.Select(s => ConvertibleTimeOnly.Parse(s, null)));
	}

	[Test]
	[TestCase(NumericFunctionOperator.Sum, new long[] { 4, 3 }, 7)]
	[TestCase(NumericFunctionOperator.And, new long[] { 2, 3 }, 2)]
	[TestCase(NumericFunctionOperator.Or, new long[] { 4, 2 }, 6)]
	[TestCase(NumericFunctionOperator.Neg, new long[] { 5 }, -5)]
	[TestCase(NumericFunctionOperator.Div, new long[] { 10, 5 }, 2)]
	[TestCase(NumericFunctionOperator.Max, new long[] { 1, 5, 3 }, 5)]
	[TestCase(NumericFunctionOperator.Abs, new long[] { -1 }, 1)]
	[TestCase(NumericFunctionOperator.Rem, new long[] { 10, 3 }, new long[] { 0, 1 })]
	[TestCase(NumericFunctionOperator.Sqrt, new long[] { 4, 9, 16 }, new long[] { 2, 3, 4 })]
	[TestCase(NumericFunctionOperator.LogN, new long[] { 2, 4, 8 }, new long[] { 1, 2, 3 })]
	[TestCase(NumericFunctionOperator.Log2, new long[] { 2, 4, 8 }, new long[] { 1, 2, 3 })]
	[TestCase(NumericFunctionOperator.Log10, new long[] { 10, 100, 1 }, new long[] { 1, 2, 0 })]
	public void IntegerTest(NumericFunctionOperator operation, long[] values, params long[] results)
	{
		var fn = CreateFunction(operation, values);

		CheckResults(fn.Execute(), results);
	}

	[Test]
	public void FractionTest1()
	{
		var fn = CreateFunction(NumericFunctionOperator.Sum, [4d, 3d]);

		CheckResults(fn.Execute(), 7d.Wrap());
	}

	[Test]
	public void FractionTest2()
	{
		var fn = CreateFunction(NumericFunctionOperator.Sqrt, [4d, 9d, 16d]);

		CheckResults(fn.Execute(), [2d, 3d, Math.Sqrt(16d)]);
	}

	[Test]
	public void FractionTest3()
	{
		var fn = CreateFunction(NumericFunctionOperator.LogN, [2d, 4d, 16d]);

		CheckResults(fn.Execute(), [1d, 2d, Math.Log2(16d)]);
	}

	[Test]
	public void FractionTest4()
	{
		var fn = CreateFunction(NumericFunctionOperator.LogE, [double.E, double.E * double.E]);

		CheckResults(fn.Execute(), [1d, 2d]);
	}

	[Test]
	[TestCase(NumericFunctionOperator.Round, 4.234, 4.23, typeof(double))]
	[TestCase(NumericFunctionOperator.Round, 4.235, 4.24, typeof(double))]
	[TestCase(NumericFunctionOperator.Ceiling, 4.235, 5, typeof(long))]
	[TestCase(NumericFunctionOperator.Floor, 4.235, 4, typeof(long))]
	public void FractionTest5(NumericFunctionOperator functionOperator, double number, double expected, Type expectedResultType)
	{
		var fn = CreateFunction(functionOperator, [number]);

		CheckResults(fn.Execute(), [expected]);
		CheckResult(fn.GetOutType(), expectedResultType);
	}

	[Test]
	[TestCase(CharacterFunctionOperator.Concat, new[] { 'a', 'l', 'm', 'a' }, "alma")]
	[TestCase(CharacterFunctionOperator.Join, new[] { 'a', 'b' }, "a,b")]
	[TestCase(CharacterFunctionOperator.Repeat, new[] { 'a' }, "aaa")]
	public void CharTest(CharacterFunctionOperator operation, char[] values, params string[] results)
	{
		var na = new IFunctionArgument[]
		{
			new ConstFunctionArgument<char>(ArgumentName.Separator, ","),
			new ConstFunctionArgument<char>(ArgumentName.Count, 3)
		};

		var fn = CreateFunction(operation, na, values);

		CheckResults(fn.Execute(), results);
		CheckResult(fn.GetOutType(), typeof(string));
	}

	[Test]
	[TestCase(StringFunctionOperator.Like, typeof(bool), new[] { "aaaa", "bbbb" }, true)]
	[TestCase(StringFunctionOperator.Like, typeof(bool), new[] { "cc", "bbbb" }, false)]
	[TestCase(StringFunctionOperator.Split, typeof(string), new[] { "al|ma" }, new[] { "al", "ma" })]
	[TestCase(StringFunctionOperator.Len, typeof(int), new[] { "alma" }, 4)]
	[TestCase(StringFunctionOperator.Join, typeof(string), new[] { "alma", "körte" }, new[] { "alma|körte" })]
	[TestCase(StringFunctionOperator.Blow, typeof(char), new[] { "string" }, new object[] { 's', 't', 'r', 'i', 'n', 'g' })]
	[TestCase(StringFunctionOperator.Trim, typeof(string), new[] { "a ", " b" }, new[] { "a", "b" })]
	public void StringTest(StringFunctionOperator operation, Type outType, string[] values, params object[] results)
	{
		var na = new IFunctionArgument[]
			{
				new ConstFunctionArgument<string>(ArgumentName.Separator, "|"),
				new ConstFunctionArgument<string>(ArgumentName.Pattern, "a{4}")
			};

		var fn = CreateFunction(operation, na, values);

		CheckResults(fn.Execute(), results);
		CheckResult(fn.GetOutType(), outType);
	}

	[Test]
	[TestCase(1993, 12, 19, 10, 5, 1)]
	public void DateAndTimeTest(int year, int month, int day, int hour, int minute, int second)
	{
		var result = new DateTime(year, month, day, hour, minute, second);

		var fnd = CreateFunction(DateTimeFunctionOperator.Const, result);

		CheckResults(fnd.Execute(), [result]);
	}

	[Test]
	[TestCase(10, 5, 2, "HH:mm:ss", "10:05:02")]
	[TestCase(10, 0, 0, "HH", "10")]
	public void TimeFormatTest(int h, int m, int s, string format, string expectedResult)
	{
		var fn = CreateFunction(DateTimeFunctionOperator.Const, [new ConstFunctionArgument<ConvertibleTimeOnly>(ArgumentName.Format, format)], new ConvertibleTimeOnly(new(h, m, s)));

		var formattedResult = fn.ExecuteAndFormat();

		CheckResults(formattedResult, expectedResult.Wrap());
	}

	[Test]
	[TestCase(1993, 12, 19, "yyyy.MM.dd", "1993.12.19")]
	[TestCase(1993, 12, 19, "yyyy-MM-dd", "1993-12-19")]
	public void DateFormatTest(int y, int m, int d, string format, string expectedResult)
	{
		var fn = CreateFunction(DateTimeFunctionOperator.Const, [new ConstFunctionArgument<ConvertibleDateOnly>(ArgumentName.Format, format)], new ConvertibleDateOnly(new(y, m, d)));

		var formattedResult = fn.ExecuteAndFormat();

		CheckResults(formattedResult, expectedResult.Wrap());
	}

	// [Test]
	// [TestCase(Shape2dOperator.Area, typeof(Rectangle), new string[] { "1", "2;3" }, new object[] { 1, 6 })]
	// [TestCase(Shape2dOperator.Perimeter, typeof(Rectangle), new string[] { "1", "2;3" }, new object[] { 4, 10 })]
	// [TestCase(Shape2dOperator.Area, typeof(RightTriangle), new string[] { "1", "2;3" }, new object[] { .5, 3 })]
	// [TestCase(Shape2dOperator.Perimeter, typeof(RightTriangle), new string[] { "2" }, new object[] { 6.83 })]
	// public void ShapeTest(Shape2dOperator functionOperator, Type type, string[] shapes, object[] results)
	// {
	// 	var fn = FunctionCollection.GetFunction(type.GetFriendlyName(), functionOperator.ToString(), null, shapes.Cast<object>());

	// 	CheckResults(fn.Execute().Cast<decimal>().Select(d => Math.Round(d, 2)).Cast<object>(), results);
	// }
}