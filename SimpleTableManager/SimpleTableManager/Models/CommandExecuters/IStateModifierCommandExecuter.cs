namespace SimpleTableManager.Models.CommandExecuters;

public interface IStateModifierCommandExecuter
{
	IStateModifierCommandExecuter GetEndReferencedObject();

	void InvokeStateModifierCommandExecutedEvent();

	event Action? StateModifierCommandExecuted;
}