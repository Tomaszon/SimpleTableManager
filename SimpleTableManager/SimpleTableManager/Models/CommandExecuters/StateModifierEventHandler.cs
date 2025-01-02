namespace SimpleTableManager.Models.CommandExecuters
{
	public delegate void StateModifierEventHandler(IStateModifierCommandExecuter sender, StateModifierCommandExecutedEventArgs args);
}