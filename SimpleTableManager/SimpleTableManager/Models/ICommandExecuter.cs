using System.Runtime.Serialization;

namespace SimpleTableManager.Models;

public interface ICommandExecuter
{
	void InvokeStateModifierCommandExecutedEvent();

	event Action? StateModifierCommandExecuted;
}

public abstract class CommandExecuterBase : ICommandExecuter
{
	public event Action? StateModifierCommandExecuted;

	public virtual void OnStateModifierCommandExecuted() { }

	public void InvokeStateModifierCommandExecutedEvent()
	{
		StateModifierCommandExecuted?.Invoke();
	}
}
