using System.Globalization;

/// <summary>
/// Provides language dependent texts for translation keys
/// </summary>
namespace SimpleTableManager.Services
{
	public static class Localizer
	{
		private static readonly Dictionary<string, string> _KEYS = [];

		public static void FromJson(string path)
		{
			_KEYS.Clear();

			foreach (var culture in Directory.GetDirectories(path))
			{
				foreach (var type in Directory.GetFiles(culture))
				{
					foreach (var pair in JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(type))!)
					{
						var typeName = Path.GetFileNameWithoutExtension(type).ToLower();

						var key = typeName == "generic" ? $"{Path.GetFileNameWithoutExtension(culture).ToLower()}.{pair.Key.ToLower()}"
						: $"{Path.GetFileNameWithoutExtension(culture).ToLower()}.{typeName}.{pair.Key.ToLower()}";

						_KEYS.Add(key, pair.Value);
					}
				}
			}
		}

		/// <summary>
		/// Returns translation for a given key for a given type
		/// </summary>
		public static bool TryLocalize(Type type, string key, out string result)
		{
			return TryLocalize(type, null, key, out result);
		}

		/// <summary>
		/// Returns translation for a given key for a given type
		/// </summary>
		/// <typeparam name="T">Type of instance to find localized text for</typeparam>
		public static string TryLocalize<T>(string key, params object[] args)
		{
			TryLocalize(typeof(T), null, key, out var result, args);

			return result;
		}

		/// <summary>
		/// Returns translation for a given key in a given method for a given type
		/// </summary>
		/// <typeparam name="T">Type of instance to find localized text for</typeparam>
		public static bool TryLocalize<T>(string? method, string key, out string result, params object[] args)
		{
			return TryLocalize(typeof(T), method, key, out result, args);
		}

		/// <summary>
		/// Returns translation for a given key in a given method for a given type in current ui culture or in its parent culture if translation not found
		/// </summary>
		public static bool TryLocalize(Type? type, string? method, string key, out string result, params object[] args)
		{
			if (TryLocalizeFor(CultureInfo.CurrentUICulture, type?.Name, method, key, args, out var result1))
			{
				result = result1;
				return true;
			}
			else if (TryLocalizeFor(CultureInfo.CurrentUICulture.Parent, type?.Name, method, key, args, out var result2))
			{

				result = result2;
				return true;
			}
			else
			{
				result = result1;
				return false;
			}
		}


		/// <summary>
		/// Returns translation for a given key
		/// </summary>
		public static string Localize<T>(string key)
		{
			return Localize(typeof(T), null, key);
		}

		/// <summary>
		/// Returns translation for a given key in a given method for a given type
		/// </summary>
		public static string Localize(Type? type, string? method, string key, params object[] args)
		{
			if (TryLocalize(type, method, key, out var result, args))
			{
				return result;
			}
			else
			{
				throw new LocalizationException(result);
			}
		}


		/// <summary>
		/// Returns translation for a given key in a given method for a given type in a given culture
		/// </summary>
		private static bool TryLocalizeFor(CultureInfo culture, string? typeName, string? method, string key, object[] args, out string result)
		{
			var cultureName = culture.Name.ToLower();

			typeName = typeName?.ToLower();
			method = method?.ToLower();
			key = key.ToLower();

			var key1 = $"{cultureName}.{typeName}.{method}.{key}";
			var key2 = $"{cultureName}.{typeName}.{key}";
			var key3 = $"{cultureName}.{key}";

			if (typeName is not null && method is not null && _KEYS.TryGetValue(key1, out var value))
			{
				result = string.Format(value, args);

				return true;
			}
			else if (typeName is not null && _KEYS.TryGetValue(key2, out value))
			{
				result = string.Format(value, args);

				return true;
			}
			else if (_KEYS.TryGetValue(key3, out value))
			{
				result = string.Format(value, args);

				return true;
			}
			else
			{
				result = $"{key}:";
				if (typeName is not null && method is not null)
				{
					result += $" {key1}";
				}
				else if (typeName is not null)
				{
					result += $" -> {key2}";
				}
				result += $" -> {key3}";

				return false;
			}
		}
	}
}