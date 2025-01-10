using System.Collections;
using System.Runtime.CompilerServices;

namespace SimpleTableManager.Models;

public struct CommandParameter
{
	public Type Type { get; set; }

	public string Name { get; set; }

	public bool IsOptional { get; set; }

	public object? DefaultValue { get; set; }

	public readonly bool IsArray => Type.IsAssignableTo(typeof(IEnumerable));

	public readonly bool IsNullable => Type.IsAssignableFrom(null);

	public IComparable? MinValue { get; set; }

	public IComparable? MaxValue { get; set; }

	public int MinLength { get; set; }

	public IEnumerable<string> ParseFormats { get; set; }

	public CommandParameter(ParameterInfo parameterInfo)
	{
		Type = parameterInfo.ParameterType;
		Name = parameterInfo.Name!;

		MinValue = parameterInfo.GetCustomAttribute<MinValueAttribute>()?.Value;
		MaxValue = parameterInfo.GetCustomAttribute<MaxValueAttribute>()?.Value;
		MinLength = parameterInfo.GetCustomAttribute<MinLengthAttribute>()?.Length ?? 0;

		var innerElementType = parameterInfo.ParameterType.GetElementType() ??
			parameterInfo.ParameterType.GenericTypeArguments.Single();

		DefaultValue = IsArray ?
			Array.CreateInstance(innerElementType, 0) :
			parameterInfo.DefaultValue;

		ParseFormats = (IsArray ? innerElementType : parameterInfo.ParameterType)
			.GetCustomAttributes<ParseFormatAttribute>().Select(a => a.Format);

		IsOptional = 
			parameterInfo.IsOptional || 
			parameterInfo.GetCustomAttribute<ParamCollectionAttribute>() is not null || 
			IsArray;
	}

	public override readonly string ToString()
	{
		var typeName = $"  type={Shared.FormatTypeName(Type)}";
		var values = Type.IsEnum ? $"  values={string.Join('|', Enum.GetNames(Type))}" : "";
		var nullable = IsNullable ? "  nullable=true" : "";
		var optional = IsOptional ? $"  default={JsonConvert.SerializeObject(DefaultValue)}" : "";
		var formats = ParseFormats.Any() ? $"  {(IsArray ? "elementFormat" : "formats")}='{string.Join("' '", ParseFormats)}'" : "";
		var minValue = MinValue is not null ? $"  min={MinValue}" : "";
		var maxValue = MaxValue is not null ? $"  max={MaxValue}" : "";
		var minLength = MinLength > 0 ? $"  minLength={MinLength}" : "";

		return $"{{{Name}:{typeName}{minLength}{minValue}{maxValue}{values}{nullable}{optional}{formats}}}";
	}
}