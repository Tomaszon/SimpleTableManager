using System.Runtime.Serialization;

namespace SimpleTableManager.Models;

public interface ICommandExecuter
{
	event Action? CommandExecuted;

	void OnCommandExecuted() { }

	[OnDeserialized]
	void OnDeserialized(StreamingContext _) { }
}
