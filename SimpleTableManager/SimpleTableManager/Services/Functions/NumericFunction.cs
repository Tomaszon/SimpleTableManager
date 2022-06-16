using System;
using System.Collections.Generic;
using System.Linq;
using SimpleTableManager.Models;

namespace SimpleTableManager.Services.Functions;

public class NumericFunction : Function
{
	public NumericFunctionType Type { get; set; }

	public NumericFunction(NumericFunctionType type, List<FunctionParameter> arguments) : base()
	{
		Type = type;

		Arguments = arguments.Select(a =>
			a.Value is null ? a : new FunctionParameter(Shared.ParseStringValue(typeof(decimal), a.Value.ToString()), a.ReferencePosition)).ToList();
	}

	public override FunctionParameter Execute(IEnumerable<FunctionParameter> parameters = null)
	{
		return Type switch
		{
			NumericFunctionType.Sum => Sum(parameters),
			NumericFunctionType.Avg => Avg(parameters),

			_ => throw new InvalidOperationException()
		};
	}

	private FunctionParameter Sum(IEnumerable<FunctionParameter> parameters)
	{
		return Arguments.Aggregate(FunctionParameter.Default<decimal>(), (a, c) => a += AggregateCore(parameters, c));
	}

	private FunctionParameter AggregateCore(IEnumerable<FunctionParameter> parameters, FunctionParameter current)
	{
		return parameters is not null &&
			parameters.SingleOrDefault(p =>
				p.ReferencePosition.Equals(current.ReferencePosition)) is var x && x is not null &&
			!x.Equals(default) ?
				new FunctionParameter(x.Value) : current;
	}

	private FunctionParameter Avg(IEnumerable<FunctionParameter> parameters)
	{
		return Sum(parameters) / new FunctionParameter(Arguments.Count());
	}

	private string GetDebuggerDisplay()
	{
		return ToString();
	}
}