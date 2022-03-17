using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleTableManager.Services
{
	public static class Shared
	{
		private static Type GetTypeByName(string name)
		{
			Dictionary<string, string> nameMap = new Dictionary<string, string>
			{
				{ "int", "int32" },
				{ "long", "int64" }
			};

			var type = nameMap.TryGetValue(name.ToLower(), out var mapped) ? mapped : name.ToLower();

			return Type.GetType($"system.{type}", true, true);
		}

		public static object ParseStringValue(string dataTypeName, string value)
		{
			if (!string.IsNullOrWhiteSpace(dataTypeName))
			{
				var type = GetTypeByName(dataTypeName);

				if (type != typeof(string))
				{
					var method = type.GetMethods().Where(m => m.Name == "Parse" && m.GetParameters().Length == 1).SingleOrDefault();

					if (method is null)
					{
						throw new Exception($"Type '{dataTypeName}' does not have 'Parse' method");
					}

					try
					{
						return method.Invoke(null, new[] { value });
					}
					catch (Exception)
					{
						throw new FormatException($"Can not format value '{value}' to type '{dataTypeName}'");
					}
				}
			}

			return value;
		}

		public static void Validate(Func<bool> validator, string error)
		{
			if (!validator())
			{
				throw new InvalidOperationException(error);
			}
		}
	}
}
