using System;
using System.Collections.Generic;
using System.Linq;

using SimpleTableManager.Models;

namespace SimpleTableManager.Services.Functions
{
	public abstract class Function2<TOpertor, TIn> : IFunction2 where TOpertor : Enum
	{
		public Dictionary<string, object> NamedArguments { get; set; } = new Dictionary<string, object>();

		public IEnumerable<Cell> ReferencedCells { get; set; } = Enumerable.Empty<Cell>();

		public IEnumerable<TIn> ReferenceArguments =>
			ReferencedCells.SelectMany(c => c.ContentFunction2.Execute().Cast<TIn>());

		public IEnumerable<TIn> Arguments { get; set; } = Enumerable.Empty<TIn>();

		public TOpertor Operator { get; set; }

		public abstract IEnumerable<object> Execute();

		

		public Function2() { }
	}
}