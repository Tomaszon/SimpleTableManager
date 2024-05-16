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

	public IEnumerable<string> ParseFormats { get; set; } = new List<string>();

	public CommandParameter(Type type, string name)
	{
		Type = type;
		Name = name;
	}

	public override string ToString()
	{
		var typeName = $"  type={Shared.FormatTypeName(Type)}";
		var values = Type.IsEnum ? $"  values={string.Join('|', Enum.GetNames(Type))}" : "";
		var nullable = IsNullable ? "  nullable=true" : "";
		var optional = IsOptional ? $"  default={JsonConvert.SerializeObject(DefaultValue)}" : "";
		var formats = ParseFormats.Any() ? $"  {(IsArray ? "elementFormat" : "formats")}={string.Join("' '", ParseFormats)}" : "";


		return $"{{{Name}:{typeName}{values}{nullable}{optional}{formats}}}";
	}
}