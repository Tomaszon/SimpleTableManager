using SimpleTableManager.Services;

namespace SimpleTableManager.Models;

public class CommandParameter
{
	public Type Type { get; set; }

	public string Name { get; set; }

	public bool IsOptional { get; set; }

	public object? DefaultValue { get; set; }

	public bool IsArray => Type.IsArray;

	public bool IsNullable => Type.IsAssignableFrom(null);

	public object? MinValue { get; set; }

	public object? MaxValue { get; set; }

	public IEnumerable<string> ParseFormats { get; set; } = new List<string>();

	public CommandParameter(ParameterInfo parameterInfo)
	{
		var isArray = parameterInfo.ParameterType.IsArray;

		Type = parameterInfo.ParameterType;
		Name = parameterInfo.Name!;

		MinValue = parameterInfo.GetCustomAttribute<MinValueAttribute>()?.Value;
		MaxValue = parameterInfo.GetCustomAttribute<MaxValueAttribute>()?.Value;

		DefaultValue = isArray ?
			Array.CreateInstance(parameterInfo.ParameterType.GetElementType()!, 0) :
			parameterInfo.DefaultValue;

		ParseFormats = (isArray ?
			parameterInfo.ParameterType.GetElementType()! :
			parameterInfo.ParameterType).GetCustomAttributes<ParseFormatAttribute>().Select(a => a.Format);

		IsOptional = parameterInfo.IsOptional || isArray;
	}

	public override string ToString()
	{
		var typeName = $"  type={Shared.FormatTypeName(Type)}";
		var values = Type.IsEnum ? $"  values={string.Join('|', Enum.GetNames(Type))}" : "";
		var nullable = IsNullable ? "  nullable=true" : "";
		var optional = IsOptional ? $"  default={JsonConvert.SerializeObject(DefaultValue)}" : "";
		var formats = ParseFormats.Any() ? $"  {(IsArray ? "elementFormat" : "formats")}={string.Join("' '", ParseFormats)}" : "";
		var minValue = MinValue is not null ? $"  min={MinValue}" : "";
		var maxValue = MaxValue is not null ? $"  min={MaxValue}" : "";


		return $"{{{Name}:{typeName}{minValue}{maxValue}{values}{nullable}{optional}{formats}}}";
	}
}