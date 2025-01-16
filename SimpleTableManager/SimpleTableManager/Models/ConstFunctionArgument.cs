namespace SimpleTableManager.Models
{
	public class ConstFunctionArgument<T>(T value) : IConstFunctionArgument
	where T : IParsable<T>
	{
		public T? Value { get; set; } = value;

		object? IConstFunctionArgument.Value
		{
			get => Value!;
			set => Value = (T?)value;
		}

		IEnumerable<object> IFunctionArgument.Resolve()
		{
			return Value.Wrap().Cast<object>();
		}

		public override string ToString()
		{
			return Value?.ToString() ?? $"({typeof(T).GetFriendlyName()})null";
		}
	}
}