namespace SimpleTableManager.Services.Functions;

[FunctionMappingType(typeof(double))]
public class FractionNumericFunction : NumericFunctionBase<double>
{
	public override IEnumerable<object> ExecuteCore()
	{
		return Operator switch
		{
			NumericFunctionOperator.Floor => UnwrappedUnnamedArguments.Select(p => (long)double.Floor(p)).Cast<IConvertible>(),

			NumericFunctionOperator.Ceiling => UnwrappedUnnamedArguments.Select(p => (long)double.Ceiling(p)).Cast<IConvertible>(),

			NumericFunctionOperator.Round => Round().Cast<IConvertible>(),

			_ => base.ExecuteCore()
		};
	}

	private IEnumerable<double> Round()
	{
		var decimals = GetNamedArgument<int>(ArgumentName.Decimals);

		return UnwrappedUnnamedArguments.Select(p => double.Round(p, decimals));
	}

	public override Type GetOutType()
	{
		return Operator switch
		{
			NumericFunctionOperator.Floor => typeof(long),
			NumericFunctionOperator.Ceiling => typeof(long),

			_ => typeof(double)
		};
	}
}