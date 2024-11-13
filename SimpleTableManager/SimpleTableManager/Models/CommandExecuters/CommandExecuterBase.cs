namespace SimpleTableManager.Models.CommandExecuters;

public abstract class CommandExecuterBase : ValidatorBase, IStateModifierCommandExecuter
{
	public IStateModifierCommandExecuter? ReferencedObject { get; set; }

	public IStateModifierCommandExecuter GetEndReferencedObject()
	{
		return ReferencedObject is not null ? ReferencedObject.GetEndReferencedObject() : this;
	}

	public event Action? StateModifierCommandExecuted;

	public virtual void OnStateModifierCommandExecuted() { }

	public void InvokeStateModifierCommandExecutedEvent()
	{
		StateModifierCommandExecuted?.Invoke();
	}
}