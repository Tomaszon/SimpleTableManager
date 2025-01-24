namespace SimpleTableManager.Services.Functions;

public abstract class NonNullableDateTimeFunctionBase<TIn, TOut> : DateTimeFunctionBase<TIn, TOut>
	where TIn : struct, IConvertible
	where TOut : struct, IConvertible
{
	protected TOut? NowProperty { get; set; }
}