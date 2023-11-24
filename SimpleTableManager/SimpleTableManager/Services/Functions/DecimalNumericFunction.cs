namespace SimpleTableManager.Services.Functions;

[NamedArgument(ArgumentName.Decimals, 2)]
public class DecimalNumericFunction : NumericFunctionBase<decimal, object>
{
	public override IEnumerable<object> Execute()
	{
		var decimals = GetNamedArgument<int>(ArgumentName.Decimals);

		return Operator switch
		{
			NumericFunctionOperator.Floor => Arguments.Select(p => (int)decimal.Floor(p)).Cast<object>(),

			NumericFunctionOperator.Ceiling => Arguments.Select(p => (int)decimal.Ceiling(p)).Cast<object>(),

			NumericFunctionOperator.Round => Arguments.Select(p => decimal.Round(p, decimals)).Cast<object>(),

			_ => base.Execute()
		};
	}
}