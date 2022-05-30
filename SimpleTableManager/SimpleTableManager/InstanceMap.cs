using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleTableManager
{
	public class InstanceMap
	{
		Dictionary<string, Func<IEnumerable<object>>> ArrayMap { get; set; } = new Dictionary<string, Func<IEnumerable<object>>>();

		public void Add<T>(Func<IEnumerable<T>> func)
		{
			ArrayMap.Add(typeof(T).Name.ToLower(), () => func.Invoke().Select(e => (object)e));
		}

		public void Add<T>(Func<T> func)
		{
			ArrayMap.Add(typeof(T).Name.ToLower(), () => new[] { func.Invoke() }.Select(e => (object)e));
		}

		public void Remove<T>(Func<T> func)
		{
			ArrayMap.Remove(typeof(T).Name.ToLower());
		}

		public IEnumerable<object> GetInstances(string typeName)
		{
			return ArrayMap[typeName].Invoke();
		}

		public IEnumerable<object> GetInstances<T>()
		{
			return GetInstances(typeof(T).Name.ToLower());
		}
	}
}
