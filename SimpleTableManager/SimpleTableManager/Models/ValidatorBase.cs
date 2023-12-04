namespace SimpleTableManager.Models;

public abstract class ValidatorBase
{
	public static void ThrowIf([DoesNotReturnIf(true)] bool validator, string error)
	{
		ThrowIf<ArgumentException>(validator, error);
	}

	public static void ThrowIf<T>([DoesNotReturnIf(true)] bool validator, string error)
	where T : Exception
	{
		if (validator)
		{
			throw (T)Activator.CreateInstance(typeof(T), error)!;
		}
	}

	public static void ThrowIfNot([DoesNotReturnIf(false)] bool validator, string error)
	{
		ThrowIfNot<ArgumentException>(validator, error);
	}

	public static void ThrowIfNot<T>([DoesNotReturnIf(false)] bool validator, string error)
	where T : Exception
	{
		if (!validator)
		{
			throw (T)Activator.CreateInstance(typeof(T), error)!;
		}
	}
}