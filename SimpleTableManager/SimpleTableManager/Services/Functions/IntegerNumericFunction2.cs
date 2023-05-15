namespace SimpleTableManager.Services.Functions
{
	public class IntegerNumericFunction2 : NumericFunction2<int, int>
	{
		public override int Convert(int value)
		{
			return value;
		}

		public override int ConvertFrom(object value)
		{
			throw new System.NotImplementedException();
		}

		public override object ConvertTo(int value)
		{
			throw new System.NotImplementedException();
		}

		public override int Cast<T>(System.Numerics.INumber<T> value)
		{
			throw new System.NotImplementedException();
		}
	}
}