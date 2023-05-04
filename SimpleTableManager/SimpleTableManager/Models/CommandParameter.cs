using System;

namespace SimpleTableManager.Models
{
	public class CommandParameter
	{
		public Type Type { get; set; }

		public string Name { get; set; }

		public bool IsOptional { get; set; }

		public object DefaultValue { get; set; }

		public bool IsArray { get; set; }

		public string ParseFormat { get; set; }

		public override string ToString()
		{
			return $"{{{Name}:  type={Type.Name}{(Type.IsEnum ? $"  values={string.Join('|', Enum.GetNames(Type))}" : "")}{(IsOptional ? $"  default={Newtonsoft.Json.JsonConvert.SerializeObject(DefaultValue)}" : "")}{(ParseFormat is not null ? $"  {(IsArray ? "elementFormat" : "format")}={ParseFormat}" : "")}}}";
		}
	}
}
