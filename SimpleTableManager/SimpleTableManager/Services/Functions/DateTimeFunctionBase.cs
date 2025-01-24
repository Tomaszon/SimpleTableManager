namespace SimpleTableManager.Services.Functions;

public abstract class DateTimeFunctionBase<TIn, TOut> : FunctionBase<DateTimeFunctionOperator, TIn, TOut>
	where TIn : IConvertible
	where TOut : IConvertible
{
	public override IEnumerable<TOut> ExecuteCore()
	{
		return Operator switch
		{
			DateTimeFunctionOperator.Const => UnwrappedUnnamedArguments.Cast<TOut>(),
			DateTimeFunctionOperator.Sum => Sum().Wrap(),
			DateTimeFunctionOperator.Now => Now().Wrap(),

			_ => throw GetInvalidOperatorException()
		};
	}

	protected abstract TOut Sum();

	protected abstract TOut Now();
}