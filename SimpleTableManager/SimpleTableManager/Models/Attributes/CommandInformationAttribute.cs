namespace SimpleTableManager.Models.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public class CommandInformationAttribute : Attribute
{
	public string Information { get; set; }

	public CommandInformationAttribute(string information)
	{
		Information = information;
	}
}