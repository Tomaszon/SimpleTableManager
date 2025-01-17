using System.Collections;
using System.Runtime.CompilerServices;

namespace SimpleTableManager.Models;

public struct CommandParameter
{
	public Type Type { get; set; }

	public string Name { get; set; }

	public bool IsOptional { get; set; }

	public object? DefaultValue { get; set; }

	public readonly bool IsArray => Type.IsAssignableTo(typeof(IEnumerable)) && Type != typeof(string);

	public readonly bool IsNullable => !Type.IsValueType || Nullable.GetUnderlyingType(Type) is not null;

	public IComparable? MinValue { get; set; }

	public IComparable? MaxValue { get; set; }

	public int MinLength { get; set; }

	public IEnumerable<string> ParseFormats { get; set; }

	public Type?[] ConstArgumentPossibleValueTypes { get; set; }

	public CommandParameter(ParameterInfo parameterInfo)
	{
		Type = parameterInfo.ParameterType;
		Name = parameterInfo.Name!;

		ConstArgumentPossibleValueTypes = parameterInfo.GetCustomAttribute<ConstArgumentPossibleValueTypesAttribute>()?.PossibleTypes ?? [null];

		MinValue = parameterInfo.GetCustomAttribute<MinValueAttribute>()?.Value;
		MaxValue = parameterInfo.GetCustomAttribute<MaxValueAttribute>()?.Value;
		MinLength = parameterInfo.GetCustomAttribute<MinLengthAttribute>()?.Length ?? 0;

		var innerElementType = parameterInfo.ParameterType.GetElementType() ??
			parameterInfo.ParameterType.GenericTypeArguments.SingleOrDefault();

		DefaultValue = IsArray ?
			Array.CreateInstance(innerElementType!, 0) :
			parameterInfo.DefaultValue;

		var typeForFormat = IsArray ? innerElementType! : parameterInfo.ParameterType;

		if (typeForFormat.IsInterface)
		{
			var implementingTypes = Assembly.GetExecutingAssembly().GetTypes().Where(t =>
				!t.IsInterface && t.IsAssignableTo(typeForFormat));

			ParseFormats = implementingTypes.SelectMany(t =>
				t.GetCustomAttributes<ParseFormatAttribute>().Select(a => a.Format));
		}
		else
		{
			ParseFormats = typeForFormat.GetCustomAttributes<ParseFormatAttribute>().Select(a => a.Format);
		}

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