namespace SimpleTableManager.Services.Functions;

public abstract class DateTimeFunctionBase<TIn, TOut> : FunctionBase<DateTimeFunctionOperator, TIn, TOut>
	where TIn : IConvertible
	where TOut : IConvertible
{
	public override IEnumerable<TOut> ExecuteCore()
	{
		return Operator switch
		{
			DateTimeFunctionOperator.Now or DateTimeFunctionOperator.Const => UnwrappedUnnamedArguments.Cast<TOut>(),
			DateTimeFunctionOperator.Sum => Sum().Wrap(),

			_ => throw GetInvalidOperatorException()
		};
	}

	protected abstract TOut Sum();
}