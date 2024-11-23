using SimpleTableManager.Models.CommandExecuters;

namespace SimpleTableManager.Models
{
	//UNDONE
	public class FunctionParameter<T>
	{
		public T? ConstValue { get; set; }

		public Table? Table { get; set; }

		public Position? Position { get; set; }

		public bool? HorizontallyLocked { get; set; }

		public bool? VerticallyLocked { get; set; }

		public FunctionParameter(Table table, Position position, bool horizontallyLocked = true, bool verticallyLocked = true)
		{
			Table = table;
			Position = position;
			HorizontallyLocked = horizontallyLocked;
			VerticallyLocked = verticallyLocked;
		}

		public FunctionParameter(T value)
		{
			ConstValue = value;
		}

		public static explicit operator List<T>?(FunctionParameter<T> parameter)
		{
			return 
				parameter.Table?[parameter.Position!].ContentFunction?.Execute().Cast<T>().ToList() ??
				parameter.ConstValue!.Wrap().ToList();
		}

		public static explicit operator FunctionParameter<T>(T value)
		{
			return new FunctionParameter<T>(value);
		}
	}
}