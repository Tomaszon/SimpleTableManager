﻿using System;
using SimpleTableManager.Services;

namespace SimpleTableManager.Models
{
	public class CommandParameter
	{
		public Type Type { get; set; }

		public string Name { get; set; }

		public bool IsOptional { get; set; }

		public object DefaultValue { get; set; }

		public bool IsArray => Type.IsArray;

		public bool IsNullable => Type.IsAssignableFrom(null);

		public string ParseFormat { get; set; }

		public override string ToString()
		{
			var typeName = $"  type={Shared.FormatTypeName(Type)}";
			var values = Type.IsEnum ? $"  values={string.Join('|', Enum.GetNames(Type))}" : "";
			var nullable = IsNullable ? "  nullable=true" : "";
			var optional = IsOptional ? $"  default={Newtonsoft.Json.JsonConvert.SerializeObject(DefaultValue)}" : "";
			var format = ParseFormat is not null ? $"  {(IsArray ? "elementFormat" : "format")}={ParseFormat}" : "";


			return $"{{{Name}:{typeName}{values}{nullable}{optional}{format}}}";
		}
	}
}
