namespace SimpleTableManager.Tests;

[SuppressMessage("Usage", "CA1861")]
public class FunctionTests : TestBase
{
	[Test]
	[TestCase(BooleanFunctionOperator.And, new[] { true, false }, false)]
	[TestCase(BooleanFunctionOperator.Or, new[] { false, true }, true)]
	[TestCase(BooleanFunctionOperator.Not, new[] { false, true }, new[] { true, false })]
	public void BoolTest(BooleanFunctionOperator operation, bool[] values, params bool[] results)
	{
		var fn = CreateFunction(operation, values);

		CheckResults(fn.Execute(), results);
	}

	//REWORK
	[Test]
	[TestCase(DateTimeFunctionOperator.Sum, new[] { "0002-02-02 10:30", "0002-02-02 02:20" }, "0004-04-04 12:50")]
	public void DateTimeTest(DateTimeFunctionOperator operation, string[] values, params string[] results)
	{
		var fn = CreateFunction(operation, values.Select(s => DateTime.Parse(s)));

		CheckResults(fn.Execute(), results.Select(s => DateTime.Parse(s)));
	}


	[Test]
	[TestCase(DateTimeFunctionOperator.Sum, new[] { "0001-01-01", "0001-02-03" }, "0002-03-04")]
	public void DateOnlyTest(DateTimeFunctionOperator operation, string[] values, params string[] results)
	{
		var fn = CreateFunction(operation, values.Select(s => ConvertibleDateOnly.Parse(s, null)));

		CheckResults(fn.Execute(), results.Select(s => ConvertibleDateOnly.Parse(s, null)));
	}

	[Test]
	[TestCase(NumericFunctionOperator.Sum, new long[] { 4, 3 }, 7)]
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
	}

	[Test]
	[TestCase(StringFunctionOperator.Split, new[] { "al|ma" }, new[] { "al", "ma" })]
	[TestCase(StringFunctionOperator.Len, new[] { "alma" }, 4)]
	[TestCase(StringFunctionOperator.Join, new[] { "alma", "körte" }, new[] { "alma|körte" })]
	[TestCase(StringFunctionOperator.Blow, new[] { "string" }, new object[] { 's', 't', 'r', 'i', 'n', 'g' })]
	[TestCase(StringFunctionOperator.Trim, new[] { "a ", " b" }, new[] { "a", "b" })]
	public void StringTest(StringFunctionOperator operation, string[] values, params object[] results)
	{
		var na = new IFunctionArgument[]
			{
				new ConstFunctionArgument<string>(ArgumentName.Separator, "|")
			};

		var fn = CreateFunction(operation, na, values);

		CheckResults(fn.Execute(), results);
	}

	[Test]
	[TestCase(1993, 12, 19, 10, 5, 1)]
	public void DateAndTimeTest(int year, int month, int day, int hour, int minute, int second)
	{
		var result = new DateTime(year, month, day, hour, minute, second);

		var fnd = CreateFunction(DateTimeFunctionOperator.Const, result);

		CheckResults(fnd.Execute(), [result]);
	}

	// [Test]
	// [TestCase(10, 5, 2, "HH:mm:ss", "10:05:02")]
	// [TestCase(10, 0, 0, "HH", "10")]
	// public void TimeFormatTest(int h, int m, int s, string format, string expectedResult)
	// {
	// 	var na = new Dictionary<ArgumentName, string>()
	// 		{
	// 			{ ArgumentName.Format, format }
	// 		};

	// 	var fn = CreateFunction(DateTimeFunctionOperator.Const, na, new TimeOnly(h, m, s));

	// 	var formattedResult = fn.ExecuteAndFormat();

	// 	CheckResults(formattedResult, expectedResult.Wrap());
	// }

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

	// [Test]
	// [TestCase(new[] { 2, 3 }, new[] { 4 }, 20)]
	// public void InnerIntFunctionTest(int[] args1, int[] args2, int result)
	// {
	// 	var inner = new IntegerNumericFunction()
	// 	{
	// 		Operator = NumericFunctionOperator.Sum,
	// 		Arguments = args1
	// 	};

	// 	var outer = new IntegerNumericFunction()
	// 	{
	// 		Operator = NumericFunctionOperator.Mul,
	// 		Arguments = inner.Execute().Union(args2)
	// 	};

	// 	CheckResults(outer.Execute().Cast<object>(), result.Wrap());
	// }

	// [Test]
	// [TestCase(new[] { 2, 3 }, new[] { 4 }, 20)]
	// public void InnerIntFunctionTest2(int[] args1, int[] args2, int result)
	// {
	// 	var fn1 = CreateFunction(NumericFunctionOperator.Sum, args1);

	// 	var fn2 = CreateFunction(NumericFunctionOperator.Mul, args2.Union(fn1.Execute().Cast<int>()));

	// 	CheckResults(fn2.Execute(), result.Wrap());
	// }
}