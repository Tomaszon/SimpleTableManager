using System.Collections.Generic;
using SimpleTableManager.Services;
using SimpleTableManager.Services.Functions;

namespace SimpleTableManager.Models;

public class FunctionParameter : ObjectFunction
{
	public bool IsReference() => Value is Position;

	public object Value { get; set; }

	public FunctionParameter(object value) : base(ObjectFunctionOperator.Const, new())
	{
		if (value is Position position && position is not null || value is not null && Position.TryParse(value.ToString(), out position))
		{
			Value = position;
		}
		else
		{
			Value = value;
		}
	}

	public void ParseArgumentValue<TParse>()
	{
		if (Value is not null)
		{
			Value = typeof(TParse) != typeof(object) && !typeof(TParse).IsAssignableFrom(Value.GetType()) ?
				Shared.ParseStringValue(typeof(TParse), Value.ToString()) : Value;
		}
		else
		{
			Value = FunctionParameter.Default<TParse>();
		}
	}

	public static FunctionParameter Default<T>() => new FunctionParameter(default(T));

	public override List<FunctionParameter> Execute(IEnumerable<FunctionParameterArray> parameters = null)
	{
		return new List<FunctionParameter>() { new FunctionParameter(Value) };
	}

	public static FunctionParameter operator +(FunctionParameter value1, FunctionParameter value2)
	{
		return new FunctionParameter((dynamic)value1.Value + (dynamic)value2.Value);
	}

	public static FunctionParameter operator -(FunctionParameter value1, FunctionParameter value2)
	{
		return new FunctionParameter((dynamic)value1.Value - (dynamic)value2.Value);
	}

	public static FunctionParameter operator /(FunctionParameter value1, FunctionParameter value2)
	{
		return new FunctionParameter((dynamic)value1.Value / (dynamic)value2.Value);
	}

	public static FunctionParameter operator *(FunctionParameter value1, FunctionParameter value2)
	{
		return new FunctionParameter((dynamic)value1.Value * (dynamic)value2.Value);
	}
}