using System.Runtime.CompilerServices;

namespace SimpleTableManager.Models.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class CommandReferenceAttribute : Attribute
{
	public string MethodReference { get; set; }

	public CommandReferenceAttribute([CallerMemberName] string reference = null!)
	{
		MethodReference = reference;
	}
}