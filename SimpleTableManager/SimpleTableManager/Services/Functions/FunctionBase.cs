using SimpleTableManager.Models;

namespace SimpleTableManager.Services.Functions
{
	public abstract class FunctionBase<TOpertor, TIn, TOut> : IFunction where TOpertor : Enum
	{
		public IEnumerable<Cell> ReferencedCells { get; set; } = Enumerable.Empty<Cell>();

		//TODO make it work, but how? reference cell or fixed position? how to decide?
		public IEnumerable<TIn> ReferenceArguments =>
			ReferencedCells.SelectMany(c => c.ContentFunction.Execute().Cast<TIn>());

		public Dictionary<ArgumentName, string> NamedArguments { get; set; } = new Dictionary<ArgumentName, string>();

		protected IEnumerable<TIn> Arguments { get; set; } = Enumerable.Empty<TIn>();

		IEnumerable<object> IFunction.Arguments => Arguments.Cast<object>();

		protected TOpertor Operator { get; set; }

		Enum IFunction.Operator => Operator;

		protected abstract IEnumerable<TOut> Execute();

		IEnumerable<object> IFunction.Execute()
		{
			try
			{
				return Execute().Cast<object>();
			}
			catch
			{
				return new object[] { "Content function error" };
			}
		}

		public Type? GetReturnType()
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

				return "None";
			}
			catch (Exception ex)
			{
				return ex.Message;
			}
		}

		public Exception GetInvalidOperatorException()
		{
			return new InvalidOperationException($"Operator '{Operator}' is not supported for function type '{GetType().Name}'");
		}

		protected TParse GetArgument<TParse>(ArgumentName key) where TParse : IParsable<TParse>
		{
			return NamedArguments.TryGetValue(key, out var s) ?
				TParse.Parse(s, null) :
				(TParse)GetType().GetCustomAttributes<NamedArgumentAttribute>().Single(p => p.Key == key).Value;
		}
	}
}