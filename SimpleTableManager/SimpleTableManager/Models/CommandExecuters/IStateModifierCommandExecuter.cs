namespace SimpleTableManager.Models.CommandExecuters;

public interface IStateModifierCommandExecuter
{
	IStateModifierCommandExecuter GetEndReferencedObject();

	void InvokeStateModifierCommandExecutedEvent(StateModifierCommandExecutedEventArgs arg);

}