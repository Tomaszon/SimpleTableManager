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
			NumericFunctionOperator.Fact => Factorial().Cast<object>(),
			NumericFunctionOperator.Term => Termial().Cast<object>(),

			_ => base.ExecuteCore()
		};
	}

	private IEnumerable<long> Factorial()
	{
		return UnwrappedUnnamedArguments.Select(a => Fact((int)a));
	}

	private IEnumerable<long> Termial()
	{
		return UnwrappedUnnamedArguments.Select(a => Term((int)a));
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

	private static long Fact(int p)
	{
		return Shared.IndexArray(p, 1).Aggregate(1, (a, c) => a *= c);
	}

	private static long Term(int p)
	{
		return Shared.IndexArray(p, 1).Aggregate(0, (a, c) => a += c);
	}
}