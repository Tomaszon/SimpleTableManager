using System;
using System.Collections.Generic;
using System.Linq;
using SimpleTableManager.Models;

namespace SimpleTableManager.Services.Functions;

public class StringFunction : Function<StringFunctionOperator, string>
{
	private string _joinSeparator = ", ";

	public StringFunction(StringFunctionOperator functionOperator, List<IFunction> arguments) : base(functionOperator, arguments)
	{

	}

	public override List<FunctionParameter> Execute(IEnumerable<FunctionParameterArray> parameters = null)
	{
		return new List<FunctionParameter>()
		{
			Operator switch
			{
				StringFunctionOperator.Con => Concat(parameters),
				StringFunctionOperator.Join => Join(parameters),
				StringFunctionOperator.Len => Length(parameters),

				_ => throw new InvalidOperationException()
			}
		};
	}

	private FunctionParameter Join(IEnumerable<FunctionParameterArray> parameters)
	{
		return Aggregate(Arguments.SelectMany(a => a.Execute(parameters)), parameters, new() { { nameof(_joinSeparator), _joinSeparator } });
	}

	private FunctionParameter Concat(IEnumerable<FunctionParameterArray> parameters)
	{
		return Aggregate(Arguments.SelectMany(a => a.Execute(parameters)), parameters);
	}

	private FunctionParameter Length(IEnumerable<FunctionParameterArray> parameters)
	{
		return new FunctionParameter(Concat(parameters).Value.ToString().Length);
	}

	protected override FunctionParameter Aggregate(IEnumerable<FunctionParameter> list, IEnumerable<FunctionParameterArray> parameters, Dictionary<string, object> aggregateArguments = null)
	{
		var hasSeparator = TryGetSeparator(out var separator, aggregateArguments);

		var result = list.Aggregate(FunctionParameter.Default<string>(), (a, c) =>
			a += hasSeparator ? AggregateCore(parameters, c, aggregateArguments) + new FunctionParameter(separator) : AggregateCore(parameters, c));

		if (hasSeparator && result.Value is string str && str is not null)
		{
			result.Value = str.Substring(0, str.Length - separator.Length);
		}

		return result;
	}

	private bool TryGetSeparator(out string joinSeparator, Dictionary<string, object> aggregateArguments = null)
	{
		if (aggregateArguments is null)
		{
			joinSeparator = null;

			return false;
		}

		var result = aggregateArguments.TryGetValue(nameof(_joinSeparator), out var separator);

		joinSeparator = separator.ToString();

		return result;
	}
}