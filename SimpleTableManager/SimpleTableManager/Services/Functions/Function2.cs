using System;
using System.Collections.Generic;

namespace SimpleTableManager.Services.Functions
{
	public abstract class Function2<TIn, TOut, TOpertor> : IFunction2<TIn, TOut> where TOpertor : Enum
	{
		public List<TIn> Arguments { get; set; }

		public TOpertor Operator { get; set; }

		public TOut Default => default(TOut);

		public abstract IEnumerable<TOut> Execute(List<TIn> referenceArguments = null);

		public abstract TOut Convert(TIn value);

		public abstract object ConvertTo(TIn value);

		public abstract TOut ConvertFrom(object value);
	}
}