namespace SimpleTableManager.Services.Functions;

[NamedArgument<int>(ArgumentName.Divider, 2)]
[FunctionMappingType(typeof(int))]
public class IntegerNumericFunction : NumericFunctionBase<int, int>
{
	public override IEnumerable<int> ExecuteCore()
	{
		var divider = GetNamedArgument<int>(ArgumentName.Divider);

		return Operator switch
		{
			NumericFunctionOperator.Rem => UnwrappedArguments.Select(p => DivRem(p, divider)),

			NumericFunctionOperator.And => And(UnwrappedArguments).Wrap(),

			NumericFunctionOperator.Or => Or(UnwrappedArguments).Wrap(),

			_ => base.ExecuteCore()
		};
	}

	private static int And(IEnumerable<int> array)
	{
		return array.Aggregate(~0, (a, c) => a &= c);
	}

	private static int Or(IEnumerable<int> array)
	{
		return array.Aggregate(0, (a, c) => a |= c);
	}

	private static int DivRem(int a, int b)
	{
		Math.DivRem(a, b, out var rem);

		return rem;
	}
}