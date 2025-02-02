namespace SimpleTableManager.Services.Functions;

[NamedArgument<long>(ArgumentName.Divider, 2)]
[FunctionMappingType(typeof(long))]
public class IntegerNumericFunction : NumericFunctionBase<long, long>
{
	public override IEnumerable<long> ExecuteCore()
	{
		var divider = GetNamedArgument<long>(ArgumentName.Divider);

		return Operator switch
		{
			NumericFunctionOperator.Rem => UnwrappedUnnamedArguments.Select(p => DivRem(p, divider)),

			NumericFunctionOperator.And => And(UnwrappedUnnamedArguments).Wrap(),

			NumericFunctionOperator.Or => Or(UnwrappedUnnamedArguments).Wrap(),

			_ => base.ExecuteCore()
		};
	}

	private static long And(IEnumerable<long> array)
	{
		return array.Aggregate(~0L, (a, c) => a &= c);
	}

	private static long Or(IEnumerable<long> array)
	{
		return array.Aggregate(0L, (a, c) => a |= c);
	}

	private static long DivRem(long a, long b)
	{
		Math.DivRem(a, b, out var rem);

		return rem;
	}
}