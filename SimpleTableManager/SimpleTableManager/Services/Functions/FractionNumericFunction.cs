namespace SimpleTableManager.Services.Functions;

[FunctionMappingType(typeof(double))]
public class FractionNumericFunction : NumericFunctionBase<FractionType ,double, IType>
{
	public override IEnumerable<IType> ExecuteCore()
	{
		return Operator switch
		{
			NumericFunctionOperator.Floor => UnwrappedUnnamedArguments.Select(p => (IntegerType)double.Floor(p)),

			NumericFunctionOperator.Ceiling => UnwrappedUnnamedArguments.Select(p => (IntegerType)double.Ceiling(p)),

			NumericFunctionOperator.Round => Round(),

			NumericFunctionOperator.Sqrt => Sqrt(),

			NumericFunctionOperator.Log2 => LogN(2),

			NumericFunctionOperator.Log10 => LogN(10),

			NumericFunctionOperator.LogE => LogN(double.E),

			NumericFunctionOperator.LogN => LogN(GetNamedArgument<double>(ArgumentName.Base)),

			_ => base.ExecuteCore()
		};
	}

	private IEnumerable<FractionType> Round()
	{
		var decimals = GetNamedArgument<int>(ArgumentName.Decimals);

		return UnwrappedUnnamedArguments.Select(p => (FractionType)double.Round(p, decimals));
	}

	public override Type GetOutType()
	{
		return Operator switch
		{
			NumericFunctionOperator.Floor or
			NumericFunctionOperator.Ceiling => typeof(IntegerType),
			< NumericFunctionOperator.Greater => typeof(FractionType),

			_ => base.GetOutType()
		};
	}
}