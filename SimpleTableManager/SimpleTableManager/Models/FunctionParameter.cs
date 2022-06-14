namespace SimpleTableManager.Models;

public class FunctionParameter<T>
{
	public T Value { get; init; }

	public FunctionParameter(T value)
	{
		Value = value;
	}

	public static FunctionParameter<T> Default => new FunctionParameter<T>(default);

	public static FunctionParameter<T> operator +(FunctionParameter<T> value1, FunctionParameter<T> value2)
	{
		return new FunctionParameter<T>((dynamic)value1.Value + (dynamic)value2.Value);
	}

	public static FunctionParameter<T> operator /(FunctionParameter<T> value1, FunctionParameter<T> value2)
	{
		return new FunctionParameter<T>((dynamic)value1.Value / (dynamic)value2.Value);
	}
}
