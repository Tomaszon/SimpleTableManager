using System.Numerics;
using SimpleTableManager.Models.Enumerations.FunctionOperators;

namespace SimpleTableManager.Services.Functions;

[NamedArgument<int>(ArgumentName.Power, 1), NamedArgument<double>(ArgumentName.Base, 2)]
[NamedArgument<int>(ArgumentName.Decimals, 2)]
public abstract class NumericFunctionBase<TIn> :
	FunctionBase<NumericFunctionOperator, TIn, object>
	where TIn : struct, INumber<TIn>, IMinMaxValue<TIn>, IConvertible
{
	public override IEnumerable<object> ExecuteCore()
	{
		return Operator switch
		{
			NumericFunctionOperator.Const => UnwrappedUnnamedArguments.Cast<object>(),
			NumericFunctionOperator.Neg => UnwrappedUnnamedArguments.Select(a => -a).Cast<object>(),
			NumericFunctionOperator.Abs => UnwrappedUnnamedArguments.Select(TIn.Abs).Cast<object>(),
			NumericFunctionOperator.Sum => Sum().Wrap<object>(),
			NumericFunctionOperator.Sub => Sub().Wrap(),
			NumericFunctionOperator.Avg => Avg().Wrap(),
			NumericFunctionOperator.Min => Min().Wrap(),
			NumericFunctionOperator.Max => Max().Wrap(),
			NumericFunctionOperator.Mul => Multiply().Wrap(),
			NumericFunctionOperator.Div => Divide().Wrap(),
			NumericFunctionOperator.Pow => Power(GetNamedArgument<int>(ArgumentName.Power)),
			NumericFunctionOperator.Sqrt => Sqrt(),
			NumericFunctionOperator.Log2 => LogN(2),
			NumericFunctionOperator.Log10 => LogN(10),
			NumericFunctionOperator.LogE => LogN(double.E),
			NumericFunctionOperator.LogN => LogN(GetNamedArgument<double>(ArgumentName.Base)),
			NumericFunctionOperator.Greater => Greater().Wrap().Cast<object>(),
			NumericFunctionOperator.Less => Less().Wrap().Cast<object>(),
			NumericFunctionOperator.GreaterOrEquals => GreaterOrEquals().Wrap().Cast<object>(),
			NumericFunctionOperator.LessOrEquals => LessOrEquals().Wrap().Cast<object>(),
			NumericFunctionOperator.Equals => Equals().Wrap().Cast<object>(),
			NumericFunctionOperator.NotEquals => NotEquals().Wrap().Cast<object>(),

			_ => throw GetInvalidOperatorException()
		};
	}

	private IEnumerable<object> LogN(double @base)
	{
		return UnwrappedUnnamedArguments.Select(p =>
			(object)double.Round(Math.Log(p.ToDouble(null), @base), GetNamedArgument<int>(ArgumentName.Decimals)));
	}

	private IEnumerable<object> Sqrt()
	{
		return UnwrappedUnnamedArguments.Select(p =>
			(object)Math.Sqrt(p.ToDouble(null)));
	}

	private IEnumerable<object> Power(int power)
	{
		return UnwrappedUnnamedArguments.Select(p =>
			(object)Shared.IndexArray(power).Aggregate(TIn.MultiplicativeIdentity, (a, c) => a *= p));
	}

	private object Avg()
	{
		return Sum() / UnwrappedUnnamedArguments.Count().ToType<TIn>();
	}

	private object Multiply()
	{
		return UnwrappedUnnamedArguments.Aggregate(TIn.MultiplicativeIdentity, (a, c) => a *= c);
	}

	protected object Divide()
	{
		return UnwrappedUnnamedArguments.Skip(1).Aggregate(UnwrappedUnnamedArguments.First(), (a, c) => a /= c);
	}

	protected TIn Sum()
	{
		return UnwrappedUnnamedArguments.Aggregate(TIn.AdditiveIdentity, (a, c) => a += c);
	}

	protected object Sub()
	{
		return UnwrappedUnnamedArguments.Skip(1).Aggregate(UnwrappedUnnamedArguments.First(), (a, c) => a -= c);
	}

	private object Min()
	{
		return UnwrappedUnnamedArguments.Any() ? UnwrappedUnnamedArguments.Min() : TIn.MaxValue;
	}

	private object Max()
	{
		return UnwrappedUnnamedArguments.Any() ? UnwrappedUnnamedArguments.Max() : TIn.MinValue;
	}

    public override Type GetOutType()
    {
        return Operator switch
		{
			>= NumericFunctionOperator.Greater and
			<= NumericFunctionOperator.NotEquals => typeof(FormattableBoolean),

			_ => throw GetInvalidOperatorException()
		};
    }
}