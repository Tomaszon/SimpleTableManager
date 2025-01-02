using System.Runtime.CompilerServices;

namespace SimpleTableManager.Models.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class CommandFunctionAttribute([CallerMemberName] string reference = null!) : Attribute
{
	public string MethodReference { get; set; } = reference;

	public bool StateModifier { get; set; } = true;

	public bool IgnoreReferencedObject { get; set; }

	public bool GlobalCacheClearNeeded { get; set; }

	public bool IsGlobalStorageCellContentClearNeeded { get; set; }
}