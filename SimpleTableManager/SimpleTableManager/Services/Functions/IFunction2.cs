using System.Collections.Generic;

namespace SimpleTableManager.Services.Functions
{
	public interface IFunction2<TIn, TOut>
	{
		public abstract IEnumerable<TOut> Execute(List<TIn> referenceArguments = null);
	}
}