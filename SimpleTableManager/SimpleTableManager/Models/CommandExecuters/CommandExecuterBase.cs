namespace SimpleTableManager.Models.CommandExecuters;

public abstract class CommandExecuterBase : ValidatorBase, IStateModifierCommandExecuter
{
	public IStateModifierCommandExecuter? ReferencedObject { get; set; }

	public IStateModifierCommandExecuter GetEndReferencedObject()
	{
		return ReferencedObject is not null ? ReferencedObject.GetEndReferencedObject() : this;
	}

	public event Action<IStateModifierCommandExecuter, IStateModifierCommandExecuter>? StateModifierCommandExecuted;

	public virtual void OnStateModifierCommandExecuted(IStateModifierCommandExecuter sender, IStateModifierCommandExecuter root) { }

	public void InvokeStateModifierCommandExecutedEvent(IStateModifierCommandExecuter root)
	{
		StateModifierCommandExecuted?.Invoke(this, root);
	}
}