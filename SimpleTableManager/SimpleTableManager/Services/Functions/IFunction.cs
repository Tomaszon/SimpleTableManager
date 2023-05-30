using System.Collections.Generic;
using System;

namespace SimpleTableManager.Services.Functions
{
	public interface IFunction
	{
		public Dictionary<string, string> NamedArguments { get; }

		public IEnumerable<object> Arguments { get; }

		public Enum Operator { get; }

		public IEnumerable<object> Execute();

		public Type GetReturnType();

		public string GetError();
	}
}