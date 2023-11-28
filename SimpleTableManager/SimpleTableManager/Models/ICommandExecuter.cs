using System.Runtime.Serialization;

namespace SimpleTableManager.Models;

public interface ICommandExecuter
{
	event Action? CommandExecuted;

	void OnCommandExecuted();

	// void OnDeserialized(StreamingContext context);
}
