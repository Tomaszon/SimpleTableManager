using System.Collections.Generic;
using System;

namespace SimpleTableManager.Services.Functions
{
	public interface IFunction
	{
		public IEnumerable<object> Execute();

		public Type GetReturnType();
	}
}