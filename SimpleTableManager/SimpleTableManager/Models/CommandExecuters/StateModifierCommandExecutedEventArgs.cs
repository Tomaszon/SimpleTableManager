namespace SimpleTableManager.Models.CommandExecuters;

public class StateModifierCommandExecutedEventArgs(IStateModifierCommandExecuter root, bool isGlobalCacheClearNeeded = false, bool isGlobalStorageCellContentClearNeeded = false, bool isPropagable = true) : EventArgs
{
	public IStateModifierCommandExecuter Root { get; set; } = root;

	public bool IsGlobalCacheClearNeeded { get; set; } = isGlobalCacheClearNeeded;

	public bool IsGlobalStorageCellContentClearNeeded { get; set; } = isGlobalStorageCellContentClearNeeded;

	public bool IsPropagable { get; set; } = isPropagable;
}