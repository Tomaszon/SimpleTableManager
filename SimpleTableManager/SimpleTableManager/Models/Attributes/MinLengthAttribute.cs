namespace SimpleTableManager.Models.Attributes;

[AttributeUsage(AttributeTargets.Parameter)]
public class MinLengthAttribute(int length) : Attribute
{
	public int Length { get; set; } = length;
}