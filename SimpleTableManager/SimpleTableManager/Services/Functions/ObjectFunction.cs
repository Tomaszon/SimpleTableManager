using System;
using System.Collections.Generic;
using System.Linq;
using SimpleTableManager.Models;

namespace SimpleTableManager.Services.Functions;

public class ObjectFunction : Function<ObjectFunctionOperator, object>
{
	public bool IsReference() => Value is Position;

	public object Value { get; set; }

	public static ObjectFunction Empty() =>
	new ObjectFunction(ObjectFunctionOperator.Const, new List<IFunction>()) { Value = null };

	public ObjectFunction(ObjectFunctionOperator functionOperator, List<IFunction> arguments) : base(functionOperator, arguments)
	{

	}

	public ObjectFunction(object value) : base(ObjectFunctionOperator.Const, new List<IFunction>())
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
			Value = ObjectFunction.Default<TParse>();
		}
	}

	public static ObjectFunction Default<T>() => new ObjectFunction(default(T));

	public override List<ObjectFunction> Execute(IEnumerable<GroupedObjectFunctions> parameters = null)
	{
		return Value is not null ? new List<ObjectFunction>() { this } : Arguments.SelectMany(a => a.Execute(parameters)).ToList();
	}

	protected override ObjectFunction Aggregate(IEnumerable<ObjectFunction> list, IEnumerable<GroupedObjectFunctions> parameters, Dictionary<string, object> aggregateArguments = null)
	{
		throw new NotSupportedException();
	}

	public static ObjectFunction operator +(ObjectFunction f1, ObjectFunction f2)
	{
		return new ObjectFunction((dynamic)f1.Value + (dynamic)f2.Value);
	}

	public static ObjectFunction operator -(ObjectFunction f1, ObjectFunction f2)
	{
		return new ObjectFunction((dynamic)f1.Value - (dynamic)f2.Value);
	}

	public static ObjectFunction operator /(ObjectFunction f1, ObjectFunction f2)
	{
		return new ObjectFunction((dynamic)f1.Value / (dynamic)f2.Value);
	}

	public static ObjectFunction operator *(ObjectFunction f1, ObjectFunction f2)
	{
		return new ObjectFunction((dynamic)f1.Value * (dynamic)f2.Value);
	}
}