using System;
using System.Collections.Generic;
using System.Linq;
using SimpleTableManager.Extensions;
using SimpleTableManager.Models;

namespace SimpleTableManager.Services.Functions;

public abstract class Function<T, T2> : IFunction where T : Enum
{
	public string TypeName => this.GetType().Name;

	public T Operator { get; set; }

	public List<IFunction> Arguments { get; set; }

	public Function(T functionOperator, IEnumerable<IFunction> arguments)
	{
		Operator = functionOperator;
		arguments.Where(a => a is ObjectFunction p && p is not null && !p.IsReference() == true).ForEach(p =>
			(p as ObjectFunction).ParseArgumentValue<T2>());
		Arguments = arguments.ToList();
	}

	public void InsertArguments(int index, IEnumerable<object> arguments)
	{
		var formattedArguments = arguments.Select(a =>
			a is ObjectFunction p && p is not null ? p : new ObjectFunction(a)).ToArray();

		Arguments.InsertRange(index, formattedArguments);
	}

	public void AddArguments(IEnumerable<object> arguments)
	{
		InsertArguments(Arguments.Count, arguments);
	}

	public void RemoveLastArgument()
	{
		Arguments.RemoveAt(Arguments.Count - 1);
	}

	public List<Position> GetReferredCellPositions()
	{
		var result = Arguments.SelectMany(a =>
		{
			if (a is ObjectFunction p && p is not null && p.IsReference() == true)
			{
				return new List<Position>() { (Position)p.Value };
			}
			else
			{
				return a.GetReferredCellPositions();
			}
		}).ToList();

		return result;
	}

	protected abstract ObjectFunction Aggregate(IEnumerable<ObjectFunction> list, IEnumerable<GroupedObjectFunctions> parameters, Dictionary<string, object> aggregateArguments = null);

	public abstract List<ObjectFunction> Execute(IEnumerable<GroupedObjectFunctions> parameters = null);

	protected ObjectFunction AggregateCore(IEnumerable<GroupedObjectFunctions> parameters, ObjectFunction current, Dictionary<string, object> aggregateArguments = null)
	{
		return parameters?.SingleOrDefault(p => p.Position?.Equals(current.Value) == true) is var x && x is not null ?
			Aggregate(x.Values.Select(p => new ObjectFunction(p.Value)), parameters, aggregateArguments) : current;
	}
}