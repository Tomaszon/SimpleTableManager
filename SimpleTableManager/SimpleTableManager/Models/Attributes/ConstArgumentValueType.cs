namespace SimpleTableManager.Models.Attributes;

/// <summary>
/// Indicates the possible type of a const argument's inner value
/// </summary>
public class PossibleValueTypesAttribute<T>() :
	ConstArgumentPossibleValueTypesAttribute([typeof(T)]);

/// <summary>
/// Indicates the possible types of a const argument's inner value. Tried in order, first successful type parse passes
/// </summary>
public class ConstArgumentPossibleValueTypesAttribute<T1, T2>() :
	ConstArgumentPossibleValueTypesAttribute([typeof(T1), typeof(T2)]);

/// <summary>
/// Indicates the possible types of a const argument's inner value. Tried in order, first successful type parse passes
/// </summary>
public class ConstArgumentPossibleValueTypesAttribute<T1, T2, T3>() :
	ConstArgumentPossibleValueTypesAttribute([typeof(T1), typeof(T2), typeof(T3)]);

/// <summary>
/// Indicates the possible types of a const argument's inner value. Tried in order, first successful type parse passes
/// </summary>
public class ConstArgumentPossibleValueTypesAttribute<T1, T2, T3, T4>() :
	ConstArgumentPossibleValueTypesAttribute([typeof(T1), typeof(T2), typeof(T3), typeof(T4)]);

/// <summary>
/// Indicates the possible types of a const argument's inner value. Tried in order, first successful type parse passes
/// </summary>
public class ConstArgumentPossibleValueTypesAttribute<T1, T2, T3, T4, T5>() :
	ConstArgumentPossibleValueTypesAttribute([typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5)]);

/// <summary>
/// Indicates the possible types of a const argument's inner value. Tried in order, first successful type parse passes
/// </summary>
public class ConstArgumentPossibleValueTypesAttribute<T1, T2, T3, T4, T5, T6>() :
	ConstArgumentPossibleValueTypesAttribute([typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6)]);

/// <summary>
/// Indicates the possible types of a const argument's inner value. Tried in order, first successful type parse passes
/// </summary>
public class ConstArgumentPossibleValueTypesAttribute<T1, T2, T3, T4, T5, T6, T7>() :
	ConstArgumentPossibleValueTypesAttribute([typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7)]);

/// <summary>
/// Indicates the possible types of a const argument's inner value. Tried in order, first successful type parse passes
/// </summary>
public class ConstArgumentPossibleValueTypesAttribute<T1, T2, T3, T4, T5, T6, T7, T8>() :
	ConstArgumentPossibleValueTypesAttribute([typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8)]);

/// <summary>
/// Indicates the possible types of a const argument's inner value. Tried in order, first successful type parse passes
/// </summary>
public class ConstArgumentPossibleValueTypesAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9>() :
	ConstArgumentPossibleValueTypesAttribute([typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9)]);

/// <summary>
/// Indicates the possible types of a const argument's inner value. Tried in order, first successful type parse passes
/// </summary>
public class ConstArgumentPossibleValueTypesAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>() :
	ConstArgumentPossibleValueTypesAttribute([typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10)]);

[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
public abstract class ConstArgumentPossibleValueTypesAttribute(params Type[] types) : Attribute
{
	public Type?[] PossibleTypes { get; } = types;
}