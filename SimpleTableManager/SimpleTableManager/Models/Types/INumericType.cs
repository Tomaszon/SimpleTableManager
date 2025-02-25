using System.Numerics;

namespace SimpleTableManager.Models.Types;

public interface INumericType<T, TUnderlying> :
	IType,
	IAdditiveIdentity<T, T>,
	IMultiplicativeIdentity<T, T>,
	IAdditionOperators<T, T, T>,
	ISubtractionOperators<T, T, T>,
	IMultiplyOperators<T, T, T>,
	IDivisionOperators<T, T, T>,
	IMinMaxValue<T>,
	IUnaryNegationOperators<T, T>
	where T :
		INumericType<T, TUnderlying>
	where TUnderlying :
		INumber<TUnderlying>,
		IMinMaxValue<TUnderlying>
{
	static T IAdditiveIdentity<T, T>.AdditiveIdentity => (T)(object)TUnderlying.AdditiveIdentity;

	static T IMultiplicativeIdentity<T, T>.MultiplicativeIdentity => (T)(object)TUnderlying.MultiplicativeIdentity;

	static T IMinMaxValue<T>.MaxValue => (T)(object)TUnderlying.MaxValue;

	static T IMinMaxValue<T>.MinValue => (T)(object)TUnderlying.MaxValue;

	static abstract T Abs(T value);
}
