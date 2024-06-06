namespace SimpleTableManager.Models.CommandExecuters;

public abstract class CommandExecuterBase : ValidatorBase, IStateModifierCommandExecuter
{
	public event Action? StateModifierCommandExecuted;

	public virtual void OnStateModifierCommandExecuted() { }

	public void InvokeStateModifierCommandExecutedEvent()
	{
		StateModifierCommandExecuted?.Invoke();
	}
}