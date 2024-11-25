using SimpleTableManager.Models.CommandExecuters;

namespace SimpleTableManager.Models
{
	public interface IFunctionArgument
	{
		//TODO handle null values
		IEnumerable<object>? Resolve();
	}


	public interface IReferenceFunctionArgument : IFunctionArgument
	{
		CellReference CellReference { get; set; }
	}

	public interface IConstFunctionArgument : IFunctionArgument
	{
		object ConstValue { get; set; }
	}

	public class ConstFunctionArgument<T> : IConstFunctionArgument
	{
		public T ConstValue { get; set; }

		object IConstFunctionArgument.ConstValue
		{
			get => ConstValue!;
			set => ConstValue = (T)value;
		}

		public ConstFunctionArgument(T value)
		{
			ConstValue = value;
		}

		public IEnumerable<T> Resolve()
		{
			return ConstValue.Wrap().ToList();
		}

		IEnumerable<object> IFunctionArgument.Resolve()
		{
			return Resolve().Cast<object>();
		}
	}

	//UNDONE
	public class ReferenceFunctionArgument<T> : IReferenceFunctionArgument
	{
		public CellReference CellReference { get; set; }

		public ReferenceFunctionArgument(Table table, Position position, bool horizontallyLocked = true, bool verticallyLocked = true)
		{
			CellReference = new CellReference(table, position, horizontallyLocked, verticallyLocked);
		}

		public IEnumerable<T>? Resolve()
		{
			return CellReference.Table[CellReference.Position].ContentFunction?.Execute().Cast<T>().ToList();
		}

		IEnumerable<object>? IFunctionArgument.Resolve()
		{
			return Resolve()?.Cast<object>();
		}

		// public static explicit operator List<T>(FunctionParameter<T> parameter)
		// {
		// 	return
		// 		parameter.CellReference?.Table[parameter.CellReference.Position].ContentFunction?.Execute().Cast<T>().ToList() ??
		// 		parameter.ConstValue!.Wrap().ToList();
		// }

		// public static explicit operator FunctionParameter<T>(T value)
		// {
		// 	return new FunctionParameter<T>(value);
		// }
	}
}