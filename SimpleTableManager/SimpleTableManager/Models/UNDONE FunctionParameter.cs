namespace SimpleTableManager.Models
{
	public interface IFunctionArgument
	{
		//TODO handle null values
		IEnumerable<object>? Resolve();

		bool TryResolve(out IEnumerable<object>? result)
		{
			try
			{
				result = Resolve();

				return true;
			}
			catch
			{
				result = null;

				return false;
			}
		}
	}

	public interface IConstFunctionArgument : IFunctionArgument
	{
		object Value { get; set; }
	}

	public class ConstFunctionArgument<T> : IConstFunctionArgument
	where T: IParsable<T>
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
			return Value.Wrap().ToList();
		}

		IEnumerable<object> IFunctionArgument.Resolve()
		{
			return Resolve().Cast<object>();
		}
	}

	public class ReferenceFunctionArgument : IFunctionArgument
	{
		public CellReference Reference { get; set; }

		public ReferenceFunctionArgument(CellReference reference)
		{
			Reference = reference;
		}

		public IEnumerable<object>? Resolve()
		{
			return Reference.Table[Reference.Position].ContentFunction?.Execute();
		}
	}
}