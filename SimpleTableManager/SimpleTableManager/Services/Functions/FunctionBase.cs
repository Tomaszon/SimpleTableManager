using System;
using System.Collections.Generic;
using System.Linq;

using SimpleTableManager.Models;

namespace SimpleTableManager.Services.Functions
{
	public abstract class FunctionBase<TOpertor, TIn, TOut> : IFunction where TOpertor : Enum
	{
		public Dictionary<string, string> NamedArguments { get; set; } = new Dictionary<string, string>();

		public IEnumerable<Cell> ReferencedCells { get; set; } = Enumerable.Empty<Cell>();

		//TODO make it work, but how? reference cell or fixed position? how to decide?
		public IEnumerable<TIn> ReferenceArguments =>
			ReferencedCells.SelectMany(c => c.ContentFunction.Execute().Cast<TIn>());

		public IEnumerable<TIn> Arguments { get; set; } = Enumerable.Empty<TIn>();

		IEnumerable<object> IFunction.Arguments => Arguments.Cast<object>();

		public Enum Operator { get; set; }

		public FunctionBase() { }

		public abstract IEnumerable<TOut> Execute();

		IEnumerable<object> IFunction.Execute() => Execute().Cast<object>();

		public Type GetReturnType()
		{
			try
			{
				return Execute().First().GetType();
			}
			catch
			{
				return null;
			}
		}

		public string GetError()
		{
			try
			{
				Execute();
			}
			catch (Exception ex)
			{
				return ex.Message;
			}

			return "None";
		}
	}
}