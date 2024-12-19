namespace SimpleTableManager.Models.Attributes;

[AttributeUsage(AttributeTargets.Parameter)]
public class MinLengthAttribute : Attribute
{
	public int Length { get; set; }

	public MinLengthAttribute(int length)
	{
		Length = length;
	}
}