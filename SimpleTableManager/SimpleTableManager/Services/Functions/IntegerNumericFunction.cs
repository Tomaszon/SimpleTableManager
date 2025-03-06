using SimpleTableManager.Models.Enumerations.FunctionOperators;

namespace SimpleTableManager.Services.Functions;

[NamedArgument<long>(ArgumentName.Divider, 2)]
[FunctionMappingType(typeof(long))]
public class IntegerNumericFunction : NumericFunctionBase<long>
{
	public override IEnumerable<object> ExecuteCore()
	{
		return Operator switch
		{
			NumericFunctionOperator.Rem => Rem().Cast<object>(),
			NumericFunctionOperator.And => And(UnwrappedUnnamedArguments).Wrap<object>(),
			NumericFunctionOperator.Or => Or(UnwrappedUnnamedArguments).Wrap<object>(),

			_ => base.ExecuteCore()
		};
	}

	private IEnumerable<long> Rem()
	{
		var divider = GetNamedArgument<long>(ArgumentName.Divider);

		return UnwrappedUnnamedArguments.Select(p => DivRem(p, divider));
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