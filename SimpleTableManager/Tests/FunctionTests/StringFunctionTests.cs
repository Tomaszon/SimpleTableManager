namespace SimpleTableManager.Tests.FunctionTests;

[ExcludeFromCodeCoverage]
public class StringFunctionTests : TestBase
{
	[TestCase(StringFunctionOperator.Split, typeof(string), new[] { "al|ma" }, new[] { "al", "ma" })]
	[TestCase(StringFunctionOperator.Len, typeof(long), new[] { "alma" }, 4)]
	[TestCase(StringFunctionOperator.Join, typeof(string), new[] { "alma", "körte" }, new[] { "alma|körte" })]
	[TestCase(StringFunctionOperator.Blow, typeof(char), new[] { "string" }, new object[] { 's', 't', 'r', 'i', 'n', 'g' })]
	[TestCase(StringFunctionOperator.Trim, typeof(string), new[] { "a ", " b" }, new[] { "a", "b" })]
	[TestCase(StringFunctionOperator.Min, typeof(string), new[] { "alma", "körte" }, "alma")]
	[TestCase(StringFunctionOperator.Max, typeof(string), new[] { "alma", "körte" }, "körte")]
	public void StringTest(StringFunctionOperator operation, Type outType, string[] values, params object[] results)
	{
		var na = new NamedConstFunctionArgument[]
		{
			new(ArgumentName.Separator, "|"),
		};

		var fn = CreateFunction(operation, na, values);

		CheckResults(fn.Execute(), results);
		CheckResult(fn.GetOutType(), outType);
	}

	[TestCase(StringFunctionOperator.Like, typeof(FormattableBoolean), new[] { "aaaa", "bbbb" }, true)]
	[TestCase(StringFunctionOperator.Like, typeof(FormattableBoolean), new[] { "cc", "bbbb" }, false)]
	public void StringTest2(StringFunctionOperator operation, Type outType, string[] values, bool result)
	{
		var na = new NamedConstFunctionArgument[]
		{
			new(ArgumentName.Pattern, "a{4}")
		};

		var fn = CreateFunction(operation, na, values);

		CheckResult(fn.Execute().Single(), (FormattableBoolean)result);
		CheckResult(fn.GetOutType(), outType);
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