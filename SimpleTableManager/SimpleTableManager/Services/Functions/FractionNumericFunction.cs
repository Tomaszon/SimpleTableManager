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
			NumericFunctionOperator.Const or
			NumericFunctionOperator.Neg or
			NumericFunctionOperator.Abs or
			NumericFunctionOperator.Sum or
			NumericFunctionOperator.Sub or
			NumericFunctionOperator.Avg or
			NumericFunctionOperator.Min or
			NumericFunctionOperator.Max or
			NumericFunctionOperator.Round or
			NumericFunctionOperator.Mul or
			NumericFunctionOperator.Div or
			NumericFunctionOperator.Pow or
			NumericFunctionOperator.Sqrt or
			NumericFunctionOperator.Log2 or
			NumericFunctionOperator.Log10 or
			NumericFunctionOperator.LogE or
			NumericFunctionOperator.LogN => typeof(double),
			NumericFunctionOperator.Floor or
			NumericFunctionOperator.Ceiling => typeof(long),

			_ => base.GetOutType()
		};
	}
}