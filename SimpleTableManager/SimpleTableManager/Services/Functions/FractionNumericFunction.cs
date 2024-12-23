namespace SimpleTableManager.Services.Functions;

[FunctionMappingType(typeof(double))]
public class FractionNumericFunction : NumericFunctionBase<double, object>
{
	public override IEnumerable<object> ExecuteCore()
	{
		var decimals = GetNamedArgument<int>(ArgumentName.Decimals);

		return Operator switch
		{
			NumericFunctionOperator.Floor => UnwrappedArguments.Select(p => (int)double.Floor(p)).Cast<object>(),

			NumericFunctionOperator.Ceiling => UnwrappedArguments.Select(p => (int)double.Ceiling(p)).Cast<object>(),

			NumericFunctionOperator.Round => UnwrappedArguments.Select(p => double.Round(p, decimals)).Cast<object>(),

			_ => base.ExecuteCore()
		};
	}

	public override Type GetOutType()
	{
		return Operator switch
		{
			NumericFunctionOperator.Floor => typeof(int),
			NumericFunctionOperator.Ceiling => typeof(int),

			_ => typeof(double)
		};
	}
}