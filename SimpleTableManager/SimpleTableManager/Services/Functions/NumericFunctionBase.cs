using System.Numerics;

namespace SimpleTableManager.Services.Functions;

[NamedArgument<int>(ArgumentName.Power, 1), NamedArgument<double>(ArgumentName.Base, 2)]
[NamedArgument<int>(ArgumentName.Decimals, 2)]
public abstract class NumericFunctionBase<TIn, TUnderLying, TOut> :
	FunctionBase<NumericFunctionOperator, TIn, TOut>
	where TIn : INumericType<TIn, TUnderLying>, IParsable<TIn>, IParsableCore<TIn>
	where TUnderLying : IComparable, INumber<TUnderLying>, IMinMaxValue<TUnderLying>
	where TOut : IType
{
	public override IEnumerable<TOut> ExecuteCore()
	{
		return Operator switch
		{
			NumericFunctionOperator.Const => UnwrappedUnnamedArguments.Cast<TOut>(),

			NumericFunctionOperator.Neg => UnwrappedUnnamedArguments.Select(a => -a).Cast<TOut>(),

			NumericFunctionOperator.Abs => UnwrappedUnnamedArguments.Select(TIn.Abs).Cast<TOut>(),

			NumericFunctionOperator.Sum => Sum().Wrap().Cast<TOut>(),

			NumericFunctionOperator.Sub => Sub().Wrap(),

			NumericFunctionOperator.Avg => Avg().Wrap(),

			NumericFunctionOperator.Min => Min().Wrap(),

			NumericFunctionOperator.Max => Max().Wrap(),

			NumericFunctionOperator.Mul => Multiply().Wrap(),

			NumericFunctionOperator.Div => Divide().Wrap(),

			NumericFunctionOperator.Pow => Power(GetNamedArgument<int>(ArgumentName.Power)),

			NumericFunctionOperator.Greater => Greater().Wrap().Cast<TOut>(),

			NumericFunctionOperator.Less => Less().Wrap().Cast<TOut>(),

			NumericFunctionOperator.GreaterOrEquals => GreaterOrEquals().Wrap().Cast<TOut>(),

			NumericFunctionOperator.LessOrEquals => LessOrEquals().Wrap().Cast<TOut>(),

			NumericFunctionOperator.Equals => Equals().Wrap().Cast<TOut>(),

			NumericFunctionOperator.NotEquals => NotEquals().Wrap().Cast<TOut>(),

			_ => throw GetInvalidOperatorException()
		};
	}

	protected IEnumerable<TOut> LogN(FractionType @base)
	{
		return UnwrappedUnnamedArguments.Select(p =>
			double.Round(Math.Log(p.ToDouble(null), @base), GetNamedArgument<int>(ArgumentName.Decimals)).ToType<TOut>());
	}

	protected IEnumerable<TOut> Sqrt()
	{
		return UnwrappedUnnamedArguments.Select(p =>
			Math.Sqrt(p.ToDouble(null)).ToType<TOut>());
	}

	private IEnumerable<TOut> Power(int power)
	{
		return UnwrappedUnnamedArguments.Select(p =>
			Shared.IndexArray(power).Aggregate(TIn.MultiplicativeIdentity, (a, c) => a *= p).ToType<TOut>());
	}

	private TOut Avg()
	{
		return (Sum() / UnwrappedUnnamedArguments.Count().ToType<TIn>()).ToType<TOut>();
	}

	private TOut Multiply()
	{
		return UnwrappedUnnamedArguments.Aggregate(TIn.MultiplicativeIdentity, (a, c) => a *= c).ToType<TOut>();
	}

	protected TOut Divide()
	{
		return UnwrappedUnnamedArguments.Skip(1).Aggregate(UnwrappedUnnamedArguments.First(), (a, c) => a /= c).ToType<TOut>();
	}

	protected TIn Sum()
	{
		return UnwrappedUnnamedArguments.Aggregate(TIn.AdditiveIdentity, (a, c) => a += c);
	}

	protected TOut Sub()
	{
		return UnwrappedUnnamedArguments.Skip(1).Aggregate(UnwrappedUnnamedArguments.First(), (a, c) => a -= c).ToType<TOut>();
	}

	private TOut Min()
	{
		return (UnwrappedUnnamedArguments.Any() ? UnwrappedUnnamedArguments.Min()! : TIn.MaxValue).ToType<TOut>();
	}

	private TOut Max()
	{
		return (UnwrappedUnnamedArguments.Any() ? UnwrappedUnnamedArguments.Max()! : TIn.MinValue).ToType<TOut>();
	}

	public override Type GetOutType()
	{
		return Operator switch
		{
			NumericFunctionOperator.Greater or
			NumericFunctionOperator.Less or
			NumericFunctionOperator.GreaterOrEquals or
			NumericFunctionOperator.LessOrEquals or
			NumericFunctionOperator.Equals or
			NumericFunctionOperator.NotEquals => typeof(BooleanType),

			_ => throw GetInvalidOperatorException()
		};
	}
}