namespace SimpleTableManager.Models;

public class CommandReference(string className, string methodName)
{
	public string ClassName { get; set; } = className;

	public string MethodName { get; set; } = methodName;
}