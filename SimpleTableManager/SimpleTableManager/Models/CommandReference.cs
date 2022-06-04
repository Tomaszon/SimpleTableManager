namespace SimpleTableManager.Models
{
	public class CommandReference
	{
		public string ClassName { get; set; }

		public string MethodName { get; set; }

		public CommandReference(string className, string methodName)
		{
			ClassName = className;
			MethodName = methodName;
		}
	}
}