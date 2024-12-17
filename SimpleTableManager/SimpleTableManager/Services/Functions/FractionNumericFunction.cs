namespace SimpleTableManager.Services.Functions;

[FunctionMappingType(typeof(decimal))]
public class FractionNumericFunction : NumericFunctionBase<decimal, object>
{
	public override IEnumerable<object> ExecuteCore()
	{
		var decimals = GetNamedArgument<int>(ArgumentName.Decimals);

		return Operator switch
		{
			NumericFunctionOperator.Floor => ConvertedUnwrappedArguments.Select(p => (int)decimal.Floor(p)).Cast<object>(),

			NumericFunctionOperator.Ceiling => ConvertedUnwrappedArguments.Select(p => (int)decimal.Ceiling(p)).Cast<object>(),

			NumericFunctionOperator.Round => ConvertedUnwrappedArguments.Select(p => decimal.Round(p, decimals)).Cast<object>(),

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