using System;
using System.Collections.Generic;
using System.Linq;

using SimpleTableManager.Models;

namespace SimpleTableManager.Services.Functions
{
	public abstract class FunctionBase2<TOpertor, TIn> : IFunction2 where TOpertor : Enum
	{
		public Dictionary<string, string> NamedArguments { get; set; } = new Dictionary<string, string>();

		public IEnumerable<Cell> ReferencedCells { get; set; } = Enumerable.Empty<Cell>();

		public IEnumerable<TIn> ReferenceArguments =>
			ReferencedCells.SelectMany(c => c.ContentFunction2.Execute(out _).Cast<TIn>());

		public IEnumerable<TIn> Arguments { get; set; } = Enumerable.Empty<TIn>();

		public TOpertor Operator { get; set; }

		public FunctionBase2(TOpertor functionOperator, Dictionary<string, string> namedArguments, IEnumerable<TIn> arguments)
		{
			Operator = functionOperator;
			Arguments = arguments.Cast<TIn>();
			if (namedArguments is not null)
			{
				NamedArguments = namedArguments;
			}
		}

		public FunctionBase2() { }

		public abstract IEnumerable<object> Execute(out Type resultType);
	}
}