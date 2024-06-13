namespace SimpleTableManager.Models.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class FunctionMappingTypeAttribute : Attribute
{
	public Type MappingType { get; }

	public FunctionMappingTypeAttribute(Type mappingType)
	{
		MappingType = mappingType;
	}
}
