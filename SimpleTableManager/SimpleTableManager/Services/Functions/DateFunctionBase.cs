namespace SimpleTableManager.Services.Functions;

public abstract class DateFunctionBase<TIn, TOut, TNow> : FunctionBase<DateTimeFunctionOperator, TIn, TOut>
where TNow: struct
{
	protected TNow? NowProperty { get; set; }

	protected override IEnumerable<TOut> Execute()
	{
		return Operator switch
		{
			DateTimeFunctionOperator.Const => Arguments.Cast<TOut>(),
			DateTimeFunctionOperator.Sum => Sum().Wrap(),
			DateTimeFunctionOperator.Now => Now().Wrap().Cast<TOut>(),

			_ => throw GetInvalidOperatorException()
		};
	}

	protected abstract TOut Sum();

	protected abstract TNow Now();
}