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

	public override List<ObjectFunction> Execute(IEnumerable<GroupedObjectFunctions> parameters = null)
	{
		return new List<ObjectFunction>()
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

	private ObjectFunction Join(IEnumerable<GroupedObjectFunctions> parameters)
	{
		return Aggregate(Arguments.SelectMany(a => a.Execute(parameters)), parameters, new() { { nameof(_joinSeparator), _joinSeparator } });
	}

	private ObjectFunction Concat(IEnumerable<GroupedObjectFunctions> parameters)
	{
		return Aggregate(Arguments.SelectMany(a => a.Execute(parameters)), parameters);
	}

	private ObjectFunction Length(IEnumerable<GroupedObjectFunctions> parameters)
	{
		return new ObjectFunction(Concat(parameters).Value.ToString().Length);
	}

	protected override ObjectFunction Aggregate(IEnumerable<ObjectFunction> list, IEnumerable<GroupedObjectFunctions> parameters, Dictionary<string, object> aggregateArguments = null)
	{
		var hasSeparator = TryGetSeparator(out var separator, aggregateArguments);

		var result = list.Aggregate(ObjectFunction.Default<string>(), (a, c) =>
			a += hasSeparator ?
				AggregateCore(parameters, c, aggregateArguments) + new ObjectFunction(separator) :
				AggregateCore(parameters, c));

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