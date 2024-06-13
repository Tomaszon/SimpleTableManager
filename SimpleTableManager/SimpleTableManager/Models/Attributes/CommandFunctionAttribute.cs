using System.Runtime.CompilerServices;

namespace SimpleTableManager.Models.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class CommandFunctionAttribute : Attribute
{
	public string MethodReference { get; set; }

	public bool StateModifier { get; set; } = true;

	public CommandFunctionAttribute([CallerMemberName] string reference = null!)
	{
		MethodReference = reference;
	}
}