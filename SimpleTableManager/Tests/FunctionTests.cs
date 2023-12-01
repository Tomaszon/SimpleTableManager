using SimpleTableManager.Services;
using SimpleTableManager.Services.Functions;

namespace SimpleTableManager.Tests
{
	[SuppressMessage("Usage", "CA1861")]
	public class FunctionTests
	{
		[OneTimeSetUp]
		public void Setup()
		{
			Settings.FromJson(@".\Configs\settings.json");
		}

		private static IFunction CreateFunction<T>(Enum functionOperator, T[] args)
		{
			return CreateFunction(functionOperator, null, args);
		}

		private static IFunction CreateFunction<T>(Enum functionOperator, Dictionary<ArgumentName, string>? namedArguments, params T[] args)
		{
			return FunctionCollection.GetFunction(typeof(T).GetFriendlyName(), functionOperator.ToString(), namedArguments, args.Cast<object>());
		}

		private static void CheckResults<T>(IEnumerable<object> result, IEnumerable<T> expectedValues)
		{
			for (int i = 0; i < result.Count(); i++)
			{
				Assert.That(result.ElementAt(i), Is.EqualTo(expectedValues.ElementAt(i)));
			}
		}

		[Test]
		[TestCase(BooleanFunctionOperator.And, new[] { true, false }, false)]
		[TestCase(BooleanFunctionOperator.Or, new[] { false, true }, true)]
		[TestCase(BooleanFunctionOperator.Not, new[] { false, true }, new[] { true, false })]
		public void BoolTest(BooleanFunctionOperator operation, bool[] values, params bool[] results)
		{
			var fn = CreateFunction(operation, values);

			CheckResults(fn.Execute(), results);
		}

		[Test]
		[TestCase(DateTimeFunctionOperator.Sum, new[] { "0001-01-01 10:30", "0001-01-01 02:20" }, "0001-01-01 12:50")]
		public void DateTimeTest(DateTimeFunctionOperator operation, string[] values, params string[] results)
		{
			var fn = CreateFunction(operation, values.Select(s => DateTime.Parse(s)).ToArray());

			CheckResults(fn.Execute(), results.Select(s => DateTime.Parse(s)).ToArray());
		}

		[Test]
		[TestCase(NumericFunctionOperator.Sum, new[] { 4, 3 }, 7)]
		[TestCase(NumericFunctionOperator.Neg, new[] { 5 }, -5)]
		[TestCase(NumericFunctionOperator.Div, new[] { 10, 5 }, 2)]
		[TestCase(NumericFunctionOperator.Max, new[] { 1, 5, 3 }, 5)]
		[TestCase(NumericFunctionOperator.Abs, new[] { -1 }, 1)]
		[TestCase(NumericFunctionOperator.Rem, new[] { 10, 3 }, new[] { 0, 1 })]
		public void IntegerTest(NumericFunctionOperator operation, int[] values, params int[] results)
		{
			var fn = CreateFunction(operation, values);

			CheckResults(fn.Execute(), results);
		}

		[Test]
		[TestCase(CharacterFunctionOperator.Concat, new[] { 'a', 'l', 'm', 'a' }, "alma")]
		[TestCase(CharacterFunctionOperator.Join, new[] { 'a', 'b' }, "a,b")]
		[TestCase(CharacterFunctionOperator.Repeat, new[] { 'a' }, "aaa")]
		public void CharTest(CharacterFunctionOperator operation, char[] values, params string[] results)
		{
			var na = new Dictionary<ArgumentName, string>()
			{
				{ ArgumentName.Separator, "," },
				{ ArgumentName.Count, "3" }
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
			var na = new Dictionary<ArgumentName, string>()
			{
				{ ArgumentName.Separator, "|" }
			};
			var fn = CreateFunction(operation, na, values);

			CheckResults(fn.Execute(), results);
		}

		[Test]
		[TestCase(10, 5, 1)]
		public void DateAndTimeTest(int hours, int minutes, int seconds)
		{
			(DateOnly d, _) = DateTime.Now;
			var t = new TimeOnly(hours, minutes, seconds);

			var fnd = CreateFunction(DateTimeFunctionOperator.Now, Array.Empty<DateOnly>());

			CheckResults(fnd.Execute(), new[] { d });

			var fnt = CreateFunction(DateTimeFunctionOperator.Const, new[] { t });

			CheckResults(fnt.Execute(), new[] { t });
		}

		[Test]
		[TestCase(10, 5, 2, "HH:mm:ss", "10:05:02")]
		[TestCase(10, 0, 0, "HH", "10")]
		public void TimeFormatTest(int h, int m, int s, string format, string expectedResult)
		{
			IFunction fn = new TimeFunction()
			{
				Arguments = new[] { new TimeOnly(h, m, s) },
				Operator = DateTimeFunctionOperator.Const,
				NamedArguments = new() { { ArgumentName.Format, format } }
			};

			var result = fn.Execute().First();

			var f = fn.GetNamedArgument<string>(ArgumentName.Format);

			var formattedResult = string.Format(new ContentFormatter(f), "{0}", result);

			CheckResults(formattedResult.Wrap(), expectedResult.Wrap());
		}
	}
}