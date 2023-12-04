namespace SimpleTableManager.Models;

public abstract class CommandExecuterBase : ValidatorBase, IStateModifierCommandExecuter
{
	public event Action? StateModifierCommandExecuted;

	public virtual void OnStateModifierCommandExecuted() { }

	public void InvokeStateModifierCommandExecutedEvent()
	{
		StateModifierCommandExecuted?.Invoke();
	}
}