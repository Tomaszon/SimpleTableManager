namespace SimpleTableManager.Models.CommandExecuters;

public interface IStateModifierCommandExecuter
{
	IStateModifierCommandExecuter GetEndReferencedObject();

	void InvokeStateModifierCommandExecutedEvent(IStateModifierCommandExecuter root);

	event Action<IStateModifierCommandExecuter, IStateModifierCommandExecuter>? StateModifierCommandExecuted;
}