namespace SimpleTableManager.Services.Functions;

[FunctionMappingType(typeof(double))]
public class FractionNumericFunction : NumericFunctionBase<double, object>
{
	public override string GetFriendlyName()
	{
		return typeof(double).GetFriendlyName();
	}

	public override IEnumerable<object> ExecuteCore()
	{
		var decimals = GetNamedArgument<int>(ArgumentName.Decimals);

		return Operator switch
		{
			NumericFunctionOperator.Floor => UnwrappedUnnamedArguments.Select(p => (int)double.Floor(p)).Cast<object>(),

			NumericFunctionOperator.Ceiling => UnwrappedUnnamedArguments.Select(p => (int)double.Ceiling(p)).Cast<object>(),

			NumericFunctionOperator.Round => UnwrappedUnnamedArguments.Select(p => double.Round(p, decimals)).Cast<object>(),

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