namespace SimpleTableManager.Models;

public class FunctionParameter
{
	public Position ReferredCellPosition { get; set; }

	public object Value { get; init; }

	public FunctionParameter(object value, Position referredCellPosition = null)
	{
		ReferredCellPosition = referredCellPosition;
		Value = value;
	}

	public static FunctionParameter Default<T>() => new FunctionParameter(default(T));

	public static FunctionParameter operator +(FunctionParameter value1, FunctionParameter value2)
	{
		return new FunctionParameter((dynamic)value1.Value + (dynamic)value2.Value);
	}

	public static FunctionParameter operator /(FunctionParameter value1, FunctionParameter value2)
	{
		return new FunctionParameter((dynamic)value1.Value / (dynamic)value2.Value);
	}
}
