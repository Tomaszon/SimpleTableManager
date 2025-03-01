namespace SimpleTableManager.Models;

public struct CommandReference(string className, string methodName, bool withSelector1 = false, bool withSelector2 = false)
{
	public string ClassName { get; set; } = className;

	public string MethodName { get; set; } = methodName;

	public bool WithSelector1 { get; set; } = withSelector1;

	public bool WithSelector2 { get; set; } = withSelector2;
}