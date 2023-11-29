namespace SimpleTableManager.Models;

public interface ICommandExecuter
{
	void InvokeStateModifierCommandExecutedEvent();

	event Action? StateModifierCommandExecuted;
}