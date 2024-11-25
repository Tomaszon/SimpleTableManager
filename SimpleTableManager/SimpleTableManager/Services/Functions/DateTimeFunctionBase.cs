// namespace SimpleTableManager.Services.Functions;

// public abstract class DateTimeFunctionBase<TIn, TOut> : FunctionBase<DateTimeFunctionOperator, TIn, TOut>
// 	where TOut : struct
// {
// 	protected TOut? NowProperty { get; set; }

// 	public override IEnumerable<TOut> Execute()
// 	{
// 		return Operator switch
// 		{
// 			DateTimeFunctionOperator.Const => Arguments.Cast<TOut>(),
// 			DateTimeFunctionOperator.Sum => Sum().Wrap(),
// 			DateTimeFunctionOperator.Now => Now().Wrap(),

// 			_ => throw GetInvalidOperatorException()
// 		};
// 	}

// 	protected abstract TOut Sum();

// 	protected abstract TOut Now();
// }