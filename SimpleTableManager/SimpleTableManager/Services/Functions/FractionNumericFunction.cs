namespace SimpleTableManager.Services.Functions;

[NamedArgument<int>(ArgumentName.Decimals, 2)]
[FunctionMappingType(typeof(decimal))]
public class FractionNumericFunction : NumericFunctionBase<decimal, object>
{
	public override IEnumerable<object> ExecuteCore()
	{
		var decimals = GetNamedArgument<int>(ArgumentName.Decimals);

		return Operator switch
		{
			NumericFunctionOperator.Floor => UnwrappedArguments.Select(p => (int)decimal.Floor(p)).Cast<object>(),

			NumericFunctionOperator.Ceiling => UnwrappedArguments.Select(p => (int)decimal.Ceiling(p)).Cast<object>(),

			NumericFunctionOperator.Round => UnwrappedArguments.Select(p => decimal.Round(p, decimals)).Cast<object>(),

			_ => base.ExecuteCore()
		};
	}

	public override Type GetOutType()
	{
		return Operator switch
		{
			NumericFunctionOperator.Floor => typeof(int),
			NumericFunctionOperator.Ceiling => typeof(int),

			_ => typeof(decimal)
		};
	}
}