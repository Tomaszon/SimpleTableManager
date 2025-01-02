namespace SimpleTableManager.Models.CommandExecuters;

public class StateModifierCommandExecutedEventArgs(IStateModifierCommandExecuter root, bool clearsCache = false, GlobalStorageKey clears = GlobalStorageKey.None, bool isPropagable = true) : EventArgs
{
	public IStateModifierCommandExecuter Root { get; set; } = root;

	public bool ClearsCache { get; set; } = clearsCache;

	public GlobalStorageKey Clears { get; set; } = clears;

	public bool IsPropagable { get; set; } = isPropagable;

	public StateModifierCommandExecutedEventArgs(IStateModifierCommandExecuter root, CommandFunctionAttribute attribute) : this(root, attribute.ClearsCache, attribute.Clears) { }
}