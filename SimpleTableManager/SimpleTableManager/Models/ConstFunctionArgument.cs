namespace SimpleTableManager.Models
{
	public class ConstFunctionArgument<T> : IConstFunctionArgument
	where T : IParsable<T>
	{
		public T Value { get; set; }

		object IConstFunctionArgument.Value
		{
			get => Value!;
			set => Value = (T)value;
		}

		public ConstFunctionArgument(T value)
		{
			Value = value;
		}

		public IEnumerable<T> Resolve()
		{
			return Value.Wrap();
		}

		IEnumerable<object> IFunctionArgument.Resolve()
		{
			return Resolve().Cast<object>();
		}
	}
}