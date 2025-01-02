namespace SimpleTableManager.Models.CommandExecuters;

public abstract class CommandExecuterBase : ValidatorBase, IStateModifierCommandExecuter
{
	public IStateModifierCommandExecuter? ReferencedObject { get; set; }

	public IStateModifierCommandExecuter GetEndReferencedObject()
	{
		return ReferencedObject is not null ? ReferencedObject.GetEndReferencedObject() : this;
	}

	public event StateModifierEventHandler StateModifierCommandExecutedEvent = delegate { };

	public virtual void OnStateModifierCommandExecuted(IStateModifierCommandExecuter sender, StateModifierCommandExecutedEventArgs args) { }

	public void InvokeStateModifierCommandExecutedEvent(StateModifierCommandExecutedEventArgs args)
	{
		StateModifierCommandExecutedEvent.Invoke(this, args);
	}
}