using System.Runtime.CompilerServices;
using SimpleTableManager.Models;

namespace SimpleTableManager.Services;

public class InstanceMap
{
	Dictionary<Type, Func<IEnumerable<ICommandExecuter>>> ArrayMap { get; set; } = new Dictionary<Type, Func<IEnumerable<ICommandExecuter>>>();

	public static InstanceMap Instance { get; } = new InstanceMap();

	public void Add<T>(Func<IEnumerable<T>> func) where T : ICommandExecuter
	{
		ArrayMap.Add(typeof(T), () => func.Invoke().Cast<ICommandExecuter>());
	}

	public void Add<T>(Func<T> func) where T : ICommandExecuter
	{
		ArrayMap.Add(typeof(T), () => new[] { func.Invoke() }.Cast<ICommandExecuter>());
	}

	public void Remove<T>() where T : ICommandExecuter
	{
		ArrayMap.Remove(typeof(T));
	}

	public IEnumerable<ICommandExecuter> GetInstances(string typeName, out Type type)
	{
		var result = ArrayMap.First(p => p.Key.Name.ToLower().Equals(typeName.ToLower()));

		type = result.Key;

		return result.Value.Invoke();
	}

	public T? GetInstance<T>() where T : ICommandExecuter
	{
		return GetInstances<T>().SingleOrDefault();
	}

	public IEnumerable<T?> GetInstances<T>() where T : ICommandExecuter
	{
		return GetInstances(typeof(T)).Select(e => (T?)e);
	}

	public IEnumerable<ICommandExecuter?> GetInstances(Type type)
	{
		return ArrayMap[type].Invoke();
	}

	public bool TryGetInstances<T>([NotNullWhen(true)] out IEnumerable<T?>? instances)
	where T : ICommandExecuter
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

	public bool TryGetInstances(string typeName, [NotNullWhen(true)] out IEnumerable<ICommandExecuter?>? instances, [NotNullWhen(true)] out Type? type)
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