namespace SimpleTableManager.Models;

public class FunctionParameter
{
	public Position ReferencePosition { get; set; }

	public bool IsReference { get; set; }

	public object Value { get; init; }

	public FunctionParameter(object value, Position referencePosition = null)
	{
		ReferencePosition = referencePosition;
		IsReference = referencePosition is not null;
		Value = value;
	}

	public static FunctionParameter Default<T>(Position referencePosition = null) => new FunctionParameter(default(T), referencePosition);

	public static FunctionParameter operator +(FunctionParameter value1, FunctionParameter value2)
	{
		return new FunctionParameter((dynamic)value1.Value + (dynamic)value2.Value);
	}

	public static FunctionParameter operator /(FunctionParameter value1, FunctionParameter value2)
	{
		return new FunctionParameter((dynamic)value1.Value / (dynamic)value2.Value);
	}
}