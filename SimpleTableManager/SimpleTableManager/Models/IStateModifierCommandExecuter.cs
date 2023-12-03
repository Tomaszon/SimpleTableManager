namespace SimpleTableManager.Models;

public interface IStateModifierCommandExecuter
{
	void InvokeStateModifierCommandExecutedEvent();

	event Action? StateModifierCommandExecuted;
}