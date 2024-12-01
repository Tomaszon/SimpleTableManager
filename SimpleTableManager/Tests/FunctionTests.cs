using SimpleTableManager.Models;
using SimpleTableManager.Services;
using SimpleTableManager.Services.Functions;

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

	// 		[Test]
	// 		[TestCase(DateTimeFunctionOperator.Sum, new[] { "0001-01-01 10:30", "0001-01-01 02:20" }, "0001-01-01 12:50")]
	// 		public void DateTimeTest(DateTimeFunctionOperator operation, string[] values, params string[] results)
	// 		{
	// 			var fn = CreateFunction(operation, values.Select(s => DateTime.Parse(s)).ToArray());

	// 			CheckResults(fn.Execute(), results.Select(s => DateTime.Parse(s)).ToArray());
	// 		}

	[Test]
	[TestCase(NumericFunctionOperator.Sum, new[] { 4, 3 }, 7)]
	[TestCase(NumericFunctionOperator.Neg, new[] { 5 }, -5)]
	[TestCase(NumericFunctionOperator.Div, new[] { 10, 5 }, 2)]
	[TestCase(NumericFunctionOperator.Max, new[] { 1, 5, 3 }, 5)]
	[TestCase(NumericFunctionOperator.Abs, new[] { -1 }, 1)]
	[TestCase(NumericFunctionOperator.Rem, new[] { 10, 3 }, new[] { 0, 1 })]
	[TestCase(NumericFunctionOperator.Sqrt, new[] { 4, 9, 16 }, new[] { 2, 3, 4 })]
	[TestCase(NumericFunctionOperator.LogN, new[] { 2, 4, 8 }, new[] { 1, 2, 3 })]
	[TestCase(NumericFunctionOperator.Log2, new[] { 2, 4, 8 }, new[] { 1, 2, 3 })]
	[TestCase(NumericFunctionOperator.Log10, new[] { 10, 100, 1 }, new[] { 1, 2, 0 })]
	public void IntegerTest(NumericFunctionOperator operation, int[] values, params int[] results)
	{
		var fn = CreateFunction(operation, values);

		CheckResults(fn.Execute(), results);
	}

	[Test]
	public void DecimalTest1()
	{
		var fn = CreateFunction(NumericFunctionOperator.Sum, new[] { 4m, 3m });

		CheckResults(fn.Execute(), 7m.Wrap());
	}

	[Test]
	public void DecimalTest2()
	{
		var fn = CreateFunction(NumericFunctionOperator.Sqrt, new[] { 4m, 9m, 16m });

		CheckResults(fn.Execute(), new[] { 2m, 3m, Convert.ToDecimal(Math.Sqrt(Convert.ToDouble(16m))) });
	}

	[Test]
	public void DecimalTest3()
	{
		var fn = CreateFunction(NumericFunctionOperator.LogN, new[] { 2m, 4m, 16m });

		CheckResults(fn.Execute(), new[] { 1m, 2m, Convert.ToDecimal(Math.Log2(16d)) });
	}

	[Test]
	public void DecimalTest4()
	{
		var e = Convert.ToDecimal(double.E);

		var fn = CreateFunction(NumericFunctionOperator.LogE, new[] { e, e * e });

		CheckResults(fn.Execute(), new[] { 1m, 2m });
	}

	// 		[Test]
	// 		[TestCase(CharacterFunctionOperator.Concat, new[] { 'a', 'l', 'm', 'a' }, "alma")]
	// 		[TestCase(CharacterFunctionOperator.Join, new[] { 'a', 'b' }, "a,b")]
	// 		[TestCase(CharacterFunctionOperator.Repeat, new[] { 'a' }, "aaa")]
	// 		public void CharTest(CharacterFunctionOperator operation, char[] values, params string[] results)
	// 		{
	// 			var na = new Dictionary<ArgumentName, string>()
	// 			{
	// 				{ ArgumentName.Separator, "," },
	// 				{ ArgumentName.Count, "3" }
	// 			};
	// 			var fn = CreateFunction(operation, na, values);

	// 			CheckResults(fn.Execute(), results);
	// 		}

	[Test]
	[TestCase(StringFunctionOperator.Split, new[] { "al|ma" }, new[] { "al", "ma" })]
	[TestCase(StringFunctionOperator.Len, new[] { "alma" }, 4)]
	[TestCase(StringFunctionOperator.Join, new[] { "alma", "körte" }, new[] { "alma|körte" })]
	[TestCase(StringFunctionOperator.Blow, new[] { "string" }, new object[] { 's', 't', 'r', 'i', 'n', 'g' })]
	[TestCase(StringFunctionOperator.Trim, new[] { "a ", " b" }, new[] { "a", "b" })]
	public void StringTest(StringFunctionOperator operation, string[] values, params object[] results)
	{
		var na = new Dictionary<ArgumentName, string>()
				{
					{ ArgumentName.Separator, "|" }
				};
		var fn = CreateFunction(operation, na, values);

		CheckResults(fn.Execute(), results);
	}

	// 		[Test]
	// 		[TestCase(10, 5, 1)]
	// 		public void DateAndTimeTest(int hours, int minutes, int seconds)
	// 		{
	// 			(DateOnly d, _) = DateTime.Now;
	// 			var t = new TimeOnly(hours, minutes, seconds);

	// 			var fnd = CreateFunction(DateTimeFunctionOperator.Now, Array.Empty<DateOnly>());

	// 			CheckResults(fnd.Execute(), new[] { d });

	// 			var fnt = CreateFunction(DateTimeFunctionOperator.Const, new[] { t });

	// 			CheckResults(fnt.Execute(), new[] { t });
	// 		}

	// 		[Test]
	// 		[TestCase(10, 5, 2, "HH:mm:ss", "10:05:02")]
	// 		[TestCase(10, 0, 0, "HH", "10")]
	// 		public void TimeFormatTest(int h, int m, int s, string format, string expectedResult)
	// 		{
	// 			IFunction fn = new TimeFunction()
	// 			{
	// 				Arguments = new[] { new TimeOnly(h, m, s) },
	// 				Operator = DateTimeFunctionOperator.Const,
	// 				NamedArguments = new() { { ArgumentName.Format, format } }
	// 			};

	// 			var result = fn.Execute().First();

	// 			var f = fn.GetNamedArgument<string>(ArgumentName.Format);

	// 			var formattedResult = string.Format(new ContentFormatter(f), "{0}", result);

	// 			CheckResults(formattedResult.Wrap(), expectedResult.Wrap());
	// 		}

	// 		[Test]
	// 		[TestCase(Shape2dOperator.Area, typeof(Rectangle), new string[] { "1", "2;3" }, new object[] { 1, 6 })]
	// 		[TestCase(Shape2dOperator.Perimeter, typeof(Rectangle), new string[] { "1", "2;3" }, new object[] { 4, 10 })]
	// 		[TestCase(Shape2dOperator.Area, typeof(RightTriangle), new string[] { "1", "2;3" }, new object[] { .5, 3 })]
	// 		[TestCase(Shape2dOperator.Perimeter, typeof(RightTriangle), new string[] { "2" }, new object[] { 6.83 })]
	// 		public void ShapeTest(Shape2dOperator functionOperator, Type type, string[] shapes, object[] results)
	// 		{
	// 			var fn = FunctionCollection.GetFunction(type.GetFriendlyName(), functionOperator.ToString(), null, shapes.Cast<object>());

	// 			CheckResults(fn.Execute().Cast<decimal>().Select(d => Math.Round(d, 2)).Cast<object>(), results);
	// 		}

	// 		[Test]
	// 		[TestCase(new[] { 2, 3 }, new[] { 4 }, 20)]
	// 		public void InnerIntFunctionTest(int[] args1, int[] args2, int result)
	// 		{
	// 			var inner = new IntegerNumericFunction()
	// 			{
	// 				Operator = NumericFunctionOperator.Sum,
	// 				Arguments = args1
	// 			};

	// 			var outer = new IntegerNumericFunction()
	// 			{
	// 				Operator = NumericFunctionOperator.Mul,
	// 				Arguments = inner.Execute().Union(args2)
	// 			};

	// 			CheckResults(outer.Execute().Cast<object>(), result.Wrap());
	// 		}

	// 		[Test]
	// 		[TestCase(new[] { 2, 3 }, new[] { 4 }, 20)]
	// 		public void InnerIntFunctionTest2(int[] args1, int[] args2, int result)
	// 		{
	// 			var fn1 = CreateFunction(NumericFunctionOperator.Sum, args1);

	// 			var fn2 = CreateFunction(NumericFunctionOperator.Mul, args2.Union(fn1.Execute().Cast<int>()).ToArray());

	// 			CheckResults(fn2.Execute(), result.Wrap());
	// 		}
}