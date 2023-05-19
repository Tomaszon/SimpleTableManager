using System;
using System.Collections.Generic;
using System.Linq;

using SimpleTableManager.Models;

namespace SimpleTableManager.Services.Functions
{
	public abstract class FunctionBase<TOpertor, TIn> : IFunction where TOpertor : Enum
	{
		public Dictionary<string, string> NamedArguments { get; set; } = new Dictionary<string, string>();

		public IEnumerable<Cell> ReferencedCells { get; set; } = Enumerable.Empty<Cell>();

		//TODO make it work, but how? reference cell or fixed position? how to decide?
		public IEnumerable<TIn> ReferenceArguments =>
			ReferencedCells.SelectMany(c => c.ContentFunction.Execute().Cast<TIn>());

		public IEnumerable<TIn> Arguments { get; set; } = Enumerable.Empty<TIn>();

		public TOpertor Operator { get; set; }

		public FunctionBase(TOpertor functionOperator, Dictionary<string, string> namedArguments, IEnumerable<TIn> arguments)
		{
			Operator = functionOperator;
			Arguments = arguments.Cast<TIn>();
			if (namedArguments is not null)
			{
				NamedArguments = namedArguments;
			}
		}

		public FunctionBase() { }

		public abstract IEnumerable<object> Execute();

		public Type GetReturnType()
		{
			return Execute().First().GetType();
		}
	}
}