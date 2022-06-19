using System;
using System.Collections.Generic;
using System.Linq;
using SimpleTableManager.Models;

namespace SimpleTableManager.Services.Functions;

public abstract class Function<T> : IFunction where T : Enum
{
	public string Type => this.GetType().Name;

	public T Operator { get; set; }

	public List<FunctionParameter> Arguments { get; init; }

	public List<Position> GetReferredCellPositions() => Arguments.Where(a => a.ReferencePosition is not null).Select(a => a.ReferencePosition).ToList();

	protected abstract FunctionParameter Aggregate(IEnumerable<FunctionParameter> list, IEnumerable<FunctionParameterArray> parameters, Dictionary<string, object> aggregateParameters = null);

	public abstract FunctionParameter Execute(IEnumerable<FunctionParameterArray> parameters = null);

	protected FunctionParameter AggregateCore(IEnumerable<FunctionParameterArray> parameters, FunctionParameter current)
	{
		return parameters?.SingleOrDefault(p => p.Position?.Equals(current.ReferencePosition) == true) is var x && x is not null ?
			Aggregate(x.Parameters.Select(p => new FunctionParameter(p.Value)), parameters) : current;
	}

	protected FunctionParameter ParseArgumentValue<TParse>(FunctionParameter argument)
	{
		return argument.Value is null ? FunctionParameter.Default<TParse>(argument.ReferencePosition) :
			new FunctionParameter(Shared.ParseStringValue(typeof(TParse), argument.Value.ToString()), argument.ReferencePosition);
	}
}