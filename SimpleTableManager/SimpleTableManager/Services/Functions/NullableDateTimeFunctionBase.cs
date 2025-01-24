namespace SimpleTableManager.Services.Functions;

public abstract class NullableDateTimeFunctionBase<TIn, TOut> : DateTimeFunctionBase<TIn, TOut>
	where TIn : class, IConvertible
	where TOut : class, IConvertible
{
	protected TOut? NowProperty { get; set; }
}