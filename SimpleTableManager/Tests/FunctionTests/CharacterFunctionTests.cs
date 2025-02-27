namespace SimpleTableManager.Tests.FunctionTests;

[ExcludeFromCodeCoverage]
public class CharacterFunctionTests : TestBase
{
	[TestCase(CharacterFunctionOperator.Concat, new[] { 'a', 'l', 'm', 'a' }, "alma")]
	[TestCase(CharacterFunctionOperator.Join, new[] { 'a', 'b' }, "a,b")]
	[TestCase(CharacterFunctionOperator.Repeat, new[] { 'a' }, "aaa")]
	public void CharTest(CharacterFunctionOperator operation, char[] values, params string[] results)
	{
		var na = new IFunctionArgument[]
		{
			new ConstFunctionArgument<CharacterType>(ArgumentName.Separator, (StringType)","),
			new ConstFunctionArgument<CharacterType>(ArgumentName.Count, (IntegerType)3)
		};

		// var fn = CreateFunction(operation, na, values.CastTo<char, CharacterType>());
		//
		// CheckResults(fn.Execute(), results.CastTo<string, StringType>());
		// CheckResult(fn.GetOutType(), typeof(StringType));
	}
}