namespace SimpleTableManager.Services
{
	public class InstanceMap
	{
		Dictionary<Type, Func<IEnumerable<object?>>> ArrayMap { get; set; } = new Dictionary<Type, Func<IEnumerable<object?>>>();

		public static InstanceMap Instance { get; } = new InstanceMap();

		public void Add<T>(Func<IEnumerable<T>> func)
		{
			ArrayMap.Add(typeof(T), () => func.Invoke().Select(e => (object?)e));
		}

		public void Add<T>(Func<T> func)
		{
			ArrayMap.Add(typeof(T), () => new[] { func.Invoke() }.Select(e => (object?)e));
		}

		public void Remove<T>(Func<T> func)
		{
			ArrayMap.Remove(typeof(T));
		}

		public IEnumerable<object?> GetInstances(string typeName, out Type type)
		{
			var result = ArrayMap.First(p => p.Key.Name.ToLower() == typeName.ToLower());

			type = result.Key;

			return result.Value.Invoke();
		}

		public IEnumerable<T?> GetInstances<T>()
		{
			return GetInstances(typeof(T)).Select(e => (T?)e);
		}

		public IEnumerable<object?> GetInstances(Type type)
		{
			return ArrayMap[type].Invoke();
		}

		public bool TryGetInstances<T>([NotNullWhen(true)] out IEnumerable<T?>? instances)
		{
			try
			{
				instances = GetInstances<T>();

				return true;
			}
			catch
			{
				instances = null;

				return false;
			}
		}

		public bool TryGetInstances(string typeName, [NotNullWhen(true)] out IEnumerable<object?>? instances, [NotNullWhen(true)] out Type? type)
		{
			try
			{
				instances = GetInstances(typeName, out type);

				return true;
			}
			catch
			{
				instances = null;
				type = null;

				return false;
			}
		}
	}
}
