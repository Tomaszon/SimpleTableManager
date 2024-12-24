namespace SimpleTableManager.Services;

/// <summary>
/// Stores instances mapped for types
/// </summary>
public class InstanceMap
{
	private readonly Dictionary<(string LocalizedName, Type Type), Func<IEnumerable<IStateModifierCommandExecuter>>> _arrayMap = [];

	public static InstanceMap Instance { get; } = new();

	public IEnumerable<Type> GetTypes() => _arrayMap.Select(p => p.Key.Type);

	private static string LocalizeKey<T>() => Localizer.Localize<InstanceMap>(typeof(T).Name);

	/// <summary>
	/// Adds new instances to storage
	/// </summary>
	/// <typeparam name="T">Type of instances</typeparam>
	public void Add<T>(Func<IEnumerable<T>> func) where T : IStateModifierCommandExecuter
	{
		_arrayMap.Add((LocalizeKey<T>(), typeof(T)), () => func.Invoke().Cast<IStateModifierCommandExecuter>());
	}

	/// <summary>
	/// Adds new instance to storage
	/// </summary>
	/// <typeparam name="T">Type of instance</typeparam>
	public void Add<T>(Func<T> func) where T : IStateModifierCommandExecuter
	{
		_arrayMap.Add((LocalizeKey<T>(), typeof(T)), () => new[] { func.Invoke() }.Cast<IStateModifierCommandExecuter>());
	}

	/// <summary>
	/// Removes instances from storage
	/// </summary>
	/// <typeparam name="T">Type of instances</typeparam>
	public void Remove<T>() where T : IStateModifierCommandExecuter
	{
		_arrayMap.Remove((LocalizeKey<T>(), typeof(T)));
	}

	/// <summary>
	/// Returns stored instances based on name of a type
	/// </summary>
	public IEnumerable<IStateModifierCommandExecuter> GetInstances(string typeName, out Type type)
	{
		var result = _arrayMap.First(p => p.Key.LocalizedName.ToLower().Equals(typeName.ToLower()));

		type = result.Key.Type;

		return result.Value.Invoke();
	}


	/// <summary>
	/// Returns stored instance based on type
	/// </summary>
	/// <typeparam name="T">Type of instance</typeparam>
	public T? GetInstance<T>() where T : IStateModifierCommandExecuter
	{
		return GetInstances<T>().SingleOrDefault();
	}

	/// <summary>
	/// Returns stored instances based on type
	/// </summary>
	/// <typeparam name="T">Type of instances</typeparam>
	public IEnumerable<T?> GetInstances<T>() where T : IStateModifierCommandExecuter
	{
		return GetInstances(typeof(T)).Select(e => (T?)e);
	}

	/// <summary>
	/// Returns stored instances based on type
	/// </summary>
	public IEnumerable<IStateModifierCommandExecuter> GetInstances(Type type)
	{
		return _arrayMap[(Localizer.Localize<InstanceMap>(type.Name), type)].Invoke();
	}

	/// <summary>
	/// Returns stored instances based on type
	/// </summary>
	/// <typeparam name="T">Type of instances</typeparam>
	public bool TryGetInstances<T>([NotNullWhen(true)] out IEnumerable<T?>? instances)
	where T : IStateModifierCommandExecuter
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

	/// <summary>
	/// Returns stored instances based on type
	/// </summary>
	public bool TryGetInstances(string typeName, [NotNullWhen(true)] out IEnumerable<IStateModifierCommandExecuter?>? instances, [NotNullWhen(true)] out Type? type)
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