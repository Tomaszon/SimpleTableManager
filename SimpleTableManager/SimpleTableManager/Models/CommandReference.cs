namespace SimpleTableManager.Models;

public struct CommandReference(string className, string methodName, bool withSelector = false)
{
	public string ClassName { get; set; } = className;

	public string MethodName { get; set; } = methodName;

	public bool WithSelector { get; set; } = withSelector;
}