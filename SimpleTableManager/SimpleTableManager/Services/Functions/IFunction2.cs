using System.Collections.Generic;
using System;

namespace SimpleTableManager.Services.Functions
{
	public interface IFunction2
	{
		public IEnumerable<object> Execute(out Type resultType);
	}
}