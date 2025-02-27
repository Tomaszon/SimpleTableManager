namespace SimpleTableManager.Services.Functions;

[NamedArgument<long>(ArgumentName.Divider, 2)]
[FunctionMappingType(typeof(IntegerType))]
public class IntegerNumericFunction : NumericFunctionBase<IntegerType, long, IType>
{
	public override IEnumerable<IType> ExecuteCore()
	{
		return Operator switch
		{
			NumericFunctionOperator.Rem => Rem().Cast<IType>(),

			NumericFunctionOperator.And => And().Wrap(),

			NumericFunctionOperator.Or => Or().Wrap(),

			NumericFunctionOperator.Sqrt => Sqrt(),

			NumericFunctionOperator.Log2 => LogN(2).Select(e => e.ToType<IntegerType>()),

			NumericFunctionOperator.Log10 => LogN(10).Select(e => e.ToType<IntegerType>()),

			NumericFunctionOperator.LogE => LogN(double.E).Select(e => e.ToType<IntegerType>()),

			NumericFunctionOperator.LogN => LogN(GetNamedArgument<double>(ArgumentName.Base)).Select(e => e.ToType<IntegerType>()),

			_ => base.ExecuteCore()
		};
	}

	private IEnumerable<IntegerType> Rem()
	{
		var divider = GetNamedArgument<long>(ArgumentName.Divider);

		return UnwrappedUnnamedArguments.Select(p => DivRem(p, divider));
	}

	private IntegerType And()
	{
		return UnwrappedUnnamedArguments.Aggregate(~0L, (a, c) => a &= c);
	}

	private IntegerType Or()
	{
		return UnwrappedUnnamedArguments.Aggregate(0L, (a, c) => a |= c);
	}

	private static IntegerType DivRem(IntegerType a, IntegerType b)
	{
		Math.DivRem(a, b, out var rem);

		return rem;
	}

	public override Type GetOutType()
	{
		return Operator switch
		{
			< NumericFunctionOperator.Greater => typeof(IntegerType),

			_ => base.GetOutType()
		};
	}
}