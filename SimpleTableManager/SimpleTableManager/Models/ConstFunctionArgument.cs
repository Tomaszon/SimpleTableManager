namespace SimpleTableManager.Models
{
	public class ConstFunctionArgument<T>(T value) : IConstFunctionArgument
	where T : IParsable<T>
	{
		public T Value { get; set; } = value;

		object IConstFunctionArgument.Value
		{
			get => Value!;
			set => Value = (T)value;
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