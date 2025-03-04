namespace SimpleTableManager.Tests.FunctionTests;

[ExcludeFromCodeCoverage]
public class CharacterFunctionTests : TestBase
{
	[TestCase(CharacterFunctionOperator.Concat, new[] { 'a', 'l', 'm', 'a' }, "alma")]
	[TestCase(CharacterFunctionOperator.Join, new[] { 'a', 'b' }, "a,b")]
	[TestCase(CharacterFunctionOperator.Repeat, new[] { 'a' }, "aaa")]
	public void CharTest(CharacterFunctionOperator operation, char[] values, params string[] results)
	{
		var na = new NamedConstFunctionArgument[]
		{
			new(ArgumentName.Separator, ","),
			new(ArgumentName.Count, 3)
		};

		var fn = CreateFunction(operation, na, values);

		CheckResults(fn.Execute(), results);
		CheckResult(fn.GetOutType(), typeof(string));
	}
}