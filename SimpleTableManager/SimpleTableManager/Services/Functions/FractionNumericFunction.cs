namespace SimpleTableManager.Services.Functions;

[FunctionMappingType(typeof(double))]
public class FractionNumericFunction : NumericFunctionBase<double, IConvertible>
{
	public override IEnumerable<IConvertible> ExecuteCore()
	{
		var decimals = GetNamedArgument<int>(ArgumentName.Decimals);

		return Operator switch
		{
			NumericFunctionOperator.Floor => UnwrappedUnnamedArguments.Select(p => (long)double.Floor(p)).Cast<IConvertible>(),

			NumericFunctionOperator.Ceiling => UnwrappedUnnamedArguments.Select(p => (long)double.Ceiling(p)).Cast<IConvertible>(),

			NumericFunctionOperator.Round => UnwrappedUnnamedArguments.Select(p => double.Round(p, decimals)).Cast<IConvertible>(),

			_ => base.ExecuteCore()
		};
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