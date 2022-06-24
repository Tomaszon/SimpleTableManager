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
		arguments.Where(a => a is FunctionParameter p && p is not null && !p.IsReference()).ForEach(p =>
			(p as FunctionParameter).ParseArgumentValue<T2>());
		Arguments = arguments.ToList();
	}

	public void InsertArguments(int index, IEnumerable<object> arguments)
	{
		var formattedArguments = arguments.Select(a => a is FunctionParameter p && p is not null ? p : new FunctionParameter(a)).ToArray();

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
			if (a is FunctionParameter p && p is not null && p.IsReference())
			{
				return new List<Position>() { (Position)(p as FunctionParameter).Value };
			}
			else
			{
				return a.GetReferredCellPositions();
			}
		}).ToList();

		return result;
	}

	protected abstract FunctionParameter Aggregate(IEnumerable<FunctionParameter> list, IEnumerable<FunctionParameterArray> parameters, Dictionary<string, object> aggregateArguments = null);

	public abstract List<FunctionParameter> Execute(IEnumerable<FunctionParameterArray> parameters = null);

	protected FunctionParameter AggregateCore(IEnumerable<FunctionParameterArray> parameters, FunctionParameter current, Dictionary<string, object> aggregateArguments = null)
	{
		return parameters?.SingleOrDefault(p => p.Position?.Equals(current.Value) == true) is var x && x is not null ?
			Aggregate(x.Parameters.Select(p => new FunctionParameter(p.Value)), parameters, aggregateArguments) : current;
	}
}