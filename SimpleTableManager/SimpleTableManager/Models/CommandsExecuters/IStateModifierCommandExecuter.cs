namespace SimpleTableManager.Models.CommandExecuters;

public interface IStateModifierCommandExecuter
{
	void InvokeStateModifierCommandExecutedEvent();

	event Action? StateModifierCommandExecuted;
}