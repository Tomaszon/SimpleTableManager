using SimpleTableManager.Models.CommandExecuters;

namespace SimpleTableManager.Services;

public class InstanceMap
{
	private readonly Dictionary<(string LocalizedName, Type Type), Func<IEnumerable<IStateModifierCommandExecuter>>> _arrayMap = new();

	public static InstanceMap Instance { get; } = new();

	public IEnumerable<Type> GetTypes() => _arrayMap.Select(p => p.Key.Type);

	private string LocalizeKey<T>() => Localizer.Localize<InstanceMap>(typeof(T).Name);

	public void Add<T>(Func<IEnumerable<T>> func) where T : IStateModifierCommandExecuter
	{
		_arrayMap.Add((LocalizeKey<T>(), typeof(T)), () => func.Invoke().Cast<IStateModifierCommandExecuter>());
	}

	public void Add<T>(Func<T> func) where T : IStateModifierCommandExecuter
	{
		
		_arrayMap.Add((LocalizeKey<T>(), typeof(T)), () => new[] { func.Invoke() }.Cast<IStateModifierCommandExecuter>());
	}

	public void Remove<T>() where T : IStateModifierCommandExecuter
	{
		_arrayMap.Remove((LocalizeKey<T>(), typeof(T)));
	}

	public IEnumerable<IStateModifierCommandExecuter> GetInstances(string typeName, out Type type)
	{
		var result = _arrayMap.First(p => p.Key.LocalizedName.ToLower().Equals(typeName.ToLower()));

		type = result.Key.Type;

		return result.Value.Invoke();
	}

	public T? GetInstance<T>() where T : IStateModifierCommandExecuter
	{
		return GetInstances<T>().SingleOrDefault();
	}

	public IEnumerable<T?> GetInstances<T>() where T : IStateModifierCommandExecuter
	{
		return GetInstances(typeof(T)).Select(e => (T?)e);
	}

	public IEnumerable<IStateModifierCommandExecuter> GetInstances(Type type)
	{
		return _arrayMap[(Localizer.Localize<InstanceMap>(type.Name), type)].Invoke();
	}

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