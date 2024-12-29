using Newtonsoft.Json.Serialization;

namespace SimpleTableManager.Services;

public class WritablePropertyContractResolver : DefaultContractResolver
{
	protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
	{
		var n = type.Name;
		IList<JsonProperty> props = base.CreateProperties(type, memberSerialization);
		return props.Where(p => p.Writable).ToList();
	}
}