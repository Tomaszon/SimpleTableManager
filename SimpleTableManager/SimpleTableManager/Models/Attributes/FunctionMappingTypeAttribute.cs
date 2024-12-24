namespace SimpleTableManager.Models.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class FunctionMappingTypeAttribute(Type mappingType) : Attribute
{
	public Type MappingType { get; } = mappingType;
}
